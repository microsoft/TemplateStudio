// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class AddItemToContextPostAction : PostAction<IReadOnlyList<ICreationPath>>
    {
        private Dictionary<string, string> _genParameters;

        public AddItemToContextPostAction(string relatedTemplate, IReadOnlyList<ICreationPath> config, Dictionary<string, string> genParameters)
            : base(relatedTemplate, config)
        {
            _genParameters = genParameters;
        }

        internal override void ExecuteInternal()
        {
            // HACK: Template engine is not replacing fileRename parameters correctly in file names, when used together with sourceName
            var itemsToAdd = Config
                                .Where(o => !string.IsNullOrWhiteSpace(o.Path))
                                .Select(o => Path.GetFullPath(Path.Combine(GenContext.Current.OutputPath, o.GetOutputPath(_genParameters))))
                                .ToList();

            GenContext.Current.ProjectItems.AddRange(itemsToAdd);
        }
    }
}
