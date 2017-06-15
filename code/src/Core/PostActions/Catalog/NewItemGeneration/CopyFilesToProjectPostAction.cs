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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class CopyFilesToProjectPostAction : PostAction
    {
        public override void Execute()
        {
            var modifiedFiles = GenContext.Current.ConflictFiles.Concat(GenContext.Current.MergeFilesFromProject.Keys);

            foreach (var file in modifiedFiles)
            {
                var sourceFile = Path.Combine(GenContext.Current.OutputPath, file);
                var destFilePath = Path.Combine(GenContext.Current.ProjectPath, file);
                var destDirectory = Path.GetDirectoryName(destFilePath);
                Fs.SafeCopyFile(sourceFile, destDirectory, true);
            }

            foreach (var file in GenContext.Current.NewFiles)
            {
                var sourceFile = Path.Combine(GenContext.Current.OutputPath, file);
                var destFileName = Path.Combine(GenContext.Current.ProjectPath, file);
                var destDirectory = Path.GetDirectoryName(destFileName);
                Fs.SafeCopyFile(sourceFile, destDirectory, true);

                // Add to projectItems to add to project later
                GenContext.Current.ProjectItems.Add(destFileName);
                GenContext.Current.FilesToOpen.Add(destFileName);
            }
        }
    }
}
