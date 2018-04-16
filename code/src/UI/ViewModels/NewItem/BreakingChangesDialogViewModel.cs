// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core.Validation;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class BreakingChangesDialogViewModel : BaseDialogViewModel
    {
        public BreakingChangesDialogViewModel(IEnumerable<BreakingChangeMessageViewModel> messages)
        {
            Title = Resources.StringRes.BreakingChanges;
            Description = Resources.StringRes.BreakingChangesDialogMessage;
            Messages = messages;
        }

        public IEnumerable<BreakingChangeMessageViewModel> Messages { get; set; }
    }
}
