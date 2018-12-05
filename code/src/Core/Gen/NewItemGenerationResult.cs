// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Templates.Core.Gen
{
    public class NewItemGenerationResult
    {
        public List<NewItemGenerationFileInfo> NewFiles { get; } = new List<NewItemGenerationFileInfo>();

        public List<NewItemGenerationFileInfo> ModifiedFiles { get; } = new List<NewItemGenerationFileInfo>();

        public List<NewItemGenerationFileInfo> ConflictingFiles { get; } = new List<NewItemGenerationFileInfo>();

        public List<NewItemGenerationFileInfo> UnchangedFiles { get; } = new List<NewItemGenerationFileInfo>();

        public bool HasChangesToApply { get; set; }
    }
}
