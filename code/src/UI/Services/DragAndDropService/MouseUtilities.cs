// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Services
{
    public class MouseUtilities
    {
        public static Point GetMousePosition(Visual relativeTo)
        {
            var mouse = default(NativeMethods.Win32Point);
            NativeMethods.GetCursorPos(ref mouse);

            return relativeTo.PointFromScreen(new Point(mouse.X, mouse.Y));
        }
    }
}
