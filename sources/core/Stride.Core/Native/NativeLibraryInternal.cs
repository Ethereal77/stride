// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Core
{
    /// <summary>
    ///   Internally this class is used by the ExecServer project in order to copy
    ///   native DLLs to shadow copy folders.
    /// </summary>
    internal static class NativeLibraryInternal
    {
        private const string AppDomainCustomDllPathKey = "native_";

#if !STRIDE_RUNTIME_CORECLR
        public static void SetShadowPathForNativeDll(AppDomain appDomain, string dllFileName, string dllPath)
        {
            if (dllFileName == null) throw new ArgumentNullException("dllFileName");
            if (dllPath == null) throw new ArgumentNullException("dllPath");
            var key = AppDomainCustomDllPathKey + dllFileName.ToLowerInvariant();
            appDomain.SetData(key, dllPath);
        }
#endif

        public static string GetShadowPathForNativeDll(string dllFileName)
        {
#if !STRIDE_RUNTIME_CORECLR
            if (dllFileName == null) throw new ArgumentNullException("dllFileName");
            var key = AppDomainCustomDllPathKey + dllFileName.ToLowerInvariant();
            return (string)AppDomain.CurrentDomain.GetData(key);
#else
            return null;
#endif
        }
    }
}
