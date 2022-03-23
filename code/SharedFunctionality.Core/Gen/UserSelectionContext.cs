// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Templates.Core.Gen
{
    public class UserSelectionContext
    {
        public string ProjectType { get; set; }

        public string FrontEndFramework { get; set; }

        public string BackEndFramework { get; set; }

        public string Platform { get; private set; }

        public Dictionary<string, string> PropertyBag { get; set; } = new Dictionary<string, string>();

        public string Language { get; private set; }

        public UserSelectionContext(string language, string platform)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentNullException(nameof(language));
            }

            if (string.IsNullOrWhiteSpace(platform))
            {
                throw new ArgumentNullException(nameof(platform));
            }

            Language = language;
            Platform = platform;
        }
    }
}
