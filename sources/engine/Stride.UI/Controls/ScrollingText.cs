// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Games;

namespace Stride.UI.Controls
{
    /// <summary>
    ///   Represents a text viewer that scrolls automatically the text from right to left.
    /// </summary>
    [DataContract(nameof(ScrollingText))]
    [DebuggerDisplay("ScrollingText - Name={Name}")]
    public class ScrollingText : TextBlock
    {
        private string textToDisplay = "";

        private float elementWidth;

        /// <summary>
        ///   The index in <see cref="TextBlock.Text"/> defining the position of the next letter to add to
        ///   <see cref="TextToDisplay"/>.
        /// </summary>
        private int nextLetterIndex;

        private bool textHasBeenAppended;
        private float scrollingSpeed = 40.0f;
        private uint desiredCharacterNumber = 10;
        private bool repeatText = true;

        /// <summary>
        ///   Gets the current offset of the text in the X axis.
        /// </summary>
        public float ScrollingOffset { get; private set; }

        /// <summary>
        ///   Gets the total accumulated width of the scrolling text since the last call the <see cref="ResetDisplayingText"/>.
        /// </summary>
        public float AccumulatedWidth { get; private set; }

        public ScrollingText()
        {
            ResetDisplayingText();
            DrawLayerNumber += 3; // (1: clipping border, 2: Text, 3: clipping border undraw)
        }

        /// <summary>
        ///   Gets or sets the scrolling speed of the text, in virtual pixels.
        /// </summary>
        /// <remarks>The value is coerced in the range [0, <see cref="float.MaxValue"/>].</remarks>
        /// <userdoc>The scrolling speed of the text, in virtual pixels.</userdoc>
        [DataMember]
        [DataMemberRange(0.0f, 3)]
        [Display(category: BehaviorCategory)]
        [DefaultValue(40.0f)]
        public float ScrollingSpeed
        {
            get => scrollingSpeed;

            set
            {
                if (float.IsNaN(value))
                    return;

                scrollingSpeed = MathUtil.Clamp(value, 0.0f, float.MaxValue);
            }
        }

        /// <summary>
        ///   Gets or sets the desired number of characters in average to display at a given time.
        ///   This value is taken in account during the measurement stage of the element.
        /// </summary>
        /// <userdoc>The desired number of character in average to display at a given time.</userdoc>
        [DataMember]
        [Display(category: BehaviorCategory)]
        [DefaultValue((uint)10)]
        public uint DesiredCharacterNumber
        {
            get => desiredCharacterNumber;

            set
            {
                if (desiredCharacterNumber == value)
                    return;

                desiredCharacterNumber = value;
                InvalidateMeasure();
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating if the text message must be repeated (wrapped) or not.
        /// </summary>
        /// <userdoc>Whether the text message must be repeated (wrapped).</userdoc>
        [DataMember]
        [Display(category: BehaviorCategory)]
        [DefaultValue(true)]
        public bool RepeatText
        {
            get => repeatText;

            set
            {
                if (repeatText == value)
                    return;

                repeatText = value;
                ResetDisplayingText();
            }
        }

        /// <summary>
        ///   Append the provided text to the end of the current <see cref="TextBlock.Text"/> without restarting
        ///   the display to the beginning of the <see cref="TextBlock.Text"/>.
        /// </summary>
        /// <param name="text">The text to append</param>
        public void AppendText(string text)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            textHasBeenAppended = true;
            Text += text;
        }

        /// <summary>
        ///   Clears the currently scrolling text.
        /// </summary>
        public void ClearText()
        {
            Text = "";
        }

        protected override void OnTextChanged()
        {
            // Text has been modified by the user: Reset scrolling offsets
            if (!textHasBeenAppended)
                ResetDisplayingText();

            textHasBeenAppended = false;
        }

        private void ResetDisplayingText()
        {
            textToDisplay = "";
            nextLetterIndex = 0;
            ScrollingOffset = IsArrangeValid? ActualWidth: float.PositiveInfinity;
            AccumulatedWidth = 0;
        }

        public override string TextToDisplay => textToDisplay;

        /// <summary>
        ///   Calculates the width of the text to display, in virtual pixels.
        /// </summary>
        /// <returns>The size of the text, in virtual pixels</returns>
        private float CalculateTextToDisplayWidth()
        {
            return CalculateTextSize(TextToDisplay).X;
        }

        protected override void Update(GameTime time)
        {
            base.Update(time);

            if (!IsEnabled)
                return;

            UpdateAndAdjustDisplayText(time);
        }

        private void UpdateAndAdjustDisplayText(GameTime time = null)
        {
            if (string.IsNullOrEmpty(Text) || Font is null || CalculateTextSize(Text).X <= float.Epsilon)
                return;

            var elapsedSeconds = time != null ? (float) time.Elapsed.TotalSeconds : 0f;

            // Calculate the shift offset
            var nextOffsetShift = elapsedSeconds * ScrollingSpeed - ScrollingOffset;

            // Calculate the size of the next TextToDisplay required
            var sizeNextTextToDisplay = nextOffsetShift + elementWidth;

            // Append characters to TextToDisplay so that it measures more than 'sizeNextTextToDisplay'
            var textToDisplayWidth = CalculateTextToDisplayWidth();
            while (textToDisplayWidth < sizeNextTextToDisplay && nextLetterIndex < Text.Length)
            {
                textToDisplay += Text[nextLetterIndex++];

                var addedCharacterWidth = CalculateTextToDisplayWidth() - textToDisplayWidth;
                AccumulatedWidth += addedCharacterWidth;

                if (RepeatText && nextLetterIndex >= Text.Length)
                    nextLetterIndex = 0;

                textToDisplayWidth += addedCharacterWidth;
            }

            // Check if all the string has finished to scroll, if clear the message
            if (CalculateTextSize(textToDisplay).X < nextOffsetShift)
                textToDisplay = "";

            // Remove characters at the beginning of TextToDisplay as long as possible
            var fontSize = new Vector2(ActualTextSize, ActualTextSize);
            while (textToDisplay.Length > 1 && Font.MeasureString(textToDisplay, ref fontSize, 1).X < nextOffsetShift)
            {
                nextOffsetShift -= Font.MeasureString(textToDisplay, ref fontSize, 1).X;
                textToDisplay = textToDisplay.Substring(1);
            }

            // Update the scroll offset of the viewer
            ScrollingOffset = -nextOffsetShift;
        }

        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            return MeasureSize();
        }

        protected override Vector3 ArrangeOverride(Vector3 finalSizeWithoutMargins)
        {
            elementWidth = finalSizeWithoutMargins.X;

            ScrollingOffset = Math.Min(elementWidth, ScrollingOffset);

            UpdateAndAdjustDisplayText();

            return base.ArrangeOverride(finalSizeWithoutMargins);
        }

        /// <summary>
        ///   Measures the size of the <see cref="ScrollingText"/> element.
        /// </summary>
        /// <returns>The size of the element.</returns>
        public Vector3 MeasureSize()
        {
            if (Font == null)
                return Vector3.Zero;

            return new Vector3(Font.MeasureString(new string('A', (int)DesiredCharacterNumber)), 0);
        }
    }
}
