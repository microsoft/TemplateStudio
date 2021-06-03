using System;

namespace WinUIMenuBarApp.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);
    }
}
