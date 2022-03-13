//***
//Test documentation
//***

using Windows.UI.Xaml.Navigation;

//Block to be included
//USING
//End of block
namespace TestData
{
    sealed partial class App : /*Block to be included*/ITestInterface/*End of block*/
    {
//Block to be included
        //PROPERTY DEFINITION
//End of block
        public App(/*Block to be included*/ITestService testService/*End of block*/)
        {
            this.InitializeComponent();

            //This might be there or not
            //This might also be there or not

//Block to be included
            //AFTER INITIALIZE!!
//End of block
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
//Block to be included

            //AFTER ONLAUNCHED!!
//End of block
            Settings.SettingsViewModel.InitAppTheme();

//Include the following block at the end of the containing block.
//Block to be included
            //BEFORE END!!
            var s = "";

//End of block

            //This might be there or not
            //This might also be there or not

        }

//Block to be included
        private static void ThisIsANewMethod()
        {
            //INSIDE NEW METHOD!!
        }
//End of block

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.ToastNotification)
            {
//Block to be included
                //INSIDE IF!!
//End of block
            }
        }

//Block to be included
        private static void ThisIsAnExistingMethod()
        {
        }
//End of block
    }
}
//Block to be included
//New comment
//End of block
