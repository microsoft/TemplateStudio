using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergePostAction : PostAction<string>
    {
        public const string Extension = "_postaction.";

        public MergePostAction(string config) : base(config)
        {
        }

        public override void Execute()
        {
            var originalFilePath = _config.Replace(Extension, ".");

            var source = File.ReadAllLines(originalFilePath).ToList();
            var merge = File.ReadAllLines(_config).ToList();

            var result = source.Merge(merge);

            File.WriteAllLines(originalFilePath, result);
            File.Delete(_config);
        }
    }
}
