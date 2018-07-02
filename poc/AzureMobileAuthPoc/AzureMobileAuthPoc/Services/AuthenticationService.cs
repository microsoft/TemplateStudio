using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace AzureMobileAuthPoc.Services
{
    // TODO: Refactor this class!!

    public class AuthenticationService
    {
        // Define a member variable for storing the signed-in user. 
        private MobileServiceUser user;

        public async Task<string> LoginAsync()
        {
            string message;
            bool success;

            var provider = MobileServiceAuthenticationProvider.MicrosoftAccount;

            // Use the PasswordVault to securely store and access credentials.
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;

            try
            {
                // Try to get an existing credential from the vault.
                credential = vault.FindAllByResource(provider.ToString()).FirstOrDefault();
            }
            catch (Exception)
            {
                // When there is no matching resource an error occurs, which we ignore.
            }

            if (credential != null)
            {
                // Create a user from the stored credentials.
                user = new MobileServiceUser(credential.UserName);
                credential.RetrievePassword();
                user.MobileServiceAuthenticationToken = credential.Password;

                // Set the user from the stored credentials.
                MobileService.Instance.Client.CurrentUser = user;

                // Consider adding a check to determine if the token is 
                // expired, as shown in this post: http://aka.ms/jww5vp.

                success = true;
                message = string.Format("Cached credentials for user - {0}", user.UserId);
            }
            else
            {
                try
                {
                    // Login with the identity provider.
                    user = await MobileService.Instance.Client.LoginAsync(provider, "azuremobileauthpoc");

                    // Create and store the user credentials.
                    credential = new PasswordCredential(provider.ToString(),
                        user.UserId, user.MobileServiceAuthenticationToken);
                    vault.Add(credential);

                    success = true;
                    message = string.Format("You are now logged in - {0}", user.UserId);
                }
                catch (InvalidOperationException ex)
                {
                    message = "You must log in. Login Required";
                }
            }


            return message;
            //return success;
        }
        public async Task<string> LogoutAsync()
        {
            if (MobileService.Instance.Client.CurrentUser != null)
            {
                PasswordVault vault = new PasswordVault();
                var user = vault.FindAllByUserName(MobileService.Instance.Client.CurrentUser.UserId).FirstOrDefault();

                if (user != null)
                {
                    await MobileService.Instance.Client.LogoutAsync();
                    vault.Remove(user);
                    return "Logout correctly";
                }
            }

            return "Not user log in app.";
        }
    }
}
