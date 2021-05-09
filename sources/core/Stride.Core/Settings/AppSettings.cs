// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

using Stride.Core.Collections;

namespace Stride.Core.Settings
{
    /// <summary>
    ///   Collection of runtime loaded application settings.
    /// </summary>
    /// <seealso cref="AppSettingsManager"/>
    [DataContract("AppSettings")]
    public sealed class AppSettings : IEnumerable<object>
    {
        /// <summary>
        ///   Gets or sets the application specific settings.
        /// </summary>
        [DataMember]
        private FastCollection<object> Settings { get; set; }


        /// <summary>
        ///   Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        /// <remarks>This default constructor is mainly used when deserializing.</remarks>
        public AppSettings() { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AppSettings"/> class with a settings collection.
        /// </summary>
        /// <param name="settings">The settings collection.</param>
        public AppSettings(IEnumerable<object> settings)
        {
            Settings = new FastCollection<object>(settings);
        }


        /// <summary>
        ///   Finds a setting of the specified type.
        /// </summary>
        /// <returns>The value of the setting if found; or <c>null</c> if not found.</returns>
        public T GetSettings<T>() where T : class
        {
            if (Settings is null)
                return null;

            foreach (var obj in Settings)
                if (obj is T setting)
                    return setting;

            return null;
        }

        /// <summary>
        ///   Returns an object that can be used to iterate through the settings.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the settings.</returns>
        public FastCollection<object>.Enumerator GetEnumerator() => Settings.GetEnumerator();

        IEnumerator<object> IEnumerable<object>.GetEnumerator() => ((IEnumerable<object>)Settings).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Settings).GetEnumerator();
    }
}
