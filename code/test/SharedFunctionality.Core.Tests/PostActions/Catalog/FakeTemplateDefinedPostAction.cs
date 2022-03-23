// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    internal class FakeTemplateDefinedPostAction : IPostAction
    {
        public string Description => "Fake post action";

        public Guid ActionId { get; private set; }

        public bool ContinueOnError { get; set; }

        private Dictionary<string, string> _args = new Dictionary<string, string>();

        public IReadOnlyDictionary<string, string> Args
        {
            get
            {
                return _args;
            }
        }

        public string ManualInstructions => throw new NotImplementedException();

        public string ConfigFile => throw new NotImplementedException();

        public FakeTemplateDefinedPostAction(Guid guid, Dictionary<string, string> args, bool continueOnError = false)
        {
            ActionId = guid;
            ContinueOnError = continueOnError;
            _args = args;
        }
    }
}
