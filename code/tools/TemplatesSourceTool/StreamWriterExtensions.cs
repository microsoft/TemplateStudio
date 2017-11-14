using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplatesSourceTool
{
    public static class StreamWriterExtensions
    {
        public static void WriteException(this StreamWriter writer, Exception ex, string title = "")
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                writer.WriteLine(ex.ToString());
            }
            else
            {
                writer.WriteLine($"{title}\r\n{new string('-', 20)}\r\n{ex.ToString()}");
            }
                
        }
    }
}
