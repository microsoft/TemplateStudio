using System;

namespace WinUIDesktopApp.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);
    }
}
