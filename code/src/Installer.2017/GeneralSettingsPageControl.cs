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

using System;
using System.Windows.Forms;

namespace Microsoft.Templates.Extension
{
    public partial class GeneralSettingsPageControl : UserControl
    {
        internal GeneralSettingsPage settingsPage;

        public GeneralSettingsPageControl()
        {
            InitializeComponent();
        }

        private void ChooseFolderClicked(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            if (!string.IsNullOrEmpty(SpecifiedCustomFolder.Text))
            {
                fbd.SelectedPath = SpecifiedCustomFolder.Text;
            }

            var dr = fbd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                SpecifiedCustomFolder.Text = fbd.SelectedPath;
            }
        }

        public void Initialize()
        {
            SpecifiedCustomFolder.Text = settingsPage.CustomTemplatesPath;
        }

        private void SpecifiedCustomFolderChanged(object sender, EventArgs e)
        {
            settingsPage.CustomTemplatesPath = SpecifiedCustomFolder.Text;
        }
    }
}
