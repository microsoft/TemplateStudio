// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Templates.Core;
using Microsoft.VisualStudio.Shell;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.Extension
{
    [Guid("BA40FCCB-7084-4161-8D14-8708906C34C9")]
    public class GeneralSettingsPage : DialogPage
    {
        public string CustomTemplatesPath { get; set; } = string.Empty;

        protected override IWin32Window Window
        {
            get
            {
                var page = new GeneralSettingsPageControl();
                page.settingsPage = this;
                page.Initialize();
                return page;
            }
        }

        public override void LoadSettingsFromStorage()
        {
            CustomTemplatesPath = CustomSettings.CustomTemplatePath;
        }

        public override void SaveSettingsToStorage()
        {
            CustomSettings.CustomTemplatePath = CustomTemplatesPath;
        }
    }
}
