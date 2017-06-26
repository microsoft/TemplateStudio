using System.IO;
using System.Reflection;
using System.Web;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.Controls
{
    public class CodeViewer : Control
    {
        private bool _isInitialized;
        private WebBrowser _webBrowser;

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }
        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register("FilePath", typeof(string), typeof(CodeViewer), new PropertyMetadata(string.Empty, OnCodeLinesPropertyChanged));

        public string OriginalFilePath
        {
            get { return (string)GetValue(OriginalFilePathProperty); }
            set { SetValue(OriginalFilePathProperty, value); }
        }
        public static readonly DependencyProperty OriginalFilePathProperty = DependencyProperty.Register("OriginalFilePath", typeof(string), typeof(CodeViewer), new PropertyMetadata(string.Empty, OnCodeLinesPropertyChanged));

        public CodeViewer()
        {
            DefaultStyleKey = typeof(CodeViewer);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _webBrowser = GetTemplateChild("webBrowser") as WebBrowser;
            _isInitialized = true;
            UpdateCodeView();
        }

        private void UpdateCodeView()
        {
            var executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace('\\', '/');

            string fileText = LoadFile(FilePath);
            string originalFileText = LoadFile(OriginalFilePath);
            if (_isInitialized && !string.IsNullOrEmpty(fileText) && !string.IsNullOrEmpty(originalFileText))
            {
                var patternText = File.ReadAllText(Path.Combine(executingDirectory, $@"Assets\Html\Compare.html"));
                patternText = patternText
                    .Replace("##ExecutingDirectory##", executingDirectory)
                    .Replace("##modifiedCode##", fileText)
                    .Replace("##originalCode##", originalFileText)
                    .Replace("##language##", "csharp");

                _webBrowser.NavigateToString(patternText);
            }
            else if (_isInitialized && !string.IsNullOrEmpty(fileText))
            {
                var patternText = File.ReadAllText(Path.Combine(executingDirectory, $@"Assets\Html\Document.html"));

                patternText = patternText
                    .Replace("##ExecutingDirectory##", executingDirectory)
                    .Replace("##code##", fileText)
                    .Replace("##language##", "csharp");
                _webBrowser.NavigateToString(patternText);
            }
        }

        private string LoadFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                return HttpUtility.JavaScriptStringEncode(File.ReadAllText(filePath));
            }
            return string.Empty;
        }

        private static void OnCodeLinesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CodeViewer;
            control.UpdateCodeView();
        }
    }
}