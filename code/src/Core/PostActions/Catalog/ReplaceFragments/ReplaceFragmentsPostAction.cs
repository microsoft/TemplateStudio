// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;

namespace Microsoft.Templates.Core.PostActions.Catalog.ReplaceFragments
{
    public class ReplaceFragmentsPostAction : PostAction
    {
        public override void Execute()
        {
            var classFiles = Directory
                .EnumerateFiles(Gen.GenContext.Current.OutputPath, "*.cs", SearchOption.AllDirectories)
                .ToList();

            foreach (var classFile in classFiles)
            {
                var fileContent = File.ReadAllLines(classFile).ToList();
                var sortResult = fileContent.ReplaceFragments();

                if (sortResult)
                {
                    File.WriteAllLines(classFile, fileContent);
                }
            }
        }
    }
}