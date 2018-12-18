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
    public class AddNugetReferenceToContextPostAction : TemplateDefinedPostAction
    {
        public const string Id = "0B814718-16A3-4F7F-89F1-69C0F9170EAD";

        private readonly Dictionary<string, string> _parameters;

        private readonly string _destinationPath;

        public override Guid ActionId { get => new Guid(Id); }

        public AddNugetReferenceToContextPostAction(string relatedTemplate, IPostAction templatePostAction, Dictionary<string, string> parameters, string destinationPath)
            : base(relatedTemplate, templatePostAction)
        {
            _parameters = parameters;
            _destinationPath = destinationPath;
        }

        internal override void ExecuteInternal()
        {
            var parameterReplacements = new FileRenameParameterReplacements(_parameters);

            var projectPath = Path.Combine(_destinationPath, parameterReplacements.ReplaceInPath(Args["projectPath"]));

            var nugetReference = new NugetReference
            {
                Project = projectPath,
                Version = Args["version"],
                PackageId = Args["packageId"],
            };

            if (!GenContext.Current.ProjectInfo.NugetReferences.Any(n => n.Equals(nugetReference)))
            {
                GenContext.Current.ProjectInfo.NugetReferences.Add(nugetReference);
            }
        }
    }
}
