using Microsoft.Templates.Core.Injection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class InjectionPostAction : PostAction<string>
    {
        public InjectionPostAction(GenShell shell, string config) : base(shell, config)
        {
        }

        public override void Execute()
        {
            var injector = ContentInjectorFactory.Find(_config);

            //TODO: EXTRACT THIS SOMEWHERE
            var originalFilePath = _config.Replace(Path.GetExtension(_config), string.Empty);

            //TODO: VERIFY NOT NULL
            var resultContent = injector.Inject(File.ReadAllText(originalFilePath));
            //TODO: VERIFY NOT NULL
            File.WriteAllText(originalFilePath, resultContent);

            File.Delete(_config);
        }
    }
}
