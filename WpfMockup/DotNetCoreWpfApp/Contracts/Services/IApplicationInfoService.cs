using System;

namespace DotNetCoreWpfApp.Contracts.Services
{
    public interface IApplicationInfoService
    {
        Version GetVersion();
    }
}
