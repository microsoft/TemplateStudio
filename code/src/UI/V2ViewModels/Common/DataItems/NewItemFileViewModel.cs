// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Media;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.UI.V2Services;

namespace Microsoft.Templates.UI.V2ViewModels.Common
{
    public class NewItemFileViewModel
    {
        public FileStatus FileStatus { get; private set; }

        public string Title { get; private set; } // TODO: mvegaca Load from resource

        public string Description { get; private set; } // TODO: mvegaca Load from resource

        public string Subject { get; private set; }

        public string Icon { get; private set; }

        public Brush CircleColor { get; private set; }

        public FileExtension FileExtension { get; private set; }

        private NewItemFileViewModel(FileStatus fileStatus)
        {
            FileStatus = fileStatus;
            Icon = GetIcon();
            CircleColor = GetCircleColor();
        }

        public static NewItemFileViewModel NewFile(NewItemGenerationFileInfo file)
        {
            return new NewItemFileViewModel(FileStatus.NewFile)
            {
                Subject = file.Name
            };
        }

        public static NewItemFileViewModel ModifiedFile(NewItemGenerationFileInfo file)
        {
            return new NewItemFileViewModel(FileStatus.ModifiedFile)
            {
                Subject = file.Name
            };
        }

        public static NewItemFileViewModel ConflictingFile(NewItemGenerationFileInfo file)
        {
            return new NewItemFileViewModel(FileStatus.ConflictingFile)
            {
                Subject = file.Name
            };
        }

        public static NewItemFileViewModel UnchangedFile(NewItemGenerationFileInfo file)
        {
            return new NewItemFileViewModel(FileStatus.UnchangedFile)
            {
                Subject = file.Name
            };
        }

        public static NewItemFileViewModel ConflictingStylesFile(FailedMergePostAction file)
        {
            return new NewItemFileViewModel(FileStatus.ConflictingStylesFile);
        }

        public static NewItemFileViewModel WarningFile(FailedMergePostAction file)
        {
            return new NewItemFileViewModel(FileStatus.WarningFile);
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
                case FileExtension.Vb:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/VisualBasic.png";
                case FileExtension.Vbproj:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/VBProj.png";
                default:
                    return "/Microsoft.Templates.UI;component/Assets/FileExtensions/DefaultFile.png";
            }
        }

        private Brush GetCircleColor()
        {
            switch (FileStatus)
            {
                case FileStatus.NewFile:
                    return UIStylesService.Instance.NewItemFileStatusNewFile;
                case FileStatus.ModifiedFile:
                    return UIStylesService.Instance.NewItemFileStatusModifiedFile;
                case FileStatus.ConflictingFile:
                    return UIStylesService.Instance.NewItemFileStatusConflictingFile;
                case FileStatus.ConflictingStylesFile:
                    return UIStylesService.Instance.NewItemFileStatusConflictingStylesFile;
                case FileStatus.WarningFile:
                    return UIStylesService.Instance.NewItemFileStatusWarningFile;
                case FileStatus.UnchangedFile:
                    return UIStylesService.Instance.NewItemFileStatusUnchangedFile;
                default:
                    return UIStylesService.Instance.ListItemText;
            }
        }
    }
}
