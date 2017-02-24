using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class SetDefaultSolutionConfigurationPostAction : PostAction
    {
        private const string Configuration = "Debug";
        private const string Platform = "x86";

        public SetDefaultSolutionConfigurationPostAction(GenShell shell) : base(shell)
        {
        }

        public override void Execute()
        {
            _shell.SetActiveConfigurationAndPlatform(Configuration, Platform);
        }
    }
}
