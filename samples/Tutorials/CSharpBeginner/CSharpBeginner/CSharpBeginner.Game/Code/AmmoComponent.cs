// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Engine;

namespace CSharpBeginner.Code
{
    /// <summary>
    /// This script is used in combination with the GettingAComponent.cs script 
    /// </summary>
    public class AmmoComponent : StartupScript
    {
        private int clips = 4;
        private int bullets = 6;

        public override void Start()
        {
        }

        // This method return the total amount of ammo
        public int GetTotalAmmo()
        {
            return bullets * clips;
        }
    }
}
