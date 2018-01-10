// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Test.Locations;
using Microsoft.Templates.Fakes;
using Xunit;

namespace Microsoft.Templates.Core.Test
{
    public class TemplatesFixture
    {
        public TemplatesRepository Repository { get; private set; }

        private bool _syncExecuted = false;

        [SuppressMessage(
            "Usage",
            "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
            Justification = "Required por unit testing.")]
        public void InitializeFixture(string language)
        {
            var source = new UnitTestsTemplatesSource();

            GenContext.Bootstrap(source, new FakeGenShell(language), language);
            if (!_syncExecuted)
            {
                GenContext.ToolBox.Repo.SynchronizeAsync(true).Wait();
                _syncExecuted = true;
            }

            Repository = GenContext.ToolBox.Repo;
        }
    }

    [SuppressMessage("StyleCop", "SA1402", Justification = "This class does not have implementation")]
    [CollectionDefinition("Unit Test Templates")]
    public class TemplatesCollection : ICollectionFixture<TemplatesFixture>
    {
    }
}
