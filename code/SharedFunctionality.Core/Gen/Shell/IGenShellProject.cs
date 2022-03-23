// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Templates.Core.Gen.Shell
{
    public interface IGenShellProject
    {
        string GetActiveProjectName();

        string GetActiveProjectNamespace();

        string GetActiveProjectPath();

        string GetActiveProjectLanguage();

        string GetActiveProjectTypeGuids();

        Guid GetProjectGuidByName(string projectName);

        bool GetActiveProjectIsWts();

        bool IsActiveProjectWpf();

        bool IsActiveProjectWinUI();

        bool IsActiveProjectUwp();
    }
}
