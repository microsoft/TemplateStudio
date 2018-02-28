// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace AutomatedUITests.Utils
{
    public static class VirtualKeyboard
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private const int KeyeventfExtendedkey = 1;
        private const int KeyeventfKeyup = 2;

        // Values from System.Windows.Forms.Keys
        private const byte WindowsKey = 91;
        private const byte LetterM = 77;

        public static void MinimizeAllWindows()
        {
            KeyDown(WindowsKey);
            KeyDown(LetterM);
            KeyUp(LetterM);
            KeyUp(WindowsKey);
        }

        public static void KeyDown(byte vKey)
        {
            keybd_event(vKey, 0, KeyeventfExtendedkey, 0);
        }

        public static void KeyUp(byte vKey)
        {
            keybd_event(vKey, 0, KeyeventfExtendedkey | KeyeventfKeyup, 0);
        }
    }
}