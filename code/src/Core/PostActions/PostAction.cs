using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions
{
    public abstract class PostAction<TConfig>
    {
        public abstract string Execute(TConfig config, string sourceContent);
    }
}
