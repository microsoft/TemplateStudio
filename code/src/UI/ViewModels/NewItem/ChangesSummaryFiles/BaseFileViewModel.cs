// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;
using System.Windows.Media;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using System.Windows;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public enum FileStatus
    {
        NewFile, ModifiedFile, ConflictingFile, WarningFile, Unchanged
    }
    public enum FileExtension
    {
        Default, CSharp, Resw, Xaml, Xml, Csproj, Appxmanifest, Json, Jpg, Png, Jpeg
    }
    public abstract class BaseFileViewModel : Observable
    {
        public string DetailTitle { get; protected set; }
        public string DetailDescription { get; protected set; }
        public string Subject { get; protected set; }
        public string Icon { get; private set; }
        public SolidColorBrush CircleColor { get; private set; }
        public FileExtension FileExtension { get; private set; }
        public Func<string, string> UpdateTextAction { get; }

        public string TempFile { get; set; }
        public string ProjectFile { get; set; }

        public abstract FileStatus FileStatus { get; }

        public virtual string UpdateText(string fileText) => fileText;

        // TODO: Review constructor to remove this suppresion. Important
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BaseFileViewModel(string name)
        {
            Subject = name;
            LoadFile();
            UpdateTextAction = fileText => UpdateText(fileText);
        }

        // TODO: Review constructor to remove this suppresion. Important
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BaseFileViewModel(NewItemGenerationFileInfo generationInfo)
        {
            Subject = generationInfo.Name;
            LoadFile();
            UpdateTextAction = fileText => UpdateText(fileText);
        }

        private void LoadFile()
        {
            TempFile = Path.Combine(GenContext.Current.OutputPath, Subject);
            ProjectFile = Path.Combine(GenContext.Current.ProjectPath, Subject);
            FileExtension = GetFileExtension();
            Icon = GetIcon();
            CircleColor = GetCircleColor();
        }

        private SolidColorBrush GetCircleColor()
        {
            if (Services.SystemService.Instance.IsHighContrast)
            {
                return SystemColors.InfoTextBrush;
            }
            switch (FileStatus)
            {
                case FileStatus.NewFile:
                    return MainViewModel.Current.MainView.FindResource("UIGreen") as SolidColorBrush;
                case FileStatus.ModifiedFile:
                    return MainViewModel.Current.MainView.FindResource("UIBlue") as SolidColorBrush;
                case FileStatus.ConflictingFile:
                    return MainViewModel.Current.MainView.FindResource("UIRed") as SolidColorBrush;
                case FileStatus.WarningFile:
                    return MainViewModel.Current.MainView.FindResource("UIDarkYellow") as SolidColorBrush;
                case FileStatus.Unchanged:
                    return MainViewModel.Current.MainView.FindResource("UIDarkBlue") as SolidColorBrush;
                default:
                    return new SolidColorBrush(Colors.Transparent);
            }
        }

        private FileExtension GetFileExtension()
        {
            switch (Path.GetExtension(TempFile))
            {
                case ".cs":
                    return FileExtension.CSharp;
                case ".xaml":
                    return FileExtension.Xaml;
                case ".xml":
                    return FileExtension.Xml;
                case ".resw":
                    return FileExtension.Resw;
                case ".csproj":
                    return FileExtension.Csproj;
                case ".appxmanifest":
                    return FileExtension.Appxmanifest;
                case ".json":
                    return FileExtension.Json;
                case ".jpg":
                    return FileExtension.Jpg;
                case ".jpeg":
                    return FileExtension.Jpeg;
                case ".png":
                    return FileExtension.Png;
                default:
                    return FileExtension.Default;
            }
        }

        private string GetIcon()
        {
            switch (FileExtension)
            {
                case FileExtension.CSharp:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/CSharp.png";
                case FileExtension.Resw:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/Resw.png";
                case FileExtension.Xaml:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/Xaml.png";
                case FileExtension.Png:
                case FileExtension.Jpg:
                case FileExtension.Jpeg:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/Image.png";
                case FileExtension.Csproj:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/Csproj.png";
                case FileExtension.Json:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/Json.png";
                default:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/DefaultFile.png";
            }
        }
    }
}
