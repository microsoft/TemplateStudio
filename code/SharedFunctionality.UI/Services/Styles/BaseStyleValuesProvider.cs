// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Services
{
    public abstract class BaseStyleValuesProvider
    {
        public abstract Brush GetColor(string className, string memberName);

        public abstract System.Drawing.Color GetThemedColor(string className, string memberName);

        public abstract Style GetStyle(object resourceKey);

        public abstract double GetFontSize(string fontSizeResourceKey);

        public abstract FontFamily GetFontFamily();

        public abstract event EventHandler ThemeChanged;

        public virtual void UnsubscribeEventHandlers()
        {
        }
    }
}
