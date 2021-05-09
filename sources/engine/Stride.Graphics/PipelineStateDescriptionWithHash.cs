// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Graphics
{
    internal struct PipelineStateDescriptionWithHash : IEquatable<PipelineStateDescriptionWithHash>
    {
        public readonly int Hash;
        public readonly PipelineStateDescription State;

        public PipelineStateDescriptionWithHash(PipelineStateDescription state)
        {
            Hash = state.GetHashCode();
            State = state;
        }

        public bool Equals(PipelineStateDescriptionWithHash other)
        {
            return Hash == other.Hash && (State == null) == (other.State == null) && (State?.Equals(other.State) ?? true);
        }

        public override bool Equals(object obj)
        {
            return obj is PipelineStateDescriptionWithHash other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Hash;
        }

        public static bool operator ==(PipelineStateDescriptionWithHash left, PipelineStateDescriptionWithHash right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PipelineStateDescriptionWithHash left, PipelineStateDescriptionWithHash right)
        {
            return !left.Equals(right);
        }
    }
}
