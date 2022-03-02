// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Templates;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    // This is a Template Defined Post-Action with the following configuration in the template.
    //   "postActions": [
    //    {
    //        "description": "Unset deploy of XamlIslandAppProj",
    //        "manualInstructions": [ ],
    //        "actionId": "F74F9FBA-D4F5-494E-970E-D99DF5E3F4F3",
    //        "args": {
    //            "projectName": "Param_ProjectName.XamlIslandApp\\Param_ProjectName.XamlIslandApp.csproj",
    //            "deploy": "false"
    //        },
    //        "continueOnError": "true"
    //    }
    //  ]
    // Expected args:
    //    - projectName -> The name of the project which configuration will be changed (project name as specified in the solution file)
    //    - deploy -> Value of the Deploy configuration
    public class AddProjectConfigurationToContextPostaction : TemplateDefinedPostAction
    {
        public const string Id = "F74F9FBA-D4F5-494E-970E-D99DF5E3F4F3";

        private readonly Dictionary<string, string> _parameters;

        private readonly string _destinationPath;

        public override Guid ActionId { get => new Guid(Id); }

        public AddProjectConfigurationToContextPostaction(string relatedTemplate, IPostAction templatePostAction, Dictionary<string, string> parameters, string destinationPath)
            : base(relatedTemplate, templatePostAction)
        {
            _parameters = parameters;
            _destinationPath = destinationPath;
        }

        internal override void ExecuteInternal()
        {
            var parameterReplacements = new FileRenameParameterReplacements(_parameters);

            var projectName = parameterReplacements.ReplaceInPath(Args["projectName"]);

            var projectConfiguration = new ProjectConfiguration
            {
                Project = projectName,
                SetDeploy = bool.Parse(Args["deploy"]),
            };

            if (!GenContext.Current.ProjectInfo.ProjectConfigurations.Any(n => n.Equals(projectConfiguration)))
            {
                GenContext.Current.ProjectInfo.ProjectConfigurations.Add(projectConfiguration);
            }
        }
    }
}
