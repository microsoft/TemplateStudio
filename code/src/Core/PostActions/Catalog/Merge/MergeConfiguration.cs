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
        public const string Suffix = "postaction";
        public const string NewSuffix = "failedpostaction";

        public const string PostactionRegex = @"(\$\S*)?(_" + Suffix + "|_g" + Suffix + @")\.";
        public const string FailedPostactionRegex = @"(\$\S*)?(_" + NewSuffix + "|_g" + NewSuffix + @")(\d)?\.";

        public const string Extension = "_" + Suffix + ".";
        public const string GlobalExtension = "$*_g" + Suffix + ".";

        public const string ResourceDictionaryMatch = @"<ResourceDictionary
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">";

        public string FilePath { get; private set; }

        public bool FailOnError { get; private set; }

        public MergeConfiguration(string fileName, bool failOnError)
        {
            FilePath = fileName;
            FailOnError = failOnError;
        }
    }
}
