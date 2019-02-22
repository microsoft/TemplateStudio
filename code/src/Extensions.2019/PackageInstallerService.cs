// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Composition;
using System.IO;
using EnvDTE;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.VisualStudio;
using Microsoft.VisualStudio.ProjectSystem.Properties;

namespace Microsoft.Templates.Extension
{
    [Export(typeof(IPackageInstallerService))]
    public class PackageInstallerService : IPackageInstallerService
    {
        public void AddNugetToCPSProject(Project project, IEnumerable<NugetReference> projectNugets)
        {
            if (project is IVsBrowseObjectContext browseObjectContext)
            {
                var threadingService = browseObjectContext.UnconfiguredProject.ProjectService.Services.ThreadingPolicy;

                threadingService.ExecuteSynchronously(
                async () =>
                {
                    var configuredProject = await browseObjectContext.UnconfiguredProject.GetSuggestedConfiguredProjectAsync().ConfigureAwait(false);

                    foreach (var reference in projectNugets)
                    {
                        GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Format(StringRes.StatusAddingNuget, Path.GetFileName(reference.PackageId)));

                        await configuredProject.Services.PackageReferences.AddAsync(reference.PackageId, reference.Version);
                    }
                });
            }
        }
    }
}
