// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 SLNTools - Christian Warren
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using Stride.Core.Annotations;

namespace Stride.Core.VisualStudio
{
    /// <summary>
    /// A collection of <see cref="Project"/>
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class ProjectCollection : KeyedCollection<Guid, Project>
    {
        private readonly Solution solution;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectCollection"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException">container</exception>
        internal ProjectCollection([NotNull] Solution container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            solution = container;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectCollection"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="items">The items.</param>
        internal ProjectCollection([NotNull] Solution container, IEnumerable<Project> items)
            : this(container)
        {
            this.AddRange(items);
        }

        /// <summary>
        /// Gets the solution this project is attached to.
        /// </summary>
        /// <value>The solution.</value>
        public Solution Solution
        {
            get
            {
                return solution;
            }
        }

        /// <summary>
        /// Finds a project by its full name.
        /// </summary>
        /// <param name="projectFullName">Full name of the project.</param>
        /// <returns>The Project or null if not found.</returns>
        [CanBeNull]
        public Project FindByFullName(string projectFullName)
        {
            return this.FirstOrDefault(item => string.Compare(item.GetFullName(solution), projectFullName, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        /// <summary>
        /// Finds a project by its unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>The project or null if not found.</returns>
        [CanBeNull]
        public Project FindByGuid(Guid guid)
        {
            return (Contains(guid)) ? this[guid] : null;
        }

        /// <summary>
        /// Sorts this instance.
        /// </summary>
        public void Sort()
        {
            Sort((p1, p2) => StringComparer.InvariantCultureIgnoreCase.Compare(p1.GetFullName(solution), p2.GetFullName(solution)));
        }

        public void Sort([NotNull] Comparison<Project> comparer)
        {
            var tempList = new List<Project>(this);
            tempList.Sort(comparer);

            Clear();
            this.AddRange(tempList);
        }

        protected override Guid GetKeyForItem([NotNull] Project item)
        {
            return item.Guid;
        }
    }
}