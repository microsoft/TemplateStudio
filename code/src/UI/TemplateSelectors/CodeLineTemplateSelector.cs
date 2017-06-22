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
using Microsoft.Templates.UI.Comparison;
using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.TemplateSelectors
{
    public class CodeLineTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultLineTemplate { get; set; }
        public DataTemplate NewLineTemplate { get; set; }
        public DataTemplate DeletedLineTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var line = item as CodeLineViewModel;
            if (line != null)
            {
                switch (line.Status)
                {
                    case LineStatus.Default:
                        return DefaultLineTemplate;
                    case LineStatus.New:
                        return NewLineTemplate;
                    case LineStatus.Deleted:
                        return DeletedLineTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
