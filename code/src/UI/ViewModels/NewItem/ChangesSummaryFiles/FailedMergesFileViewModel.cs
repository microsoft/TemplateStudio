// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.UI.Resources;
using System;
using System.Text;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class FailedMergesFileViewModel : BaseFileViewModel
    {
        public override FileStatus FileStatus => FileStatus.WarningFile;

        public FailedMergesFileViewModel(FailedMergePostAction warning) : base(warning.FailedFileName)
        {
            DetailTitle = StringRes.ChangesSummaryDetailTitleFailedMerges;

            var sb = new StringBuilder();
            sb.AppendLine(StringRes.ChangesSummaryDetailDescriptionFailedMerges);
            if (!string.IsNullOrEmpty(warning.Description))
            {
                sb.AppendLine(warning.Description);
            }
            if (!string.IsNullOrEmpty(warning.ExtendedInfo))
            {
                sb.AppendLine(warning.ExtendedInfo);
            }

            DetailDescription = sb.ToString();
            Subject = warning.FileName;
        }

        public override string UpdateText(string fileText) => base.UpdateText(fileText).AsUserFriendlyPostAction();
    }
}
