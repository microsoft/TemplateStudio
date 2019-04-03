using System;
using System.Threading.Tasks;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.Core.Services
{
    public interface IMicrosoftGraphService
    {
        Task<User> GetUserInfoAsync(string accessToken);

        Task<string> GetUserPhoto(string accessToken);
    }
}
