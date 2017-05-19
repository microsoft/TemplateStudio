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
using System.IO;

namespace Microsoft.Templates.Core.Locations
{
    public class LocalTemplatesSource : TemplatesSource
    {
        public string LocalTemplatesVersion { get; private set; }
        public string LocalWizardVersion { get; private set; }

        public override string Id { get => "Local"; }

        public string Origin { get => $@"..\..\..\..\..\{SourceFolderName}"; }

        public LocalTemplatesSource() : this("0.0.0.0", "0.0.0.0")
        {
        }

        public LocalTemplatesSource(string wizardVersion, string templatesVersion)
        {
            LocalTemplatesVersion = templatesVersion;
            LocalWizardVersion = wizardVersion;
        }

        public override void Acquire(string targetFolder)
        {
            var targetVersionFolder = Path.Combine(targetFolder, LocalTemplatesVersion);
            Copy(Origin, targetVersionFolder);
        }

        public override void ExtractFromMstx(string mstxFilePath, string targetFolder)
        {
            //Running locally "Extract" a version compatible with the wizard
            var targetVersionFolder = Path.Combine(targetFolder, LocalWizardVersion);
            Copy(Origin, targetVersionFolder);
        }

        protected static void Copy(string sourceFolder, string targetFolder)
        {
            Fs.SafeDeleteDirectory(targetFolder);
            Fs.CopyRecursive(sourceFolder, targetFolder);
        }
    }
}
