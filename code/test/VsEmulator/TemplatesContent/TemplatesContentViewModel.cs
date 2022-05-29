﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.UI.Mvvm;

namespace Microsoft.Templates.VsEmulator.TemplatesContent
{
    public class TemplatesContentViewModel : Observable
    {
        private const string DefaultName = "App";

        private readonly Window _host;

        public TemplatesContentViewModel(Window host, string wizardVersion)
        {
            _host = host;
            _useWizardVersion = wizardVersion;
            _isWizardVersionReconfigurable = wizardVersion == "0.0.0.0";

            AvailableContent = new ObservableCollection<string>();
        }

        public RelayCommand CloseCommand => new RelayCommand(Close);

        public RelayCommand SetVersionAndCloseCommand => new RelayCommand(SetVersionAndClose);

        public RelayCommand CleanCommand => new RelayCommand(Clean, CanClean);

        private string _versionInfo;

        public string VersionInfo
        {
            get => _versionInfo;
            set => SetProperty(ref _versionInfo, value);
        }

        private string _templatesLocation;

        public string TemplatesLocation
        {
            get => _templatesLocation;
            set => SetProperty(ref _templatesLocation, value);
        }

        private string _useWizardVersion;

        public string UseWizardVersion
        {
            get => _useWizardVersion;
            set => SetProperty(ref _useWizardVersion, value);
        }

        private bool _isWizardVersionReconfigurable;

        public bool IsWizardVersionReconfigurable
        {
            get => _isWizardVersionReconfigurable;
            set => SetProperty(ref _isWizardVersionReconfigurable, value);
        }

        public string Result { get; private set; }

        private ObservableCollection<string> _availableContent;

        public ObservableCollection<string> AvailableContent
        {
            get => _availableContent;
            set => _availableContent = value;
        }

        public void Initialize()
        {
            TemplatesLocation = GetTemplatesFolder();
            LoadProperties();
        }

        private void ReadAvailableContent()
        {
            var di = new DirectoryInfo(GetTemplatesFolder());
            AvailableContent.Clear();
            if (Directory.Exists(di.FullName))
            {
                foreach (var sdi in di.EnumerateDirectories())
                {
                    AvailableContent.Add(sdi.Name);
                }
            }
        }

        private bool CanClean() => true;

        private void Clean()
        {
            var di = new DirectoryInfo(GetTemplatesFolder());

            foreach (var sdi in di.EnumerateDirectories())
            {
                Fs.SafeDeleteDirectory(sdi.FullName);
            }

            LoadProperties();
        }

        private void SetVersionAndClose()
        {
            Result = _useWizardVersion;

            _host.DialogResult = true;
            _host.Close();
        }

        private void Close()
        {
            _host.DialogResult = false;
            _host.Close();
        }

        private void LoadProperties()
        {
            ReadAvailableContent();
        }

        private string GetTemplatesFolder()
        {
            return @"C:\ProgramData\WindowsTemplateStudio\Templates\LocalEnvTS";
        }
    }
}
