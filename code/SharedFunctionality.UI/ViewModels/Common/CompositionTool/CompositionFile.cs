// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class CompositionFile : Observable
    {
        private ICommand _openFileCommand;
        private Brush _linkColor;

        public string Name { get; set; }

        public string Path { get; set; }

        public Brush LinkColor
        {
            get => _linkColor;
            set => SetProperty(ref _linkColor, value);
        }

        public ICommand OpenFileCommand => _openFileCommand ?? (_openFileCommand = new RelayCommand(OnOpenFile));

        public CompositionFile(string file)
        {
            Path = file;
            Name = file.Replace(GenContext.ToolBox.Repo.CurrentContentFolder, string.Empty);
            LinkColor = IsPostaction(file) ? UIStylesService.Instance.NewItemFileStatusModifiedFile : UIStylesService.Instance.NewItemFileStatusNewFile;
        }

        private bool IsPostaction(string file)
        {
            return file.Contains("postaction");
        }

        private void OnOpenFile()
        {
            if (!string.IsNullOrEmpty(Path))
            {
                Process.Start("code", $@"--new-window ""{Path}""");
            }
        }
    }
}
