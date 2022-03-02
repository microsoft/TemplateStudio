// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.Core.Gen
{
    public class GenToolBox
    {
        public TemplatesRepository Repo { get; }

        public IGenShell Shell { get; }

        public string WizardVersion => Repo.WizardVersion;

        public string TemplatesVersion => Repo.TemplatesVersion;

        public GenToolBox(TemplatesRepository repo, IGenShell shell)
        {
            Repo = repo;
            Shell = shell;
        }
    }
}
