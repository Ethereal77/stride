// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 NShader - Alexandre Mutel, Microsoft Corporation
// See the LICENSE.md file in the project root for full license information.

using System.Drawing;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

using Stride.VisualStudio.Classifiers;

namespace NShader
{
    public class NShaderColorableItem : ColorableItem
    {
        public Color HiForeColorLight { get; private set; }
        public Color HiForeColorDark { get; private set; }
        public COLORINDEX ForeColorLight { get; private set; }
        public COLORINDEX ForeColorDark { get; private set; }

        public NShaderColorableItem(VisualStudioTheme theme, string name, COLORINDEX foreColorLight, COLORINDEX foreColorDark, COLORINDEX backColor)
            : base(name, name, theme == VisualStudioTheme.Dark ? foreColorDark : foreColorLight, backColor, Color.Empty, Color.Empty, FONTFLAGS.FF_DEFAULT)
        {
            ForeColorLight = foreColorLight;
            ForeColorDark = foreColorDark;
        }

        public NShaderColorableItem(VisualStudioTheme theme, string name, COLORINDEX foreColorLight, COLORINDEX foreColorDark, COLORINDEX backColor, FONTFLAGS fontFlags)
            : base(name, name, theme == VisualStudioTheme.Dark ? foreColorDark : foreColorLight, backColor, Color.Empty, Color.Empty, fontFlags)
        {
            ForeColorLight = foreColorLight;
            ForeColorDark = foreColorDark;
        }

        public NShaderColorableItem(VisualStudioTheme theme, string name, string displayName, COLORINDEX foreColorLight, COLORINDEX foreColorDark, COLORINDEX backColor)
            : base(name, displayName, theme == VisualStudioTheme.Dark ? foreColorDark : foreColorLight, backColor, Color.Empty, Color.Empty, FONTFLAGS.FF_DEFAULT)
        {
            ForeColorLight = foreColorLight;
            ForeColorDark = foreColorDark;
        }

        public NShaderColorableItem(VisualStudioTheme theme, string name, string displayName, COLORINDEX foreColorLight, COLORINDEX foreColorDark, COLORINDEX backColor, FONTFLAGS fontFlags)
            : base(name, displayName, theme == VisualStudioTheme.Dark ? foreColorDark : foreColorLight, backColor, Color.Empty, Color.Empty, fontFlags)
        {
            ForeColorLight = foreColorLight;
            ForeColorDark = foreColorDark;
        }

        public NShaderColorableItem(VisualStudioTheme theme, string name, string displayName, COLORINDEX foreColorLight, COLORINDEX foreColorDark, COLORINDEX backColor, Color hiForeColorLight, Color hiForeColorDark, Color hiBackColor, FONTFLAGS fontFlags)
            : base(name, displayName, theme == VisualStudioTheme.Dark ? foreColorDark : foreColorLight, backColor, theme == VisualStudioTheme.Dark ? hiForeColorDark : hiForeColorLight,  hiBackColor, fontFlags)
        {
            ForeColorLight = foreColorLight;
            ForeColorDark = foreColorDark;

            HiForeColorLight = hiForeColorLight;
            HiForeColorDark = hiForeColorDark;
        }

        public override int GetMergingPriority(out int priority)
        {
           priority = 0x2000;
           return VSConstants.S_OK;
        }

        public override int GetColorData(int cdElement, out uint crColor)
        {
            return base.GetColorData(cdElement, out crColor);
        }

        public override int GetDefaultColors(COLORINDEX[] foreColor, COLORINDEX[] backColor)
        {
            return base.GetDefaultColors(foreColor, backColor);
        }
    }
}
