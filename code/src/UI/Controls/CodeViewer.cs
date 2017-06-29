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

using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Web;

using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.Controls
{
    public class CodeViewer : Control
    {
        private bool _isInitialized;
        private WebBrowser _webBrowser;

        public object Item
        {
            get { return GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(object), typeof(CodeViewer), new PropertyMetadata(true, OnItemChanged));

        public CodeViewer()
        {
            DefaultStyleKey = typeof(CodeViewer);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _webBrowser = GetTemplateChild("webBrowser") as WebBrowser;
            _isInitialized = true;
            var item = Item as BaseFileViewModel;
            if (item != null)
            {
                UpdateCodeView(item);
            }
        }

        private string lastSubject;
        private void UpdateCodeView(string subject, Func<string, string> updateTextAction, string original, string modified = null, bool renderSideBySide = false)
        {
            if (!_isInitialized || subject == lastSubject)
            {
                return;
            }
            lastSubject = subject;

            var executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace('\\', '/');
            string fileText = LoadFile(original, updateTextAction);
            string originalFileText = LoadFile(modified, updateTextAction);
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
                var language = GetLanguage(original);
                if (!string.IsNullOrEmpty(language))
                {
                    patternText = patternText.Replace("##language##", language);
                }
                patternText = patternText.Replace("##ExecutingDirectory##", executingDirectory).Replace("##renderSideBySide##", (renderSideBySide.ToString().ToLower()));
                _webBrowser.NavigateToString(patternText);
            }
        }

        private string GetLanguage(string filePath)
        {
            string extension = Path.GetExtension(filePath);
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

        private string LoadFile(string filePath, Func<string, string> updateTextAction)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                string fileText = File.ReadAllText(filePath);
                fileText = updateTextAction(fileText);
                return HttpUtility.JavaScriptStringEncode(fileText);
            }
            return string.Empty;
        }

        private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CodeViewer;
            var item = e.NewValue as BaseFileViewModel;
            if (control != null && item != null)
            {
                control.UpdateCodeView(item);
            }
        }

        public void UpdateCodeView(BaseFileViewModel item)
        {
            switch (item.FileStatus)
            {
                case FileStatus.NewFile:
                case FileStatus.WarningFile:
                case FileStatus.Unchanged:
                    UpdateCodeView(item.Subject, item.UpdateTextAction, item.TempFile);
                    break;
                case FileStatus.ModifiedFile:
                    UpdateCodeView(item.Subject, item.UpdateTextAction, item.TempFile, item.ProjectFile);
                    break;
                case FileStatus.ConflictingFile:
                    UpdateCodeView(item.Subject, item.UpdateTextAction, item.TempFile, item.ProjectFile, true);
                    break;
            }
        }
    }
}
