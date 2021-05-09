// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Storage;
using Stride.Graphics;

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents a <see cref="ResourceGroupLayout"/> specifically implemented for use by the <see cref="RenderSystem"/>,
    ///   with some extra information needed by its processes.
    /// </summary>
    public class RenderSystemResourceGroupLayout : ResourceGroupLayout
    {
        internal LogicalGroup[] LogicalGroups;
        internal int[] ConstantBufferOffsets;

        /// <summary>
        ///   Describes what state the effect is in (compiling, error, etc..).
        /// </summary>
        public RenderEffectState State;

        public int GetConstantBufferOffset(ConstantBufferOffsetReference offsetReference)
        {
            return ConstantBufferOffsets[offsetReference.Index];
        }

        public LogicalGroup GetLogicalGroup(LogicalGroupReference logicalGroupReference)
        {
            return LogicalGroups[logicalGroupReference.Index];
        }

        internal LogicalGroup CreateLogicalGroup(string name)
        {
            var logicalGroup = new LogicalGroup
            {
                DescriptorEntryStart = -1,
                ConstantBufferMemberStart = -1,
            };

            var hashBuilder = new ObjectIdBuilder();

            if (ConstantBufferReflection != null)
            {
                for (int index = 0; index < ConstantBufferReflection.Members.Length; index++)
                {
                    var member = ConstantBufferReflection.Members[index];
                    if (member.LogicalGroup == name)
                    {
                        // First item?
                        if (logicalGroup.ConstantBufferMemberStart == -1)
                        {
                            logicalGroup.ConstantBufferOffset = member.Offset;
                            logicalGroup.ConstantBufferMemberStart = index;
                        }

                        // Update count
                        logicalGroup.ConstantBufferMemberCount = index + 1 - logicalGroup.ConstantBufferMemberStart;
                        logicalGroup.ConstantBufferSize = member.Offset + member.Size - logicalGroup.ConstantBufferOffset;

                        // Hash
                        Effect.HashConstantBufferMember(ref hashBuilder, ref member, logicalGroup.ConstantBufferOffset);
                    }
                    else if (logicalGroup.ConstantBufferMemberStart != -1)
                    {
                        // Group is finished, no need to scan the end
                        break;
                    }
                }
            }

            for (int index = 0, slot = 0; index < DescriptorSetLayoutBuilder.Entries.Count; index++)
            {
                var descriptorSetEntry = DescriptorSetLayoutBuilder.Entries[index];
                if (descriptorSetEntry.LogicalGroup == name)
                {
                    // First item?
                    if (logicalGroup.DescriptorEntryStart == -1)
                    {
                        logicalGroup.DescriptorSlotStart = slot;
                        logicalGroup.DescriptorEntryStart = index;
                    }

                    // Update count
                    logicalGroup.DescriptorEntryCount = index + 1 - logicalGroup.DescriptorEntryStart;
                    logicalGroup.DescriptorSlotCount += descriptorSetEntry.ArraySize;

                    // Hash
                    hashBuilder.Write(descriptorSetEntry.Key.Name);
                    hashBuilder.Write(descriptorSetEntry.Class);
                    hashBuilder.Write(descriptorSetEntry.ArraySize);
                }
                else if (logicalGroup.DescriptorEntryStart != -1)
                {
                    // Group is finished, no need to scan the end
                    break;
                }

                slot += descriptorSetEntry.ArraySize;
            }

            logicalGroup.Hash = hashBuilder.ComputeHash();

            return logicalGroup;
        }
    }
}
