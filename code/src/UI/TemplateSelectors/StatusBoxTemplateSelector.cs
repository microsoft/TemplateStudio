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

using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.TemplateSelectors
{
    public class StatusBoxTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EmptyStatusTemplate { get; set; }
        public DataTemplate InformationStatusTemplate { get; set; }
        public DataTemplate WarningStatusTemplate { get; set; }
        public DataTemplate ErrorStatusTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var status = item as StatusViewModel;
            if (status != null)
            {
                switch (status.Status)
                {
                    case StatusType.Empty:
                        return EmptyStatusTemplate;
                    case StatusType.Information:
                        return InformationStatusTemplate;
                    case StatusType.Warning:
                        return WarningStatusTemplate;
                    case StatusType.Error:
                        return ErrorStatusTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
