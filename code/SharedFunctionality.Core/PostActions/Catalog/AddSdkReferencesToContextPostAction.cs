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
    public class AddSdkReferencesToContextPostAction : TemplateDefinedPostAction
    {
        public const string Id = "9E683FB4-CE5B-4AAE-8A36-63CD8A5B4977";

        public override Guid ActionId { get => new Guid(Id); }

        private readonly Dictionary<string, string> _parameters;
        private readonly string _destinationPath;

        public AddSdkReferencesToContextPostAction(string relatedTemplate, IPostAction templatePostAction, Dictionary<string, string> parameters, string destinationPath)
            : base(relatedTemplate, templatePostAction)
        {
            _parameters = parameters;
            _destinationPath = destinationPath;
        }

        internal override void ExecuteInternal()
        {
            var parameterReplacements = new FileRenameParameterReplacements(_parameters);
            var projectPath = Path.Combine(_destinationPath, parameterReplacements.ReplaceInPath(Args["projectPath"]));

            var sdkReference = new SdkReference
            {
                Project = projectPath,
                Name = Args["name"],
                Sdk = Args["sdk"],
            };

            if (!GenContext.Current.ProjectInfo.SdkReferences.Any(n => n.Equals(sdkReference)))
            {
                GenContext.Current.ProjectInfo.SdkReferences.Add(sdkReference);
            }
        }
    }
}
