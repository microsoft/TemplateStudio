// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Gen.Shell
{
    public interface IGenShellUI
    {
        void ShowModal(IWindow shell);

        void CancelWizard(bool back = true);

        void ShowStatusBarMessage(string message);

        void WriteOutput(string data);

        void OpenProjectOverview();

        void OpenItems(params string[] itemsFullPath);

        void ShowTaskList();
    }
}
