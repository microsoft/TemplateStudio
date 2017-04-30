// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Templates.Core;
using Microsoft.VisualStudio.Shell;

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
