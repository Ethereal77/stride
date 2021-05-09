// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Annotations;

namespace Stride.Core.Diagnostics
{
    /// <summary>
    ///   Contains data about the progress of an operation to be used by a progress-reporting event.
    /// </summary>
    public class ProgressStatusEventArgs : EventArgs
    {
        /// <summary>
        ///   Gets the message associated with the progress.
        /// </summary>
        /// <value>The message associated with the progress.</value>
        public string Message { get; }

        /// <summary>
        ///   Gets a value indicating whether this instance has known steps
        ///   (<see cref="CurrentStep"/> and  <see cref="StepCount"/> are valid).
        /// </summary>
        /// <value><c>true</c> if this instance has known steps; otherwise, <c>false</c>.</value>
        public bool HasKnownSteps { get; }

        /// <summary>
        /// Gets the index of the current step.
        /// </summary>
        /// <value>The index of the current step.</value>
        /// <remarks>
        ///   This property returns the current step and gives indication about how much still needs to be processed as
        ///   indicated by the property <see cref="StepCount"/>.
        /// </remarks>
        public int CurrentStep { get; }

        /// <summary>
        ///   Gets the total number of steps.
        /// </summary>
        /// <value>The step count.</value>
        /// <remarks>
        ///   This property provides an estimation of the duration of an operation in terms of "steps".
        ///   The <see cref="CurrentStep"/> property returns the current step and gives indication about
        ///   how much still needs to be processed.
        /// </remarks>
        public int StepCount { get; }


        /// <summary>
        ///   Initializes a new instance of the <see cref="ProgressStatusEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is a <c>null</c> reference.</exception>
        public ProgressStatusEventArgs([NotNull] string message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            Message = message;

            StepCount = 1;
            HasKnownSteps = false;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ProgressStatusEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="currentStep">The current step.</param>
        /// <param name="stepCount">The step count.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="currentStep"/> must be greater or equal than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="stepCount"/> must be greater or equal than 1.</exception>
        public ProgressStatusEventArgs([NotNull] string message, int currentStep, int stepCount)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            if (currentStep < 0)
                throw new ArgumentOutOfRangeException(nameof(currentStep), "Expecting value >= 0");
            if (stepCount < 1)
                throw new ArgumentOutOfRangeException(nameof(stepCount), "Expecting value >= 1");

            Message = message;
            CurrentStep = currentStep;
            StepCount = stepCount;

            HasKnownSteps = true;
        }
    }
}
