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

       

        private string text;

        public string Text
        {
            get { return text; }
            set { Set(ref text, value); }
        }
    }
}
