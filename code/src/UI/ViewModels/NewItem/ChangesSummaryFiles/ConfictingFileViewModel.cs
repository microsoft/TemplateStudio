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

using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class ConfictingFileViewModel : BaseFileViewModel
    {
        public override FileType FileType => FileType.ConflictingFile;
        public string ConflictingDetailDescription => string.Format(StringRes.ConflictingDetailDescription_SF, Subject);

        public ConfictingFileViewModel(NewItemGenerationFileInfo generationInfo) : base(generationInfo)
        {
            DetailTitle = string.Format(StringRes.ChangesSummaryDetailTitleOverwrittenFiles, generationInfo.Name);
            DetailDescription = string.Format(StringRes.ChangesSummaryDetailDescriptionOverwrittenFiles, generationInfo.Name);
        }
    }
}
