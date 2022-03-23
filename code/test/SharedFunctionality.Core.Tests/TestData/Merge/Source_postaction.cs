//{**
//Test documentation
//**}

using Windows.UI.Xaml.Navigation;

//{[{
//USING
//}]}
namespace TestData
{
//{--{
    //THIS COMMENT SHOULD BE REMOVED
//}--}

    sealed partial class App : /*{[{*/ITestInterface/*}]}*/
    {
//{[{
        //PROPERTY DEFINITION
//}]}
        public App(/*{[{*/ITestService testService/*}]}*/)
        {
            this.InitializeComponent();
//{??{
            //This might be there or not
            //This might also be there or not
//}??}
//{[{
            //AFTER INITIALIZE!!
//}]}
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
//{[{

            //AFTER ONLAUNCHED!!
//}]}
            Settings.SettingsViewModel.InitAppTheme();

//^^
//{[{
            //BEFORE END!!
            var s = "";

//}]}
//{??{
            //This might be there or not
            //This might also be there or not
//}??}
        }

//{[{
        private static void ThisIsANewMethod()
        {
            //INSIDE NEW METHOD!!
        }
//}]}

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.ToastNotification)
            {
//{[{
                //INSIDE IF!!
//}]}
            }
        }

//{[{
        private static void ThisIsAnExistingMethod()
        {
        }
//}]}
    }
}
//{[{
//New comment
//}]}
