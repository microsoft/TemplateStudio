// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public enum TemplateOrigin
    {
        Layout,
        UserSelection,
        Dependency
    }

    public class UserSelectionViewModel : Observable
    {
        private bool _isInitialized;
        private string _projectTypeName;
        private string _frameworkName;
        private string _language;

        public ObservableCollection<SavedTemplateViewModel> Pages { get; } = new ObservableCollection<SavedTemplateViewModel>();

        public ObservableCollection<SavedTemplateViewModel> Features { get; } = new ObservableCollection<SavedTemplateViewModel>();

        public ObservableCollection<LicenseViewModel> Licenses { get; } = new ObservableCollection<LicenseViewModel>();

        public bool HasItemsAddedByUser { get; private set; }

        public UserSelectionViewModel()
        {
        }

        public void Initialize(string projectTypeName, string frameworkName, string language)
        {
            _projectTypeName = projectTypeName;
            _frameworkName = frameworkName;
            _language = language;
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
                    var template = MainViewModel.Instance.GetTemplate(item.Template);
                    if (template != null)
                    {
                        Add(TemplateOrigin.Layout, template, item.Layout.Name);
                    }
                }
            }

            _isInitialized = true;
        }

        public IEnumerable<string> GetNames() => Pages.Select(t => t.Name)
                                                    .Concat(Features.Select(f => f.Name));

        public void Add(TemplateOrigin templateOrigin, TemplateInfoViewModel template, string layoutName = null)
        {
            var dependencies = GenComposer.GetAllDependencies(template.Template, _frameworkName);
            foreach (var dependency in dependencies)
            {
                var dependencyTemplate = MainViewModel.Instance.GetTemplate(dependency);
                if (dependencyTemplate == null)
                {
                    // Case of hidden templates, it's not found on templat lists
                    Add(TemplateOrigin.Dependency, new TemplateInfoViewModel(dependency, _frameworkName));
                }
                else
                {
                    Add(TemplateOrigin.Dependency, dependencyTemplate);
                }
            }

            template.UpdateSelection();
            var savedTemplate = new SavedTemplateViewModel(template);
            if (!IsTemplateAdded(template) || template.MultipleInstance)
            {
                var storageCollection = template.TemplateType == TemplateType.Page ? Pages : Features;
                if (!string.IsNullOrEmpty(layoutName))
                {
                    savedTemplate.Name = layoutName;
                }
                else
                {
                    savedTemplate.Name = ValidationService.InferTemplateName(template.Name, template.ItemNameEditable, template.ItemNameEditable);
                    if (savedTemplate.ItemNameEditable)
                    {
                        savedTemplate.Focus();
                    }
                }

                storageCollection.Add(savedTemplate);

                // Notiffy collection name to update the visibillity on the layout
                var collectionName = template.TemplateType == TemplateType.Page ? nameof(Pages) : nameof(Features);
                OnPropertyChanged(collectionName);

                if (templateOrigin == TemplateOrigin.UserSelection)
                {
                    HasItemsAddedByUser = true;
                }

                var userSelection = GetUserSelection();
                var licenses = GenComposer.GetAllLicences(userSelection);
                LicensesService.SyncLicenses(licenses, Licenses);

                // Notiffy Licenses name to update the visibillity on the layout
                OnPropertyChanged(nameof(Licenses));
            }
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

        private UserSelection GetUserSelection()
        {
            var selection = new UserSelection(_projectTypeName, _frameworkName, _language)
            {
                HomeName = Pages.First().Name
            };

            foreach (var page in Pages)
            {
                selection.Pages.Add(page.GetUserSelection());
            }

            foreach (var feature in Features)
            {
                selection.Features.Add(feature.GetUserSelection());
            }

            return selection;
        }
    }
}
