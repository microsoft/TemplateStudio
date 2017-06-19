using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;

namespace Microsoft.Templates.Core.Gen
{
    public interface IContextProvider
    {
        string ProjectName { get; }
        string OutputPath { get; }
        string ProjectPath { get; }
        List<string> ProjectItems { get; }

        List<string> NewFiles { get; }
        List<string> ModifiedFiles { get; }
        List<string> ConflictFiles { get; }
        List<string> UnchangedFiles { get; }
        List<string> FilesToOpen { get; }

        List<GenerationWarning> GenerationWarnings { get; }
        Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; }
    }
}
