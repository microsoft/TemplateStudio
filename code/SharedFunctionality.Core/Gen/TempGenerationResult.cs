// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Templates.Core.Gen
{
    public class TempGenerationResult
    {
        public List<string> NewFiles { get; } = new List<string>();

        public List<string> ModifiedFiles { get; } = new List<string>();

        public List<string> ConflictingFiles { get; } = new List<string>();

        public List<string> UnchangedFiles { get; } = new List<string>();

        public bool SyncGeneration { get; set; }
    }
}
