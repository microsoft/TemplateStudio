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
    public class FileExtensionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CodeFileTemplate { get; set; }
        public DataTemplate ImageFileTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var file = item as BaseFileViewModel;
            if (file != null)
            {
                switch (file.FileExtension)
                {
                    case FileExtension.Jpeg:
                    case FileExtension.Jpg:
                    case FileExtension.Png:
                        return ImageFileTemplate;
                    default:
                        return CodeFileTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
