// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.Templates.Core;
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

        public RelayCommand(Package package, int commandId, Guid commandSet, Action<object, EventArgs> menuCallback, Action<object, TemplateType> beforeQueryStatus, TemplateType templateType)
        {
            _package = package;
            if (ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
            {
                var menuCommandID = new CommandID(commandSet, commandId);
                var menuItem = new OleMenuCommand(menuCallback.Invoke, menuCommandID);
                if (beforeQueryStatus != null)
                {
                    menuItem.BeforeQueryStatus += (s, e) => beforeQueryStatus.Invoke(s, templateType);
                }

                commandService.AddCommand(menuItem);
            }
        }

        private IServiceProvider ServiceProvider => _package;
    }
}
