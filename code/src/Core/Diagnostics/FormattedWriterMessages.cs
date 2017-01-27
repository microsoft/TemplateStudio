using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class FormattedWriterMessages
    {
        public const string ExHeader = "===================== Exception Info =====================";
        public const string ExFooter = "----------------------------------------------------------";
        public static string LogEntryStart
        {
            get
            {
                return $"[{DateTime.Now.ToString("yyyyMMdd hh:mm:ss.fff")}]\t{Environment.UserName}\t{Process.GetCurrentProcess().Id.ToString()}({System.Threading.Thread.CurrentThread.ManagedThreadId})\t";
            }
        }
    }
}
