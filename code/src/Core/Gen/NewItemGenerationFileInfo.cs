// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Gen
{
    public class NewItemGenerationFileInfo
    {
        public string Name { get; set; }

        public string NewItemGenerationFilePath { get; set; }

        public string ProjectFilePath { get; set; }

        public NewItemGenerationFileInfo(string name, string newItemGenerationFilePath, string projectFilePath)
        {
            Name = name;
            NewItemGenerationFilePath = newItemGenerationFilePath;
            ProjectFilePath = projectFilePath;
        }
    }
}
