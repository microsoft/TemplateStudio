// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Test.Locations;
using Microsoft.Templates.Core.Test.TestFakes.GenShell;

namespace Microsoft.Templates.Core.Test
{
    public class TemplatesFixture
    {
        public TemplatesRepository Repository { get; private set; }

        private bool _syncExecuted = false;

        public void InitializeFixture(string platform, string language)
        {
            var source = new UnitTestsTemplatesSource(null);

            GenContext.Bootstrap(source, new TestGenShell(), platform, language, "0.0.0.7");
            if (!_syncExecuted)
            {
                ////GenContext.ToolBox.Repo.OnTemplatesAvailableAsync();
                _syncExecuted = true;
            }

            Repository = GenContext.ToolBox.Repo;
        }
    }
}
