// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;

using Stride.Core.Reflection;
using Stride.Core.Yaml.Events;
using Stride.Core.Yaml.Serialization.Serializers;

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    ///   Represents an object that can serialize and deserialize other objects into and from YAML documents.
    /// </summary>
    public sealed class Serializer
    {
        internal readonly IYamlSerializable ObjectSerializer;
        internal readonly RoutingSerializer RoutingSerializer;
        internal readonly ITypeDescriptorFactory TypeDescriptorFactory;

        private static readonly IYamlSerializableFactory[] DefaultFactories = new IYamlSerializableFactory[]
        {
            new PrimitiveSerializer(),
            new DictionarySerializer(),
            new CollectionSerializer(),
            new ArraySerializer(),
            new ObjectSerializer()
        };

        /// <summary>
        ///   Gets the settings for configuring this serializer.
        /// </summary>
        /// <value>The settings for this serializer.</value>
        public SerializerSettings Settings { get; }


        /// <summary>
        ///   Initializes a new instance of the <see cref="Serializer"/> class.
        /// </summary>
        public Serializer() : this(null) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Serializer"/> class.
        /// </summary>
        /// <param name="settings">The settings for the serializer.</param>
        public Serializer(SerializerSettings settings)
        {
            Settings = settings ?? new SerializerSettings();
            TypeDescriptorFactory = CreateTypeDescriptorFactory();

            RegisterSerializerFactories();

            ObjectSerializer = CreateProcessor(out RoutingSerializer routingSerializer);
            RoutingSerializer = routingSerializer;
        }


        /// <summary>
        ///   Serializes the specified object to a <see cref="string"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A YAML <see cref="string"/> of the whole object graph.</returns>
        public string Serialize(object obj)
        {
            var stringWriter = new StringWriter();
            Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }

        /// <summary>
        ///   Serializes the specified object to a <see cref="string"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="contextSettings">The context settings. Specify <c>null</c> to use the defailt settings.</param>
        /// <returns>A YAML <see cref="string"/> of the whole object graph.</returns>
        public string Serialize(object obj, Type expectedType, SerializerContextSettings contextSettings = null)
        {
            var stringWriter = new StringWriter();
            Serialize(stringWriter, obj, expectedType, contextSettings);
            return stringWriter.ToString();
        }

        /// <summary>
        ///   Serializes the specified object.
        /// </summary>
        /// <param name="stream">The stream to which to serialize the object.</param>
        /// <param name="obj">The object to serialize.</param>
        public void Serialize(Stream stream, object obj)
        {
            Serialize(stream, obj, expectedType: null);
        }

        /// <summary>
        ///   Serializes the specified object.
        /// </summary>
        /// <param name="stream">The stream to which to serialize the object.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="contextSettings">The context settings. Specify <c>null</c> to use the defailt settings.</param>
        public void Serialize(Stream stream, object obj, Type expectedType, SerializerContextSettings contextSettings = null)
        {
            var writer = new StreamWriter(stream);

            try
            {
                Serialize(writer, obj, expectedType, contextSettings);
            }
            finally
            {
                try
                {
                    writer.Flush();
                }
                catch { }
            }
        }

        /// <summary>
        ///   Serializes the specified object.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter" /> where to serialize the object.</param>
        /// <param name="obj">The object to serialize.</param>
        public void Serialize(TextWriter writer, object obj)
        {
            Serialize(new Emitter(writer, Settings.PreferredIndent), obj);
        }

        /// <summary>
        ///   Serializes the specified object.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter" /> where to serialize the object.</param>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="type">The static type of the object to serialize.</param>
        /// <param name="contextSettings">The context settings.</param>
        public void Serialize(TextWriter writer, object obj, Type type, SerializerContextSettings contextSettings = null)
        {
            Serialize(new Emitter(writer, Settings.PreferredIndent), obj, type, contextSettings);
        }

        /// <summary>
        ///   Serializes the specified object.
        /// </summary>
        /// <param name="emitter">The <see cref="IEmitter" /> where to serialize the object.</param>
        /// <param name="obj">The object to serialize.</param>
        public void Serialize(IEmitter emitter, object obj)
        {
            Serialize(emitter, obj, obj is null ? typeof(object) : null);
        }

        /// <summary>
        ///   Serializes the specified object.
        /// </summary>
        /// <param name="emitter">The <see cref="IEmitter" /> where to serialize the object.</param>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="type">The static type of the object to serialize.</param>
        /// <param name="contextSettings">The context settings.</param>
        /// <exception cref="ArgumentNullException"><paramref name="emitter"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="type"/> is a <c>null</c> reference and does not specify the type for <paramref name="obj"/>
        ///   which is also a <c>null</c> reference.
        /// </exception>
        public void Serialize(IEmitter emitter, object obj, Type type, SerializerContextSettings contextSettings = null)
        {
            if (emitter is null)
                throw new ArgumentNullException(nameof(emitter));

            if (obj is null && type is null)
                throw new ArgumentNullException(nameof(type));

            // Configure the emitter
            // TODO: The current emitter is not enough configurable to format its output. This should be improved
            if (emitter is Emitter defaultEmitter)
            {
                defaultEmitter.ForceIndentLess = Settings.IndentLess;
            }

            var context = new SerializerContext(this, contextSettings) { Emitter = emitter, Writer = CreateEmitter(emitter) };

            // Serialize the document
            context.Writer.StreamStart();
            context.Writer.DocumentStart();
            var objectContext = new ObjectContext(context, obj, context.FindTypeDescriptor(type)) { Style = DataStyle.Any };
            context.Serializer.ObjectSerializer.WriteYaml(ref objectContext);
            context.Writer.DocumentEnd();
            context.Writer.StreamEnd();
        }


        /// <summary>
        ///   Deserializes an object from the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>A deserialized object.</returns>
        public object Deserialize(Stream stream)
        {
            return Deserialize(stream, expectedType: null);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>A deserialized object.</returns>
        public object Deserialize(TextReader reader)
        {
            return Deserialize(reader, expectedType: null);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="Stream" /> with an expected specific type.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="contextSettings">The context settings.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is a <c>null</c> reference.</exception>
        public object Deserialize(Stream stream, Type expectedType, SerializerContextSettings contextSettings = null)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            return Deserialize(new StreamReader(stream), expectedType, existingObject: null, contextSettings);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="Stream" /> with an expected specific type.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="contextSettings">The context settings.</param>
        /// <param name="context">The context used to deserialize this object.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is a <c>null</c> reference.</exception>
        public object Deserialize(Stream stream, Type expectedType, SerializerContextSettings contextSettings, out SerializerContext context)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            return Deserialize(new StreamReader(stream), expectedType, existingObject: null, contextSettings, out context);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="Stream" /> with an expected specific type.
        /// </summary>
        /// <typeparam name="T">The expected type</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is a <c>null</c> reference.</exception>
        public T Deserialize<T>(Stream stream)
        {
            return (T) Deserialize(stream, typeof(T));
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="TextReader" /> with an expected specific type.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="existingObject">The object to deserialize into. If <c>null</c> is specified (the default), a new object will be created.</param>
        /// <param name="contextSettings">The context settings.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is a <c>null</c> reference.</exception>
        public object Deserialize(TextReader reader, Type expectedType, object existingObject = null, SerializerContextSettings contextSettings = null)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            return Deserialize(new EventReader(new Parser(reader)), expectedType, existingObject, contextSettings);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="TextReader" /> with an expected specific type.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="existingObject">The object to deserialize into. If <c>null</c> is specified (the default), a new object will be created.</param>
        /// <param name="contextSettings">The context settings.</param>
        /// <param name="context">The context used to deserialize this object.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is a <c>null</c> reference.</exception>
        public object Deserialize(TextReader reader, Type expectedType, object existingObject, SerializerContextSettings contextSettings, out SerializerContext context)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            return Deserialize(new EventReader(new Parser(reader)), expectedType, existingObject, contextSettings, out context);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="TextReader" /> with an expected specific type.
        /// </summary>
        /// <typeparam name="T">The expected type</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="existingObject">The object to deserialize into. If <c>null</c> is specified (the default), a new object will be created.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is a <c>null</c> reference.</exception>
        public T Deserialize<T>(TextReader reader, object existingObject = null)
        {
            return (T) Deserialize(reader, typeof(T), existingObject);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="string"/>.
        /// </summary>
        /// <param name="fromText">Text string to deserialize into an object.</param>
        /// <param name="existingObject">The object to deserialize into. If <c>null</c> is specified (the default), a new object will be created.</param>
        /// <returns>A deserialized object.</returns>
        public object Deserialize(string fromText, object existingObject = null)
        {
            return Deserialize(fromText, null, existingObject);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="string"/> with an expected specific type.
        /// </summary>
        /// <param name="fromText">Text string to deserialize into an object.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="existingObject">The object to deserialize into. If <c>null</c> is specified (the default), a new object will be created.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fromText"/> is a <c>null</c> reference.</exception>
        public object Deserialize(string fromText, Type expectedType, object existingObject = null)
        {
            if (fromText is null)
                throw new ArgumentNullException(nameof(fromText));

            return Deserialize(new StringReader(fromText), expectedType, existingObject);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="string"/> with an expected specific type.
        /// </summary>
        /// <param name="fromText">Text string to deserialize into an object.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="existingObject">The object to deserialize into. If <c>null</c> is specified (the default), a new object will be created.</param>
        /// <param name="context">The context used to deserialize this object.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fromText"/> is a <c>null</c> reference.</exception>
        public object Deserialize(string fromText, Type expectedType, object existingObject, out SerializerContext context)
        {
            if (fromText is null)
                throw new ArgumentNullException(nameof(fromText));

            return Deserialize(new StringReader(fromText), expectedType, existingObject, null, out context);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="string"/> with an expected specific type.
        /// </summary>
        /// <typeparam name="T">The expected type</typeparam>
        /// <param name="fromText">Text string to deserialize into an object.</param>
        /// <returns>A deserialized object.</returns>
        public T Deserialize<T>(string fromText)
        {
            return (T) Deserialize(fromText, typeof(T));
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="EventReader" /> with an expected specific type.
        /// </summary>
        /// <typeparam name="T">The expected type</typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is a <c>null</c> reference.</exception>
        public T Deserialize<T>(EventReader reader)
        {
            return (T) Deserialize(reader, typeof(T));
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="string"/>. with an expected specific type.
        /// </summary>
        /// <typeparam name="T">The expected type</typeparam>
        /// <param name="fromText">Text string to deserialize into an object.</param>
        /// <param name="existingObject">The object to deserialize into.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fromText"/> is a <c>null</c> reference.</exception>
        ///
        /// Note: These need a different name, because otherwise they will conflict with existing Deserialize(string,Type). They are new so the difference should not matter
        public T DeserializeInto<T>(string fromText, T existingObject)
        {
            return (T) Deserialize(fromText, typeof(T), existingObject);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="EventReader" /> with an expected specific type.
        /// </summary>
        /// <typeparam name="T">The expected type</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="existingObject">The object to deserialize into.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is a <c>null</c> reference.</exception>
        public T DeserializeInto<T>(EventReader reader, T existingObject)
        {
            return (T) Deserialize(reader, typeof(T), existingObject);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="string"/>. with an expected specific type.
        /// </summary>
        /// <typeparam name="T">The expected type</typeparam>
        /// <param name="fromText">Text string to deserialize into an object.</param>
        /// <param name="context">The context.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fromText"/> is a <c>null</c> reference.</exception>
        public T Deserialize<T>(string fromText, out SerializerContext context)
        {
            return (T) Deserialize(fromText, typeof(T), existingObject: null, out context);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="EventReader" /> with an expected specific type.
        /// </summary>
        /// <typeparam name="T">The expected type</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="context">The context used to deserialize this object.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is a <c>null</c> reference.</exception>
        public T Deserialize<T>(EventReader reader, out SerializerContext context)
        {
            return (T) Deserialize(reader, typeof(T), existingObject: null, contextSettings: null, out context);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="string"/>. with an expected specific type.
        /// </summary>
        /// <typeparam name="T">The expected type</typeparam>
        /// <param name="fromText">Text string to deserialize into an object.</param>
        /// <param name="existingObject">The object to deserialize into.</param>
        /// <param name="context">The context used to deserialize this object.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fromText"/> is a <c>null</c> reference.</exception>
        // NOTE: These need a different name, because otherwise they will conflict with existing Deserialize(string,Type). They are new so the difference should not matter
        public T DeserializeInto<T>(string fromText, T existingObject, out SerializerContext context)
        {
            return (T) Deserialize(fromText, typeof(T), existingObject, out context);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="EventReader" /> with an expected specific type.
        /// </summary>
        /// <typeparam name="T">The expected type</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="existingObject">The object to deserialize into.</param>
        /// <param name="context">The context used to deserialize this object.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is a <c>null</c> reference.</exception>
        public T DeserializeInto<T>(EventReader reader, T existingObject, out SerializerContext context)
        {
            return (T) Deserialize(reader, typeof(T), existingObject, null, out context);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="EventReader" /> with an expected specific type.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="expectedType">The expected type, maybe null.</param>
        /// <param name="existingObject">An existing object. May be <c>null</c>.</param>
        /// <param name="contextSettings">The context settings.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is a <c>null</c> reference.</exception>
        public object Deserialize(EventReader reader, Type expectedType, object existingObject = null, SerializerContextSettings contextSettings = null)
        {
            return Deserialize(reader, expectedType, existingObject, contextSettings, out _);
        }

        /// <summary>
        ///   Deserializes an object from the specified <see cref="EventReader" /> with an expected specific type.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="expectedType">The expected type, maybe null.</param>
        /// <param name="existingObject">An existing object. May be <c>null</c>.</param>
        /// <param name="contextSettings">The context settings.</param>
        /// <param name="context">The context used to deserialize the object.</param>
        /// <returns>A deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is a <c>null</c> reference.</exception>
        public object Deserialize(EventReader reader, Type expectedType, object existingObject, SerializerContextSettings contextSettings, out SerializerContext context)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            var hasStreamStart = reader.Allow<StreamStart>() != null;
            var hasDocumentStart = reader.Allow<DocumentStart>() != null;
            context = null;

            object result = null;
            if (!reader.Accept<DocumentEnd>() && !reader.Accept<StreamEnd>())
            {
                context = new SerializerContext(this, contextSettings) { Reader = reader };
                var node = context.Reader.Parser.Current;
                try
                {
                    var objectContext = new ObjectContext(context, existingObject, context.FindTypeDescriptor(expectedType));
                    result = context.Serializer.ObjectSerializer.ReadYaml(ref objectContext);
                }
                catch (YamlException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    ex = ex.Unwrap();
                    throw new YamlException(node, ex);
                }
            }

            if (hasDocumentStart)
            {
                reader.Expect<DocumentEnd>();
            }

            if (hasStreamStart)
            {
                reader.Expect<StreamEnd>();
            }

            return result;
        }

        public IYamlSerializable GetSerializer(SerializerContext context, ITypeDescriptor typeDescriptor)
        {
            return Settings.SerializerFactorySelector.GetSerializer(context, typeDescriptor);
        }

        private void RegisterSerializerFactories()
        {
            // Add registered factories
            foreach (var factory in Settings.AssemblyRegistry.SerializableFactories)
            {
                Settings.SerializerFactorySelector.TryAddFactory(factory);
            }

            // Add default factories
            foreach (var defaultFactory in DefaultFactories)
            {
                Settings.SerializerFactorySelector.TryAddFactory(defaultFactory);
            }
            Settings.SerializerFactorySelector.Seal();
        }

        private IYamlSerializable CreateProcessor(out RoutingSerializer routingSerializer)
        {
            routingSerializer = new RoutingSerializer(Settings.SerializerFactorySelector);

            var tagTypeSerializer = new TagTypeSerializer();
            routingSerializer.Prepend(tagTypeSerializer);
            if (Settings.EmitAlias)
            {
                var anchorSerializer = new AnchorSerializer();
                tagTypeSerializer.Prepend(anchorSerializer);
            }

            Settings.ChainedSerializerFactory?.Invoke(routingSerializer.First);
            return routingSerializer.First;
        }

        private ITypeDescriptorFactory CreateTypeDescriptorFactory()
        {
            return new TypeDescriptorFactory(Settings.Attributes, Settings.EmitDefaultValues, Settings.NamingConvention, Settings.ComparerForKeySorting);
        }

        private IEventEmitter CreateEmitter(IEmitter emitter)
        {
            var writer = (IEventEmitter) new WriterEventEmitter(emitter);

            if (Settings.EmitJsonComptible)
            {
                writer = new JsonEventEmitter(writer);
            }
            return Settings.EmitAlias ? new AnchorEventEmitter(writer) : writer;
        }
    }
}
