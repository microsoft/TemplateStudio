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

using System.Collections.Generic;
using System.IO;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class AddProjectToSolutionPostAction : PostAction<IReadOnlyList<ICreationPath>>
    {
        public AddProjectToSolutionPostAction(IReadOnlyList<ICreationPath> config) : base(config)
        {
        }

        public override void Execute()
        {
            foreach (var output in _config)
            {
                if (!string.IsNullOrWhiteSpace(output.Path))
                {
                    var projectPath = Path.GetFullPath(Path.Combine(GenContext.Current.ProjectPath, output.Path));
                    GenContext.ToolBox.Shell.AddProjectToSolution(projectPath);
                }
            }
        }
    }
}
