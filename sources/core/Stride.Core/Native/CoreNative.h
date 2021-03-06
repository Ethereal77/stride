// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#ifndef _CoreNative_h_
#define _CoreNative_h_

/*
 * Some platforms requires a special declaration before the function declaration to export them 
 * in the shared library. Defining NEED_DLL_EXPORT will define DLL_EXPORT_API to do the right thing
 * for those platforms.
 *
 * To export void foo(int a), do:
 *
 *   DLL_EXPORT_API void foo (int a);
 */
#ifdef NEED_DLL_EXPORT
#define DLL_EXPORT_API __declspec(dllexport)
#else
#define DLL_EXPORT_API
#endif

typedef void(*CnPrintDebugFunc)(const char* string);

DLL_EXPORT_API CnPrintDebugFunc cnDebugPrintLine;

#endif
