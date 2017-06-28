using System;
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

        public string OriginalFilePath
        {
            get { return (string)GetValue(OriginalFilePathProperty); }
            set { SetValue(OriginalFilePathProperty, value); }
        }
        public static readonly DependencyProperty OriginalFilePathProperty = DependencyProperty.Register("OriginalFilePath", typeof(string), typeof(CodeViewer), new PropertyMetadata(string.Empty, OnFilePathChanged));

        public string ModifiedFilePath
        {
            get { return (string)GetValue(ModifiedFilePathProperty); }
            set { SetValue(ModifiedFilePathProperty, value); }
        }
        public static readonly DependencyProperty ModifiedFilePathProperty = DependencyProperty.Register("ModifiedFilePath", typeof(string), typeof(CodeViewer), new PropertyMetadata(string.Empty, OnFilePathChanged));

        public Func<string, string> UpdateTextAction
        {
            get { return (Func<string, string>)GetValue(UpdateTextActionProperty); }
            set { SetValue(UpdateTextActionProperty, value); }
        }
        public static readonly DependencyProperty UpdateTextActionProperty = DependencyProperty.Register("UpdateTextAction", typeof(Func<string, string>), typeof(CodeViewer), new PropertyMetadata(null, OnFilePathChanged));


        public bool RenderSideBySide
        {
            get { return (bool)GetValue(RenderSideBySideProperty); }
            set { SetValue(RenderSideBySideProperty, value); }
        }
        public static readonly DependencyProperty RenderSideBySideProperty = DependencyProperty.Register("RenderSideBySide", typeof(bool), typeof(CodeViewer), new PropertyMetadata(true, OnFilePathChanged));

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
            if (!_isInitialized || UpdateTextAction == null)
            {
                return;
            }

            var executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace('\\', '/');
            string fileText = LoadFile(OriginalFilePath);
            string originalFileText = LoadFile(ModifiedFilePath);
            string patternText = string.Empty;
            if (!string.IsNullOrEmpty(fileText) && !string.IsNullOrEmpty(originalFileText))
            {
                patternText = File.ReadAllText(Path.Combine(executingDirectory, $@"Assets\Html\Compare.html"));
                patternText = patternText
                    .Replace("##modifiedCode##", fileText)
                    .Replace("##originalCode##", originalFileText);
            }
            else if (!string.IsNullOrEmpty(fileText))
            {
                patternText = File.ReadAllText(Path.Combine(executingDirectory, $@"Assets\Html\Document.html"));

                patternText = patternText
                    .Replace("##code##", fileText);
            }
            if (!string.IsNullOrEmpty(patternText))
            {
                var language = GetLanguage(OriginalFilePath);
                if (!string.IsNullOrEmpty(language))
                {
                    patternText = patternText.Replace("##language##", language);
                }
                patternText = patternText.Replace("##ExecutingDirectory##", executingDirectory).Replace("##renderSideBySide##", (RenderSideBySide.ToString().ToLower()));
                _webBrowser.NavigateToString(patternText);
            }
        }

        private string GetLanguage(string filePath)
        {
            string extension = Path.GetExtension(OriginalFilePath);
            if (extension == ".xaml" || extension == ".csproj" || extension == ".appxmanifest" || extension == ".resw" || extension == ".xml")
            {
                return "xml";
            }
            else if (extension == ".cs")
            {
                return "csharp";
            }
            else if (extension == ".json")
            {
                return "json";
            }
            return string.Empty;
        }

        private string LoadFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                string fileText = File.ReadAllText(filePath);
                fileText = UpdateTextAction(fileText);
                return HttpUtility.JavaScriptStringEncode(fileText);
            }
            return string.Empty;
        }

        private static void OnFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CodeViewer;
            control.UpdateCodeView();
        }
    }
}