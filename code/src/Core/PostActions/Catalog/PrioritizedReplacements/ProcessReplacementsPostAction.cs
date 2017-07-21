// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog.PrioritizedReplacements
{
    public class ProcessReplacementsPostAction : PostAction
    {
        public override void Execute()
        {
            // Apply all the prioritized removals
            // Must be done after all the postactions have been done to allow for postactions adding something that might be replaced
            Directory
                .EnumerateFiles(GenContext.Current.OutputPath, $"*{PriorityReplacePostAction.Extension}*", SearchOption.AllDirectories)
                .ToList()
                .ForEach(f => new PriorityReplacePostAction(f).Execute());

            // Now remove any priority comments as they shouldn't be seen in the geenrated code
            Directory
               .EnumerateFiles(GenContext.Current.OutputPath, "*.cs", SearchOption.AllDirectories)
               .ToList()
               .ForEach(f =>
               {
                   var fileContent = File.ReadAllLines(f).ToList();
                   var anyChangesmade = fileContent.RemovePlaceholders();

                   if (anyChangesmade)
                   {
                       File.WriteAllLines(f, fileContent);
                   }
               });
        }
    }
}
