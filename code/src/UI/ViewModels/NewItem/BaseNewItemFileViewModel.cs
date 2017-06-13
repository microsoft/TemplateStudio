using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
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
            if (File.Exists(newFilePath))
            {
                uint lineNumber = 0;
                foreach (var line in File.ReadAllLines(newFilePath))
                {
                    NewFileLines.Add(new CodeLineViewModel(++lineNumber, line));
                }
            }
            var currentFilePath = Path.Combine(GenContext.Current.ProjectPath, Subject);
            if (File.Exists(currentFilePath))
            {
                uint lineNumber = 0;
                foreach (var line in File.ReadAllLines(currentFilePath))
                {
                    CurrentFileLines.Add(new CodeLineViewModel(++lineNumber, line));
                }
            }
            MergedFileLines = MergeLines();
        }

        private ObservableCollection<CodeLineViewModel> MergeLines()
        {
            var result = new ObservableCollection<CodeLineViewModel>();
            int positionCurrentLine = 1;
            int positionNewLine = 1;

            int currentCount = CurrentFileLines.Count();
            int newCount = NewFileLines.Count();

            uint lineNumber = 1;

            while (positionCurrentLine <= currentCount || positionNewLine <= newCount)
            {
                CodeLineViewModel currentLine = CurrentFileLines.FirstOrDefault(l => l.Number == positionCurrentLine);
                CodeLineViewModel newLine = NewFileLines.FirstOrDefault(l => l.Number == positionNewLine);

                if (currentLine == null)
                {
                    //Finish to cover current file. Pendding lines in new file are new.
                    result.Add(new CodeLineViewModel(++lineNumber, newLine, LineStatus.New));
                    positionNewLine++;
                }
                else if (newLine == null)
                {
                    //Finish to cover new file. Pendding lines in current file are removed.
                    result.Add(new CodeLineViewModel(++lineNumber, currentLine, LineStatus.Deleted));
                    positionCurrentLine++;
                }
                else if (currentLine.Line == newLine.Line)
                {
                    //Current line matches with new line.
                    result.Add(new CodeLineViewModel(++lineNumber, currentLine));
                    positionNewLine++;
                    positionCurrentLine++;
                }
                else
                {
                    var nextMatchLine = NewFileLines.FirstOrDefault(l => l.Number > positionNewLine && l.Line == currentLine.Line);
                    if (nextMatchLine == null)
                    {
                        //Current line not found in new file lines. This line has been deleted.
                        result.Add(new CodeLineViewModel(++lineNumber, currentLine, LineStatus.Deleted));
                        positionCurrentLine++;
                    }
                    else
                    {
                        //New line found before current line.
                        result.Add(new CodeLineViewModel(++lineNumber, newLine, LineStatus.New));
                        positionNewLine++;
                    }
                }
            }
            return result;            
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
