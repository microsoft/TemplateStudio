using Microsoft.Templates.Core.Gen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class CleanLocAnchorPostAction : PostAction
    {
        //TODO: REVIEW THIS. IS DUPLICATED

        private const string AnchorPattern = @"\""" + LocAnchor.Tag + @"(?<Key>\w+)~(?<Value>.+?)\""";

        public override void Execute()
        {
            var projectFiles = Directory.EnumerateFiles(GenContext.Current.OutputPath, "*", SearchOption.AllDirectories);

            foreach (var file in projectFiles)
            {
                var fileContent = File.ReadAllText(file);

                var modified = CleanUpAnchors(ref fileContent);

                if (modified)
                {
                    File.WriteAllText(file, fileContent);
                }

            }
        }

        private static bool CleanUpAnchors(ref string fileContent)
        {
            var modified = false;

            if (fileContent.IndexOf(LocAnchor.Tag) > -1)
            {
                var matches = Regex.Matches(fileContent, AnchorPattern);
                for (int i = 0; i < matches.Count; i++)
                {
                    var m = matches[i];

                    var replacement = $"\"{m.Groups["Value"].Value}\"";
                    fileContent = fileContent.Replace(m.Value, replacement);

                    modified = true;
                }
            }

            return modified;
        }
    }
}
