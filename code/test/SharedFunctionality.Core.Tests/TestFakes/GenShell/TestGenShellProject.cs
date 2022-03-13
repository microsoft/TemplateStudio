// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.Core.Test.TestFakes.GenShell
{
    public class TestGenShellProject : IGenShellProject
    {
        public bool GetActiveProjectIsWts() => true;

        public bool IsActiveProjectWpf() => false;

        public bool IsActiveProjectWinUI() => false;

        public bool IsActiveProjectUwp() => false;

        public string GetActiveProjectLanguage() => string.Empty;

        public string GetActiveProjectName() => string.Empty;

        public string GetActiveProjectNamespace() => string.Empty;

        public string GetActiveProjectPath() => string.Empty;

        public string GetActiveProjectTypeGuids() => Guid.Empty.ToString();

        public Guid GetProjectGuidByName(string projectName) => Guid.Empty;
    }
}
