// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Packaging;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Locations
{
    public abstract class TemplatesSource
    {
        protected const string TemplatesFolderName = "Templates";

        public TemplatesSourceConfig Config { get; protected set; }

        public virtual string Id { get => Configuration.Current.Environment; }

        protected virtual bool VerifyPackageSignatures { get => true; }

        public abstract void LoadConfig();

        public abstract TemplatesContentInfo GetContent(TemplatesPackageInfo packageInfo, string workingFolder);

        public abstract void Acquire(ref TemplatesPackageInfo packageInfo);
    }
}
