// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Linq;

using Stride.Core.Annotations;
using Stride.Core.Reflection;
using Stride.Core.Quantum;
using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.Quantum.Presenters;

namespace Stride.Core.Assets.Editor.Quantum.NodePresenters.Commands
{
    public class AddPrimitiveKeyCommand : SyncNodePresenterCommandBase
    {
        /// <summary>
        ///   The name of this command.
        /// </summary>
        public const string CommandName = "AddPrimitiveKey";

        /// <inheritdoc/>
        public override string Name => CommandName;

        /// <inheritdoc/>
        public override CombineMode CombineMode => CombineMode.CombineOnlyForAll;

        /// <inheritdoc/>
        public override bool CanAttach(INodePresenter nodePresenter)
        {
            // We are in a dictionary
            if (!(nodePresenter.Descriptor is DictionaryDescriptor dictionaryDescriptor))
                return false;

            // It is not read-only
            var memberCollection = (nodePresenter as MemberNodePresenter)?.MemberAttributes
                .OfType<MemberCollectionAttribute>()
                .FirstOrDefault() ??
                nodePresenter.Descriptor.Attributes
                .OfType<MemberCollectionAttribute>()
                .FirstOrDefault();
            if (memberCollection?.ReadOnly == true)
                return false;

            // It can construct the key type
            if (!AddNewItemCommand.CanConstruct(dictionaryDescriptor.KeyType))
                return false;

            // And it can construct the value type
            var elementType = dictionaryDescriptor.ValueType;
            return AddNewItemCommand.CanAdd(elementType);
        }

        /// <inheritdoc/>
        protected override void ExecuteSync(INodePresenter nodePresenter, object parameter, object preExecuteResult)
        {
            var assetNodePresenter = nodePresenter as IAssetNodePresenter;
            var dictionaryDescriptor = (DictionaryDescriptor)nodePresenter.Descriptor;
            var value = nodePresenter.Value;

            NodeIndex newKey;
            if(dictionaryDescriptor.KeyType == typeof(string))
                newKey = GenerateStringKey(value, dictionaryDescriptor, parameter as string);
            else if (dictionaryDescriptor.KeyType.IsEnum)
                newKey = new NodeIndex(parameter);
            else
                newKey = new NodeIndex(Activator.CreateInstance(dictionaryDescriptor.KeyType));

            var newItem = dictionaryDescriptor.ValueType.Default();
            var instance = CreateInstance(dictionaryDescriptor.ValueType);
            if (!AddNewItemCommand.IsReferenceType(dictionaryDescriptor.ValueType) &&
                (assetNodePresenter is null || !assetNodePresenter.IsObjectReference(instance)))
                newItem = instance;

            nodePresenter.AddItem(newItem, newKey);
        }

        /// <summary>
        ///   Creates an instance of the specified type using that type's default constructor.
        /// </summary>
        /// <param name="type">The type of object to create.</param>
        /// <returns>A reference to the newly created object.</returns>
        /// <seealso cref="Activator.CreateInstance(Type)"/>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is a <c>null</c> reference.</exception>
        private static object CreateInstance(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            // Abstract type cannot be instantiated
            if (type.IsAbstract)
                return null;

            // String is a special case
            if (type == typeof(string))
                return string.Empty;

            // NOTE: Types not having a public parameterless constructor will throw a MissingMethodException at this point.
            //       This is intended as YAML serialization requires this constructor.
            return ObjectFactoryRegistry.NewInstance(type);
        }

        internal static NodeIndex GenerateStringKey(object dictionary, ITypeDescriptor descriptor, string baseValue)
        {
            // TODO: Use a dialog service and popup a message when the given key is invalid
            const string defaultKey = "Key";

            if (string.IsNullOrWhiteSpace(baseValue))
                baseValue = defaultKey;

            var i = 1;
            string baseName = baseValue;
            var dictionaryDescriptor = (DictionaryDescriptor) descriptor;
            while (dictionaryDescriptor.ContainsKey(dictionary, baseValue))
            {
                baseValue = baseName + " " + ++i;
            }

            return new NodeIndex(baseValue);
        }
    }
}
