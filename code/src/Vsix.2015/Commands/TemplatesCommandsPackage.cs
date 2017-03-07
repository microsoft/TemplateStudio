//------------------------------------------------------------------------------
// <copyright file="TemplatesCommandsPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Extension.Resources;
using Microsoft.Templates.Wizard;
using Microsoft.Templates.Wizard.Host;
using Microsoft.VisualStudio.Shell;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Microsoft.Templates.Extension.Commands
{
    [ProvideAutoLoad("{f1536ef8-92ec-443c-9ed7-fdadf150da82}")]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.guidTemplatesCommandsPackageString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class TemplatesCommandsPackage : Package
    {
        private VsRelayCommand _addPageCommand;
        private VsRelayCommand _addFeatureCommand;

        public TemplatesCommandsPackage()
        {
        }

        protected override void Initialize()
        {

            _addPageCommand = new VsRelayCommand(this, PackageIds.AddPageCommand, PackageGuids.guidTemplatesCommandPackageCmdSet,
                AddPageToProject,
                (sender, e) =>
                {
                    var cmd = (OleMenuCommand)sender;
                    cmd.Visible = true;
                }
            );

            _addFeatureCommand = new VsRelayCommand(this, PackageIds.AddFeatureCommand, PackageGuids.guidTemplatesCommandPackageCmdSet,
                AddFeatureToProject,
                (sender, e) =>
                {
                    var cmd = (OleMenuCommand)sender;
                    cmd.Visible = true;
                });
            base.Initialize();
        }

        private void AddPageToProject(object sender, EventArgs e)
        {
            try
            {
                //TODO: AddPage Command implementation    
            }
            catch (Exception ex)
            {
                MessageBox.Show(StringRes.UnexpectedExPattern.UseParams(ex.ToString()), StringRes.UIMessageBoxTitlePattern.UseParams(StringRes.AddPageAction), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void AddFeatureToProject(object sender, EventArgs e)
        {
            try
            {
                //TODO: AddFeature command implementation.
            }
            catch (Exception ex)
            {
                MessageBox.Show(StringRes.UnexpectedExPattern.UseParams(ex.ToString()), StringRes.UIMessageBoxTitlePattern.UseParams(StringRes.AddFeatureAction), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}

