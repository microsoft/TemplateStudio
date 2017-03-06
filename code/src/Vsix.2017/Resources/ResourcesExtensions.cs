using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Templates.Extension
{
    internal static class ResourcesExtensions
    {
        public static string UseParams(this string res, params object[] args)
        {
            string pattern = @"(\{\d\})+";
            if (Regex.IsMatch(res, pattern))
            {
                try
                {
                    return String.Format(res, args);
                }
                catch
                {
                    return res + "<INVALID FORMAT>";
                }
            }
            else
            {
                return res;
            }
        }

    }
}
