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
using System.Linq;

namespace Microsoft.Templates.Core.PostActions.Catalog.ReplaceFragments
{
    public class ReplaceFragmentsPostAction : PostAction
    {
        public override void Execute()
        {
            var classFiles = Directory
                .EnumerateFiles(Gen.GenContext.Current.OutputPath, "*.cs", SearchOption.AllDirectories)
                .ToList();

            foreach (var classFile in classFiles)
            {
                var fileContent = File.ReadAllLines(classFile).ToList();
                var sortResult = fileContent.ReplaceFragments();

                if (sortResult)
                {
                    File.WriteAllLines(classFile, fileContent);
                }
            }
        }
    }
}
