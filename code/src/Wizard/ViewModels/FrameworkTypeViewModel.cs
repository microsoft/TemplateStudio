namespace Microsoft.Templates.Wizard.ViewModels
{
    public class FrameworkTypeViewModel : ObservableBase
    {
        private string _frameworkType;
        public string FrameworkType
        {
            get { return _frameworkType; }
            set { SetProperty(ref _frameworkType, value); }
        }

        public FrameworkTypeViewModel(string frameworkType)
        {
            //TODO: Recover info
            this.FrameworkType = frameworkType;
        }
    }
}
