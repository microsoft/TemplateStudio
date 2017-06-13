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
}
