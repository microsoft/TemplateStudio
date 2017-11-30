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
        public static bool Verbose { get; set; }

        public static void WriteException(this TextWriter writer, Exception ex, string title = "")
        {
            writer.WriteLine(">> ERROR");
            writer.WriteLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                writer.WriteLine(ex.ToString());
            }
            else
            {
                string exText = Verbose ? ex.ToString() : ex.Message;
                writer.WriteLine($"{title}\r\n{exText}");
            }
        }

        public static void WriteHeader(this TextWriter writer, string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                writer.WriteLine(new string('#', data.Length + 40));
                writer.WriteLine($"{new string('#', 3)}{new string(' ', 17)}{data}{new string(' ', 17)}{new string('#', 3)}");
                writer.WriteLine(new string('#', data.Length + 40));
                writer.WriteLine();
            }
        }

        public static void WriteFooter(this TextWriter writer, string data)
        {
            writer.WriteLine();
            writer.WriteLine($">> [{data}]");
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

        public static void WriteVerbose(this TextWriter writer, string data)
        {
            if (Verbose)
            {
                writer.WriteLine($"   {data}");
            }
        }
    }
}
