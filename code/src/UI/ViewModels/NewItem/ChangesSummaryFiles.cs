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

        public ConfictingNewItemFileViewModel(string subject) : base(subject)
        {
        }
    }

    public class AddedNewItemFileViewModel : BaseNewItemFileViewModel
    {
        public override FileType FileType => FileType.AddedFile;
        public AddedNewItemFileViewModel(string subject) : base(subject)
        {
        }
    }

    public class ModifiedNewItemFileViewModel : BaseNewItemFileViewModel
    {
        public override FileType FileType => FileType.ModifiedFile;
        public ModifiedNewItemFileViewModel(string subject) : base(subject)
        {
        }
    }
}
