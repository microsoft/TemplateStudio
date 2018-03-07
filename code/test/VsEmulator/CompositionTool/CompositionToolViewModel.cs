// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI;

namespace Microsoft.Templates.VsEmulator
{
    public class CompositionToolViewModel : Observable
    {
        private MetadataInfo _projectType;
        private MetadataInfo _framework;
        private ITemplateInfo _template;

        public MetadataInfo ProjectType
        {
            get => _projectType;
            set => SelectProjectType(value, true);
        }

        public MetadataInfo Framework
        {
            get => _framework;
            set => SelectFramework(value, true);
        }

        public ITemplateInfo Template
        {
            get => _template;
            set => SelectTemplate(value);
        }

        public ObservableCollection<Selectable<MetadataInfo>> ProjectTypes { get; } = new ObservableCollection<Selectable<MetadataInfo>>();

        public ObservableCollection<Selectable<MetadataInfo>> Frameworks { get; } = new ObservableCollection<Selectable<MetadataInfo>>();

        public ObservableCollection<Selectable<ITemplateInfo>> Pages { get; } = new ObservableCollection<Selectable<ITemplateInfo>>();

        public ObservableCollection<Selectable<ITemplateInfo>> Features { get; } = new ObservableCollection<Selectable<ITemplateInfo>>();

        public CompositionToolViewModel()
        {

        }

        public void LoadData(MetadataInfo projectType = null, MetadataInfo framework = null)
        {
            if (!ProjectTypes.Any())
            {
                var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes()
                    .Select(m => new Selectable<MetadataInfo>(m));
                ProjectTypes.AddRange(projectTypes);
            }

            SelectProjectType(projectType ?? ProjectTypes.First().Item);

            if (!Frameworks.Any())
            {
                var supportedFx = GenComposer.GetSupportedFx(ProjectType.Name);
                var frameworks = GenContext.ToolBox.Repo.GetFrameworks()
                    .Where(m => supportedFx.Contains(m.Name))
                    .Select(m => new Selectable<MetadataInfo>(m));
                Frameworks.AddRange(frameworks);
            }

            SelectFramework(framework ?? Frameworks.First().Item);

            if (!Pages.Any())
            {
                var pages = GenContext.ToolBox.Repo.Get(t =>
                    t.GetTemplateType() == TemplateType.Page &&
                    t.GetFrameworkList().Contains(Framework.Name) &&
                    !t.GetIsHidden())
                    .Select(t => new Selectable<ITemplateInfo>(t));

                Pages.AddRange(pages);
                SelectTemplate(Pages.First().Item);
            }

            if (!Features.Any())
            {
                var features = GenContext.ToolBox.Repo.Get(t =>
                    t.GetTemplateType() == TemplateType.Feature &&
                    t.GetFrameworkList().Contains(Framework.Name) &&
                    !t.GetIsHidden())
                    .Select(t => new Selectable<ITemplateInfo>(t));

                Features.AddRange(features);
            }
        }

        private void SelectProjectType(MetadataInfo value, bool refreshData = false)
        {
            SetProperty(ref _projectType, value);
            SelectMetadataInfoItem(ProjectTypes, value);
            if (refreshData)
            {
                Frameworks.Clear();
                LoadData(value, null);
            }
        }

        private void SelectFramework(MetadataInfo value, bool refreshData = false)
        {
            SetProperty(ref _framework, value);
            SelectMetadataInfoItem(Frameworks, value);
            if (refreshData)
            {
                LoadData(null, value);
            }
        }

        private void SelectTemplate(ITemplateInfo value)
        {
            SetProperty(ref _template, value);

            foreach (var item in Pages)
            {
                item.IsSelected = item.Item == value;
            }

            foreach (var item in Features)
            {
                item.IsSelected = item.Item == value;
            }
        }

        private void SelectMetadataInfoItem(IEnumerable<Selectable<MetadataInfo>> selectableItems, MetadataInfo selectedItem)
        {
            foreach (var item in selectableItems)
            {
                item.IsSelected = item.Item == selectedItem;
            }
        }
    }
}
