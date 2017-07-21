// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.PostActions
{
    // TODO [ML]: add ability for postaction to operate on secondary projects
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
