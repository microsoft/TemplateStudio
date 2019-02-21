// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using EnvDTE;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.UI.VisualStudio
{
    public interface IPackageInstallerService
    {
        void AddNugetToCPSProject(Project project, IEnumerable<NugetReference> projectNugets);
    }
}
