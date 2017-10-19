// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Templates.Core.PostActions
{
    public abstract class PostAction
    {
        public abstract void Execute();
    }

    [SuppressMessage(
       "StyleCop.CSharp.MaintainabilityRules",
       "SA1402:File may only contain a single class",
       Justification = "For simplicity we're allowing generic and non-generic versions in one file.")]

    public abstract class PostAction<TConfig> : PostAction
    {
        protected TConfig Config { get; }

        public PostAction(TConfig config)
        {
            Config = config;
        }
    }
}
