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
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.VsEmulator.TemplatesContent
{
    public class TemplatesContentViewModel : Observable
    {
        private const string DefaultName = "App";

        private readonly Window _host;

        public TemplatesContentViewModel(Window host, string wizardVersion, string templateVersion)
        {
            _host = host;
            _useWizardVersion = wizardVersion;
            _useTemplatesVersion = templateVersion;
            _isWizardVersionReconfigurable = (wizardVersion == "0.0.0.0");

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

        private string _useTemplatesVersion;
        public string UseTemplatesVersion
        {
            get => _useTemplatesVersion;
            set => SetProperty(ref _useTemplatesVersion, value);
        }

        private bool _isWizardVersionReconfigurable;
        public bool IsWizardVersionReconfigurable
        {
            get => _isWizardVersionReconfigurable;
            set => SetProperty(ref _isWizardVersionReconfigurable, value);
        }

        public (string WizardVersion, string TemplatesVersion) Result { get; private set; }

        private ObservableCollection<string> _availableContent;
        public ObservableCollection<string> AvailableContent
        {
            get => _availableContent;
            set => _availableContent = value ;
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
            Result = (_useWizardVersion, _useTemplatesVersion);

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
            var _templatesSource = new LocalTemplatesSource(_useWizardVersion, _useTemplatesVersion);
            var _templatesSync = new TemplatesSynchronization(_templatesSource, new Version(_useWizardVersion));
            string currentTemplatesFolder = _templatesSync.CurrentTemplatesFolder;

            return currentTemplatesFolder;
        }
    }
}
