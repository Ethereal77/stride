// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Stride.Core.Storage;
using Stride.Core.Shaders.Ast.Stride;
using Stride.Core.Shaders.Ast;
using Stride.Core.Shaders.Utility;

namespace Stride.Shaders.Parser.Mixins
{
    [DebuggerDisplay("Mixin: {mixinName}")]
    internal class ModuleMixinInfo
    {
        private ShaderClassType mixinAst;


        /// <summary>
        ///   Gets or sets the shader source code.
        /// </summary>
        public ShaderSource ShaderSource { get; set; }

        /// <summary>
        ///   Gets the name of the mixin.
        /// </summary>
        public string MixinName { get; private set; } = "";

        /// <summary>
        ///   Name of the mixin with its hashed code.
        /// </summary>
        public string MixinGenericName;

        /// <summary>
        ///   The log stored by this mixin info.
        /// </summary>
        public readonly LoggerResult Log;

        /// <summary>
        ///   Gets or sets the shader class type.
        /// </summary>
        public ShaderClassType MixinAst
        {
            get => mixinAst;

            set
            {
                mixinAst = value;
                MixinName = mixinAst.Name.Text;
            }
        }

        /// <summary>
        ///   Tests if this instance is a <see cref="ShaderClassCode"/> of the specified type name.
        /// </summary>
        /// <param name="typeName">The type name to test.</param>
        /// <returns><c>true</c> if it has the same type name.</returns>
        public bool IsShaderClass(string typeName)
        {
            if (!(ShaderSource is ShaderClassCode classSource))
                return false;

            return classSource.ClassName == typeName;
        }

        /// <summary>
        ///   The module mixin.
        /// </summary>
        public ModuleMixin Mixin = new ModuleMixin();

        /// <summary>
        ///   Gets or sets a value indicating whether the mixin is instanciated.
        /// </summary>
        public bool Instanciated { get; set; }

        /// <summary>
        ///   Value indicating if the check for replacement has been done.
        /// </summary>
        public bool ReplacementChecked = false;

        /// <summary>
        ///   The source hash.
        /// </summary>
        public ObjectId SourceHash;

        /// <summary>
        ///   The SHA1 hash of the source.
        /// </summary>
        public ObjectId HashPreprocessSource;

        /// <summary>
        ///   The macros used for this mixin.
        /// </summary>
        public Core.Shaders.Parser.ShaderMacro[] Macros = new Core.Shaders.Parser.ShaderMacro[0];

        /// <summary>
        ///   List of all the necessary MixinInfos to compile the shader.
        /// </summary>
        public HashSet<ModuleMixinInfo> MinimalContext = new HashSet<ModuleMixinInfo>();

        /// <summary>
        ///   The referenced shaders. Used to invalidate shaders.
        /// </summary>
        public HashSet<string> ReferencedShaders = new HashSet<string>();


        public ModuleMixinInfo()
        {
            Log = new LoggerResult();
            Instanciated = true;
        }


        public ModuleMixinInfo Copy(Core.Shaders.Parser.ShaderMacro[] macros)
        {
            return new ModuleMixinInfo
            {
                ShaderSource = ShaderSource,
                MixinAst = MixinAst,
                MixinGenericName = MixinGenericName,
                Mixin = Mixin,
                Instanciated = Instanciated,
                HashPreprocessSource = HashPreprocessSource,
                Macros = macros
            };
        }

        public bool AreEqual(ShaderSource shaderSource, Stride.Core.Shaders.Parser.ShaderMacro[] macros)
        {
            return ShaderSource.Equals(shaderSource) &&
                   macros.All(macro => Macros.Any(x => x.Name == macro.Name &&
                                                       x.Definition == macro.Definition)) &&
                   Macros.All(macro => macros.Any(x => x.Name == macro.Name &&
                                                       x.Definition == macro.Definition));
        }

        #region Static members

        /// <summary>
        ///   Cleans the identifiers (i.e. make them use the minimal string).
        /// </summary>
        /// <param name="genList">The list of identifiers.</param>
        public static void CleanIdentifiers(List<Identifier> genList)
        {
            foreach (var gen in genList.OfType<LiteralIdentifier>())
            {
                gen.Text = gen.Value.Value.ToString();
            }
        }

        #endregion
    }
}
