// Copyright (c) Xenko contributors (https://xenko.com) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
/*
#if XENKO_PLATFORM_WINDOWS_DESKTOP
using System.IO;

using Xunit;
using Xenko.Core.IO;
using Xenko.Core.Serialization.Assets;
using Xenko.Core.Storage;
using Xenko.Rendering;
using Xenko.Games;
using Xenko.Shaders;
using Xenko.Shaders.Compiler;
using EffectCompiler = Xenko.Shaders.Compiler.EffectCompiler;

namespace Xenko.Graphics
{
    public class TestEffect : Game
    {
        public TestEffect()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 480,
                //PreferredGraphicsProfile = new[] { GraphicsProfile.Level_9_1 }
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

                foreach (var shaderName in Directory.EnumerateFiles(@"..\..\..\..\shaders", "*.xksl"))
                    CopyStream(database, shaderName);

                foreach (var shaderName in Directory.EnumerateFiles(@"Compiler", "*.xksl"))
                    CopyStream(database, shaderName);

                var compiler = new EffectCompiler();
                compiler.SourceDirectories.Add("assets/shaders");
                var compilerCache = new EffectCompilerCache(compiler);

                var compilerParmeters = new CompilerParameters { Platform = GraphicsPlatform.Direct3D };

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
#endif
*/
