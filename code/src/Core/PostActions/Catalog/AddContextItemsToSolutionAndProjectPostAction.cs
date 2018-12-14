// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class AddContextItemsToSolutionAndProjectPostAction : PostAction
    {
        internal override async Task ExecuteInternalAsync()
        {
            await GenContext.ToolBox.Shell.AddContextItemsToSolutionAsync(GenContext.Current.ProjectInfo);

            ////GenContext.Current.ProjectInfo.Projects.Clear();
            ////GenContext.Current.ProjectInfo.NugetReferences.Clear();
            ////GenContext.Current.ProjectInfo.SdkReferences.Clear();
            ////GenContext.Current.ProjectInfo.ProjectReferences.Clear();
            ////GenContext.Current.ProjectInfo.ProjectItems.Clear();
        }
    }
}
