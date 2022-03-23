// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !CORETEST

using System;
using System.Windows;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Gen.Shell;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.Fakes.GenShell
{
    public class FakeGenShellUI : IGenShellUI
    {
        private readonly Action<string> _changeStatus;
        private readonly Action<string> _addLog;
        private readonly Window _owner;

        public FakeGenShellUI(Action<string> changeStatus, Action<string> addLog, Window owner)
        {
            _changeStatus = changeStatus;
            _addLog = addLog;
            _owner = owner;
        }

        public void CancelWizard(bool back = true)
        {
            if (back)
            {
                throw new WizardBackoutException();
            }
            else
            {
                throw new WizardCancelledException();
            }
        }

        public void OpenItems(params string[] itemsFullPath)
        {
        }

        public void OpenProjectOverview()
        {
        }

        public void ShowModal(IWindow shell)
        {
            if (shell is Window dialog)
            {
                dialog.Owner = _owner;
                dialog.ShowDialog();
            }
        }

        public void ShowStatusBarMessage(string message)
        {
            _changeStatus?.Invoke(message);
        }

        public void ShowTaskList()
        {
        }

        public void WriteOutput(string data)
        {
            _addLog?.Invoke(data);
        }
    }
}

#endif
