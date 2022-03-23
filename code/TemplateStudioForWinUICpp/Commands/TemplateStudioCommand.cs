// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.Design;
using Microsoft.Templates.Core;
using Microsoft.VisualStudio.Shell;

namespace TemplateStudioForWinUICpp.Commands
{
    internal sealed class TemplateStudioCommand
    {
        public TemplateStudioCommand(AsyncPackage package, OleMenuCommandService commandService, int commandId, Guid commandSet, Action<object, EventArgs> menuCallback, Action<object, TemplateType> beforeQueryStatus, TemplateType templateType)
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
}
