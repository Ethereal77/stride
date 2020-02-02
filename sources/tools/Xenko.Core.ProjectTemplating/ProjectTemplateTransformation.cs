// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Microsoft.VisualStudio.TextTemplating;

namespace Xenko.Core.ProjectTemplating
{
    /// <summary>
    /// Base that must be used for all ProjectTemplate
    /// </summary>
    public abstract class ProjectTemplateTransformation : TextTransformation
    {
        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        public string ProjectName
        {
            get
            {
                return Session["ProjectName"].ToString();
            }
        }

        /// <summary>
        /// Gets the project unique identifier.
        /// </summary>
        /// <value>The project unique identifier.</value>
        public Guid ProjectGuid
        {
            get
            {
                return (Guid)Session["ProjectGuid"];
            }
        }

        /// <summary>
        /// Dynamic properties accessible from the template
        /// </summary>
        public dynamic Properties
        {
            get
            {
                return ((CustomTemplatingSession)Session).Dynamic;
            }
        }
    }
}
