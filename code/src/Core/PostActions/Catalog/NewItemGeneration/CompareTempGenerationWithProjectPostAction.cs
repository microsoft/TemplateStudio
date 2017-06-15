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
using System.Text.RegularExpressions;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class CompareTempGenerationWithProjectPostAction : PostAction
    {
        public CompareTempGenerationWithProjectPostAction() : base()
        {
        }

        public override void Execute()
        {
            var files = Directory
                .EnumerateFiles(GenContext.Current.OutputPath, "*", SearchOption.AllDirectories)
                .Where(f => !Regex.IsMatch(f, MergePostAction.PostactionRegex) && !Regex.IsMatch(f, MergePostAction.FailedPostactionRegex))
                .ToList();

            foreach (var file in files)
            {
                var destFilePath = file.Replace(GenContext.Current.OutputPath, GenContext.Current.ProjectPath);
                var fileName = file.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);

                var projectFileName = Path.GetFullPath(Path.Combine(GenContext.Current.ProjectPath, fileName));

                if (File.Exists(projectFileName))
                {
                    if (GenContext.Current.MergeFilesFromProject.ContainsKey(fileName))
                    {
                        if (FilesAreEqual(file, destFilePath))
                        {
                            GenContext.Current.MergeFilesFromProject.Remove(fileName);
                        }
                    }
                    else
                    {
                        if (!FilesAreEqual(file, destFilePath))
                        {
                            GenContext.Current.ConflictFiles.Add(fileName);
                        }
                    }
                }
                else
                {
                    GenContext.Current.NewFiles.Add(fileName);
                }
            }
        }

        private static bool FilesAreEqual(string file, string destFilePath)
        {
            return File.ReadAllLines(file).SequenceEqual(File.ReadAllLines(destFilePath));
        }
    }
}
