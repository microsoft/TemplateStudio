// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class NewFileViewModel : BaseFileViewModel
    {
        public NewFileViewModel(NewItemGenerationFileInfo generationInfo)
            : base(generationInfo, FileStatus.NewFile)
        {
            DetailTitle = StringRes.ChangesSummaryDetailTitleNewFiles;
            DetailDescription = StringRes.ChangesSummaryDetailDescriptionNewFiles;
        }
    }
}
