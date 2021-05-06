// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Contents;
using Stride.Core.BuildEngine;
using Stride.Core.Assets;
using Stride.Core.Assets.Compiler;
using Stride.Core.Assets.Analysis;
using Stride.Assets.Textures;
using Stride.Graphics.Data;
using Stride.Rendering;
using Stride.Physics;

using VHACDSharp;

using Buffer = Stride.Graphics.Buffer;

namespace Stride.Assets.Physics
{
    [AssetCompiler(typeof(ColliderShapeAsset), typeof(AssetCompilationContext))]
    internal class ColliderShapeAssetCompiler : AssetCompilerBase
    {
        static ColliderShapeAssetCompiler()
        {
            NativeLibraryHelper.Load("VHACD", typeof(ColliderShapeAssetCompiler));
        }

        public override IEnumerable<BuildDependencyInfo> GetInputTypes(AssetItem assetItem)
        {
            foreach (var type in AssetRegistry.GetAssetTypes(typeof(Model)))
                yield return new BuildDependencyInfo(type, typeof(AssetCompilationContext), BuildDependencyType.CompileContent);

            foreach (var type in AssetRegistry.GetAssetTypes(typeof(Skeleton)))
                yield return new BuildDependencyInfo(type, typeof(AssetCompilationContext), BuildDependencyType.CompileContent);

            foreach (var type in AssetRegistry.GetAssetTypes(typeof(Heightmap)))
                yield return new BuildDependencyInfo(type, typeof(AssetCompilationContext), BuildDependencyType.CompileContent);
        }

        public override IEnumerable<Type> GetInputTypesToExclude(AssetItem assetItem)
        {
            foreach(var type in AssetRegistry.GetAssetTypes(typeof(Material)))
                yield return type;

            yield return typeof(TextureAsset);
        }

        public override IEnumerable<ObjectUrl> GetInputFiles(AssetItem assetItem)
        {
            var asset = (ColliderShapeAsset) assetItem.Asset;

            foreach (var desc in asset.ColliderShapes)
            {
                if (desc is ConvexHullColliderShapeDesc convexHullDesc)
                {
                    if (convexHullDesc.Model is not null)
                    {
                        var url = AttachedReferenceManager.GetUrl(convexHullDesc.Model);

                        if (!string.IsNullOrEmpty(url))
                            yield return new ObjectUrl(UrlType.Content, url);
                    }
                }
                else if (desc is HeightfieldColliderShapeDesc heightfieldDesc)
                {
                    if (heightfieldDesc.HeightStickArraySource is HeightStickArraySourceFromHeightmap heightmapSource &&
                        heightmapSource.Heightmap is not null)
                    {
                        var url = AttachedReferenceManager.GetUrl(heightmapSource.Heightmap);

                        if (!string.IsNullOrEmpty(url))
                            yield return new ObjectUrl(UrlType.Content, url);
                    }
                }
            }
        }

        protected override void Prepare(AssetCompilerContext context, AssetItem assetItem, string targetUrlInStorage, AssetCompilerResult result)
        {
            var asset = (ColliderShapeAsset) assetItem.Asset;

            result.BuildSteps = new AssetBuildStep(assetItem);
            result.BuildSteps.Add
                (
                    new ColliderShapeCombineCommand(targetUrlInStorage, asset, assetItem.Package)
                    {
                        InputFilesGetter = () => GetInputFiles(assetItem)
                    }
                );
        }

        public class ColliderShapeCombineCommand : AssetCommand<ColliderShapeAsset>
        {
            public ColliderShapeCombineCommand(string url, ColliderShapeAsset parameters, IAssetFinder assetFinder)
                : base(url, parameters, assetFinder)
            { }

            protected override Task<ResultStatus> DoCommandOverride(ICommandContext commandContext)
            {
                var assetManager = new ContentManager(MicrothreadLocalDatabases.ProviderService);

                // Cloned list of collider shapes
                var descriptions = Parameters.ColliderShapes.ToList();

                var validShapes = Parameters.ColliderShapes
                    .Where(shape => shape is not null &&
                                    (shape is not ConvexHullColliderShapeDesc ||
                                     ((ConvexHullColliderShapeDesc) shape).Model is not null))
                    .ToList();

                // Pre-process special types
                foreach (var convexHullDesc in validShapes.Where(s => s is ConvexHullColliderShapeDesc)
                                                          .Cast<ConvexHullColliderShapeDesc>())
                {
                    // Clone the convex hull shape description so the fields that should not be serialized can be cleared (Model in this case)
                    var convexHullDescClone = new ConvexHullColliderShapeDesc
                    {
                        Scaling = convexHullDesc.Scaling,
                        LocalOffset = convexHullDesc.LocalOffset,
                        LocalRotation = convexHullDesc.LocalRotation,
                        Decomposition = convexHullDesc.Decomposition
                    };

                    // Replace shape in final result with cloned description
                    int replaceIndex = descriptions.IndexOf(convexHullDesc);
                    descriptions[replaceIndex] = convexHullDescClone;

                    var loadSettings = new ContentManagerLoaderSettings
                    {
                        ContentFilter = ContentManagerLoaderSettings.NewContentFilterByType(typeof(Mesh), typeof(Skeleton))
                    };

                    var modelAsset = assetManager.Load<Model>(AttachedReferenceManager.GetUrl(convexHullDesc.Model), loadSettings);
                    if (modelAsset is null)
                        continue;

                    convexHullDescClone.ConvexHulls = new List<List<List<Vector3>>>();
                    convexHullDescClone.ConvexHullsIndices = new List<List<List<uint>>>();

                    commandContext.Logger.Info("Processing convex hull generation, this might take a while!");

                    var nodeTransforms = new List<Matrix>();

                    // Pre-compute all node transforms, assuming nodes are ordered. See ModelViewHierarchyUpdater

                    if (modelAsset.Skeleton is null)
                    {
                        Matrix.Transformation(ref convexHullDescClone.Scaling,
                                              ref convexHullDescClone.LocalRotation,
                                              ref convexHullDescClone.LocalOffset,
                                              out Matrix baseMatrix);

                        nodeTransforms.Add(baseMatrix);
                    }
                    else
                    {
                        var nodesLength = modelAsset.Skeleton.Nodes.Length;
                        for (var nodeIndex = 0; nodeIndex < nodesLength; nodeIndex++)
                        {
                            Matrix.Transformation(ref modelAsset.Skeleton.Nodes[nodeIndex].Transform.Scale,
                                                  ref modelAsset.Skeleton.Nodes[nodeIndex].Transform.Rotation,
                                                  ref modelAsset.Skeleton.Nodes[nodeIndex].Transform.Position,
                                                  out Matrix localMatrix);

                            Matrix worldMatrix;
                            if (modelAsset.Skeleton.Nodes[nodeIndex].ParentIndex != -1)
                            {
                                var nodeTransform = nodeTransforms[modelAsset.Skeleton.Nodes[nodeIndex].ParentIndex];
                                Matrix.Multiply(ref localMatrix, ref nodeTransform, out worldMatrix);
                            }
                            else
                                worldMatrix = localMatrix;

                            if (nodeIndex == 0)
                            {
                                Matrix.Transformation(ref convexHullDescClone.Scaling,
                                                      ref convexHullDescClone.LocalRotation,
                                                      ref convexHullDescClone.LocalOffset,
                                                      out Matrix baseMatrix);

                                nodeTransforms.Add(baseMatrix * worldMatrix);
                            }
                            else
                                nodeTransforms.Add(worldMatrix);
                        }
                    }

                    for (var transformIndex = 0; transformIndex < nodeTransforms.Count; transformIndex++)
                    {
                        if (modelAsset.Meshes.All(mesh => mesh.NodeIndex != transformIndex))
                            // No geometry in the node
                            continue;

                        var combinedVerts = new List<float>();
                        var combinedIndices = new List<uint>();

                        var hullsList = new List<List<Vector3>>();
                        convexHullDescClone.ConvexHulls.Add(hullsList);

                        var indicesList = new List<List<uint>>();
                        convexHullDescClone.ConvexHullsIndices.Add(indicesList);

                        foreach (var meshData in modelAsset.Meshes.Where(mesh => mesh.NodeIndex == transformIndex))
                        {
                            var indexOffset = (uint) combinedVerts.Count / 3;

                            var vertexStride = meshData.Draw.VertexBuffers[0].Declaration.VertexStride;

                            byte[] vertexData;
                            var vertexBufferRef = AttachedReferenceManager.GetAttachedReference(meshData.Draw.VertexBuffers[0].Buffer);
                            if (vertexBufferRef.Data is not null)
                            {
                                vertexData = ((BufferData) vertexBufferRef.Data).Content;
                            }
                            else if (!string.IsNullOrEmpty(vertexBufferRef.Url))
                            {
                                var dataAsset = assetManager.Load<Buffer>(vertexBufferRef.Url);
                                vertexData = dataAsset.GetSerializationData().Content;
                            }
                            else
                                continue;

                            var vertexIndex = meshData.Draw.VertexBuffers[0].Offset;
                            for (var v = 0; v < meshData.Draw.VertexBuffers[0].Count; v++)
                            {
                                var posMatrix = Matrix.Translation(new Vector3(BitConverter.ToSingle(vertexData, vertexIndex + 0),
                                                                               BitConverter.ToSingle(vertexData, vertexIndex + 4),
                                                                               BitConverter.ToSingle(vertexData, vertexIndex + 8)));

                                var nodeTransform = nodeTransforms[transformIndex];
                                Matrix.Multiply(ref posMatrix, ref nodeTransform, out Matrix rotatedMatrix);

                                combinedVerts.Add(rotatedMatrix.TranslationVector.X);
                                combinedVerts.Add(rotatedMatrix.TranslationVector.Y);
                                combinedVerts.Add(rotatedMatrix.TranslationVector.Z);

                                vertexIndex += vertexStride;
                            }

                            byte[] indexData;
                            var indexBufferRef = AttachedReferenceManager.GetAttachedReference(meshData.Draw.IndexBuffer.Buffer);
                            if (indexBufferRef.Data is not null)
                            {
                                indexData = ((BufferData) indexBufferRef.Data).Content;
                            }
                            else if (!string.IsNullOrEmpty(indexBufferRef.Url))
                            {
                                var dataAsset = assetManager.Load<Buffer>(indexBufferRef.Url);
                                indexData = dataAsset.GetSerializationData().Content;
                            }
                            else
                                throw new Exception("Failed to find index buffer while building a convex hull.");

                            var indexIndex = meshData.Draw.IndexBuffer.Offset;
                            for (var i = 0; i < meshData.Draw.IndexBuffer.Count; i++)
                            {
                                if (meshData.Draw.IndexBuffer.Is32Bit)
                                {
                                    combinedIndices.Add(BitConverter.ToUInt32(indexData, indexIndex) + indexOffset);
                                    indexIndex += 4;
                                }
                                else
                                {
                                    combinedIndices.Add(BitConverter.ToUInt16(indexData, indexIndex) + indexOffset);
                                    indexIndex += 2;
                                }
                            }
                        }

                        var decompositionDesc = new ConvexHullMesh.DecompositionDesc
                        {
                            VertexCount = (uint) combinedVerts.Count / 3,
                            IndicesCount = (uint) combinedIndices.Count,
                            Vertexes = combinedVerts.ToArray(),
                            Indices = combinedIndices.ToArray(),
                            Depth = convexHullDesc.Decomposition.Depth,
                            PosSampling = convexHullDesc.Decomposition.PosSampling,
                            PosRefine = convexHullDesc.Decomposition.PosRefine,
                            AngleSampling = convexHullDesc.Decomposition.AngleSampling,
                            AngleRefine = convexHullDesc.Decomposition.AngleRefine,
                            Alpha = convexHullDesc.Decomposition.Alpha,
                            Threshold = convexHullDesc.Decomposition.Threshold,
                            SimpleHull = !convexHullDesc.Decomposition.Enabled,
                        };

                        var convexHullMesh = new ConvexHullMesh();
                        convexHullMesh.Generate(decompositionDesc);

                        var count = convexHullMesh.Count;

                        commandContext.Logger.Info("Node generated " + count + " convex hulls");

                        var vertexCountHull = 0;

                        for (uint h = 0; h < count; h++)
                        {
                            convexHullMesh.CopyPoints(h, out float[] points);

                            var pointList = new List<Vector3>();

                            for (var v = 0; v < points.Length; v += 3)
                            {
                                var vert = new Vector3(points[v + 0], points[v + 1], points[v + 2]);
                                pointList.Add(vert);

                                vertexCountHull++;
                            }

                            hullsList.Add(pointList);

                            convexHullMesh.CopyIndices(h, out uint[] indices);

                            for (var t = 0; t < indices.Length; t += 3)
                            {
                                Core.Utilities.Swap(ref indices[t], ref indices[t + 2]);
                            }

                            var indexList = new List<uint>(indices);

                            indicesList.Add(indexList);
                        }

                        convexHullMesh.Dispose();

                        commandContext.Logger.Info("for a total of " + vertexCountHull + " vertices.");
                    }
                }

                var runtimeShape = new PhysicsColliderShape(descriptions);
                assetManager.Save(Url, runtimeShape);

                return Task.FromResult(ResultStatus.Successful);
            }
        }
    }
}
