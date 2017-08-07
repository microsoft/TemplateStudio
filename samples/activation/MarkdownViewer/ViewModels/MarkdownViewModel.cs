// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

using MarkdownViewer.Helpers;

using Windows.UI.Xaml;

namespace MarkdownViewer.ViewModels
{
    public class MarkdownViewModel : Observable
    {
        public MarkdownViewModel()
        {
        }
        
        private string _text;

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }
    }
}
