// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Localization.Extensions
{
    public static class ConsoleExt
    {
        public static void WriteSuccess(string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(string.Concat(value));
            Console.ResetColor();
        }
        public static void WriteWarning(string value)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(string.Concat(value));
            Console.ResetColor();
        }

        public static void WriteError(string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Concat(value));
            Console.ResetColor();
        }
    }
}
