using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Wizard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Microsoft.Templates.VsEmulator.TemplatesContent
{
    public class TemplatesContentViewModel : Observable
    {
        private const string DefaultName = "App";

        private readonly Window _host;

        TemplatesSynchronization _templatesSync;
        LocalTemplatesSource _templatesSource;


        public TemplatesContentViewModel(Window host)
        {
            _host = host;
            _templatesSource = new LocalTemplatesSource();
            _templatesSync = new TemplatesSynchronization(_templatesSource);
        }

        public RelayCommand CloseCommand => new RelayCommand(_host.Close);
        public RelayCommand CleanAllContent => new RelayCommand(Clean, CanClean);
        public RelayCommand CreateContentCommand => new RelayCommand(CreateContent);

        private string _versionInfo;
        public string VersionInfo
        {
            get { return _versionInfo; }
            set { SetProperty(ref _versionInfo, value); }
        }

        private string _templatesLocation;
        public string TemplatesLocation
        {
            get { return _templatesLocation; }
            set { SetProperty(ref _templatesLocation, value); }
        }

        private ObservableCollection<string> _availableContent;
        public ObservableCollection<string> AvailableContent
        {
            get { return _availableContent; }
            set { SetProperty(ref _availableContent, value); }
        }

        public void Initialize()
        {
            TemplatesLocation = _templatesSync.CurrentTemplatesFolder;
            SetVersion();
            ReadAvailableContent();
        }

        private void SetVersion()
        {
            Version v = _templatesSync.CurrentContentVersion;
            VersionInfo = new Version(v.Major, v.Minor, v.Build, v.Revision + 1).ToString();
        }

        private void ReadAvailableContent()
        {
            DirectoryInfo di = new DirectoryInfo(_templatesSync.CurrentTemplatesFolder);
            AvailableContent.AddRange(di.EnumerateDirectories().Select(info => info.Name));
        }

        private bool CanClean() { return AvailableContent.Count > 1; }

        private void Clean()
        {
            DirectoryInfo di = new DirectoryInfo(_templatesSync.CurrentTemplatesFolder);
            foreach(var sdi in di.EnumerateDirectories())
            {
                if(sdi.Name != "0.0.0.0")
                {
                    Fs.SafeDeleteDirectory(sdi.FullName);
                }
            }
            ReadAvailableContent();
        }


        private void CreateContent()
        {
            Fs.CopyRecursive(_templatesSource.Origin, Path.Combine(_templatesSync.CurrentTemplatesFolder, VersionInfo)); 
        }
    }
}
