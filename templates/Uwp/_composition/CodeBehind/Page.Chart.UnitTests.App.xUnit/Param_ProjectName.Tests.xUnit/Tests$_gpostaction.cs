//{[{
using System.Threading.Tasks;
using Param_RootNamespace.Views;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
//}]}

namespace Param_RootNamespace.Tests.XUnit
{
    public class Tests
    {
        //^^
        //{[{

        // TODO WTS: Add tests for functionality you add to wts.ItemNamePage.
        [Fact]
        public async Task Testwts.ItemNamePageCreation()
        {
            // Creating pages requires the UI thread so execute the test there.
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // This test is trivial. Add your own tests for the logic you add to the code-behind file for the page.
                var view = new wts.ItemNamePage();
                Assert.NotNull(view);
            });
        }
        //}]}
    }
}
