// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.Controls
{
    /// <summary>
    /// Interaction logic for CodeViewer.xaml
    /// </summary>
    public partial class CodeViewer : UserControl
    {
        private bool _isInitialized;

        private string _currentHtml = string.Empty;

        public object Item
        {
            get => GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(object), typeof(CodeViewer), new PropertyMetadata(true, OnItemChanged));

        public double CodeFontSize
        {
            get => (double)GetValue(CodeFontSizeProperty);
            set => SetValue(CodeFontSizeProperty, value);
        }

        public static readonly DependencyProperty CodeFontSizeProperty = DependencyProperty.Register(nameof(CodeFontSize), typeof(double), typeof(CodeViewer), new PropertyMetadata(SystemService.Instance.GetCodeFontSize(), OnItemChanged));

        public CodeViewer()
        {
            InitializeComponent();
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
                    .Replace("##renderSideBySide##", renderSideBySide ? "true" : "false")
                    .Replace("##theme##", SystemService.Instance.IsHighContrast ? "theme: 'hc-black'," : string.Empty)
                    .Replace("##fontSize##", $"fontSize: {CodeFontSize},");
                if (_currentHtml != patternText)
                {
                    webBrowser.NavigateToString(patternText);
                    _currentHtml = patternText;
                }
            }
        }

        private string GetLanguage(string filePath)
        {
            string extension = Path.GetExtension(filePath);

            var language = string.Empty;

            switch (extension)
            {
                case ".xaml":
                case ".csproj":
                case ".vbproj":
                case ".appxmanifest":
                case ".resw":
                case ".xml":
                    language = "xml";
                    break;
                case ".cs":
                    language = "csharp";
                    break;
                case ".vb":
                    language = "vb";
                    break;
                case ".json":
                    language = "json";
                    break;
            }

            return language;
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
            var item = control.Item as BaseFileViewModel;
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
                case FileStatus.ConflictingStylesFile:
                    UpdateCodeView(item.UpdateTextAction, item.FailedPostaction);
                    break;
            }
        }
    }
}
