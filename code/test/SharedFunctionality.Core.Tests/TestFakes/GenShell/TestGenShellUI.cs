// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.Core.Test.TestFakes.GenShell
{
    public class TestGenShellUI : IGenShellUI
    {
        public TestGenShellUI()
        {
        }

        public void CancelWizard(bool back = true)
        {
        }

        public void OpenItems(params string[] itemsFullPath)
        {
        }

        public void OpenProjectOverview()
        {
        }

        public void ShowModal(IWindow shell)
        {
        }

        public void ShowStatusBarMessage(string message)
        {
        }

        public void ShowTaskList()
        {
        }

        public void WriteOutput(string data)
        {
        }
    }
}
