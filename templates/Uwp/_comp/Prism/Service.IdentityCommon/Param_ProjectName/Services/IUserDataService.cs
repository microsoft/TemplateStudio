using System;
using System.Threading.Tasks;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Services
{
    public interface IUserDataService
    {
        event EventHandler<UserViewModel> UserDataUpdated;

        void Initialize();

        Task<UserViewModel> GetUserAsync();
    }
}