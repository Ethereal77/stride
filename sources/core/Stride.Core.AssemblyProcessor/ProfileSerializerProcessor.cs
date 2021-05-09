// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.AssemblyProcessor.Serializers;

namespace Stride.Core.AssemblyProcessor
{
    internal class ProfileSerializerProcessor : ICecilSerializerProcessor
    {
        public void ProcessSerializers(CecilSerializerContext context)
        {
            var defaultProfile = context.SerializableTypes;

            foreach (var profile in context.SerializableTypesProfiles)
            {
                // Skip default profile
                if (profile.Value == defaultProfile)
                    continue;

                defaultProfile.IsFrozen = true;

                // For each profile, try to instantiate all types existing in default profile
                foreach (var type in defaultProfile.SerializableTypes)
                {
                    context.GenerateSerializer(type.Key, false, profile.Key);
                }
            }
        }
    }
}
