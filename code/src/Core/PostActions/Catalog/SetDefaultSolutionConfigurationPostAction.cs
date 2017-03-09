using Microsoft.Templates.Core.Gen;
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

        public override void Execute()
        {
            GenContext.ToolBox.Shell.SetActiveConfigurationAndPlatform(Configuration, Platform);
        }
    }
}
