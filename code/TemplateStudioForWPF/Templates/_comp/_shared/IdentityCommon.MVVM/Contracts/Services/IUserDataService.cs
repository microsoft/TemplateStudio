using System;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Contracts.Services;

public interface IUserDataService
{
    event EventHandler<UserViewModel> UserDataUpdated;

    void Initialize();

    UserViewModel GetUser();
}
