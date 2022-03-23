// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace Microsoft.Templates.Core.Helpers
{
    public static class FileHelper
    {
        public static string GetFileContent(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }

            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
