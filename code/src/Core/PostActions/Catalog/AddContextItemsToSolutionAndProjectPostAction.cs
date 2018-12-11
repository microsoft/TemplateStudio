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
            GenContext.ToolBox.Shell.ShowStatusBarMessage(StringRes.StatusAddingProjects);

            var chrono = Stopwatch.StartNew();

            await GenContext.ToolBox.Shell.AddContextItemsToSolutionAsync(GenContext.Current.Projects, GenContext.Current.NugetReferences, GenContext.Current.ProjectItems.ToArray());

            GenContext.ToolBox.Shell.AddSdkReferencesToProjects(GenContext.Current.SdkReferences);
            GenContext.ToolBox.Shell.AddReferencesToProjects(GenContext.Current.ProjectReferences);
            chrono.Reset();

            GenContext.ToolBox.Shell.ShowStatusBarMessage(StringRes.StatusAddingItems);
            //GenContext.ToolBox.Shell.AddItems(GenContext.Current.ProjectItems.ToArray());

            chrono.Stop();

            GenContext.Current.ProjectMetrics[ProjectMetricsEnum.AddFilesToProject] = chrono.Elapsed.TotalSeconds;
            GenContext.Current.ProjectItems.Clear();
        }
    }
}
