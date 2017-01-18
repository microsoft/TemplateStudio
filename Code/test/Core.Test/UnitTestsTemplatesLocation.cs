using System.IO;
using Microsoft.Templates.Core.Locations;

namespace Core.Test
{
    public class UnitTestsTemplatesLocation : TemplatesLocation
    {
        public override void Copy(string workingFolder)
        {
            Copy($@"..\..\..\{TemplatesLocation.PackagesName}", workingFolder);
            Copy($@"..\..\{TemplatesLocation.TemplatesName}", workingFolder);
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