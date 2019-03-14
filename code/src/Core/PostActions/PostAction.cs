// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions
{
    public abstract class PostAction
    {
        public bool ContinueOnError { get; set; }

        public string RelatedTemplate { get; set; }

        public PostAction()
        {
            RelatedTemplate = "None";
        }

        public PostAction(string relatedTemplate, bool continueOnError)
        {
            ContinueOnError = continueOnError;
            RelatedTemplate = relatedTemplate;
        }

        internal abstract void ExecuteInternal();

        public void Execute()
        {
            try
            {
                ExecuteInternal();
            }
            catch (Exception ex)
            {
                if (!ContinueOnError)
                {
                    throw new Exception(string.Format(StringRes.PostActionException, this.GetType(), RelatedTemplate), ex);
                }
                else
                {
                    string msg = string.Format(StringRes.PostActionContinuerOnErrorWarning, this.GetType(), RelatedTemplate);
                    AppHealth.Current.Warning.TrackAsync(msg, ex).FireAndForget();
                }
            }
        }
    }

    [SuppressMessage(
       "StyleCop.CSharp.MaintainabilityRules",
       "SA1402:File may only contain a single class",
       Justification = "For simplicity we're allowing generic and non-generic versions in one file.")]

    public abstract class PostAction<TConfig> : PostAction
    {
        protected TConfig Config { get; }

        public PostAction(TConfig config)
            : base()
        {
            Config = config;
        }

        public PostAction(string relatedTemplate, TConfig config)
            : base(relatedTemplate, false)
        {
            Config = config;
        }
    }
}
