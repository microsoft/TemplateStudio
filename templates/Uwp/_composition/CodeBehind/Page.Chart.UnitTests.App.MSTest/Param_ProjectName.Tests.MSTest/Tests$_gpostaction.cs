//{[{
using Param_RootNamespace.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
//}]}

namespace Param_RootNamespace.Tests.MSTest
{
    public class Tests
    {
        //^^
        //{[{

        // TODO WTS: Add tests for functionality you add to wts.ItemNamePage.
        [UITestMethod]
        public void Testwts.ItemNamePageCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the code-behind file for the page.
            var view = new wts.ItemNamePage();
            Assert.IsNotNull(view);
        }
        //}]}
    }
}
