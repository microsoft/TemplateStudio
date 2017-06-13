namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class ModifiedNewItemFileViewModel : BaseNewItemFileViewModel
    {
        public override FileType FileType => FileType.ModifiedFile;        

        public ModifiedNewItemFileViewModel(NewItemGenerationFileInfo generationInfo) : base(generationInfo)
        {
        }
    }
}
