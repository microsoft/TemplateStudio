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

using Microsoft.Templates.Core.Gen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergePostAction : PostAction<MergeConfiguration>
    {
        private const string Suffix = "postaction";
        private const string NewSuffix = "failedpostaction";

        public const string Extension = "_" + Suffix + ".";
        public const string GlobalExtension = "$*_g" + Suffix + ".";
        public const string PostActionIntentExtension = ".md";

        public const string PostactionRegex = @"(\$\S*)?(_" + Suffix + "|_g" + Suffix + @")\.";
        
        public const string FailedPostactionRegex = @"(\$\S*)?(_" + NewSuffix + "|_g" + NewSuffix + @")\.";

        public MergePostAction(MergeConfiguration config) : base(config)
        {
        }

        public override void Execute()
        {
            string originalFilePath = GetFilePath();

            if (string.IsNullOrEmpty(originalFilePath))
            {
                if (_config.FailOnError)
                { 
                    throw new FileNotFoundException($"There is no merge target for file '{_config.FilePath}'");
                }
                else
                {
                    AddGenerationWarning();
                    File.Copy(_config.FilePath, _config.FilePath.Replace(Suffix, NewSuffix), true);
                    File.Delete(_config.FilePath);
                    return;
                }
            }   

            var source = File.ReadAllLines(originalFilePath).ToList();
            var merge = File.ReadAllLines(_config.FilePath).ToList();

            IEnumerable<string> result = source.HandleRemovals(merge);
            var mergeSnippets = new Dictionary<int, IEnumerable<string>>();

            if (!_config.GenerateMergeSnippets)
            {
                result = result.Merge(merge.RemoveRemovals());
            }
            else
            {
                result = result.Merge(merge.RemoveRemovals(), out mergeSnippets);
            }
            

            File.WriteAllLines(originalFilePath, result);

            if (_config.GenerateMergeSnippets)
            {
                var fileName = Path.GetFileName(originalFilePath);

                if (GenContext.Current.MergeFilesFromProject.Select(f => f.FileName).Contains(fileName))
                {
                    var mergeFile = GenContext.Current.MergeFilesFromProject.FirstOrDefault(f => f.FileName == fileName);
                    
                    foreach (var mergeSnippet in mergeSnippets)
                    {
                        if (mergeFile.MergeSnippets.ContainsKey(mergeSnippet.Key))
                        {
                            mergeFile.MergeSnippets[mergeSnippet.Key]= mergeSnippet.Value.Concat(mergeFile.MergeSnippets[mergeSnippet.Key]);
                        }
                        else
                        {
                            mergeFile.MergeSnippets[mergeSnippet.Key] = mergeSnippet.Value;
                        }
                    }   
                } 
            }

            File.Delete(_config.FilePath);

            //REFRESH PROJECT TO UN-DIRTY IT
            if (Path.GetExtension(_config.FilePath).Equals(".csproj", StringComparison.OrdinalIgnoreCase))
            {
                Gen.GenContext.ToolBox.Shell.RefreshProject();
            }
        }

        private void AddGenerationWarning()
        {
            var fileName = _config.FilePath.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, String.Empty);
            var description = $"Could not find merge target for file '{fileName}'. Please integrate the content from the postaction file manually.";
            var intent = GetPostActionIntent();
            var failedFileName = fileName.Replace(Suffix, NewSuffix);
            GenContext.Current.GenerationWarnings.Add(new GenerationWarning(failedFileName, _config.FilePath, description, intent));
        }

        private string GetPostActionIntent()
        {
            var intentFile = _config.FilePath.Replace(Path.GetExtension(_config.FilePath), PostActionIntentExtension);
            if (File.Exists(intentFile))
            {
                return File.ReadAllText(intentFile);
            }
            return string.Empty;
        }

        private string GetFilePath()
        {
            if (Path.GetFileName(_config.FilePath).StartsWith(Extension))
            {
                var extension = Path.GetExtension(_config.FilePath);
                var directory = Path.GetDirectoryName(_config.FilePath);

                return Directory.EnumerateFiles(directory, $"*{extension}").FirstOrDefault(f => !f.Contains(Suffix));
            }
            else
            {
                var path = Regex.Replace(_config.FilePath, PostactionRegex, ".");

                return (File.Exists(path) ? path : String.Empty);
            }
        }
    }
}
