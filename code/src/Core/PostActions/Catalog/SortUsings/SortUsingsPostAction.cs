// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Templates.Core.PostActions.Catalog.SortUsings
{
    public class SortUsingsPostAction : PostAction
    {
        public override void Execute()
        {
            var classFiles = Directory
                                .EnumerateFiles(Gen.GenContext.Current.OutputPath, "*.cs", SearchOption.AllDirectories)
                                .ToList();

            foreach (var classFile in classFiles)
            {
                var fileContent = File.ReadAllLines(classFile).ToList();
                var sortResult = fileContent.SortUsings();

                if (sortResult)
                {
                    File.WriteAllLines(classFile, fileContent, Encoding.UTF8);
                }
            }
        }
    }
}
