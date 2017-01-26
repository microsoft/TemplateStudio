using Microsoft.Templates.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    public class FileHealthFixture
    {
        public FileHealthWriter FileLogWriter;
        public FileHealthFixture()
        {
            FileLogWriter = new FileHealthWriter(new TestConfiguration());
        } 
    }
}
