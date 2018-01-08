// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public enum TemplateOrigin
    {
        Layout,
        UserSelection
    }

    public class UserSelection : Observable
    {
        private bool _isInitialized;

        public ObservableCollection<SavedTemplateViewModel> Pages { get; } = new ObservableCollection<SavedTemplateViewModel>();

        public ObservableCollection<SavedTemplateViewModel> Features { get; } = new ObservableCollection<SavedTemplateViewModel>();

        public bool HasItemsAddedByUser { get; private set; }

        public UserSelection()
        {
        }

        public void Initialize(string projectTypeName, string frameworkName, IEnumerable<TemplateGroupViewModel> pageGroups, IEnumerable<TemplateGroupViewModel> featureGroups)
        {
            if (_isInitialized)
            {
                Pages.Clear();
                Features.Clear();
            }

            var layout = GenComposer.GetLayoutTemplates(projectTypeName, frameworkName);
            foreach (var item in layout)
            {
                if (item.Template != null)
                {
                    var groups = item.Template.GetTemplateType() == TemplateType.Page ? pageGroups : featureGroups;
                    foreach (var group in groups)
                    {
                        var template = group.GetTemplate(item);
                        if (template != null)
                        {
                            Add(TemplateOrigin.Layout, template, item.Layout.Name);
                        }
                    }
                }
            }

            _isInitialized = true;
        }

        public IEnumerable<string> GetNames() => Pages.Select(t => t.Name)
                                                    .Concat(Features.Select(f => f.Name));

        public SavedTemplateViewModel Add(TemplateOrigin templateOrigin, TemplateInfoViewModel template, string name = null)
        {
            template.UpdateSelection();
            var items = template.TemplateType == Core.TemplateType.Page ? Pages : Features;
            var newTemplate = new SavedTemplateViewModel(template);

            if (!string.IsNullOrEmpty(name))
            {
                newTemplate.Name = name;
            }
            else
            {
                newTemplate.Name = ValidationService.InferTemplateName(template.Name, template.ItemNameEditable, template.ItemNameEditable);
                newTemplate.UpdateSelection();
            }

            items.Add(newTemplate);

            // Notiffy collection name to update the visibillity on the layout
            var collectionName = template.TemplateType == TemplateType.Page ? nameof(Pages) : nameof(Features);
            OnPropertyChanged(collectionName);

            if (templateOrigin == TemplateOrigin.UserSelection)
            {
                HasItemsAddedByUser = true;
            }

            return newTemplate;
        }

        internal void ResetUserSelection()
        {
            HasItemsAddedByUser = false;
            _isInitialized = false;
            Pages.Clear();
            Features.Clear();
        }

        internal bool IsTemplateAdded(TemplateInfoViewModel template)
        {
            var collection = template.TemplateType == TemplateType.Page ? Pages : Features;
            return collection.Any(t => t.Equals(template));
        }
    }
}
