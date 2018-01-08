using Plugin.Settings;
using Plugin.Settings.Abstractions;
using XamarinUwpNative.Core.Helpers;
using XamarinUwpNative.Models;
using XamarinUwpNative.Services;
using Xamarin.Forms;

namespace XamarinUwpNative.ViewModels
{
    public class SettingsViewModel : Observable
    {
        private readonly ISettings _appSettings;
        private readonly IPlatformInfoService _platformInfoService;

        public SettingsViewModel()
        {
            _appSettings = CrossSettings.Current;
            _platformInfoService = DependencyService.Get<IPlatformInfoService>();

            AppName = $"{_platformInfoService.AppName} - {_platformInfoService.AppVersion}";
        }

        public string AppName { get; }

        public string AboutDescription { get; } = "Settings page placeholder text. Your app description goes here.";

        public string PrivacyTermsLink { get; } = @"https://YourPrivacyUrlGoesHere/";

        public bool SampleBoolSetting
        {
            get => _appSettings.GetValueOrDefault(nameof(SampleBoolSetting), false);
            set
            {
                if(_appSettings.AddOrUpdateValue(nameof(SampleBoolSetting), value))
                {
                    OnPropertyChanged(nameof(SampleBoolSetting));
                }
            }
        }

        public SampleProgramOptions SampleEnumSetting
        {
            get => (SampleProgramOptions)_appSettings.GetValueOrDefault(nameof(SampleEnumSetting), 0);
            set
            {
                if (_appSettings.AddOrUpdateValue(nameof(SampleEnumSetting), (int)value))
                {
                    OnPropertyChanged(nameof(SampleEnumSetting));
                }
            }
        }
    }
}
