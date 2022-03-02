// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class GenInfoComposition : GenInfo
    {
        public string TemplatePath { get; set; }

        public ObservableCollection<CompositionFile> Files { get; } = new ObservableCollection<CompositionFile>();

        public GenInfoComposition(GenInfo item)
            : base(item.Name, item.Template)
        {
            foreach (var parameter in item.Parameters)
            {
                Parameters.Add(parameter.Key, parameter.Value);
            }

            LoadFiles();
        }

        private void LoadFiles()
        {
            TemplatePath = $"{GenContext.ToolBox.Repo.CurrentContentFolder}{Template.ConfigPlace.Replace("/", "\\")}";
            var configLoc = Template.ConfigPlace.Replace("/.template.config/template.json", string.Empty);
            var fullConfigLoc = $"{GenContext.ToolBox.Repo.CurrentContentFolder}{configLoc.Replace("/", "\\")}";
            var files = Directory.EnumerateFiles(fullConfigLoc, "*.*", SearchOption.AllDirectories)
                                 .Where(f => !f.Contains(".template.config"))
                                 .ToList();
            foreach (var file in files)
            {
                Files.Add(new CompositionFile(file));
            }
        }
    }
}
