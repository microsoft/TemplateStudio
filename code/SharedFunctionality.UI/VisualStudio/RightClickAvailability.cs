// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class RightClickAvailability
    {
        public string Platform { get; set; }

        public string Language { get; set; }

        public string AppModel { get; set; }

        public IEnumerable<TemplateType> TemplateTypes { get; set; }

        public RightClickAvailability(string platform, string language, string appModel = null)
        {
            Platform = platform;
            Language = language;
            AppModel = appModel;
        }
    }
}
