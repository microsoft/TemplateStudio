using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Locations
{
    public abstract class TemplatesSource
    {
        protected const string SourceFolderName = "Templates";

        public abstract string Id { get; }
        public abstract void Adquire(string targetFolder);
    }
}
