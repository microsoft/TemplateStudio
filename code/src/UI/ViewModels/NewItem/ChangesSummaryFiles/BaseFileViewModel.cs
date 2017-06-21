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

using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Comparison;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public enum FileType
    {
        AddedFile, ModifiedFile, ConflictingFile, WarningFile, Unchanged
    }
    public enum FileExtension
    {
        Default, CSharp, Resw, Xaml
    }
    public abstract class BaseFileViewModel : Observable
    {
        public string DetailTitle { get; protected set; }
        public string DetailDescription { get; protected set; }
        public string DetailExtendedInfo { get; protected set; }
        public string Subject { get; protected set; }
        public string Icon { get; private set; }
        public SolidColorBrush CircleColor { get; private set; }
        public FileExtension FileExtension { get; private set; }

        public ObservableCollection<CodeLineViewModel> NewFileLines { get; private set; } = new ObservableCollection<CodeLineViewModel>();
        public ObservableCollection<CodeLineViewModel> CurrentFileLines { get; private set; } = new ObservableCollection<CodeLineViewModel>();
        public ObservableCollection<CodeLineViewModel> MergedFileLines { get; private set; } = new ObservableCollection<CodeLineViewModel>();

        public ICommand UpdateFontSizeCommand { get; }
        public abstract FileType FileType { get; }

        public BaseFileViewModel(string name)
        {
            Subject = name;
            FileExtension = GetFileExtension(name);
            Icon = GetIcon(FileExtension);
            CircleColor = GetCircleColor();
            LoadFile();
        }

        private SolidColorBrush GetCircleColor()
        {
            switch (FileType)
            {
                case FileType.AddedFile:
                    return MainViewModel.Current.MainView.FindResource("UIGreen") as SolidColorBrush;
                case FileType.ModifiedFile:
                    return MainViewModel.Current.MainView.FindResource("UIDarkBlue") as SolidColorBrush;
                case FileType.ConflictingFile:
                    return MainViewModel.Current.MainView.FindResource("UIDarkRed") as SolidColorBrush;
                case FileType.WarningFile:
                    return MainViewModel.Current.MainView.FindResource("UIDarkYellow") as SolidColorBrush;
                case FileType.Unchanged:
                    return MainViewModel.Current.MainView.FindResource("UIDarkBlue") as SolidColorBrush;
                default:
                    return new SolidColorBrush(Colors.Transparent);
            }
        }

        public BaseFileViewModel(NewItemGenerationFileInfo generationInfo)
        {
            Subject = generationInfo.Name;
            FileExtension = GetFileExtension(generationInfo.Name);
            Icon = GetIcon(FileExtension);
            CircleColor = GetCircleColor();
            LoadFile();
        }

        private void LoadFile()
        {
            var newFilePath = Path.Combine(GenContext.Current.OutputPath, Subject);
            var newFileCodeLines = ComparisonService.FromPath(newFilePath);
            NewFileLines.AddRange(newFileCodeLines.Select(cl => new CodeLineViewModel(cl)));

            var currentFilePath = Path.Combine(GenContext.Current.ProjectPath, Subject);
            var currentFileCodeLines = ComparisonService.FromPath(currentFilePath);
            CurrentFileLines.AddRange(currentFileCodeLines.Select(cl => new CodeLineViewModel(cl)));

            var comparsion = ComparisonService.CompareFiles(currentFileCodeLines, newFileCodeLines);
            MergedFileLines.AddRange(comparsion.Select(cl => new CodeLineViewModel(cl)));
        }

        public void UpdateFontSize(double points)
        {
            if (NewFileLines != null && NewFileLines.Any())
            {
                foreach (var line in NewFileLines)
                {
                    line.FontSize = points;
                }
            }
            if (CurrentFileLines != null && CurrentFileLines.Any())
            {
                foreach (var line in CurrentFileLines)
                {
                    line.FontSize = points;
                }
            }
            if (MergedFileLines != null && MergedFileLines.Any())
            {
                foreach (var line in MergedFileLines)
                {
                    line.FontSize = points;
                }
            }
        }

        private FileExtension GetFileExtension(string name)
        {
            switch (Path.GetExtension(name).ToLowerInvariant())
            {
                case "cs":
                    return FileExtension.CSharp;
                case "xaml":
                    return FileExtension.Xaml;
                case "resw":
                    return FileExtension.Resw;
                default:
                    return FileExtension.Default;
            }
        }

        private string GetIcon(FileExtension fileExtension)
        {
            switch (FileExtension)
            {
                case FileExtension.Default:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/DefaultFile.png";
                case FileExtension.CSharp:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/CSharp.png";
                case FileExtension.Resw:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/Resw.png";
                case FileExtension.Xaml:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/Xaml.png";
                default:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/DefaultFile.png";
            }
        }
    }
}
