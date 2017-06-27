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
