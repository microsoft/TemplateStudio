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

﻿using System.Windows;
using System.Windows.Controls;

using Microsoft.Templates.UI.ViewModels;

namespace Microsoft.Templates.UI.TemplateSelectors
{
    public class SummaryItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MicrosoftTemplate { get; set; }
        public DataTemplate CommunityTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var summaryItem = item as SavedTemplateViewModel;
            if (summaryItem != null)
            {
                if (summaryItem.Author.ToLower() == "microsoft")
                {
                    return MicrosoftTemplate;
                }
            }
            return CommunityTemplate;
        }
    }
}
