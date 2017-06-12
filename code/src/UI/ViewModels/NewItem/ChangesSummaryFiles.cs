using System;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class WarningNewItemFileViewModel : BaseNewItemFileViewModel
    {
        public string Description { get; private set; }
        public string ExtendedInfo { get; private set; }

        public override FileType FileType => FileType.WarningFile;

        public WarningNewItemFileViewModel(GenerationWarning warning) : base(warning.FileName)
        {
            this.Description = warning.Description;
            this.ExtendedInfo = warning.ExtendedInfo;
        }
    }

    public class ConfictingNewItemFileViewModel : BaseNewItemFileViewModel
    {
        public override FileType FileType => FileType.ConflictingFile;

        public ConfictingNewItemFileViewModel(NewItemGenerationFileInfo generationInfo) : base(generationInfo)
        {
        }
    }

    public class AddedNewItemFileViewModel : BaseNewItemFileViewModel
    {
        public override FileType FileType => FileType.AddedFile;
        public AddedNewItemFileViewModel(NewItemGenerationFileInfo generationInfo) : base(generationInfo)
        {
        }
    }

    public class ModifiedNewItemFileViewModel : BaseNewItemFileViewModel
    {
        public override FileType FileType => FileType.ModifiedFile;        

        public ModifiedNewItemFileViewModel(NewItemGenerationFileInfo generationInfo) : base(generationInfo)
        {
        }
    }
}
