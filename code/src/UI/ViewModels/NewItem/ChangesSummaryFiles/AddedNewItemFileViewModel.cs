namespace Microsoft.Templates.UI.ViewModels.NewItem
{

    public class AddedNewItemFileViewModel : BaseNewItemFileViewModel
    {
        public override FileType FileType => FileType.AddedFile;
        public AddedNewItemFileViewModel(NewItemGenerationFileInfo generationInfo) : base(generationInfo)
        {
        }
    }
}
