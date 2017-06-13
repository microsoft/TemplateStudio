namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class ConfictingNewItemFileViewModel : BaseNewItemFileViewModel
    {
        public override FileType FileType => FileType.ConflictingFile;

        public ConfictingNewItemFileViewModel(NewItemGenerationFileInfo generationInfo) : base(generationInfo)
        {
        }
    }    
}
