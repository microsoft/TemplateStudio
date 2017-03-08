using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions
{
    public abstract class PostAction
    {
        public abstract void Execute();
    }

    public abstract class PostAction<TConfig> : PostAction
    {
        protected readonly TConfig _config;

        public PostAction(TConfig config)
        {
            _config = config;
        }
    }
}
