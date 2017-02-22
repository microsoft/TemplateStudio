using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions
{
    public abstract class PostAction
    {
        protected readonly GenShell _shell;

        public PostAction(GenShell shell)
        {
            _shell = shell;
        }

        public abstract void Execute();
    }

    public abstract class PostAction<TConfig> : PostAction
    {
        protected readonly TConfig _config;

        public PostAction(GenShell shell, TConfig config) : base(shell)
        {
            _config = config;
        }
    }
}
