// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class AddItemToContextPostAction : PostAction<IReadOnlyList<ICreationPath>>
    {
        private readonly Dictionary<string, string> _genParameters;
        private readonly string _destinationPath;

        public AddItemToContextPostAction(string relatedTemplate, IReadOnlyList<ICreationPath> config, Dictionary<string, string> genParameters, string destinationPath)
            : base(relatedTemplate, config)
        {
            _genParameters = genParameters;
            _destinationPath = destinationPath;
        }

        internal override void ExecuteInternal()
        {
            var itemsToAdd = Config
                                .Where(o => !string.IsNullOrWhiteSpace(o.Path))
                                .Select(o => Path.GetFullPath(Path.Combine(_destinationPath, o.Path)))
                                .ToList();

            GenContext.Current.ProjectInfo.ProjectItems.AddRange(itemsToAdd);
        }
    }
}
