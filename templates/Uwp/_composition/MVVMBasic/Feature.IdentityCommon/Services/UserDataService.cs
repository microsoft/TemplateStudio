using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.ViewModels;
using Windows.Storage;

namespace Param_RootNamespace.Services
{
    public class UserDataService
    {
        private const string _userSettingsKey = "IdentityUser";

        private IdentityService IdentityService => Singleton<IdentityService>.Instance;

        private MicrosoftGraphService MicrosoftGraphService => Singleton<MicrosoftGraphService>.Instance;

        public UserDataService()
        {
            IdentityService.LoggedOut += OnLoggedOut;
        }

        private async void OnLoggedOut(object sender, EventArgs e)
        {
            await ApplicationData.Current.LocalFolder.SaveAsync<User>(_userSettingsKey, null);
        }

        public async Task<UserViewModel> GetUserFromCacheAsync()
        {
            var cacheData = await ApplicationData.Current.LocalFolder.ReadAsync<User>(_userSettingsKey);
            return await GetUserViewModelFromData(cacheData);
        }

        public async Task<UserViewModel> GetUserFromGraphApiAsync()
        {
            var accessToken = await IdentityService.GetAccessTokenAsync();
            if (string.IsNullOrEmpty(accessToken))
            {
                return null;
            }

            var userData = await MicrosoftGraphService.GetUserInfoAsync(accessToken);
            if (userData != null)
            {
                userData.Photo = await MicrosoftGraphService.GetUserPhoto(accessToken);
                await ApplicationData.Current.LocalFolder.SaveAsync(_userSettingsKey, userData);
            }

            return await GetUserViewModelFromData(userData);
        }

        private async Task<UserViewModel> GetUserViewModelFromData(User userData)
        {
            if (userData == null)
            {
                return null;
            }

            var userPhoto = string.IsNullOrEmpty(userData.Photo)
                ? ImageHelper.ImageFromAssetsFile("DefaultIcon.png")
                : await ImageHelper.ImageFromStringAsync(userData.Photo);

            return new UserViewModel()
            {
                Name = userData.DisplayName,
                UserPrincipalName = userData.UserPrincipalName,
                Photo = userPhoto
            };
        }

        internal UserViewModel GetDefaultUserData()
        {
            return new UserViewModel()
            {
                Name = IdentityService.GetAccountUserName(),
                Photo = ImageHelper.ImageFromAssetsFile("DefaultIcon.png")
            };
        }
    }
}
