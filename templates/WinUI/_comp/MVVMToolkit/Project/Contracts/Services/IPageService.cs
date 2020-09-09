using System;
using Microsoft.UI.Xaml.Controls;

namespace Param_RootNamespace.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);
    }
}
