// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    ///   Represents an object that can create and manage GPU resources (like <see cref="Buffer"/>s, <see cref="Texture"/>s,
    ///   states, or shaders), and manipulate <see cref="CommandList"/>s.
    /// </summary>
    public partial class GraphicsDevice : ComponentBase
    {
        internal readonly Dictionary<PipelineStateDescriptionWithHash, PipelineState> CachedPipelineStates = new Dictionary<PipelineStateDescriptionWithHash, PipelineState>();

        internal readonly Dictionary<SamplerStateDescription, SamplerState> CachedSamplerStates = new Dictionary<SamplerStateDescription, SamplerState>();

        /// <summary>
        ///     Gets the features supported by this graphics device.
        /// </summary>
        public GraphicsDeviceFeatures Features;

        internal HashSet<GraphicsResourceBase> Resources = new HashSet<GraphicsResourceBase>();

        internal readonly bool NeedWorkAroundForUpdateSubResource;
        internal Effect CurrentEffect;

        private readonly List<IDisposable> sharedDataToDispose = new List<IDisposable>();
        private readonly Dictionary<object, IDisposable> sharedDataPerDevice;
        private readonly Dictionary<object, IDisposable> sharedDataPerDeviceContext = new Dictionary<object, IDisposable>();
        private GraphicsPresenter presenter;

        internal PipelineState DefaultPipelineState;

        internal CommandList InternalMainCommandList;

        internal PrimitiveQuad PrimitiveQuad;
        private ColorSpace colorSpace;

        public uint FrameTriangleCount;
        public uint FrameDrawCalls;
        private long bufferMemory;
        private long textureMemory;

        /// <summary>
        ///   Gets the GPU memory currently allocated to buffers, in bytes.
        /// </summary>
        public long BuffersMemory => Interlocked.Read(ref bufferMemory);

        /// <summary>
        ///   Gets the GPU memory currently allocated to texture, in bytes.
        /// </summary>
        public long TextureMemory => Interlocked.Read(ref textureMemory);

        /// <summary>
        ///   Gets the type of the platform this graphics device is using.
        /// </summary>
        public static GraphicsPlatform Platform => GraphicPlatform;

        public string RendererName => GetRendererName();

        /// <summary>
        ///   Initializes a new instance of the <see cref="GraphicsDevice" /> class.
        /// </summary>
        /// <param name="adapter">The graphics adapter.</param>
        /// <param name="profile">The graphics profile.</param>
        /// <param name="deviceCreationFlags">The device creation flags.</param>
        /// <param name="windowHandle">The window handle.</param>
        protected GraphicsDevice(GraphicsAdapter adapter, GraphicsProfile[] profile, DeviceCreationFlags deviceCreationFlags, WindowHandle windowHandle)
        {
            // Create shared data
            sharedDataPerDevice = new Dictionary<object, IDisposable>();

            Recreate(adapter, profile, deviceCreationFlags, windowHandle);

            // Helpers
            PrimitiveQuad = new PrimitiveQuad(this);
        }

        public void Recreate(GraphicsAdapter adapter, GraphicsProfile[] graphicsProfiles, DeviceCreationFlags deviceCreationFlags, WindowHandle windowHandle)
        {
            if (adapter is null)
                throw new ArgumentNullException(nameof(adapter));
            if (graphicsProfiles is null)
                throw new ArgumentNullException(nameof(graphicsProfiles));

            Adapter = adapter;
            IsDebugMode = (deviceCreationFlags & DeviceCreationFlags.Debug) != 0;

            // Default fallback
            if (graphicsProfiles.Length == 0)
                graphicsProfiles = new[] { GraphicsProfile.Level_11_2, GraphicsProfile.Level_11_1, GraphicsProfile.Level_11_0 };

            // Initialize this instance
            InitializePlatformDevice(graphicsProfiles, deviceCreationFlags, windowHandle);

            // Create a new graphics device
            Features = new GraphicsDeviceFeatures(this);

            SamplerStates = new SamplerStateFactory(this);

            var defaultPipelineStateDescription = new PipelineStateDescription();
            defaultPipelineStateDescription.SetDefaults();
            AdjustDefaultPipelineStateDescription(ref defaultPipelineStateDescription);
            DefaultPipelineState = PipelineState.New(this, ref defaultPipelineStateDescription);

            InitializePostFeatures();
        }

        protected override void Destroy()
        {
            // Clear shared data
            for (int index = sharedDataToDispose.Count - 1; index >= 0; index--)
                sharedDataToDispose[index].Dispose();
            sharedDataPerDevice.Clear();
            sharedDataPerDeviceContext.Clear();

            SamplerStates.Dispose();
            SamplerStates = null;

            DefaultPipelineState.Dispose();

            PrimitiveQuad.Dispose();

            // Notify listeners
            Disposing?.Invoke(this, EventArgs.Empty);

            // Destroy resources
            lock (Resources)
            {
                foreach (var resource in Resources)
                {
                    // Destroy leftover resources (note: should not happen if ResumeManager.OnDestroyed has properly been called)
                    if (resource.LifetimeState != GraphicsResourceLifetimeState.Destroyed)
                    {
                        resource.OnDestroyed();
                        resource.LifetimeState = GraphicsResourceLifetimeState.Destroyed;
                    }

                    // Remove Reload code in case it was preventing objects from being GC
                    resource.Reload = null;
                }
                Resources.Clear();
            }

            DestroyPlatformDevice();

            base.Destroy();
        }

        /// <summary>
        ///   Occurs while this component is disposing and before it is disposed.
        /// </summary>
        public event EventHandler<EventArgs> Disposing;

        /// <summary>
        ///   Delegate called to create shareable data.
        /// </summary>
        /// <typeparam name="T">Type of the data to create.</typeparam>
        /// <returns>A new instance of the data to share.</returns>
        /// <remarks>
        ///   Because this method is being called from a lock region, this method should not be time consuming.
        /// </remarks>
        public delegate T CreateSharedData<out T>(GraphicsDevice device) where T : class, IDisposable;

        /// <summary>
        ///   Gets the adapter this instance is attached to.
        /// </summary>
        public GraphicsAdapter Adapter { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether this device is in debug mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is in debug mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsDebugMode { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether this device allows for concurrent building and deferred submission
        ///   of <see cref="CommandList"/>s.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deferred; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeferred { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether this instance supports GPU markers and profiling.
        /// </summary>
        public bool IsProfilingSupported { get; private set; }

        /// <summary>
        ///   Gets the default color space.
        /// </summary>
        /// <value>The default color space.</value>
        public ColorSpace ColorSpace
        {
            get => Features.HasSRgb ? colorSpace : ColorSpace.Gamma;
            set => colorSpace = value;
        }

        /// <summary>
        ///   Gets or sets the current presenter used to display the frame.
        /// </summary>
        /// <value>The current presenter.</value>
        public virtual GraphicsPresenter Presenter
        {
            get => presenter;
            set => presenter = value;
        }

        /// <summary>
        ///   Gets the <see cref="SamplerStateFactory" /> factory.
        /// </summary>
        /// <value>
        ///   A <see cref="SamplerStateFactory"/> that can be used to get one of the default <see cref="SamplerState"/>s
        ///   or create a new one.
        /// </value>
        public SamplerStateFactory SamplerStates { get; private set; }

        /// <summary>
        ///   Gets the index of the thread this device is created in.
        /// </summary>
        /// <value>The index of the <see cref="Thread"/> this device is created in.</value>
        public int ThreadIndex { get; internal set; }

        /// <summary>
        ///   Gets the shader profile.
        /// </summary>
        /// <value>The shader profile.</value>
        internal GraphicsProfile? ShaderProfile { get; set; }


        /// <summary>
        ///   Initializes a new instance of the <see cref="GraphicsDevice" /> class.
        /// </summary>
        /// <param name="creationFlags">The creation flags.</param>
        /// <param name="graphicsProfiles">The graphics profiles.</param>
        /// <returns>The created <see cref="GraphicsDevice"/>.</returns>
        public static GraphicsDevice New(DeviceCreationFlags creationFlags = DeviceCreationFlags.None, params GraphicsProfile[] graphicsProfiles)
        {
            return New(GraphicsAdapterFactory.Default, creationFlags, graphicsProfiles);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GraphicsDevice" /> class.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="creationFlags">The creation flags.</param>
        /// <param name="graphicsProfiles">The graphics profiles.</param>
        /// <returns>The created <see cref="GraphicsDevice"/>.</returns>
        public static GraphicsDevice New(GraphicsAdapter adapter, DeviceCreationFlags creationFlags = DeviceCreationFlags.None, params GraphicsProfile[] graphicsProfiles)
        {
            return new GraphicsDevice(adapter ?? GraphicsAdapterFactory.Default, graphicsProfiles, creationFlags, null);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GraphicsDevice" /> class.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="creationFlags">The creation flags.</param>
        /// <param name="windowHandle">The window handle.</param>
        /// <param name="graphicsProfiles">The graphics profiles.</param>
        /// <returns>The created <see cref="GraphicsDevice"/>.</returns>
        public static GraphicsDevice New(GraphicsAdapter adapter, DeviceCreationFlags creationFlags = DeviceCreationFlags.None, WindowHandle windowHandle = null, params GraphicsProfile[] graphicsProfiles)
        {
            return new GraphicsDevice(adapter ?? GraphicsAdapterFactory.Default, graphicsProfiles, creationFlags, windowHandle);
        }


        /// <summary>
        ///   Gets a shared data object for this device.
        /// </summary>
        /// <typeparam name="T">Type of the shared data to get or create.</typeparam>
        /// <param name="type">Type of the data to share.</param>
        /// <param name="key">The key of the shared data.</param>
        /// <param name="sharedDataCreator"> A delegate to create the shared data if it is not present.</param>
        /// <returns>
        ///   The shared data object. It will be disposed by this <see cref="GraphicsDevice" /> instance.
        /// </returns>
        public T GetOrCreateSharedData<T>(GraphicsDeviceSharedDataType type, object key, CreateSharedData<T> sharedDataCreator) where T : class, IDisposable
        {
            Dictionary<object, IDisposable> dictionary = (type == GraphicsDeviceSharedDataType.PerDevice) ? sharedDataPerDevice : sharedDataPerDeviceContext;

            lock (dictionary)
            {
                if (!dictionary.TryGetValue(key, out IDisposable localValue))
                {
                    localValue = sharedDataCreator(this);
                    if (localValue is null)
                        return null;

                    sharedDataToDispose.Add(localValue);
                    dictionary.Add(key, localValue);
                }

                return (T) localValue;
            }
        }

        internal void RegisterTextureMemoryUsage(long memoryChange)
        {
            Interlocked.Add(ref textureMemory, memoryChange);
        }

        internal void RegisterBufferMemoryUsage(long memoryChange)
        {
            Interlocked.Add(ref bufferMemory, memoryChange);
        }
    }
}
