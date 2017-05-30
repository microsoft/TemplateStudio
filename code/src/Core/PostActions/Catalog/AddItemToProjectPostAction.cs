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

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class AddItemToProjectPostAction : PostAction<IReadOnlyList<ICreationPath>>
    {
        public AddItemToProjectPostAction(IReadOnlyList<ICreationPath> config) : base(config)
        {
        }

        public override void Execute()
        {            
            var itemsToAdd = _config
                                .Where(o => !string.IsNullOrWhiteSpace(o.Path))
                                .Select(GetPath)
                                .ToList();
            //.Select(o => Path.GetFullPath(Path.Combine(GenContext.Current.OutputPath, o.Path)))                                

            GenContext.ToolBox.Shell.AddItems(itemsToAdd.ToArray());
        }
        // mvegaca: This is a temporaly code in POC to generate xamarin template before get a final solution
        private string GetPath(ICreationPath path)
        {
            return Path.GetFullPath(Path.Combine($"{GenContext.Current.OutputPath}\\{GenContext.Current.ProjectName}", path.Path));            
        }
    }
}
