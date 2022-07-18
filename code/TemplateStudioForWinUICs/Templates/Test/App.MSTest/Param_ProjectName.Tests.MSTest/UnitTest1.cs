using System.Diagnostics;

namespace Param_RootNamespace.Tests.MSTest;

[TestClass]
public class UnitTest1
{
    [ClassInitialize()]
    public static void ClassInitialize(TestContext context)
    {
        Debug.WriteLine("ClassInitialize");
    }

    [ClassCleanup()]
    public static void ClassCleanup()
    {
        Debug.WriteLine("ClassCleanup");
    }

    [TestInitialize()]
    public void Initialize()
    {
        Debug.WriteLine("TestInitialize");
    }

    [TestCleanup()]
    public void Cleanup()
    {
        Debug.WriteLine("TestCleanup");
    }

    [TestMethod]
    public void TestMethod1()
    {
        // TODO: Write unit tests.
        // https://docs.microsoft.com/visualstudio/test/getting-started-with-unit-testing
        // https://docs.microsoft.com/visualstudio/test/using-microsoft-visualstudio-testtools-unittesting-members-in-unit-tests
        // https://docs.microsoft.com/visualstudio/test/run-unit-tests-with-test-explorer

        Assert.IsTrue(true);
    }
}
