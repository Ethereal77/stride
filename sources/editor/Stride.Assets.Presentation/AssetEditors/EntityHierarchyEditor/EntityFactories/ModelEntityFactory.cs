// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

using Stride.Core;
using Stride.Core.Assets.Editor.Services;
using Stride.Assets.Models;
using Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels;
using Stride.Engine;
using Stride.Rendering;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.EntityFactories
{
    [Display(10, "Model", "Model")]
    public class ModelEntityFactory : EntityFactory
    {
        [ModuleInitializer]
        internal static void RegisterCategory()
        {
            EntityFactoryCategory.RegisterCategory(10, "Model");
        }

        public override async Task<Entity> CreateEntity(EntityHierarchyItemViewModel parent)
        {
            var model = await PickupAsset(parent.Editor.Session, new [] { typeof(IModelAsset) });
            if (model is null)
                return null;

            var name = ComputeNewName(parent, model.Name);
            var component = new ModelComponent { Model = ContentReferenceHelper.CreateReference<Model>(model) };
            return await CreateEntityWithComponent(name, component);
        }
    }

    // TODO: Hierarchies are currently not supported
    //[Display(20, "Instanced Model with Instance", "Model")]
    //public class InstancedModelWithInstanceEntityFactory : EntityFactory
    //{
    //    public override async Task<Entity> CreateEntity(EntityHierarchyItemViewModel parent)
    //    {
    //        var model = await PickupAsset(parent.Editor.Session, new[] { typeof(IModelAsset) });
    //        if (model is null)
    //            return null;

    //        var name = ComputeNewName(parent, model.Name);
    //        var component = new ModelComponent { Model = ContentReferenceHelper.CreateReference<Model>(model) };
    //        var instancingComponent = new InstancingComponent();
    //        var entity = await CreateEntityWithComponent(name, component, instancingComponent);

    //        var instanceName = ComputeNewName(parent, "Instance");
    //        var instanceComponent = new InstanceComponent();
    //        var child = await CreateEntityWithComponent(instanceName, instanceComponent);

    //        entity.AddChild(child);

    //        return entity;
    //    }
    //}

    [Display(30, "Instanced Model", "Model")]
    public class InstancedModelEntityFactory : EntityFactory
    {
        public override async Task<Entity> CreateEntity(EntityHierarchyItemViewModel parent)
        {
            var model = await PickupAsset(parent.Editor.Session, new[] { typeof(IModelAsset) });
            if (model is null)
                return null;

            var name = ComputeNewName(parent, model.Name);
            var component = new ModelComponent { Model = ContentReferenceHelper.CreateReference<Model>(model) };
            var instancingComponent = new InstancingComponent();
            return await CreateEntityWithComponent(name, component, instancingComponent);
        }
    }

    [Display(40, "Model Instance", "Model")]
    public class InstanceEntityFactory : EntityFactory
    {
        public override Task<Entity> CreateEntity(EntityHierarchyItemViewModel parent)
        {
            var name = ComputeNewName(parent, "Instance");
            var component = new InstanceComponent();
            return CreateEntityWithComponent(name, component);
        }
    }
}
