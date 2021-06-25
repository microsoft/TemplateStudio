// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace VsTemplates.Test.Models
{
    public class VSTemplateData
    {
        public string Platform { get; set; }

        public string AppModel { get; set; }

        public string Language { get; set; }

        public string RootPath { get; set; }

        public VSTemplateData(string platform, string appModel, string language, string rootPath)
        {
            Platform = platform;
            Language = language;
            AppModel = appModel;
            RootPath = rootPath;
        }
    }
}
