using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Param_RootNamespace.Services
{
    public class LocalSettingsServiceUnpackaged : ILocalSettingsService
    {
        private readonly IFileService _fileService;
        private IDictionary<string, object> _settings;
        private readonly string _localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private LocalSettingsOptions _options;

        public LocalSettingsServiceUnpackaged(IFileService fileService)
        {
            _fileService = fileService;

            // https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration#basic-example
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _options = config.GetRequiredSection("LocalSettingsOptions").Get<LocalSettingsOptions>();
        }
        public async Task<T> ReadSettingAsync<T>(string key)
        {
            if (_settings is null)
            {
                var folderPath = Path.Combine(_localAppData, _options.ApplicationDataFolder);
                var fileName = _options.LocalSettingsFile;
                _settings = _fileService.Read<IDictionary<string, object>>(folderPath, fileName) ?? new Dictionary<string, object>();
            }

            object obj = null;

            if (_settings.TryGetValue(key, out obj))
            {
                return await Json.ToObjectAsync<T>((string)obj);
            }

            return default;
        }

        public async Task SaveSettingAsync<T>(string key, T value)
        {
            _settings[key] = await Json.StringifyAsync(value);

            var folderPath = Path.Combine(_localAppData, _options.ApplicationDataFolder);
            var fileName = _options.LocalSettingsFile;
            await Task.Run(() => _fileService.Save(folderPath, fileName, _settings));
        }
    }
}