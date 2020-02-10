// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2014 OxyPlot contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using Xenko.Core.Annotations;
using Xenko.Core.Presentation.Extensions;
using Xenko.Core.Presentation.Internal;

using MathUtil = Xenko.Core.Mathematics.MathUtil;

namespace Xenko.Core.Presentation.Controls
{
    [TemplatePart(Name = HorizontalLinePartName, Type = typeof(Line))]
    [TemplatePart(Name = VerticalLinePartName, Type = typeof(Line))]
    public class TrackerControl : Control
    {
        /// <summary>
        /// The name of the part for the horizontal line.
        /// </summary>
        private const string HorizontalLinePartName = "PART_HorizontalLine";

        /// <summary>
        /// The name of the part for  the vertical line.
        /// </summary>
        private const string VerticalLinePartName = "PART_VerticalLine";

        /// <summary>
        /// Identifies the <see cref="HorizontalLineVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalLineVisibilityProperty =
            DependencyProperty.Register(nameof(HorizontalLineVisibility), typeof(Visibility), typeof(TrackerControl), new PropertyMetadata(VisibilityBoxes.VisibleBox));

        /// <summary>
        /// Identifies the <see cref="LineExtents"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineExtentsProperty =
            DependencyProperty.Register(nameof(LineExtents), typeof(Rect), typeof(TrackerControl));

        /// <summary>
        /// Identifies the <see cref="LineStroke"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineStrokeProperty =
            DependencyProperty.Register(nameof(LineStroke), typeof(Brush), typeof(TrackerControl));

        /// <summary>
        /// Identifies the <see cref="LineThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineThicknessProperty =
            DependencyProperty.Register(nameof(LineThickness), typeof(Thickness), typeof(TrackerControl));

        /// <summary>
        /// Identifies the <see cref="Position"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(nameof(Position), typeof(Point), typeof(TrackerControl), new PropertyMetadata(new Point(), OnPositionChanged));

        /// <summary>
        /// Identifies the <see cref="TrackMouse"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackMouseProperty =
            DependencyProperty.Register(nameof(TrackMouse), typeof(bool), typeof(TrackerControl), new PropertyMetadata(BooleanBoxes.FalseBox, OnTrackMouseChanged));

        /// <summary>
        /// Identifies the <see cref="VerticalLineVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalLineVisibilityProperty =
            DependencyProperty.Register(nameof(VerticalLineVisibility), typeof(Visibility), typeof(TrackerControl), new PropertyMetadata(VisibilityBoxes.VisibleBox));

        private Line horizontalLine;
        private Line verticalLine;
        private FrameworkElement parent;

        static TrackerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TrackerControl), new FrameworkPropertyMetadata(typeof(TrackerControl)));
        }

        public Visibility HorizontalLineVisibility { get { return (Visibility)GetValue(HorizontalLineVisibilityProperty); }  set { SetValue(HorizontalLineVisibilityProperty, value); } }
        
        public Rect LineExtents { get { return (Rect)GetValue(LineExtentsProperty); }  set { SetValue(LineExtentsProperty, value); } }

        public Brush LineStroke { get { return (Brush)GetValue(LineStrokeProperty); }  set { SetValue(LineStrokeProperty, value); } }

        public Thickness LineThickness { get { return (Thickness)GetValue(LineThicknessProperty); } set { SetValue(LineThicknessProperty, value); } }

        public Point Position { get { return (Point)GetValue(PositionProperty); } set { SetValue(PositionProperty, value); } }

        public bool TrackMouse { get { return (bool)GetValue(TrackMouseProperty); } set { SetValue(TrackMouseProperty, value.Box()); } }
        
        public Visibility VerticalLineVisibility { get { return (Visibility)GetValue(VerticalLineVisibilityProperty); }  set { SetValue(VerticalLineVisibilityProperty, value); } }

        private static void OnPositionChanged([NotNull] DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((TrackerControl)sender).OnPositionChanged();
        }

        private static void OnTrackMouseChanged([NotNull] DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((TrackerControl)sender).OnTrackMouseChanged(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            horizontalLine = GetTemplateChild(HorizontalLinePartName) as Line;
            verticalLine = GetTemplateChild(VerticalLinePartName) as Line;

            if (parent != null && TrackMouse)
                parent.MouseMove -= OnMouseMove;
            parent = this.FindVisualParentOfType<FrameworkElement>();
            if (TrackMouse)
                parent.MouseMove += OnMouseMove;
        }

        private void OnMouseMove(object sender, [NotNull] MouseEventArgs e)
        {
            if (!TrackMouse)
                return;
            Position = e.GetPosition(this);
        }
        
        private void OnPositionChanged()
        {
            UpdatePositionAndBorder();
        }

        private void OnTrackMouseChanged(DependencyPropertyChangedEventArgs e)
        {
            if (parent == null)
                return;

            if ((bool)e.NewValue)
                parent.MouseMove += OnMouseMove;
            else
                parent.MouseMove -= OnMouseMove;
        }

        private void UpdatePositionAndBorder()
        {
            if (parent == null)
                return;

            var width = parent.ActualWidth;
            var height = parent.ActualHeight;
            var lineExtents = LineExtents;
            var pos = Position;

            if (horizontalLine != null)
            {
                if (LineExtents.Width > 0)
                {
                    horizontalLine.X1 = lineExtents.Left;
                    horizontalLine.X2 = lineExtents.Right;
                    pos.Y = MathUtil.Clamp(pos.Y, lineExtents.Top, lineExtents.Bottom);
                }
                else
                {
                    horizontalLine.X1 = 0;
                    horizontalLine.X2 = width;
                }

                horizontalLine.Y1 = pos.Y;
                horizontalLine.Y2 = pos.Y;
            }

            if (verticalLine != null)
            {
                if (LineExtents.Width > 0)
                {
                    verticalLine.Y1 = lineExtents.Top;
                    verticalLine.Y2 = lineExtents.Bottom;
                    pos.X = MathUtil.Clamp(pos.X, lineExtents.Left, lineExtents.Right);
                }
                else
                {
                    verticalLine.Y1 = 0;
                    verticalLine.Y2 = height;
                }
                verticalLine.X1 = pos.X;
                verticalLine.X2 = pos.X;
            }
        }
    }
}
