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
using System.Windows.Media;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;

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

        public string NewFile { get; set; }
        public string CurrentFile { get; set; }

        //public ObservableCollection<string> NewFileLines { get; private set; } = new ObservableCollection<string>();
        //public ObservableCollection<string> CurrentFileLines { get; private set; } = new ObservableCollection<string>();

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
                    return MainViewModel.Current.MainView.FindResource("UIBlue") as SolidColorBrush;
                case FileType.ConflictingFile:
                    return MainViewModel.Current.MainView.FindResource("UIRed") as SolidColorBrush;
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
            NewFile = Path.Combine(GenContext.Current.OutputPath, Subject);
            CurrentFile = Path.Combine(GenContext.Current.ProjectPath, Subject);
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
