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
//using System.Windows.Forms;

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
            AvailableContent = new ObservableCollection<string>();
        }

        public RelayCommand CloseCommand => new RelayCommand(_host.Close);
        public RelayCommand CleanCommand => new RelayCommand(Clean, CanClean);
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
            set { _availableContent = value ; }
        }

        public void Initialize()
        {
            TemplatesLocation = _templatesSync.CurrentTemplatesFolder;
            LoadProperties();
        }

        private void SetVersion()
        {
            Version latest = new Version(0, 0, 0, 0);
            DirectoryInfo di = new DirectoryInfo(_templatesSync.CurrentTemplatesFolder);
            foreach(var sdi in di.EnumerateDirectories())
            {
                Version.TryParse(sdi.Name, out Version v);
                if(v > latest)
                {
                    latest = v;
                }
            }
            VersionInfo = new Version(latest.Major, latest.Minor, latest.Build, latest.Revision + 1).ToString();
        }

        private void ReadAvailableContent()
        {
            DirectoryInfo di = new DirectoryInfo(_templatesSync.CurrentTemplatesFolder);
            AvailableContent.Clear();
            foreach(var sdi in di.EnumerateDirectories())
            {
                AvailableContent.Add(sdi.Name);
            }
        }

        //private bool CanClean() { return AvailableContent != null ? AvailableContent.Count > 1 : false; }
        private bool CanClean() => true;

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

            MessageBox.Show("Done!", $"Clean created content.", MessageBoxButton.OK, MessageBoxImage.Information);

            LoadProperties();
        }


        private void CreateContent()
        {
            Fs.CopyRecursive(_templatesSource.Origin, Path.Combine(_templatesSync.CurrentTemplatesFolder, VersionInfo));

            MessageBox.Show("Done!", $"Create Content for v{VersionInfo}", MessageBoxButton.OK, MessageBoxImage.Information);

            LoadProperties();
        }

        private void LoadProperties()
        {
            SetVersion();
            ReadAvailableContent();
        }
    }
}
