using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Gen
{
    public class GenToolBox
    {
        public TemplatesRepository Repo { get; }
        public GenShell Shell { get; }

        public GenToolBox(TemplatesRepository repo, GenShell shell)
        {
            Repo = repo;
            Shell = shell;
        }
    }
}
