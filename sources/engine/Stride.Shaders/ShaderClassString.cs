// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

using Stride.Core;
using Stride.Core.Serialization;

namespace Stride.Shaders
{
    /// <summary>
    ///   A shader class based on a source code <see cref="System.String"/>. Used for mixins.
    /// </summary>
    [DataContract("ShaderClassString")]
    public sealed class ShaderClassString : ShaderClassCode, IEquatable<ShaderClassString>
    {
        /// <summary>
        ///   Gets the source code of this shader class as a <see cref="System.String"/>,
        ///   using Stride SDSL syntax.
        /// </summary>
        /// <value>The source code of the shader class.</value>
        public string ShaderSourceCode { get; set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ShaderClassString"/> class.
        /// </summary>
        public ShaderClassString() { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ShaderClassString"/> class.
        /// </summary>
        /// <param name="className">Name of the shader class.</param>
        public ShaderClassString(string className, string shaderSourceCode)
            : this(className, shaderSourceCode, null)
        { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ShaderClassString"/> class.
        /// </summary>
        /// <param name="className">Name of the shader class.</param>
        /// <param name="genericArguments">Generic arguments that the shader class defines.</param>
        public ShaderClassString(string className, string shaderSourceCode, params string[] genericArguments)
        {
            ClassName = className;
            ShaderSourceCode = shaderSourceCode;
            GenericArguments = genericArguments;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ShaderClassString"/> class.
        /// </summary>
        /// <param name="className">Name of the shader class.</param>
        /// <param name="genericArguments">Generic arguments that the shader class defines.</param>
        public ShaderClassString(string className, string shaderSourceCode, params object[] genericArguments)
        {
            ClassName = className;
            ShaderSourceCode = shaderSourceCode;

            if (genericArguments != null)
            {
                GenericArguments = new string[genericArguments.Length];
                for (int i = 0; i < genericArguments.Length; ++i)
                {
                    var genArg = genericArguments[i];
                    if (genArg is bool boolArg)
                        GenericArguments[i] = boolArg ? "true" : "false";
                    else
                        GenericArguments[i] = genArg == null ? "null" : Convert.ToString(genArg, CultureInfo.InvariantCulture);
                }
            }
        }

        public bool Equals(ShaderClassString shaderClassString)
        {
            if (shaderClassString is null)
                return false;
            if (ReferenceEquals(this, shaderClassString))
                return true;

            return string.Equals(ClassName, shaderClassString.ClassName) &&
                   Utilities.Compare(GenericArguments, shaderClassString.GenericArguments);
        }

        public override bool Equals(object other)
        {
            if (other is null)
                return false;
            if (other is ShaderClassString otherShader)
                return Equals(otherShader);

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = ClassName?.GetHashCode() ?? 0;
                if (GenericArguments != null)
                {
                    foreach (var current in GenericArguments)
                        hashCode = (hashCode * 397) ^ (current?.GetHashCode() ?? 0);
                }

                return hashCode;
            }
        }

        public override object Clone()
        {
            return new ShaderClassString(ClassName, ShaderSourceCode, GenericArguments = GenericArguments != null ? GenericArguments.ToArray() : null);
        }

        public override string ToString()
        {
            return ToClassName();
        }
    }
}
