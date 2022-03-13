// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.Core.Test.TestFakes.GenShell
{
    public class TestGenShellSolution : IGenShellSolution
    {
        public void AddContextItemsToSolution(ProjectInfo projectInfo)
        {
        }

        public void ChangeSolutionConfiguration(IEnumerable<ProjectConfiguration> projectConfigurations)
        {
        }

        public void CloseSolution()
        {
        }

        public void CollapseSolutionItems()
        {
        }

        public void SetDefaultSolutionConfiguration(string configurationName, string platformName, string projectName)
        {
        }
    }
}
