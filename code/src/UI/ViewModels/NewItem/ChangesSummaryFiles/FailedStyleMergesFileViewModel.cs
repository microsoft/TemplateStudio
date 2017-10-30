// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;

using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class FailedStyleMergesFileViewModel : BaseFileViewModel
    {
        public FailedStyleMergesFileViewModel(FailedMergePostActionInfo warning)
            : base(warning.FailedFileName, FileStatus.ConflictingStylesFile)
        {
            DetailTitle = StringRes.ChangesSummaryDetailTitleFailedStyleMerges;

            var sb = new StringBuilder();
            sb.AppendLine(StringRes.ChangesSummaryDetailDescriptionFailedStyleMerges);
            if (!string.IsNullOrEmpty(warning.Description))
            {
                sb.Append(warning.Description);
            }

            DetailDescription = sb.ToString();
            Subject = warning.FileName;
        }

        public override string UpdateText(string fileText) => base.UpdateText(fileText).AsUserFriendlyPostAction();
    }
}
