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
    }

    internal class PackageGuids
    {
        /// <summary>
        /// RelayCommandPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "995f080c-9f70-4550-8a21-b3ffeeff17eb";
        public static Guid GuidRelayCommandPackageCmdSet = Guid.Parse("dec1ebd7-fb6b-49e7-b562-b46af0d419d1");
    }
}
