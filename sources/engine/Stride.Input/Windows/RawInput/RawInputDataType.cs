// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_UI_WINFORMS || STRIDE_UI_WPF

namespace Stride.Input.RawInput
{
    internal enum RawInputDataType
    {
        RID_HEADER = 0x10000005,
        RID_INPUT = 0x10000003
    }
}

#endif
