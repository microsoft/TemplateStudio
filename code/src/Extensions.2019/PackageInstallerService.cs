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
        
    }
}
