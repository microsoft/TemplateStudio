// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.UI.ViewModels.NewProject;
using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.Templates.UI.Services
{
    public static class UserSelectionService
    {
        private static Func<ObservableCollection<ObservableCollection<SavedTemplateViewModel>>> _getSavedPages;
        private static Func<ObservableCollection<SavedTemplateViewModel>> _getSavedFeatures;
        public static void Initialize(Func<ObservableCollection<ObservableCollection<SavedTemplateViewModel>>> getSavedPages, Func<ObservableCollection<SavedTemplateViewModel>> getSavedFeatures)
        {
            _getSavedPages = getSavedPages;
            _getSavedFeatures = getSavedFeatures;
        }

        public static void SetupTemplatesFromLayout(string projectTypeName, string frameworkName)
        {
            var layout = GenComposer.GetLayoutTemplates(projectTypeName, frameworkName);

            foreach (var item in layout)
            {
                if (item.Template != null)
                {
                    AddTemplateAndDependencies((item.Layout.name, item.Template), frameworkName, !item.Layout.@readonly);
                }
            }
        }

        public static void AddTemplateAndDependencies((string name, ITemplateInfo template) item, string frameworkName, bool isRemoveEnabled = true)
        {
            var newItem = new SavedTemplateViewModel(item, isRemoveEnabled);
            SaveNewTemplate(newItem);

            var dependencies = GenComposer.GetAllDependencies(item.template, frameworkName);
            foreach (var dependencyTemplate in dependencies)
            {
                var dependencyItem = new SavedTemplateViewModel((dependencyTemplate.GetDefaultName(), dependencyTemplate), isRemoveEnabled);
                SaveNewTemplate(dependencyItem);
            }
            MainViewModel.Current.RebuildLicenses();
        }

        public static bool SaveNewTemplate(SavedTemplateViewModel newItem)
        {
            if (newItem == null)
            {
                return false;
            }
            var identities = new List<string>();
            _getSavedPages().ToList().ForEach(spg => identities.AddRange(spg.Select(sp => sp.Identity)));
            identities.AddRange(_getSavedFeatures().Select(sf => sf.Identity));

            if (newItem.MultipleInstance == false && identities.Any(i => i == newItem.Identity))
            {
                return false;
            }
            if (newItem.TemplateType == TemplateType.Page)
            {
                while (_getSavedPages().Count < newItem.GenGroup + 1)
                {
                    var items = new ObservableCollection<SavedTemplateViewModel>();
                    _getSavedPages().Add(items);
                    OrderingService.AddList(items, _getSavedPages().Count == 1);
                }
                _getSavedPages()[newItem.GenGroup].Add(newItem);
            }
            else if (newItem.TemplateType == TemplateType.Feature)
            {
                _getSavedFeatures().Add(newItem);
            }
            return true;
        }

        // Return null if template was removed successfully and a Existing SavedTemplate if it can not be removed becaouse this template depends on it
        public static SavedTemplateViewModel RemoveTemplate(SavedTemplateViewModel item)
        {
            // Look if is there any templates that depends on item
            SavedTemplateViewModel dependency = AnySavedTemplateDependsOnItem(item);
            if (dependency == null)
            {
                // Remove template
                if (_getSavedPages()[item.GenGroup].Contains(item))
                {
                    _getSavedPages()[item.GenGroup].Remove(item);
                }
                else if (_getSavedFeatures().Contains(item))
                {
                    _getSavedFeatures().Remove(item);
                }

                TryRemoveHiddenDependencies(item);
                MainViewModel.Current.RebuildLicenses();
                AppHealth.Current.Telemetry.TrackEditSummaryItemAsync(EditItemActionEnum.Remove).FireAndForget();
            }
            return dependency;
        }

        private static void TryRemoveHiddenDependencies(SavedTemplateViewModel item)
        {
            foreach (var identity in item.DependencyList)
            {
                var dependency = _getSavedFeatures().FirstOrDefault(sf => sf.Identity == identity);
                if (dependency == null)
                {
                    foreach (var pageGroup in _getSavedPages())
                    {
                        dependency = pageGroup.FirstOrDefault(sf => sf.Identity == identity);
                        if (dependency != null)
                        {
                            break;
                        }
                    }
                }

                if (dependency != null)
                {
                    // If the template is not hidden we can not remove it because it could be added in wizard
                    if (dependency.IsHidden)
                    {
                        // Look if there are another saved template that depends on it.
                        // For example, if it's added two different chart pages, when remove the first one SampleDataService can not be removed, but if no saved templates use SampleDataService, it can be removed.
                        if (!_getSavedFeatures().Any(sf => sf.DependencyList.Any(d => d == dependency.Identity)) || _getSavedPages().Any(spg => spg.Any(sp => sp.DependencyList.Any(d => d == dependency.Identity))))
                        {
                            RemoveTemplate(dependency);
                        }
                    }
                }
            }
        }

        private static SavedTemplateViewModel AnySavedTemplateDependsOnItem(SavedTemplateViewModel item)
        {
            SavedTemplateViewModel dependencyItem = null;
            foreach (var group in _getSavedPages())
            {
                dependencyItem = group.FirstOrDefault(st => st.DependencyList.Any(d => d == item.Identity));
                if (dependencyItem != null)
                {
                    break;
                }
            }

            if (dependencyItem == null)
            {
                dependencyItem = _getSavedFeatures().FirstOrDefault(st => st.DependencyList.Any(d => d == item.Identity));
            }

            return dependencyItem;
        }
    }
}
