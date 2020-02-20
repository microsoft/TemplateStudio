using System;
using System.Collections;
using System.IO;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Models;

namespace Param_RootNamespace.Services
{
    public class PersistAndRestoreService : IPersistAndRestoreService
    {
        private readonly IFileService _fileService;
        private readonly AppConfig _config;
        private readonly string _localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public PersistAndRestoreService(IFileService fileService, Param_ConfigType config)
        {
            _fileService = fileService;
            _config = Param_ConfigValue;
        }

        public void PersistData()
        {
            if (App.Current.Properties != null)
            {
                var folderPath = Path.Combine(_localAppData, _config.ConfigurationsFolder);
                var fileName = _config.AppPropertiesFileName;
                _fileService.Save(folderPath, fileName, App.Current.Properties);
            }
        }

        public void RestoreData()
        {
            var folderPath = Path.Combine(_localAppData, _config.ConfigurationsFolder);
            var fileName = _config.AppPropertiesFileName;
            var properties = _fileService.Read<IDictionary>(folderPath, fileName);
            if (properties != null)
            {
                foreach (DictionaryEntry property in properties)
                {
                    App.Current.Properties.Add(property.Key, property.Value);
                }
            }
        }
    }
}
