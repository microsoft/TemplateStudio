// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class FailedMergePostActionInfo
    {
        public MergeFailureType MergeFailureType { get; private set; }

        public string FileName { get; private set; }

        public string FailedFileName { get; private set; }

        public string FailedFilePath { get; private set; }

        public string FilePath { get; set; }

        public string Description { get; private set; }

        public FailedMergePostActionInfo(string fileName, string filePath, string failedFileName, string failedFilePath, string description, MergeFailureType mergeFailureType)
        {
            FileName = fileName;
            FilePath = filePath;
            FailedFileName = failedFileName;
            FailedFilePath = failedFilePath;
            Description = description;
            MergeFailureType = mergeFailureType;
        }
    }
}
