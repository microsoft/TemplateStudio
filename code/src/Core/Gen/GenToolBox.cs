// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Gen
{
    public class GenToolBox
    {
        public TemplatesRepository Repo { get; }

        public GenShell Shell { get; }

        public string WizardVersion => Repo.WizardVersion;

        public string TemplatesVersion => Repo.TemplatesVersion;

        public GenToolBox(TemplatesRepository repo, GenShell shell)
        {
            Repo = repo;
            Shell = shell;
        }
    }
}
