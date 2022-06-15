﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class QuestionDialogViewModel : BaseDialogViewModel
    {
        public QuestionDialogViewModel(MetadataType metadataType)
        {
            Title = metadataType == MetadataType.ProjectType ? Resources.ProjectDetailsProjectTypeResetTitle : Resources.ProjectDetailsFrameworkResetTitle;
            Description = metadataType == MetadataType.ProjectType ? Resources.ProjectDetailsProjectTypeResetDescription : Resources.ProjectDetailsFrameworkResetDescription;
        }
    }
}
