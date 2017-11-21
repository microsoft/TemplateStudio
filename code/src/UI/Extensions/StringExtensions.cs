// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Media;

namespace Microsoft.Templates.UI.Extensions
{
    public static class StringExtensions
    {
        private static BrushConverter _converter = new BrushConverter();

        public static Brush AsBrush(this string rgb) => _converter.ConvertFrom(rgb) as Brush;
    }
}
