// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.UI.Controls
{
    public partial class EditText
    {
        private static void InitializeStaticImpl()
        {
        }

        private void InitializeImpl()
        {
        }

        private int GetLineCountImpl()
        {
            if (Font == null)
                return 1;

            return text.Split('\n').Length;
        }

        private void OnMaxLinesChangedImpl()
        {
        }

        private void OnMinLinesChangedImpl()
        {
        }

        private void UpdateTextToEditImpl()
        {
        }

        private void UpdateInputTypeImpl()
        {
        }

        private void UpdateSelectionFromEditImpl()
        {
        }

        private void UpdateSelectionToEditImpl()
        {
        }

        private void OnTouchUpImpl(TouchEventArgs args)
        {
        }
    }
}
