using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.Controls
{
    public class CodeViewer : Control
    {
        private bool _isInitialized;
        private WebBrowser _webBrowser;



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(CodeViewer), new PropertyMetadata(string.Empty, OnTextPropertyChanged));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CodeViewer;
            control.UpdateCodeView();
        }

        public CodeViewer()
        {
            DefaultStyleKey = typeof(CodeViewer);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _webBrowser = GetTemplateChild("webBrowser") as WebBrowser;
            _isInitialized = true;
        }

        private void UpdateCodeView()
        {
            if (_isInitialized)
            {

            }
        }
    }
}
