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

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergePostAction : PostAction<string>
    {
        private const string Suffix = "postaction";

        public const string Extension = "_" + Suffix + ".";
        public const string GlobalExtension = "$*_g" + Suffix + ".";
        public const string PostactionRegex = @"(\$\S*)?(_" + Suffix + "|_g" + Suffix + @")\.";

        public MergePostAction(string config) : base(config)
        {
        }

        public override void Execute()
        {
            string originalFilePath = GetFilePath();

            if (string.IsNullOrEmpty(originalFilePath))
            {
                throw new FileNotFoundException($"There is no merge target for file '{_config}'");
            }

            var source = File.ReadAllLines(originalFilePath).ToList();
            var merge = File.ReadAllLines(_config).ToList();

            IEnumerable<string> result = source.HandleRemovals(merge);
            result = result.Merge(merge.RemoveRemovals());

            File.WriteAllLines(originalFilePath, result);
            File.Delete(_config);

            //REFRESH PROJECT TO UN-DIRTY IT
            if (Path.GetExtension(_config).Equals(".csproj", StringComparison.OrdinalIgnoreCase))
            {
                Gen.GenContext.ToolBox.Shell.RefreshProject();
            }
        }

        private string GetFilePath()
        {
            if (Path.GetFileName(_config).StartsWith(Extension))
            {
                var extension = Path.GetExtension(_config);
                var directory = Path.GetDirectoryName(_config);

                return Directory.EnumerateFiles(directory, $"*{extension}").FirstOrDefault(f => !f.Contains(Suffix));
            }
            else
            {
                var path = Regex.Replace(_config, PostactionRegex, ".");

                return (File.Exists(path) ? path : String.Empty);
            }
        }
    }
}
