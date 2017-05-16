using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Gen
{
    public interface IContextProvider
    {
        string ProjectName { get; }
        string OutputPath { get; }
    }
}
