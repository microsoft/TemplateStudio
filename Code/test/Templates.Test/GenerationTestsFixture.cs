using System;
using System.IO;
using System.Linq;

namespace Microsoft.Templates.Test
{
    public class GenerationTestsFixture : IDisposable
    {
        internal string TestRunPath = @"..\..\TestRuns\{0}\";
        internal string TestAppsPath;
        internal string TestPagesPath;

        internal const string TemplatePath = @"..\..\..\..\..\Templates";

        public GenerationTestsFixture()
        {
            TestRunPath = string.Format(TestRunPath, DateTime.Now.ToString("yyyyMMdd_hhmm"));
            TestAppsPath = Path.Combine(TestRunPath, "Apps");
            TestPagesPath = Path.Combine(TestRunPath, "Pages");
        }

        public void Dispose()
        {
            if (Directory.Exists(TestRunPath))
            {
                if ((!Directory.Exists(TestAppsPath) || Directory.EnumerateDirectories(TestAppsPath).Count() == 0) 
                    && (!Directory.Exists(TestPagesPath) || Directory.EnumerateDirectories(TestPagesPath).Count() == 0))
                {
                    Directory.Delete(TestRunPath, true);
                }
            }
            
        }
    }
}
