// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Media;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.Templates.UI.V2Services
{
    public interface IStyleValuesProvider
    {
        Brush GetColor(ThemeResourceKey themeResourceKey);

        double GetFontSize(string fontSizeResourceKey);
    }
}
