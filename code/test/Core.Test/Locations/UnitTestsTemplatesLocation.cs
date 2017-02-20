using System.IO;
using Microsoft.Templates.Core.Locations;
using System;

namespace Microsoft.Templates.Core.Test.Locations
{
    public class UnitTestsTemplatesLocation : TemplatesLocation
    {
        public override (LocationCopyStatus Status, string Message) Copy(string workingFolder)
        {
            var status = LocationCopyStatus.SourceAdquired;
            var message = "Local templates copy started.";

            Copy($@"..\..\{TemplatesLocation.TemplatesName}", workingFolder);

            status = LocationCopyStatus.TargetUpdated;
            message = "Local templates sucessfully copied.";

            return (status, message);
        }

        public override string GetVersion(string workingFolder)
        {
            return string.Empty;
        }

        protected static void Copy(string sourceFolder, string workingFolder)
        {
            var sourceFolderName = new DirectoryInfo(Path.GetFullPath(sourceFolder)).Name;
            workingFolder = Path.Combine(workingFolder, sourceFolderName);

            SafeDelete(workingFolder);

            CopyRecursive(sourceFolder, workingFolder);
        }
    }
}