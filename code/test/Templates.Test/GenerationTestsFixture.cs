using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Test
{
    public sealed class GenerationTestsFixture : IDisposable
    {
        internal string TestRunPath = $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\{DateTime.Now.ToString("dd_hhmm")}\\";
        internal string TestProjectsPath => Path.GetFullPath(Path.Combine(TestRunPath, "Proj"));
        
        private static readonly Lazy<TemplatesRepository> _repos = new Lazy<TemplatesRepository>(() => CreateNewRepos(), true);
        public static IEnumerable<ITemplateInfo> Templates => _repos.Value.GetAll();

        private static TemplatesRepository CreateNewRepos()
        {
            var source = new LocalTemplatesSource();
            CodeGen.Initialize(source.Id);

            var repos = new TemplatesRepository(source);

            repos.SynchronizeAsync(true).Wait();
            return repos;
        }

        public GenerationTestsFixture()
        {
        }

        public void Dispose()
        {
            if (Directory.Exists(TestRunPath))
            {
                if (!Directory.Exists(TestProjectsPath) || Directory.EnumerateDirectories(TestProjectsPath).Count() == 0)
                {
                    Directory.Delete(TestRunPath, true);
                }
            }
        }
    }
}
