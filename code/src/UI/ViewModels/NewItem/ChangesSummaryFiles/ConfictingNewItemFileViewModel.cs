using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class ConfictingNewItemFileViewModel : BaseNewItemFileViewModel
    {
        public override FileType FileType => FileType.ConflictingFile;
        public string ConflictingDetailDescription => string.Format(StringRes.ConflictingDetailDescription_SF, Subject);

        public ConfictingNewItemFileViewModel(NewItemGenerationFileInfo generationInfo) : base(generationInfo)
        {
        }
    }
}
