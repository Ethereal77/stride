/*
   LZ4 HC - High Compression Mode of LZ4
   Header File
   Copyright (C) 2011-2013, Yann Collet.
   BSD 2-Clause License (http://www.opensource.org/licenses/bsd-license.php)
*/

#pragma once

#include "../CoreNative.h"

#if defined (__cplusplus)
extern "C" {
#endif


DLL_EXPORT_API int LZ4_compressHC (const char* source, char* dest, int inputSize);

/*
LZ4_compressHC :
	return : the number of bytes in compressed buffer dest
	note : destination buffer must be already allocated. 
		To avoid any problem, size it to handle worst cases situations (input data not compressible)
		Worst case size evaluation is provided by function LZ4_compressBound() (see "lz4.h")
*/

DLL_EXPORT_API int LZ4_compressHC_limitedOutput (const char* source, char* dest, int inputSize, int maxOutputSize);

/*
LZ4_compress_limitedOutput() :
	Compress 'inputSize' bytes from 'source' into an output buffer 'dest' of maximum size 'maxOutputSize'.
	If it cannot achieve it, compression will stop, and result of the function will be zero.
	This function never writes outside of provided output buffer.

	inputSize  : Max supported value is ~1.9GB
	maxOutputSize : is maximum allowed size into the destination buffer (which must be already allocated)
	return : the number of output bytes written in buffer 'dest'
			 or 0 if the compression fails
*/


/* Note :
Decompression functions are provided within regular LZ4 source code (see "lz4.h") (BSD license)
*/


#if defined (__cplusplus)
}
#endif
