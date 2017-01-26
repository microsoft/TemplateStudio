using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    public class TestConfiguration : Configuration
    {
        public override string RemoteTelemetryKey
        {
            get
            {
                return ""; //SECRET
            }
        }
    }
}
