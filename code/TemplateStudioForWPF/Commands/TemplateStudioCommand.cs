using System;
using System.ComponentModel.Design;
using Microsoft.Templates.Core;
using Microsoft.VisualStudio.Shell;

namespace TemplateStudioForWPF.Commands
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
