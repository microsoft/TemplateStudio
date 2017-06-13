using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Comparison;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public enum FileType { AddedFile, ModifiedFile, ConflictingFile, WarningFile }
    public enum FileExtension { Default, CSharp, Resw, Xaml }
    public abstract class BaseNewItemFileViewModel : Observable
    {
        public string Subject { get; private set; }
        public string Icon { get; private set; }
        public FileExtension FileExtension { get; private set; }

        public ObservableCollection<CodeLineViewModel> NewFileLines { get; private set; } = new ObservableCollection<CodeLineViewModel>();
        public ObservableCollection<CodeLineViewModel> CurrentFileLines { get; private set; } = new ObservableCollection<CodeLineViewModel>();
        public ObservableCollection<CodeLineViewModel> MergedFileLines { get; private set; } = new ObservableCollection<CodeLineViewModel>();

        public ICommand UpdateFontSizeCommand { get; }
        public abstract FileType FileType { get; }
        private Dictionary<int, IEnumerable<string>> _mergeSnippets { get; }

        public BaseNewItemFileViewModel(string name)
        {
            Subject = name;
            FileExtension = GetFileExtenion(name);
            Icon = GetIcon(FileExtension);
            LoadFile();
        }

        public BaseNewItemFileViewModel(NewItemGenerationFileInfo generationInfo)
        {
            Subject = generationInfo.Name;
            _mergeSnippets = generationInfo.MergeSnippets;
            FileExtension = GetFileExtenion(generationInfo.Name);
            Icon = GetIcon(FileExtension);
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

        private FileExtension GetFileExtenion(string name)
        {
            FileExtension result = FileExtension.Default;
            if (name.Contains("."))
            {
                var strings = name.Split('.');
                if (strings?.Last() == "cs")
                {
                    result = FileExtension.CSharp;
                }
                else if (strings?.Last() == "xaml")
                {
                    result = FileExtension.Xaml;
                }
                else if (strings?.Last() == "resw")
                {
                    result = FileExtension.Resw;
                }
            }
            return result;
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
