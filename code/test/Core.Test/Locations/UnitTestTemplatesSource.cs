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

using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Core.Test.Locations
{
    public class UnitTestsTemplatesSource : TemplatesSource
    {
        private string LocalVersion = "0.0.0.0";

        public override string Id => "UnitTest";

        public override void Acquire(string targetFolder)
        {
            var targetVersionFolder = Path.Combine(targetFolder, LocalVersion);

            Copy($@"..\..\TestData\{SourceFolderName}", targetVersionFolder);
        }

        public override void ExtractFromMstx(string mstxFilePath, string targetFolder)
        {
            //Actually we do not extract from an Mstx, we want to copy local test templates to work with latest local content
            Acquire(targetFolder);
        }

        protected static void Copy(string sourceFolder, string targetFolder)
        {
            Fs.SafeDeleteDirectory(targetFolder);
            Fs.CopyRecursive(sourceFolder, targetFolder);
        }
    }
}
