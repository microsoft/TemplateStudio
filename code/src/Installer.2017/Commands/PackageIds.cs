// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Extension.Commands
{
    internal class PackageIds
    {
        public const int AddPageCommand = 0x0400;
        public const int AddFeatureCommand = 0x500;
        public const int OpenTempFolder = 0x600;
    }

    internal class PackageGuids
    {
        /// <summary>
        /// RelayCommandPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "ae1b4c32-9c93-45b8-a36b-8734f4b120dd";
        public static Guid GuidRelayCommandPackageCmdSet = Guid.Parse("caa4fb82-0dca-40fe-bae0-081e0f96226f");
    }
}
