// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Templates.Core.Gen.Shell
{
    public interface IGenShellSolution
    {
        void ChangeSolutionConfiguration(IEnumerable<ProjectConfiguration> projectConfigurations);

        void SetDefaultSolutionConfiguration(string configurationName, string platformName, string projectName);

        void AddContextItemsToSolution(ProjectInfo projectInfo);

        void CloseSolution();

        void CollapseSolutionItems();
    }
}
