// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Services
{
    public interface IStyleValuesProvider
    {
        Brush GetColor(string className, string memberName);

        System.Drawing.Color GetThemedColor(string className, string memberName);

        double GetFontSize(string fontSizeResourceKey);

        FontFamily GetFontFamily();

        event EventHandler ThemeChanged;
    }
}
