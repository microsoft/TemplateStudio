using System;
using Param_RootNamespace.Models;

namespace Param_RootNamespace.Contracts.Services
{
    public interface IUserDataService
    {
        event EventHandler<UserData> UserDataUpdated;

        void Initialize();

        UserData GetUser();
    }
}
