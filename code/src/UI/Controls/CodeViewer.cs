using Microsoft.Templates.UI.ViewModels.NewItem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.Controls
{
    public class CodeViewer : Control
    {
        private bool _isInitialized;
        private WebBrowser _webBrowser;

        public IEnumerable<CodeLineViewModel> CodeLines
        {
            get { return (IEnumerable<CodeLineViewModel>)GetValue(CodeLinesProperty); }
            set { SetValue(CodeLinesProperty, value); }
        }
        public static readonly DependencyProperty CodeLinesProperty = DependencyProperty.Register("CodeLines", typeof(IEnumerable<CodeLineViewModel>), typeof(CodeViewer), new PropertyMetadata(null, OnCodeLinesPropertyChanged));

        public IEnumerable<CodeLineViewModel> OriginalCodeLines
        {
            get { return (IEnumerable<CodeLineViewModel>)GetValue(OriginalCodeLinesProperty); }
            set { SetValue(OriginalCodeLinesProperty, value); }
        }
        public static readonly DependencyProperty OriginalCodeLinesProperty = DependencyProperty.Register("OriginalCodeLines", typeof(IEnumerable<CodeLineViewModel>), typeof(CodeViewer), new PropertyMetadata(null, OnCodeLinesPropertyChanged));

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

            if (_isInitialized && CodeLines != null && CodeLines.Any() && OriginalCodeLines != null && OriginalCodeLines.Any())
            {
                var patternText = File.ReadAllText(Path.Combine(executingDirectory, $@"Assets\Html\Compare.html"));
                patternText = patternText
                    .Replace("##ExecutingDirectory##", executingDirectory)
                    .Replace("##modifiedCode##", HttpUtility.JavaScriptStringEncode(GetFileString(CodeLines)))
                    .Replace("##originalCode##", HttpUtility.JavaScriptStringEncode(GetFileString(OriginalCodeLines)))
                    .Replace("##language##", "csharp");

                _webBrowser.NavigateToString(patternText);
            }
            else if (_isInitialized && CodeLines != null && CodeLines.Any())
            {
                var patternText = File.ReadAllText(Path.Combine(executingDirectory, $@"Assets\Html\Document.html"));

                patternText = patternText.Replace("##ExecutingDirectory##", executingDirectory).Replace("##code##", GetCodeString(CodeLines)).Replace("##language##", "csharp");
                _webBrowser.NavigateToString(patternText);
            }
        }

        private string GetCodeString(IEnumerable<CodeLineViewModel> codeLines)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < codeLines.Count() - 1; i++)
            {
                sb.Append($"'{codeLines.ElementAt(i).Text}',");
            }
            sb.Append($"'{codeLines.ElementAt(codeLines.Count() - 1).Text}'");
            return sb.ToString();
        }

        private string GetFileString(IEnumerable<CodeLineViewModel> codeLines)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var line in codeLines)
            {
                sb.AppendLine(line.Text);
            }
            return sb.ToString();
        }

        private static void OnCodeLinesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CodeViewer;
            control.UpdateCodeView();
        }
    }
}