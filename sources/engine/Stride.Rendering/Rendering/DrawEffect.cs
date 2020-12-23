// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;
using Stride.Rendering.Images;

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents the base class in charge of applying and drawing an effect.
    /// </summary>
    [DataContract]
    public abstract class DrawEffect : RendererBase
    {
        private ImageScaler scaler;

        /// <summary>
        ///   Initializes a new instance of the <see cref="DrawEffect"/> class.
        /// </summary>
        /// <param name="name">Name to set for this renderer. Specify <c>null</c> to use the type name automatically.</param>
        protected DrawEffect(string name) : base(name)
        {
            Enabled = true;
            Parameters = new ParameterCollection();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="DrawEffect"/> class.
        /// </summary>
        protected DrawEffect() : this(null) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="DrawEffect"/> class.
        /// </summary>
        /// <param name="context">The rendering context.</param>
        /// <param name="name">Name to set for this renderer. Specify <c>null</c> to use the type name automatically.</param>
        protected DrawEffect(RenderContext context, string name = null) : this(name)
        {
            Initialize(context);
        }


        [DataMemberIgnore]
        public SamplingPattern SamplingPattern { get; set; } = SamplingPattern.Linear;

        /// <summary>
        ///   Gets the effect parameters.
        /// </summary>
        /// <value>The effect parameters.</value>
        [DataMemberIgnore]
        public ParameterCollection Parameters { get; protected set; }

        /// <summary>
        ///   Gets a shared <see cref="ImageScaler"/>.
        /// </summary>
        [DataMemberIgnore]
        protected ImageScaler Scaler
        {
            get
            {
                // TODO
                // return scaler ?? (scaler = Context.GetSharedEffect<ImageScaler>());
                if (scaler is null || SamplingPattern != scaler.FilterPattern)
                {
                    scaler?.Dispose();

                    // Scaler typically get random input and output from allocator, so we delay the set of render targets to avoid potential D3D11 warnings
                    scaler = new ImageScaler(SamplingPattern, delaySetRenderTargets: true);
                    scaler.Initialize(Context);
                }
                return scaler;
            }
        }

        /// <summary>
        ///   Resets the state of this effect.
        /// </summary>
        public virtual void Reset()
        {
            SetDefaultParameters();
        }

        /// <summary>
        ///   Sets the default parameters (called at constructor time and if <see cref="Reset"/> is called).
        /// </summary>
        protected virtual void SetDefaultParameters()
        {
            SamplingPattern = SamplingPattern.Linear;
        }

        /// <summary>
        ///   Draws a full screen quad using iterating on each pass of this effect.
        /// </summary>
        /// <param name="context">The rendering context.</param>
        /// <param name="name">A name to use for this specific drawing operation.</param>
        public void Draw(RenderDrawContext context, string name)
        {
            var previousDebugName = Name;

            if (name != null)
                Name = name;

            Draw(context);

            Name = previousDebugName;
        }

        /// <summary>
        ///   Draws a full screen quad using iterating on each pass of this effect.
        /// </summary>
        /// <param name="context">The rendering context.</param>
        public void Draw(RenderDrawContext context, string nameFormat, params object[] args)
        {
            // TODO: This is alocating a string, we should try to not allocate here.
            Draw(context, name: string.Format(nameFormat, args));
        }

        /// <summary>
        ///   Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString() => $"Effect {Name}";
    }
}
