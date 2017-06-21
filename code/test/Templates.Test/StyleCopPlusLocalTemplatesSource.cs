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

using System.IO;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Test
{
    public class StyleCopPlusLocalTemplatesSource : TemplatesSource
    {
        public string LocalTemplatesVersion { get; private set; }

        public string LocalWizardVersion { get; private set; }

        protected override bool VerifyPackageSignatures => false;

        public string Origin => $@"..\..\..\..\..\{SourceFolderName}";

        public StyleCopPlusLocalTemplatesSource() : this("0.0.0.0", "0.0.0.0")
        {
        }

        public StyleCopPlusLocalTemplatesSource(string wizardVersion, string templatesVersion)
        {
            LocalTemplatesVersion = templatesVersion;
            LocalWizardVersion = wizardVersion;
        }

        protected override string AcquireMstx()
        {
            // Compress Content adding version return templatex path.
            var tempFolder = Path.Combine(GetTempFolder(), SourceFolderName);

            Copy(Origin, tempFolder);

            Fs.CopyRecursive(@".\TestData\StyleCop", Path.Combine(tempFolder, "Features", "StyleCop"));

            File.WriteAllText(Path.Combine(tempFolder, "version.txt"), LocalTemplatesVersion);

            return Templatex.Pack(tempFolder);
        }

        protected static void Copy(string sourceFolder, string targetFolder)
        {
            Fs.SafeDeleteDirectory(targetFolder);
            Fs.CopyRecursive(sourceFolder, targetFolder);
        }
    }
}