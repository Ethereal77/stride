// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using Stride.Core;
using Stride.Core.Collections;
using Stride.Core.Diagnostics;
using Stride.Core.Mathematics;
using Stride.Games;

namespace Stride.Input
{
    /// <summary>
    ///   Represents a class that manages the collecting of input from connected input devices.
    ///   Also provides some convenience functions for most commonly used devices.
    /// </summary>
    public partial class InputManager : ComponentBase
    {
        // This is used in some mobile platform for accelerometer stuff
        internal const float G = 9.81f;
        internal const float DesiredSensorUpdateRate = 60;

        /// <summary>
        ///   Deadzone amount applied to all game controller axis.
        /// </summary>
        public static float GameControllerAxisDeadZone = 0.05f;

        internal static Logger Logger = GlobalLogger.GetLogger("Input");

        private readonly List<IInputDevice> devices = new List<IInputDevice>();

        private readonly List<InputEvent> events = new List<InputEvent>();
        private readonly List<GestureEvent> currentGestureEvents = new List<GestureEvent>();

        private readonly Dictionary<GestureConfig, GestureRecognizer> gestureConfigToRecognizer = new Dictionary<GestureConfig, GestureRecognizer>();
        private readonly List<Dictionary<object, float>> virtualButtonValues = new List<Dictionary<object, float>>();

        // Mapping of device GUID to device
        private readonly Dictionary<Guid, IInputDevice> devicesById = new Dictionary<Guid, IInputDevice>();

        // List mapping GamePad index to the GUID of the device
        private readonly List<List<IGamePadDevice>> gamePadRequestedIndex = new List<List<IGamePadDevice>>();

        private readonly List<IKeyboardDevice> keyboards = new List<IKeyboardDevice>();
        private readonly List<IPointerDevice> pointers = new List<IPointerDevice>();
        private readonly List<IGameControllerDevice> gameControllers = new List<IGameControllerDevice>();
        private readonly List<IGamePadDevice> gamePads = new List<IGamePadDevice>();
        private readonly List<ISensorDevice> sensors = new List<ISensorDevice>();

        private readonly Dictionary<Type, IInputEventRouter> eventRouters = new Dictionary<Type, IInputEventRouter>();

        private GameContext gameContext;

        private Dictionary<IInputSource, EventHandler<TrackingCollectionChangedEventArgs>> devicesCollectionChangedActions = new Dictionary<IInputSource, EventHandler<TrackingCollectionChangedEventArgs>>();

#if STRIDE_INPUT_RAWINPUT
        private bool rawInputEnabled = false;
#endif

        /// <summary>
        ///   Initializes a new instance of the <see cref="InputManager"/> class.
        /// </summary>
        /// <param name="gameContext">The game context.</param>
        public InputManager()
        {
            Gestures = new TrackingCollection<GestureConfig>();
            Gestures.CollectionChanged += GesturesOnCollectionChanged;

            Sources = new TrackingCollection<IInputSource>();
            Sources.CollectionChanged += SourcesOnCollectionChanged;
        }

        /// <summary>
        ///   Gets or sets the configuration for virtual buttons.
        /// </summary>
        /// <value>The current virtual buttons bindings.</value>
        public VirtualButtonConfigSet VirtualButtonConfigSet { get; set; }

        /// <summary>
        ///   List of the gestures to recognize.
        /// </summary>
        public TrackingCollection<GestureConfig> Gestures { get; }

        /// <summary>
        ///   Gets the registered input sources.
        /// </summary>
        public TrackingCollection<IInputSource> Sources { get; }

        /// <summary>
        ///   Gets the accelerometer sensor. The accelerometer measures all the acceleration forces
        ///   applied on the device.
        /// </summary>
        public IAccelerometerSensor Accelerometer { get; private set; }

        /// <summary>
        ///   Gets the compass sensor. The compass measures the angle between the device top and the north.
        /// </summary>
        public ICompassSensor Compass { get; private set; }

        /// <summary>
        ///   Gets the gyroscope sensor. The gyroscope measures the rotation speed of the device.
        /// </summary>
        public IGyroscopeSensor Gyroscope { get; private set; }

        /// <summary>
        ///   Gets the user acceleration sensor. The user acceleration sensor measures the acceleration produced by
        ///   the user on the device (no gravity).
        /// </summary>
        public IUserAccelerationSensor UserAcceleration { get; private set; }

        /// <summary>
        ///   Gets the gravity sensor. The gravity sensor measures the gravity vector applied to
        ///   the device.
        /// </summary>
        public IGravitySensor Gravity { get; private set; }

        /// <summary>
        ///   Gets the orientation sensor. The orientation sensor measures the orientation of the device in the world.
        /// </summary>
        public IOrientationSensor Orientation { get; private set; }

        /// <summary>
        ///   Gets a value indicating if the mouse position is currently locked.
        /// </summary>
        public bool IsMousePositionLocked => HasMouse && Mouse.IsPositionLocked;

        /// <summary>
        ///   Gets all input events that have been registered since the last frame.
        /// </summary>
        public IReadOnlyList<InputEvent> Events => events;

        /// <summary>
        ///   Gets the collection of gesture events that have been registered since the previous updates.
        /// </summary>
        /// <value>The gesture events.</value>
        public IReadOnlyList<GestureEvent> GestureEvents => currentGestureEvents;

        /// <summary>
        ///   Gets a value indicating whether a pointer device is available.
        /// </summary>
        /// <value><c>true</c> if pointer devices are available; otherwise, <c>false</c>.</value>
        public bool HasPointer { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether a mouse is available.
        /// </summary>
        /// <value><c>true</c> if a mouse is available; otherwise, <c>false</c>.</value>
        public bool HasMouse { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether a keyboard is available.
        /// </summary>
        /// <value><c>true</c> if a keyboard is available; otherwise, <c>false</c>.</value>
        public bool HasKeyboard { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether game controllers are available.
        /// </summary>
        /// <value><c>true</c> if game controllers are available; otherwise, <c>false</c>.</value>
        public bool HasGameController { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether gamepads are available.
        /// </summary>
        /// <value><c>true</c> if gamepads are available; otherwise, <c>false</c>.</value>
        public bool HasGamePad { get; private set; }

        /// <summary>
        ///   Gets the number of game controllers connected.
        /// </summary>
        /// <value>The number of game controllers connected.</value>
        public int GameControllerCount { get; private set; }

        /// <summary>
        ///   Gets the number of gamepads connected.
        /// </summary>
        /// <value>The number of gamepads connected.</value>
        public int GamePadCount { get; private set; }

        /// <summary>
        ///   Gets the first pointer device.
        /// </summary>
        /// <value>
        ///   The first pointer device, or <c>null</c> if there is none.
        /// </value>
        public IPointerDevice Pointer { get; private set; }

        /// <summary>
        ///   Gets the first mouse pointer device.
        /// </summary>
        /// <value>
        ///   The first mouse pointer device, or <c>null</c> if there is none.
        /// </value>
        public IMouseDevice Mouse { get; private set; }

        /// <summary>
        ///   Gets the first keyboard device.
        /// </summary>
        /// <value>
        ///   The first keyboard device, or <c>null</c> if there is none.
        /// </value>
        public IKeyboardDevice Keyboard { get; private set; }

        /// <summary>
        ///   Gets the first device that supports text input.
        /// </summary>
        /// <value>
        ///   The first device that supports text input, or <c>null</c> if there is none.
        /// </value>
        public ITextInputDevice TextInput { get; private set; }

        /// <summary>
        ///   Gets the first gamepad that was added to the device.
        /// </summary>
        public IGamePadDevice DefaultGamePad { get; private set; }

        /// <summary>
        ///   Gets the collection of connected game controllers.
        /// </summary>
        public IReadOnlyList<IGameControllerDevice> GameControllers => gameControllers;

        /// <summary>
        ///   Gets the collection of connected gamepads.
        /// </summary>
        public IReadOnlyList<IGamePadDevice> GamePads => gamePads;

        /// <summary>
        ///   Gets the collection of connected pointing devices (mouses, touchpads, etc).
        /// </summary>
        public IReadOnlyList<IPointerDevice> Pointers => pointers;

        /// <summary>
        ///   Gets the collection of connected keyboard inputs.
        /// </summary>
        public IReadOnlyList<IKeyboardDevice> Keyboards => keyboards;

        /// <summary>
        ///   Gets the collection of connected sensor devices.
        /// </summary>
        public IReadOnlyList<ISensorDevice> Sensors => sensors;

        /// <summary>
        ///   Gets a value indicating whether Raw Input should be used on Windows.
        /// </summary>
        public bool UseRawInput
        {
#if (STRIDE_UI_WINFORMS || STRIDE_UI_WPF) && STRIDE_INPUT_RAWINPUT
            get => rawInputEnabled;

            set
            {
                InputSourceWindowsRawInput rawInputSource = Sources.OfType<InputSourceWindowsRawInput>().FirstOrDefault();

                if (value)
                {
                    if (rawInputSource is null && gameContext is GameContextWinforms gameContextWinforms)
                    {
                        rawInputSource = new InputSourceWindowsRawInput(gameContextWinforms.Control);
                        Sources.Add(rawInputSource);
                    }
                }
                else
                {
                    // Disable by removing the raw input source
                    if (rawInputSource != null)
                    {
                        Sources.Remove(rawInputSource);
                    }
                }
                rawInputEnabled = value;
            }
#else
            get => false;
            set { }
#endif
        }

        /// <summary>
        ///   Raised before new input is sent to their respective event listeners.
        /// </summary>
        public event EventHandler<InputPreUpdateEventArgs> PreUpdateInput;

        /// <summary>
        ///   Raised when a device was removed from the system.
        /// </summary>
        public event EventHandler<DeviceChangedEventArgs> DeviceRemoved;

        /// <summary>
        ///   Raised when a device was added to the system.
        /// </summary>
        public event EventHandler<DeviceChangedEventArgs> DeviceAdded;

        /// <summary>
        ///   Transforms mouse and pointer event positions to sub rectangles.
        /// </summary>
        /// <param name="fromSize">The size of the source rectangle.</param>
        /// <param name="destinationRectangle">The destination viewport rectangle.</param>
        /// <param name="screenCoordinates">The normalized screen coordinates.</param>
        /// <returns>
        ///   The coordinates transformed to be relative to the <paramref name="destinationRectangle"/> and
        ///   proportional to its size.
        /// </returns>
        public static Vector2 TransformPosition(Size2F fromSize, RectangleF destinationRectangle, Vector2 screenCoordinates)
        {
            return new Vector2(
                (screenCoordinates.X * fromSize.Width - destinationRectangle.X) / destinationRectangle.Width,
                (screenCoordinates.Y * fromSize.Height - destinationRectangle.Y) / destinationRectangle.Height);
        }

        public void Initialize(GameContext gameContext)
        {
            this.gameContext = gameContext ?? throw new ArgumentNullException(nameof(gameContext));

            AddSources();

            // After adding initial devices, reassign gamepad Ids.
            // This creates a beter index assignment in the case where you have both an Xbox Controller and another controller at startup
            var sortedGamePads = GamePads.OrderBy(x => x.CanChangeIndex);

            foreach (var gamePad in sortedGamePads)
            {
                if (gamePad.CanChangeIndex)
                    gamePad.Index = GetFreeGamePadIndex(gamePad);
            }

            // Register event types
            RegisterEventType<KeyEvent>();
            RegisterEventType<TextInputEvent>();
            RegisterEventType<MouseButtonEvent>();
            RegisterEventType<MouseWheelEvent>();
            RegisterEventType<PointerEvent>();
            RegisterEventType<GameControllerButtonEvent>();
            RegisterEventType<GameControllerAxisEvent>();
            RegisterEventType<GameControllerDirectionEvent>();
            RegisterEventType<GamePadButtonEvent>();
            RegisterEventType<GamePadAxisEvent>();

            // Add global input state to listen for input events
            AddListener(this);
        }

        /// <summary>
        ///   Locks the mouse position and hides it until the next call to <see cref="UnlockMousePosition"/>.
        /// </summary>
        /// <param name="forceCenter">A value indicating whether to force the mouse cursor position to stay in the center of the client window.</param>
        /// <remarks>
        ///   This function has no effects on devices that does not have mouse.
        /// </remarks>
        public void LockMousePosition(bool forceCenter = false)
        {
            // Lock primary mouse
            if (HasMouse)
                Mouse.LockPosition(forceCenter);
        }

        /// <summary>
        ///   Unlocks the mouse position previously locked by calling <see cref="LockMousePosition"/> and restores
        ///   the mouse visibility.
        /// </summary>
        /// <remarks>
        ///   This function has no effects on devices that does not have mouse.
        /// </remarks>
        public void UnlockMousePosition()
        {
            if (HasMouse)
                Mouse.UnlockPosition();
        }

        /// <summary>
        ///   Gets the first gamepad with a specific index.
        /// </summary>
        /// <param name="gamePadIndex">The index of the gamepad.</param>
        /// <returns>The gamepad, or <c>null</c> if no gamepad is found with this index.</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="gamePadIndex"/> is less than 0.</exception>
        public IGamePadDevice GetGamePadByIndex(int gamePadIndex)
        {
            if (gamePadIndex < 0)
                throw new IndexOutOfRangeException(nameof(gamePadIndex));
            if (gamePadIndex >= gamePadRequestedIndex.Count)
                return null;

            return gamePadRequestedIndex[gamePadIndex].FirstOrDefault();
        }

        /// <summary>
        ///   Gets all the gamepads with a specific index.
        /// </summary>
        /// <param name="gamePadIndex">The index of the gamepad.</param>
        /// <returns>The gamepads, or <c>null</c> if no gamepad is found with this index.</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="gamePadIndex"/> is less than 0.</exception>
        public IEnumerable<IGamePadDevice> GetGamePadsByIndex(int gamePadIndex)
        {
            if (gamePadIndex < 0)
                throw new IndexOutOfRangeException(nameof(gamePadIndex));
            if (gamePadIndex >= gamePadRequestedIndex.Count)
                return null;

            return gamePadRequestedIndex[gamePadIndex];
        }

        /// <summary>
        ///   Rescans all input devices in order to detect new devices connected to the system.
        /// </summary>
        /// <remarks>
        ///   This method could take several milliseconds and should be used at specific time in a game where
        ///   performance is not crucial (pause, configuration screens, etc.).
        /// </remarks>
        public void Scan()
        {
            foreach (var source in Sources)
            {
                source.Scan();
            }
        }

        public void Update(GameTime gameTime)
        {
            ResetGlobalInputState();

            // Recycle input event to reduce garbage generation
            foreach (var evt in events)
            {
                // The router takes care of putting the event back in its respective InputEventPool
                // since it already has the type information
                eventRouters[evt.GetType()].PoolEvent(evt);
            }
            events.Clear();

            // Update all input sources so they can route events to input devices and possible register new devices
            foreach (var source in Sources)
            {
                source.Update();
            }

            // Update all input devices so they can send events and update their state
            foreach (var inputDevice in devices)
            {
                inputDevice.Update(events);
            }

            // Notify PreUpdateInput
            PreUpdateInput?.Invoke(this, new InputPreUpdateEventArgs { GameTime = gameTime });

            // Send events to input listeners
            foreach (var evt in events)
            {
                if (!eventRouters.TryGetValue(evt.GetType(), out IInputEventRouter router))
                    throw new InvalidOperationException($"The event type {evt.GetType()} was not registered with the input manager and cannot be processed.");

                router.RouteEvent(evt);
            }

            // Update virtual buttons
            UpdateVirtualButtonValues();

            // Update gestures
            UpdateGestureEvents(gameTime.Elapsed);
        }

        /// <summary>
        ///   Registers an object that listens for certain types of events using the specialized versions of
        ///   <see cref="IInputEventListener{T}"/>
        /// </summary>
        /// <param name="listener">The listener to register.</param>
        public void AddListener(IInputEventListener listener)
        {
            foreach (var router in eventRouters)
            {
                router.Value.TryAddListener(listener);
            }
        }

        /// <summary>
        ///   Removes a previously registered event listener.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void RemoveListener(IInputEventListener listener)
        {
            foreach (var pair in eventRouters)
            {
                pair.Value.Listeners.Remove(listener);
            }
        }

        /// <summary>
        ///   Gets a binding value for the specified name and the specified config extract from the current
        ///   <see cref="VirtualButtonConfigSet"/>.
        /// </summary>
        /// <param name="configIndex">An index to a <see cref="VirtualButtonConfig"/> stored in the <see cref="VirtualButtonConfigSet"/></param>
        /// <param name="bindingName">Name of the binding.</param>
        /// <returns>The value of the binding.</returns>
        public virtual float GetVirtualButton(int configIndex, object bindingName)
        {
            if (VirtualButtonConfigSet is null || configIndex < 0 || configIndex >= virtualButtonValues.Count)
                return 0.0f;

            virtualButtonValues[configIndex].TryGetValue(bindingName, out float value);
            return value;
        }

        /// <summary>
        ///   Pauses all input sources.
        /// </summary>
        public void Pause()
        {
            foreach (var source in Sources)
            {
                source.Pause();
            }
        }

        /// <summary>
        ///   Resumes all input sources.
        /// </summary>
        public void Resume()
        {
            foreach (var source in Sources)
            {
                source.Resume();
            }
        }

        private void SourcesOnCollectionChanged(object o, TrackingCollectionChangedEventArgs e)
        {
            var source = (IInputSource) e.Item;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (Sources.Count(x => x == source) > 1)
                        throw new InvalidOperationException("Input Source already added.");

                    EventHandler<TrackingCollectionChangedEventArgs> eventHandler = (sender, args) => InputDevicesOnCollectionChanged(source, args);
                    devicesCollectionChangedActions.Add(source, eventHandler);
                    source.Devices.CollectionChanged += eventHandler;
                    source.Initialize(this);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    source.Dispose();
                    source.Devices.CollectionChanged -= devicesCollectionChangedActions[source];
                    devicesCollectionChangedActions.Remove(source);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(e.Action));
            }
        }

        /// <summary>
        /// Registers an input event type to process.
        /// </summary>
        /// <typeparam name="TEventType">The event type to process.</typeparam>
        public void RegisterEventType<TEventType>() where TEventType : InputEvent, new()
        {
            var type = typeof(TEventType);
            eventRouters.Add(type, new InputEventRouter<TEventType>());
        }

        /// <summary>
        ///   Inserts any registered event back into it's <see cref="InputEventPool{T}"/>.
        /// </summary>
        /// <param name="inputEvent">The event to insert into it's event pool.</param>
        public void PoolInputEvent(InputEvent inputEvent)
        {
            eventRouters[inputEvent.GetType()].PoolEvent(inputEvent);
        }

        /// <summary>
        ///   Resets the <see cref="Sources"/> collection back to it's default values.
        /// </summary>
        public void ResetSources()
        {
            Sources.Clear();
            AddSources();
        }

        /// <summary>
        ///   Suggests an index that is unused for a given <see cref="IGamePadDevice"/>.
        /// </summary>
        /// <param name="gamePad">The gamepad to find an index for.</param>
        /// <returns>The unused gamepad index.</returns>
        public int GetFreeGamePadIndex(IGamePadDevice gamePad)
        {
            if (gamePad is null)
                throw new ArgumentNullException(nameof(gamePad));
            if (!GamePads.Contains(gamePad))
                throw new InvalidOperationException("Not a valid gamepad.");

            // Find a new index for this game controller
            int targetIndex = 0;
            for (int i = 0; i < gamePadRequestedIndex.Count; i++)
            {
                var collection = gamePadRequestedIndex[i];
                if (collection.Count == 0 || (collection.Count == 1 && collection[0] == gamePad))
                {
                    targetIndex = i;
                    break;
                }
                targetIndex++;
            }

            return targetIndex;
        }

        private void AddSources()
        {
            var context = gameContext;

            // Add window specific input source
            var windowInputSource = InputSourceFactory.NewWindowInputSource(context);
            Sources.Add(windowInputSource);

            // Add platform specific input sources
            switch (context.ContextType)
            {
                case AppContextType.Desktop:
#if STRIDE_UI_WINFORMS || STRIDE_UI_WPF
                    Sources.Add(new InputSourceWindowsDirectInput());
                    if (InputSourceWindowsXInput.IsSupported())
                        Sources.Add(new InputSourceWindowsXInput());
    #if STRIDE_INPUT_RAWINPUT
                    if (rawInputEnabled && context is GameContextWinforms gameContextWinforms)
                        Sources.Add(new InputSourceWindowsRawInput(gameContextWinforms.Control));
    #endif
#endif
                    break;

                default:
                    throw new InvalidOperationException("GameContext type is not supported by the InputManager.");
            }
        }

        protected override void Destroy()
        {
            base.Destroy();

            // Unregister all gestures
            Gestures.Clear();

            // Destroy all input sources
            foreach (var source in Sources)
            {
                source.Dispose();
            }
        }

        private void GesturesOnCollectionChanged(object sender, TrackingCollectionChangedEventArgs trackingCollectionChangedEventArgs)
        {
            switch (trackingCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    StartGestureRecognition((GestureConfig)trackingCollectionChangedEventArgs.Item);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    StopGestureRecognition((GestureConfig)trackingCollectionChangedEventArgs.Item);
                    break;

                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    throw new NotSupportedException("ActivatedGestures collection was modified but the action was not supported by the system.");

                case NotifyCollectionChangedAction.Move:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StartGestureRecognition(GestureConfig config)
        {
            float aspectRatio = Pointer?.SurfaceAspectRatio ?? 1.0f;
            gestureConfigToRecognizer.Add(config, config.CreateRecognizer(aspectRatio));
        }

        private void StopGestureRecognition(GestureConfig config)
        {
            gestureConfigToRecognizer.Remove(config);
        }

        private void SetMousePosition(Vector2 normalizedPosition)
        {
            // Set mouse position for first mouse device
            if (HasMouse)
                Mouse.SetPosition(normalizedPosition);
        }

        private void InputDevicesOnCollectionChanged(IInputSource source, TrackingCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OnInputDeviceAdded(source, (IInputDevice)e.Item);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    OnInputDeviceRemoved((IInputDevice)e.Item);
                    break;

                default:
                    throw new InvalidOperationException("Unsupported collection operation.");
            }
        }

        private void OnInputDeviceAdded(IInputSource source, IInputDevice device)
        {
            devices.Add(device);
            if (devicesById.ContainsKey(device.Id))
                throw new InvalidOperationException($"Device with Id {device.Id} ({device.Name}) already registered to {devicesById[device.Id].Name}.");

            devicesById.Add(device.Id, device);

            if (device is IKeyboardDevice keyboard)
            {
                RegisterKeyboard(keyboard);
                keyboards.Sort((l, r) => -l.Priority.CompareTo(r.Priority));
            }
            else if (device is IPointerDevice pointer)
            {
                RegisterPointer(pointer);
                pointers.Sort((l, r) => -l.Priority.CompareTo(r.Priority));
            }
            else if (device is IGameControllerDevice controller)
            {
                RegisterGameController(controller);
                gameControllers.Sort((l, r) => -l.Priority.CompareTo(r.Priority));
            }
            else if (device is IGamePadDevice gamePad)
            {
                RegisterGamePad(gamePad);
                gamePads.Sort((l, r) => -l.Priority.CompareTo(r.Priority));
            }
            else if (device is ISensorDevice sensor)
            {
                RegisterSensor(sensor);
            }
            UpdateConnectedDevices();

            DeviceAdded?.Invoke(this, new DeviceChangedEventArgs { Device = device, Source = source, Type = DeviceChangedEventType.Added });
        }

        private void OnInputDeviceRemoved(IInputDevice device)
        {
            if (!devices.Contains(device))
                throw new InvalidOperationException("Input device was not registered.");

            var source = device.Source;
            devices.Remove(device);
            devicesById.Remove(device.Id);

            if (device is IKeyboardDevice keyboard)
            {
                UnregisterKeyboard(keyboard);
            }
            else if (device is IPointerDevice pointer)
            {
                UnregisterPointer(pointer);
            }
            else if (device is IGameControllerDevice controller)
            {
                UnregisterGameController(controller);
            }
            else if (device is IGamePadDevice gamePad)
            {
                UnregisterGamePad(gamePad);
            }
            else if (device is ISensorDevice sensor)
            {
                UnregisterSensor(sensor);
            }
            UpdateConnectedDevices();

            DeviceRemoved?.Invoke(this, new DeviceChangedEventArgs { Device = device, Source = source, Type = DeviceChangedEventType.Removed });
        }

        private void UpdateConnectedDevices()
        {
            Keyboard = keyboards.FirstOrDefault();
            HasKeyboard = Keyboard != null;

            TextInput = devices.OfType<ITextInputDevice>().FirstOrDefault();

            Mouse = pointers.OfType<IMouseDevice>().FirstOrDefault();
            HasMouse = Mouse != null;

            Pointer = pointers.FirstOrDefault();
            HasPointer = Pointer != null;

            GameControllerCount = GameControllers.Count;
            HasGameController = GameControllerCount > 0;

            GamePadCount = GamePads.Count;
            HasGamePad = GamePadCount > 0;

            gamePads.Sort((l, r) => l.Index.CompareTo(r.Index));

            DefaultGamePad = gamePads.FirstOrDefault();

            Accelerometer = sensors.OfType<IAccelerometerSensor>().FirstOrDefault();
            Gyroscope = sensors.OfType<IGyroscopeSensor>().FirstOrDefault();
            Compass = sensors.OfType<ICompassSensor>().FirstOrDefault();
            UserAcceleration = sensors.OfType<IUserAccelerationSensor>().FirstOrDefault();
            Orientation = sensors.OfType<IOrientationSensor>().FirstOrDefault();
            Gravity = sensors.OfType<IGravitySensor>().FirstOrDefault();
        }

        private void RegisterPointer(IPointerDevice pointer)
        {
            pointers.Add(pointer);
        }

        private void UnregisterPointer(IPointerDevice pointer)
        {
            pointers.Remove(pointer);
        }

        private void RegisterKeyboard(IKeyboardDevice keyboard)
        {
            keyboards.Add(keyboard);
        }

        private void UnregisterKeyboard(IKeyboardDevice keyboard)
        {
            keyboards.Remove(keyboard);
        }

        private void RegisterGamePad(IGamePadDevice gamePad)
        {
            gamePads.Add(gamePad);

            // Check if the gamepad provides an interface for assigning gamepad index
            if (gamePad.CanChangeIndex)
            {
                gamePad.Index = GetFreeGamePadIndex(gamePad);
            }

            // Handle later index changed
            gamePad.IndexChanged += GamePadOnIndexChanged;
            UpdateGamePadRequestedIndices();
        }

        private void UnregisterGamePad(IGamePadDevice gamePad)
        {
            // Free the gamepad index in the gamepad list.
            //   This will allow another gamepad to use this index again
            if (gamePadRequestedIndex.Count <= gamePad.Index || gamePad.Index < 0)
                throw new IndexOutOfRangeException("Gamepad index was out of range.");

            gamePadRequestedIndex[gamePad.Index].Remove(gamePad);

            gamePads.Remove(gamePad);
            gamePad.IndexChanged -= GamePadOnIndexChanged;
        }

        private void RegisterGameController(IGameControllerDevice gameController)
        {
            gameControllers.Add(gameController);
        }

        private void UnregisterGameController(IGameControllerDevice gameController)
        {
            gameControllers.Remove(gameController);
        }

        private void GamePadOnIndexChanged(object sender, GamePadIndexChangedEventArgs e)
        {
            UpdateGamePadRequestedIndices();
        }

        private void RegisterSensor(ISensorDevice sensorDevice)
        {
            sensors.Add(sensorDevice);
        }

        private void UnregisterSensor(ISensorDevice sensorDevice)
        {
            sensors.Remove(sensorDevice);
        }

        /// <summary>
        ///   Updates the <see cref="gamePadRequestedIndex"/> collection to contains every gamepad with a given index.
        /// </summary>
        private void UpdateGamePadRequestedIndices()
        {
            foreach (var gamePads in gamePadRequestedIndex)
            {
                gamePads.Clear();
            }

            foreach (var gamePad in GamePads)
            {
                while (gamePad.Index >= gamePadRequestedIndex.Count)
                {
                    gamePadRequestedIndex.Add(new List<IGamePadDevice>());
                }
                gamePadRequestedIndex[gamePad.Index].Add(gamePad);
            }
        }

        private void UpdateGestureEvents(TimeSpan elapsedGameTime)
        {
            currentGestureEvents.Clear();

            foreach (var gestureRecognizer in gestureConfigToRecognizer.Values)
            {
                gestureRecognizer.ProcessPointerEvents(elapsedGameTime, pointerEvents, currentGestureEvents);
            }
        }

        private void UpdateVirtualButtonValues()
        {
            if (VirtualButtonConfigSet != null)
            {
                for (int i = 0; i < VirtualButtonConfigSet.Count; i++)
                {
                    var config = VirtualButtonConfigSet[i];

                    Dictionary<object, float> mapNameToValue;
                    if (i == virtualButtonValues.Count)
                    {
                        mapNameToValue = new Dictionary<object, float>();
                        virtualButtonValues.Add(mapNameToValue);
                    }
                    else
                    {
                        mapNameToValue = virtualButtonValues[i];
                    }

                    mapNameToValue.Clear();

                    if (config != null)
                    {
                        foreach (var name in config.BindingNames)
                        {
                            mapNameToValue[name] = config.GetValue(this, name);
                        }
                    }
                }
            }
        }

        private interface IInputEventRouter
        {
            HashSet<IInputEventListener> Listeners { get; }

            void PoolEvent(InputEvent evt);

            void RouteEvent(InputEvent evt);

            void TryAddListener(IInputEventListener listener);
        }

        private class InputEventRouter<TEventType> : IInputEventRouter where TEventType : InputEvent, new()
        {
            public HashSet<IInputEventListener> Listeners { get; } = new HashSet<IInputEventListener>(ReferenceEqualityComparer<IInputEventListener>.Default);

            public void RouteEvent(InputEvent evt)
            {
                var listeners = Listeners.ToArray();
                foreach (var gesture in listeners)
                {
                    ((IInputEventListener<TEventType>)gesture).ProcessEvent((TEventType)evt);
                }
            }

            public void TryAddListener(IInputEventListener listener)
            {
                if (listener is IInputEventListener<TEventType> specific)
                {
                    Listeners.Add(specific);
                }
            }

            public void PoolEvent(InputEvent evt)
            {
                InputEventPool<TEventType>.Enqueue((TEventType)evt);
            }
        }
    }
}
