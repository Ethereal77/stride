// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Mathematics;

namespace Stride.Particles.Modules
{
    /// <summary>
    /// The <see cref="UpdaterSpeedToDirection"/> calculates the particle's direction (not normalized) based on its delta position
    /// </summary>
    [DataContract("UpdaterSpeedToDirection")]
    [Display("Direction from Speed")]
    public class UpdaterSpeedToDirection : ParticleUpdater
    {
        /// <inheritdoc />
        [DataMemberIgnore]
        public override bool IsPostUpdater => true;

        public UpdaterSpeedToDirection()
        {
            RequiredFields.Add(ParticleFields.Position);
            RequiredFields.Add(ParticleFields.OldPosition);
            RequiredFields.Add(ParticleFields.Direction);
        }

        public override unsafe void Update(float dt, ParticlePool pool)
        {
            if (dt <= MathUtil.ZeroTolerance)
                return;

            if (!pool.FieldExists(ParticleFields.Position) || !pool.FieldExists(ParticleFields.OldPosition) || !pool.FieldExists(ParticleFields.Direction))
                return;

            var posField = pool.GetField(ParticleFields.Position);
            var oldField = pool.GetField(ParticleFields.OldPosition);
            var dirField = pool.GetField(ParticleFields.Direction);

            foreach (var particle in pool)
            {
                (*((Vector3*)particle[dirField])) = ((*((Vector3*)particle[posField])) - (*((Vector3*)particle[oldField]))) / dt;
            }
        }
    }
}
