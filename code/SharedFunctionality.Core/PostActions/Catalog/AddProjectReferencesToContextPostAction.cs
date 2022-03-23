// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Templates;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    // This is a Template Defined Post-Action with the following configuration in the template.
    //   "postActions": [
    //    {
    //        "description": "Have UWP app reference this project",
    //        "manualInstructions": [ ],
    //        "actionId": "849AAEB8-487D-45B3-94B9-77FA74E83A012",
    //        "args": {
    //            "fileIndex" : "0",
    //            "projectPath": "Param_ProjectName\\Param_ProjectName.csproj",
    //            "specifiedPathIsTarget": "false"
    //        },
    //        "continueOnError": "true"
    //    }
    //  ]
    // Expected args:
    //    - fileIndex -> The file index from the primary outputs which is the project file that will be referenced by the other project.
    //    - referencedProject -> Alternatively you can specify the referenced project if not available on the primary outputs (e.g. on a composition template)
    //    - projectPath -> The path to the project where the reference will be added.
    // Optional args:
    //    - specifiedPathIsTarget -> If 'true' the direction of the reference is reversed. (i.e. The indexed file will reference the project specified by path)
    public class AddProjectReferencesToContextPostAction : TemplateDefinedPostAction
    {
        public const string Id = "849AAEB8-487D-45B3-94B9-77FA74E83A01";

        public override Guid ActionId { get => new Guid(Id); }

        private readonly Dictionary<string, string> _parameters;
        private readonly IReadOnlyList<ICreationPath> _primaryOutputs;
        private readonly string _destinationPath;

        public AddProjectReferencesToContextPostAction(string relatedTemplate, IPostAction templatePostAction, IReadOnlyList<ICreationPath> primaryOutputs, Dictionary<string, string> parameters, string destinationPath)
            : base(relatedTemplate, templatePostAction)
        {
            _parameters = parameters;
            _primaryOutputs = primaryOutputs;
            _destinationPath = destinationPath;
        }

        internal override void ExecuteInternal()
        {
            var parameterReplacements = new FileRenameParameterReplacements(_parameters);
            var projectPath = Path.Combine(_destinationPath, parameterReplacements.ReplaceInPath(Args["projectPath"]));
            var referencedProject = string.Empty;

            if (Args.ContainsKey("referencedProjectPath"))
            {
                referencedProject = Path.Combine(_destinationPath, parameterReplacements.ReplaceInPath(Args["referencedProjectPath"]));
            }
            else
            {
                int targetProjectIndex = int.Parse(Args["fileIndex"]);
                referencedProject = Path.GetFullPath(Path.Combine(_destinationPath, _primaryOutputs[targetProjectIndex].Path));
            }

            var invert = Args.ContainsKey("specifiedPathIsTarget") && bool.Parse(Args["specifiedPathIsTarget"] ?? "False");

            var projectReference = new ProjectReference();

            if (invert)
            {
                projectReference.Project = referencedProject;
                projectReference.ReferencedProject = projectPath;
            }
            else
            {
                projectReference.Project = projectPath;
                projectReference.ReferencedProject = referencedProject;
            }

            if (!GenContext.Current.ProjectInfo.ProjectReferences.Any(n => n.Equals(projectReference)))
            {
                GenContext.Current.ProjectInfo.ProjectReferences.Add(projectReference);
            }
        }
    }
}
