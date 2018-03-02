// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Packaging;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Locations
{
    public abstract class TemplatesSource
    {
        public event Action<object, ProgressEventArgs> NewVersionAcquisitionProgress;

        public event Action<object, ProgressEventArgs> GetContentProgress;

        public event Action<object, ProgressEventArgs> CopyProgress;

        protected const string TemplatesFolderName = "Templates";

        public TemplatesSourceConfig Config { get; protected set; }

        public virtual string Id { get => Configuration.Current.Environment; }

        protected virtual bool VerifyPackageSignatures { get => true; }

        public abstract Task LoadConfigAsync(CancellationToken ct);

        public abstract Task<TemplatesContentInfo> GetContentAsync(TemplatesPackageInfo packageInfo, string workingFolder, CancellationToken ct);

        public abstract Task AcquireAsync(TemplatesPackageInfo packageInfo, CancellationToken ct);

        protected virtual void OnNewVersionAcquisitionProgress(object sender, ProgressEventArgs eventArgs)
        {
            NewVersionAcquisitionProgress?.Invoke(this, eventArgs);
        }

        protected virtual void OnGetContentProgress(object sender, ProgressEventArgs eventArgs)
        {
            GetContentProgress?.Invoke(this, eventArgs);
        }

        protected virtual void OnCopyProgress(object sender, ProgressEventArgs eventArgs)
        {
            CopyProgress?.Invoke(this, eventArgs);
        }
    }
}
