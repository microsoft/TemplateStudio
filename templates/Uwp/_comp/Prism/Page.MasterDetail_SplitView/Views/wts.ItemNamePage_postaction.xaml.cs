//{[{
using Microsoft.Toolkit.Uwp.UI.Controls;
//}]}

namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {

        //^^
        //{[{
        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Workaround for issue on MasterDetail Control. Find More info at https://github.com/Microsoft/WindowsTemplateStudio/issues/2739.
            if (MasterDetailsViewControl.ViewState == MasterDetailsViewState.Both)
            {
                ViewModel.SetDefaultSelection();
            }
        }

        //}]}
    }
}
