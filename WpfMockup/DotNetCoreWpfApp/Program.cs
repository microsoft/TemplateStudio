using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCoreWpfApp
{
    public class Program
    {
        [System.STAThreadAttribute]
        public static void Main()
        {
            using (new DotNetCoreWpfApp.XamlIslandApp.App())
            {
                var app = new DotNetCoreWpfApp.App();
                app.InitializeComponent();
                app.Run();
            }
        }
    }
}
