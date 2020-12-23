// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Linq;

using Stride.Core.Annotations;
using Stride.Core.Reflection;
using Stride.Core.Assets;
using Stride.Core.Assets.Compiler;
using Stride.Engine;

namespace Stride.Assets.Entities.ComponentChecks
{
    /// <summary>
    ///   Represents a check that verifies if all members of a <see cref="EntityComponent"/> with a
    ///   <see cref="MemberRequiredAttribute"/> are assigned a value.
    /// </summary>
    public class RequiredMembersCheck : IEntityComponentCheck
    {
        /// <inheritdoc/>
        public bool AppliesTo(Type componentType)
        {
            // Applies to any component
            return true;
        }

        /// <inheritdoc/>
        public void Check(EntityComponent component, Entity entity, AssetItem assetItem, string targetUrlInStorage, AssetCompilerResult result)
        {
            var typeDescriptor = TypeDescriptorFactory.Default.Find(component.GetType());
            var componentName = typeDescriptor.Type.Name; // TODO: Should we check attributes for another name?

            var members = typeDescriptor.Members;

            foreach (var member in members)
            {
                // Value types cannot be null, and must always have a proper default value
                if (member.Type.IsValueType)
                    continue;

                MemberRequiredAttribute memberRequired;
                if ((memberRequired = member.GetCustomAttributes<MemberRequiredAttribute>(inherit: true)
                                            .FirstOrDefault()) != null)
                {
                    if (member.Get(component) is null)
                        WriteResult(result, componentName, targetUrlInStorage, entity.Name, member.Name, memberRequired.ReportAs);
                }
            }
        }

        private void WriteResult(AssetCompilerResult result, string componentName, string targetUrlInStorage, string entityName, string memberName, MemberRequiredReportType reportType)
        {
            var logMsg = $"The component {componentName} on entity [{targetUrlInStorage}:{entityName}] is missing a value on a required field '{memberName}'.";
            switch (reportType)
            {
                case MemberRequiredReportType.Warning:
                    result.Warning(logMsg);
                    break;

                case MemberRequiredReportType.Error:
                    result.Error(logMsg);
                    break;
            }
        }
    }
}
