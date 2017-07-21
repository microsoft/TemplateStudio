// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Forms;

namespace Microsoft.Templates.Extension
{
    // TODO [ML]: localize this UI
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
