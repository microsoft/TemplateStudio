// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    // This is a Template Defined Post-Action with the following configuration in the template.
    //   "postActions": [
    //    {
    //        "description": "Generate Test Certificate",
    //        "manualInstructions": [ ],
    //        "actionId": "65057255-BD7B-443C-8180-5D82B9DA9E22",
    //        "args": {
    //            "files" : "0"
    //        },
    //        "continueOnError": "true"
    //    }
    //  ]
    // Expected args:
    //    - files -> reference the file index from the primary outputs which is the target project where the certificate will be added
    // Remarks: this post action can work with single project templates or multi project templates.
    public class GenerateTestCertificatePostAction : TemplateDefinedPostAction
    {
        public const string Id = "65057255-BD7B-443C-8180-5D82B9DA9E22";

        public override Guid ActionId { get => new Guid(Id); }

        private Dictionary<string, string> _parameters;
        private string _publisherName;
        private IReadOnlyList<ICreationPath> _primaryOutputs;
        private string _destinationPath;

        public GenerateTestCertificatePostAction(string relatedTemplate, string publisherName, IPostAction templatePostAction, IReadOnlyList<ICreationPath> primaryOutputs, Dictionary<string, string> parameters, string destinationPath)
            : base(relatedTemplate, templatePostAction)
        {
            _parameters = parameters;
            _publisherName = publisherName;
            _primaryOutputs = primaryOutputs;
            _destinationPath = destinationPath;
        }

        internal override void ExecuteInternal()
        {
            int targetProjectIndex = int.Parse(Args["files"]);
            var projectFile = _primaryOutputs[targetProjectIndex].Path;
            string projectFileWithoutExtension = projectFile.Replace(Path.GetExtension(projectFile), string.Empty);

            var pfx = GenContext.ToolBox.Shell.Certificate.CreateCertificate(_publisherName);

            AddToProject(pfx, projectFileWithoutExtension);
            RemoveFromStore(pfx);
        }

        private void AddToProject(string base64Encoded, string projectFileWithoutExtension)
        {
            var filePath = Path.GetFullPath(Path.Combine(_destinationPath, projectFileWithoutExtension) + "_TemporaryKey.pfx");
            File.WriteAllBytes(filePath, Convert.FromBase64String(base64Encoded));

            GenContext.Current.ProjectInfo.ProjectItems.Add(filePath);
        }

        private static void RemoveFromStore(string base64Encoded)
        {
            var certificate = new X509Certificate2(Convert.FromBase64String(base64Encoded), string.Empty);
            var store = new X509Store(StoreLocation.CurrentUser);

            store.Open(OpenFlags.ReadWrite);
            store.Remove(certificate);
            store.Close();
        }
    }
}
