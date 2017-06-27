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

using System.Windows;
using System.Windows.Controls;

using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.TemplateSelectors
{
    public class FileStatusTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AddedFileTemplate { get; set; }
        public DataTemplate ModifiedFileTemplate { get; set; }
        public DataTemplate ConflictingFileTemplate { get; set; }
        public DataTemplate WarningFileTemplate { get; set; }
        public DataTemplate UnchangedFileTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var newItemFile = item as BaseFileViewModel;
            if (newItemFile != null)
            {
                switch (newItemFile.FileStatus)
                {
                    case FileStatus.AddedFile:
                        return AddedFileTemplate;
                    case FileStatus.ModifiedFile:
                        return ModifiedFileTemplate;
                    case FileStatus.ConflictingFile:
                        return ConflictingFileTemplate;
                    case FileStatus.WarningFile:
                        return WarningFileTemplate;
                    case FileStatus.Unchanged:
                        return UnchangedFileTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
