// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WtsTool
{
    public class ConsoleHelper
    {
        public static bool Available { get => GetConsoleAvailability(); }

        private static bool GetConsoleAvailability()
        {
            try
            {
                Console.CursorVisible = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void HideCursor()
        {
            if (Available)
            {
                Console.CursorVisible = false;
            }
        }

        public static void ShowCursor()
        {
            if (Available)
            {
                Console.CursorVisible = true;
            }
        }

        public static void OverrideWriteLine()
        {
            if (Available)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }
    }
}
