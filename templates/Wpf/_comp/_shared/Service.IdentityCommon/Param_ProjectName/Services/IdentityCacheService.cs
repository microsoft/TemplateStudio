using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Param_RootNamespace.Core.Contracts.Services;

namespace Param_RootNamespace.Services
{
    public class IdentityCacheService : IIdentityCacheService
    {
        public static readonly string _msalCacheFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\{Assembly.GetExecutingAssembly().GetName().Name}";
        public static readonly string _msalCacheFileName = ".msalcache.bin3";
        private readonly object _fileLock = new object();

        public byte[] ReadMsalToken()
        {
            lock (_fileLock)
            {
                var filePath = Path.Combine(_msalCacheFilePath, _msalCacheFileName);
                if (File.Exists(filePath))
                {
                    var encryptedData = File.ReadAllBytes(filePath);
                    return ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
                }

                return default;
            }
        }

        public void SaveMsalToken(byte[] token)
        {
            lock (_fileLock)
            {
                if (!Directory.Exists(_msalCacheFilePath))
                {
                    Directory.CreateDirectory(_msalCacheFilePath);
                }

                var encryptedData = ProtectedData.Protect(token, null, DataProtectionScope.CurrentUser);
                var filePath = Path.Combine(_msalCacheFilePath, _msalCacheFileName);
                File.WriteAllBytes(filePath, encryptedData);
            }
        }
    }
}