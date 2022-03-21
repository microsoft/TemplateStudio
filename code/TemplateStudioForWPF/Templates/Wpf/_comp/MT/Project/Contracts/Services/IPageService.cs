using System;
using System.Windows.Controls;

namespace Param_RootNamespace.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);

        Page GetPage(string key);
    }
}
