using System.Text;

namespace Param_RootNamespace;

public class Program
{
    [System.STAThreadAttribute]
    public static void Main()
    {
        using (new Param_RootNamespace.XamlIslandApp.App())
        {
            var app = new Param_RootNamespace.App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
