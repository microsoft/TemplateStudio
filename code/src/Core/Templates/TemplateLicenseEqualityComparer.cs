using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class TemplateLicenseEqualityComparer : IEqualityComparer<TemplateLicense>
    {
        public bool Equals(TemplateLicense x, TemplateLicense y)
        {
            if (x == null && x == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }
            else if (x.Url == y.Url)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(TemplateLicense obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.Url))
            {
                return 0;
            }

            return obj.Url.GetHashCode();
        }
    }
}
