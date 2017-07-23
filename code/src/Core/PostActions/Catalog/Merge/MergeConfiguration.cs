// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergeConfiguration
    {
        public string FilePath { get; private set; }

        public bool FailOnError { get; private set; }

        public MergeConfiguration(string fileName, bool failOnError)
        {
            FilePath = fileName;
            FailOnError = failOnError;
        }
    }
}
