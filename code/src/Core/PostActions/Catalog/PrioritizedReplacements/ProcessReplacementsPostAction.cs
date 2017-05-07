// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
