// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#include "../../../../deps/NativePath/NativeThreading.h"
#include "CoreNative.h"

#ifdef __cplusplus
extern "C" {
#endif

	DLL_EXPORT_API void cnSleep(int milliseconds)
	{
		npThreadSleep(milliseconds);
	}

	DLL_EXPORT_API void cnSetup(void* printDebugPtr)
	{
		cnDebugPrintLine = (CnPrintDebugFunc)printDebugPtr;
	}

#ifdef __cplusplus
}
#endif
