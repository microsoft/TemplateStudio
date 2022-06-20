using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Param_RootNamespace.Core.Contracts.Services;

public interface IIdentityCacheService
{
    void SaveMsalToken(byte[] token);

    byte[] ReadMsalToken();
}
