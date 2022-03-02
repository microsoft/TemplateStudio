// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergeConfiguration
    {
        public const string Suffix = "postaction";
        public const string FailedPostactionSuffix = "failedpostaction";
        public const string SearchReplaceSuffix = "searchreplace";

        public const string PostactionRegex = @"(\$\S*)?(_" + Suffix + "|_g" + Suffix + @")\.";
        public const string PostactionAndSearchReplaceRegex = @"(\$\S*)?(_" + Suffix + "|_" + SearchReplaceSuffix + "|_g" + Suffix + @")\.";

        public const string FailedPostactionRegex = @"(\$\S*)?(_" + FailedPostactionSuffix + "|_g" + FailedPostactionSuffix + @")(\d)?\.";

        public const string Extension = "_" + Suffix + ".";
        public const string SearchReplaceExtension = "_" + SearchReplaceSuffix + ".";
        public const string GlobalExtension = "$*_g" + Suffix + ".";

        public const string ResourceDictionaryMatch = "<ResourceDictionary\r\n    xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">";

        public string FilePath { get; private set; }

        public BaseCodeStyleProvider CodeStyleProvider { get; private set; }

        public bool FailOnError { get; private set; }

        public MergeConfiguration(string fileName, BaseCodeStyleProvider codeStyleProvider, bool failOnError)
        {
            FilePath = fileName;
            CodeStyleProvider = codeStyleProvider;
            FailOnError = failOnError;
        }
    }
}
