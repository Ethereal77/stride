// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Rendering.Materials;
using Stride.Assets.Presentation.AssetEditors.GameEditor;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor
{
    /// <summary>
    ///   Represents the rendering modes for the scene editor that will be displayed in the rendering mode combobox
    ///   (Game, Editor, Diffuse, etc).
    /// </summary>
    public class EditorRenderMode
    {
        public static readonly EditorRenderMode DefaultEditor = new("Editor");
        public static readonly EditorRenderMode DefaultGamePreview = new("Game preview") { PreviewGameGraphicsCompositor = true };


        public EditorRenderMode(string name)
        {
            Name = name;
        }


        /// <summary>
        ///   Initializes an <see cref="EditorRenderMode"/> from displaying a single stream specified by
        ///   a <see cref="MaterialStreamDescriptor"/>.
        /// </summary>
        /// <param name="streamDescriptor">The material stream this rendering mode will display.</param>
        public EditorRenderMode(MaterialStreamDescriptor streamDescriptor)
        {
            Mode = RenderMode.SingleStream;

            Name = streamDescriptor.Name;
            StreamDescriptor = streamDescriptor;
        }


        /// <summary>
        ///   Gets the name of the rendering mode.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///   Gets the rendering mode.
        /// </summary>
        public RenderMode Mode { get; private set; } = RenderMode.Lighting;

        /// <summary>
        ///   Gets or sets a value indicating whether we should use the user's graphics compositor instead
        ///   of the editor's one.
        /// </summary>
        public bool PreviewGameGraphicsCompositor { get; set; }

        /// <summary>
        ///   Gets the material stream to display if <see cref="Mode"/> is <see cref="RenderMode.SingleStream"/>.
        /// </summary>
        public MaterialStreamDescriptor StreamDescriptor { get; private set; }
    }
}
