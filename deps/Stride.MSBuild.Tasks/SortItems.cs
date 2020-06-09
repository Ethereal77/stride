// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Stride.MSBuild.Tasks
{
    public class SortItems : Task
    {
        [Required]
        public ITaskItem[] InputItems { get; set; }

        [Output]
        public ITaskItem[] OutputItems { get; set; }

        public override bool Execute()
        {
            if (InputItems is null)
            {
                Log.LogError("InputItems not specified.");
                return false;
            }

            OutputItems = InputItems.OrderBy(x => x.ItemSpec).ToArray();
            return true;
        }
    }
}
