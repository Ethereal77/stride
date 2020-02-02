// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Linq;

using Xenko.Core.Assets;
using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Core.Presentation.Extensions;
using Xenko.Assets;
using Xenko.Data;

namespace Xenko.Editor.Build
{
    /// <summary>
    /// Arguments of the <see cref="GameSettingsProviderService.GameSettingsChanged"/> event.
    /// </summary>
    public class GameSettingsChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameSettingsChangedEventArgs"/> class.
        /// </summary>
        /// <param name="gameSettings">A copy of the current game settings asset.</param>
        public GameSettingsChangedEventArgs(GameSettingsAsset gameSettings)
        {
            GameSettings = gameSettings;
        }

        /// <summary>
        /// Gets a copy of the game settings asset that has changed.
        /// </summary>
        public GameSettingsAsset GameSettings { get; }
    }

    public class GameSettingsProviderService : IDisposable, IGameSettingsAccessor
    {
        private readonly SessionViewModel session;
        private Package currentPackage;

        public GameSettingsProviderService(SessionViewModel session)
        {
            this.session = session;
            session.SessionStateChanged += SessionOnSessionStateChanged;
            session.AssetPropertiesChanged += AssetPropertyChanged;
            UpdateCurrentGameSettings();
        }

        public GameSettingsAsset CurrentGameSettings { get; private set; }

        public event EventHandler<GameSettingsChangedEventArgs> GameSettingsChanged;

        public void Dispose()
        {
            session.SessionStateChanged -= SessionOnSessionStateChanged;
            session.AssetPropertiesChanged -= AssetPropertyChanged;
        }

        /// <inheritdoc/>
        public T GetConfiguration<T>() where T : Configuration
        {
            var configuration = CurrentGameSettings?.TryGet<T>();
            return configuration;
        }

        private void AssetPropertyChanged(object sender, AssetChangedEventArgs e)
        {
            if (e.Assets.Any(x => x.Asset == CurrentGameSettings))
            {
                RaiseGameSettings(CurrentGameSettings);
            }
        }

        private void SessionOnSessionStateChanged(object sender, SessionStateChangedEventArgs sessionStateChangedEventArgs)
        {
            if (session.CurrentProject?.Package != currentPackage)
            {
                UpdateCurrentGameSettings();
            }
        }

        private void UpdateCurrentGameSettings()
        {
            currentPackage = session.CurrentProject?.Package;
            CurrentGameSettings = currentPackage?.GetGameSettingsAssetOrDefault() ?? GameSettingsFactory.Create();
            RaiseGameSettings(CurrentGameSettings);
        }

        private void RaiseGameSettings(GameSettingsAsset gameSettingsAsset)
        {
            GameSettingsChanged?.Invoke(this, new GameSettingsChangedEventArgs(AssetCloner.Clone(gameSettingsAsset)));
        }
    }
}
