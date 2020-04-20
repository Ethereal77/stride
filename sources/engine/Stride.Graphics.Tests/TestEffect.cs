// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

/*
using System.IO;

using Xunit;

using Stride.Core.IO;
using Stride.Core.Serialization.Assets;
using Stride.Core.Storage;
using Stride.Rendering;
using Stride.Games;
using Stride.Shaders;
using Stride.Shaders.Compiler;
using EffectCompiler = Stride.Shaders.Compiler.EffectCompiler;

namespace Stride.Graphics
{
    public class TestEffect : Game
    {
        public TestEffect()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 480,
                PreferredGraphicsProfile = new[] { GraphicsProfile.Level_11_0 }
            };
        }

        protected override void Update(GameTime gameTime)
        {
            Exit();
        }


        [Fact]
        public void TestSimpleEffect()
        {
            EffectBytecode effectBytecode;

            // Create and mount database file system
            var objDatabase = new ObjectDatabase(VirtualFileSystem.ApplicationDatabasePath);
            using (var contentIndexMap = new contentIndexMap("/assets"))
            {
                contentIndexMap.LoadNewValues();
                var database = new DatabaseFileProvider(contentIndexMap, objDatabase);

                foreach (var shaderName in Directory.EnumerateFiles(@"..\..\..\..\shaders", "*.sdsl"))
                    CopyStream(database, shaderName);

                foreach (var shaderName in Directory.EnumerateFiles(@"Compiler", "*.sdsl"))
                    CopyStream(database, shaderName);

                var compiler = new EffectCompiler();
                compiler.SourceDirectories.Add("assets/shaders");
                var compilerCache = new EffectCompilerCache(compiler);

                var compilerParmeters = new CompilerParameters { Platform = GraphicsPlatform.Direct3D11 };

                var compilerResults = compilerCache.Compile(new ShaderMixinSource("SimpleEffect"), compilerParmeters);
                Assert.That(compilerResults.HasErrors, Is.False);

                effectBytecode = compilerResults.Bytecodes[0];
            }

            var graphicsDevice = GraphicsDevice.New();

            var effect = new Effect(graphicsDevice, effectBytecode);
            effect.Apply();
        }

        private void CopyStream(DatabaseFileProvider database, string fromFilePath)
        {
            var shaderFilename = string.Format("shaders/{0}", Path.GetFileName(fromFilePath));
            if (!database.FileExists(shaderFilename))
            {
                using (var outStream = database.OpenStream(shaderFilename, VirtualFileMode.Create, VirtualFileAccess.Write, VirtualFileShare.Write))
                {
                    using (var inStream = new FileStream(fromFilePath, FileMode.Open, FileAccess.Read))
                    {
                        inStream.CopyTo(outStream);
                    }
                }
            }
        }
    }
}
*/
