// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.Templates.Extension.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class RelayCommand
    {
        private readonly Package _package;

        public RelayCommand(Package package, int commandId, Guid commandSet, Action<object, EventArgs> menuCallback, Action<object, EventArgs> beforeQueryStatus = null)
        {
            _package = package;

            OleMenuCommandService commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService != null)
            {
                var menuCommandID = new CommandID(commandSet, commandId);
                var menuItem = new OleMenuCommand(menuCallback.Invoke, menuCommandID);
                if (beforeQueryStatus != null)
                {
                    menuItem.BeforeQueryStatus += beforeQueryStatus.Invoke;
                }

                commandService.AddCommand(menuItem);
            }
        }

        private IServiceProvider ServiceProvider => _package;
    }
}
