namespace Microsoft.Templates.Extension.Commands
{
    using System;

    /// <summary>
    /// Helper class that exposes all GUIDs used across VS Package.
    /// </summary>
    internal sealed partial class PackageGuids
    {
        public const string guidTemplatesCommandsPackageString = "70616fbe-b608-4e19-a769-cb46095a2f3d";
        public const string guidTemplatesCommandsPackageCmdSetString = "b3e2b592-29a1-469b-817a-0e4b7a2c6049";
        public const string guidImagesString = "560f61e9-e98d-4fc0-815e-328c11792076";
        public static Guid guidTemplatesCommandPackage = new Guid(guidTemplatesCommandsPackageString);
        public static Guid guidTemplatesCommandPackageCmdSet = new Guid(guidTemplatesCommandsPackageCmdSetString);
        public static Guid guidImages = new Guid(guidImagesString);
    }
    /// <summary>
    /// Helper class that encapsulates all CommandIDs uses across VS Package.
    /// </summary>
    internal sealed partial class PackageIds
    {
        public const int TemplatesContextGroup = 0x0100;
        public const int TemplatesContextMenu = 0x0200;
        public const int TemplatesContextMenuGroup = 0x0300;
        public const int AddPageCommand = 0x0400;
        public const int AddFeatureCommand = 0x0500;
        public const int bmpPic1 = 0x0001;
        public const int bmpPic2 = 0x0002;
        public const int bmpPicSearch = 0x0003;
        public const int bmpPicX = 0x0004;
        public const int bmpPicArrows = 0x0005;
        public const int bmpPicStrikethrough = 0x0006;
    }
}
