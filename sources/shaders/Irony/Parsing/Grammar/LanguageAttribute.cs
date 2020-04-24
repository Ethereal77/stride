// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2011 Irony - Roman Ivantsov
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.Irony.Parsing {

  [AttributeUsage(AttributeTargets.Class)]
  public class LanguageAttribute : Attribute {
    public LanguageAttribute() : this(null) { }
    public LanguageAttribute(string languageName) : this(languageName, "1.0", string.Empty) { }

    public LanguageAttribute(string languageName, string version, string description) {
      _languageName = languageName;
      _version = version;
      _description = description;
    }
    
    public string LanguageName {
      get { return _languageName; }
    } string _languageName;

    public string Version {
      get { return _version; }
    } string _version;

    public string Description {
      get { return _description; }
    } string _description; 

    public static LanguageAttribute GetValue(Type grammarClass) {
      return grammarClass.GetTypeInfo().GetCustomAttribute<LanguageAttribute>(true);
    }

  }//class
}//namespace
