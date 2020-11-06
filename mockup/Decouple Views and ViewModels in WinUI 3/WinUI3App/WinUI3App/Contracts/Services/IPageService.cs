using System;

namespace WinUI3App.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);
    }
}
