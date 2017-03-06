using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard
{
    public class GenException : Exception
    {
        public GenException(string name, string template, string reason) : base (string.Format(Resources.StringRes.ExceptionGenerating, template, name, reason))
        {
        }
    }
}
