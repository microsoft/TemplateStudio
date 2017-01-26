using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    internal static class HealthWriters
    {
        public static List<IHealthWriter> Available { get; } = new List<IHealthWriter>();
    }
}
