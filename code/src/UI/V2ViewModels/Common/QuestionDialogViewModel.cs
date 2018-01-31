// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.V2Resources;

namespace Microsoft.Templates.UI.V2ViewModels.Common
{
    public class QuestionDialogViewModel : BaseDialogViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public QuestionDialogViewModel(Window window, MetadataType metadataType)
            : base(window)
        {
            Title = metadataType == MetadataType.Framework ? StringRes.ProjectDetailsProjectTypeResetTitle : StringRes.ProjectDetailsFrameworkResetTitle;
            Description = metadataType == MetadataType.Framework ? StringRes.ProjectDetailsProjectTypeResetDescription : StringRes.ProjectDetailsFrameworkResetDescription;
        }

        protected override void OnFinish(object parameter)
        {
            Window.DialogResult = bool.Parse(parameter.ToString());
            base.OnFinish(parameter);
        }
    }
}
