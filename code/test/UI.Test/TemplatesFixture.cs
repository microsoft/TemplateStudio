using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;

namespace UI.Test
{
    public class TemplatesFixture : IContextProvider
    {
        public TemplatesRepository Repository { get; private set; }

        public string ProjectName => "Test";

        public string OutputPath => throw new NotImplementedException();

        public string ProjectPath => throw new NotImplementedException();

        public List<string> ProjectItems => throw new NotImplementedException();

        public List<string> FilesToOpen => throw new NotImplementedException();

        public List<FailedMergePostAction> FailedMergePostActions => throw new NotImplementedException();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject => throw new NotImplementedException();

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics => throw new NotImplementedException();

        public void InitializeFixture(string language)
        {
            var source = new LocalTemplatesSource();

            GenContext.Bootstrap(source, new FakeGenShell(language), language);

            GenContext.ToolBox.Repo.SynchronizeAsync().Wait();

            Repository = GenContext.ToolBox.Repo;
            GenContext.Current = this;
        }
    }
}
