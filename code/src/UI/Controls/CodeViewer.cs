// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Web;

using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.Controls
{
    public class CodeViewer : Control
    {
        private bool _isInitialized;
        private WebBrowser _webBrowser;
        private string _currentHtml = string.Empty;

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

            if (Item is BaseFileViewModel item)
            {
                UpdateCodeView(item);
            }
        }

        private void UpdateCodeView(Func<string, string> updateTextAction, string original, string modified = null, bool renderSideBySide = false)
        {
            if (!_isInitialized)
            {
                return;
            }

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
                patternText = patternText.Replace("##language##", language);
                patternText = patternText
                    .Replace("##ExecutingDirectory##", executingDirectory)
                    .Replace("##renderSideBySide##", (renderSideBySide.ToString().ToLower()))
                    .Replace("##theme##", SystemService.Instance.IsHighContrast ? "theme: 'hc-black'," : string.Empty);
                if (_currentHtml != patternText)
                {
                    _webBrowser.NavigateToString(patternText);
                    _currentHtml = patternText;
                }
            }
        }

        private string GetLanguage(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            if (extension == ".xaml" || extension == ".csproj" || extension == ".vbproj" || extension == ".appxmanifest" || extension == ".resw" || extension == ".xml")
            {
                return "xml";
            }
            else if (extension == ".cs")
            {
                return "csharp";
            }
            else if (extension == ".vb")
            {
                return "vb.net";
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
                    UpdateCodeView(item.UpdateTextAction, item.TempFile);
                    break;
                case FileStatus.ModifiedFile:
                    UpdateCodeView(item.UpdateTextAction, item.TempFile, item.ProjectFile);
                    break;
                case FileStatus.ConflictingFile:
                    UpdateCodeView(item.UpdateTextAction, item.TempFile, item.ProjectFile, true);
                    break;
            }
        }
    }
}
