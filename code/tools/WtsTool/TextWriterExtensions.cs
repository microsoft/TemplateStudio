// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WtsTool
{
    public static class TextWriterExtensions
    {
        public static void WriteException(this TextWriter writer, Exception ex, string title = "")
        {
            writer.WriteLine(new string('!', 20));
            writer.WriteLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                writer.WriteLine(ex.ToString());
            }
            else
            {
                writer.WriteLine($"{title}\r\n{new string('-', 20)}\r\n{ex.ToString()}");
            }
        }

        public static void WriteHeader(this TextWriter writer, string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                writer.WriteLine(new string('#', data.Length + 20));
                writer.WriteLine($"{new string('#', 3)}{new string(' ', 7)}{data}{new string(' ', 7)}{new string('#', 3)}");
                writer.WriteLine(new string('#', data.Length + 20));
                writer.WriteLine();
            }
        }

        public static void WriteFooter(this TextWriter writer)
        {
            writer.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            writer.WriteLine();
        }

        public static void WriteCommandHeader(this TextWriter writer, string data)
        {
            writer.WriteLine($"## {data}");
            writer.WriteLine();
        }

        public static void WriteCommandText(this TextWriter writer, string data)
        {
            writer.WriteLine($"   {data}");
        }
    }
}
