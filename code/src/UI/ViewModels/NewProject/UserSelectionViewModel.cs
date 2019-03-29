// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public enum TemplateOrigin
    {
        Layout,
        UserSelection,
    }

    public class UserSelectionViewModel : Observable
    {
        private SavedTemplateViewModel _selectedPage;
        private SavedTemplateViewModel _selectedFeature;
        private bool _isInitialized;
        private string _projectTypeName;
        private string _frameworkName;
        private string _platform;
        private string _language;
        private ICommand _editPageCommand;
        private RelayCommand<SavedTemplateViewModel> _deletePageCommand;
        private ICommand _editFeatureCommand;
        private ICommand _deleteFeatureCommand;
        private ICommand _movePageUpCommand;
        private ICommand _movePageDownCommand;
        private string _emptyBackendFramework = string.Empty;

        public ObservableCollection<SavedTemplateViewModel> Pages { get; } = new ObservableCollection<SavedTemplateViewModel>();

        public ObservableCollection<SavedTemplateViewModel> Features { get; } = new ObservableCollection<SavedTemplateViewModel>();

        public ObservableCollection<LicenseViewModel> Licenses { get; } = new ObservableCollection<LicenseViewModel>();

        public ICommand EditPageCommand => _editPageCommand ?? (_editPageCommand = new RelayCommand<SavedTemplateViewModel>((page) => page.IsTextSelected = true, (page) => page != null));

        public RelayCommand<SavedTemplateViewModel> DeletePageCommand => _deletePageCommand ?? (_deletePageCommand = new RelayCommand<SavedTemplateViewModel>((page) => OnDeletePageAsync(page).FireAndForget(), CanDeletePage));

        public ICommand EditFeatureCommand => _editFeatureCommand ?? (_editFeatureCommand = new RelayCommand<SavedTemplateViewModel>((feature) => feature.IsTextSelected = true, (feature) => feature != null));

        public ICommand DeleteFeatureCommand => _deleteFeatureCommand ?? (_deleteFeatureCommand = new RelayCommand<SavedTemplateViewModel>((feature) => OnDeleteFeatureAsync(feature).FireAndForget()));

        public ICommand MovePageUpCommand => _movePageUpCommand ?? (_movePageUpCommand = new RelayCommand(() => OrderingService.MoveUp(SelectedPage)));

        public ICommand MovePageDownCommand => _movePageDownCommand ?? (_movePageDownCommand = new RelayCommand(() => OrderingService.MoveDown(SelectedPage)));

        public bool HasItemsAddedByUser { get; private set; }

        public SavedTemplateViewModel SelectedPage
        {
            get => _selectedPage;
            set => SetProperty(ref _selectedPage, value);
        }

        public SavedTemplateViewModel SelectedFeature
        {
            get => _selectedFeature;
            set => SetProperty(ref _selectedFeature, value);
        }

        public UserSelectionViewModel()
        {
        }

        public async Task InitializeAsync(string projectTypeName, string frameworkName, string platform, string language)
        {
            _projectTypeName = projectTypeName;
            _frameworkName = frameworkName;
            _platform = platform;
            _language = language;
            if (_isInitialized)
            {
                Pages.Clear();
                Features.Clear();
            }

            var layout = GenContext.ToolBox.Repo.GetLayoutTemplates(platform, projectTypeName, frameworkName, _emptyBackendFramework);
            foreach (var item in layout)
            {
                if (item.Template != null)
                {
                    var template = MainViewModel.Instance.GetTemplate(item.Template);
                    if (template != null)
                    {
                        await AddAsync(TemplateOrigin.Layout, template, item.Layout.Name, item.Layout.Readonly);
                    }
                }
            }

            _isInitialized = true;
        }

        public IEnumerable<string> GetNames()
            => Pages.Select(t => t.Name).Concat(Features.Select(f => f.Name));

        public IEnumerable<string> GetPageNames()
            => Pages.Where(p => p.ItemNameEditable).Select(t => t.Name);

        public async Task AddAsync(TemplateOrigin templateOrigin, TemplateInfoViewModel template, string layoutName = null, bool isReadOnly = false)
        {
            foreach (var dependency in template.Template.Dependencies)
            {
                var dependencyTemplate = MainViewModel.Instance.GetTemplate(dependency);
                if (dependencyTemplate == null)
                {
                    // Case of hidden templates, it's not found on templat lists
                    dependencyTemplate = new TemplateInfoViewModel(dependency, _platform, _projectTypeName, _frameworkName);
                }

                await AddAsync(templateOrigin, dependencyTemplate);
            }

            template.IncreaseSelection();
            if (template.IsGroupExclusiveSelection)
            {
                if (template.TemplateType == TemplateType.Feature)
                {
                    var exclusiveSelectionAddedTemplates = Features.Where(f => f.Group == template.Group).ToList();
                    foreach (var exclusiveFeature in exclusiveSelectionAddedTemplates)
                    {
                        await RemoveAsync(exclusiveFeature);
                    }
                }
            }

            var savedTemplate = new SavedTemplateViewModel(template, templateOrigin, isReadOnly);
            var focus = false;
            if (!IsTemplateAdded(template) || template.MultipleInstance)
            {
                if (!string.IsNullOrEmpty(layoutName))
                {
                    savedTemplate.SetName(layoutName, true);
                }
                else
                {
                    savedTemplate.SetName(ValidationService.InferTemplateName(template.Name, template.ItemNameEditable, template.ItemNameEditable), true);
                    if (savedTemplate.ItemNameEditable)
                    {
                        focus = true;
                    }
                }

                AddToCollection(GetCollection(template.TemplateType), savedTemplate);
                RaiseCollectionChanged(template.TemplateType);
                UpdateHasItemsAddedByUser();
                BuildLicenses();
                if (focus)
                {
                    savedTemplate.IsTextSelected = true;
                }
            }
        }

        private ObservableCollection<SavedTemplateViewModel> GetCollection(TemplateType templateType) => templateType == TemplateType.Page ? Pages : Features;

        public bool IsTemplateAdded(TemplateInfoViewModel template) => GetCollection(template.TemplateType).Any(t => t.Equals(template));

        private void AddToCollection(ObservableCollection<SavedTemplateViewModel> collection, SavedTemplateViewModel savedTemplate)
        {
            Func<SavedTemplateViewModel, bool> genGroupEqual = (SavedTemplateViewModel st) => st.GenGroup == savedTemplate.GenGroup;
            Func<SavedTemplateViewModel, bool> genGroupPrevious = (SavedTemplateViewModel st) => st.GenGroup < savedTemplate.GenGroup;

            int index = 0;
            if (collection.Any(genGroupEqual))
            {
                index = collection.IndexOf(collection.Last(genGroupEqual)) + 1;
            }
            else if (collection.Any())
            {
                index = collection.IndexOf(collection.Last(genGroupPrevious)) + 1;
            }

            collection.Insert(index, savedTemplate);
        }

        public void ResetUserSelection()
        {
            HasItemsAddedByUser = false;
            _isInitialized = false;
            Pages.Clear();
            Features.Clear();
        }

        private void BuildLicenses()
        {
            var userSelection = GetUserSelection();
            var licenses = GenComposer.GetAllLicences(userSelection);
            LicensesService.SyncLicenses(licenses, Licenses);

            // Notiffy Licenses name to update the visibillity on the layout
            OnPropertyChanged(nameof(Licenses));
        }

        private void RaiseCollectionChanged(TemplateType templateType)
        {
            // Notify collection name to update the visibillity on the layout
            var collectionName = templateType == TemplateType.Page ? nameof(Pages) : nameof(Features);
            OnPropertyChanged(collectionName);
        }

        public UserSelection GetUserSelection()
        {
            var selection = new UserSelection(_projectTypeName, _frameworkName, _emptyBackendFramework, _platform, _language);

            if (Pages.Any())
            {
                selection.HomeName = Pages.First().Name;
            }

            selection.Pages.AddRange(Pages.Select(p => p.ToUserSelectionItem()));
            selection.Features.AddRange(Features.Select(f => f.ToUserSelectionItem()));

            return selection;
        }

        private async Task<IEnumerable<SavedTemplateViewModel>> RemoveAsync(SavedTemplateViewModel savedTemplate)
        {
            // Look if is there any templates that depends on item
            var dependencies = GetSavedTemplateDependencies(savedTemplate);
            if (!dependencies.Any())
            {
                if (Pages.Contains(savedTemplate))
                {
                    Pages.Remove(savedTemplate);
                }
                else if (Features.Contains(savedTemplate))
                {
                    Features.Remove(savedTemplate);
                }

                if (savedTemplate.HasErrors)
                {
                    await NotificationsControl.CleanErrorNotificationsAsync(ErrorCategory.NamingValidation);
                    WizardStatus.Current.HasValidationErrors = false;
                }

                RaiseCollectionChanged(savedTemplate.TemplateType);
                var template = MainViewModel.Instance.GetTemplate(savedTemplate.Template);
                template?.DecreaseSelection();

                await TryRemoveHiddenDependenciesAsync(savedTemplate);
                UpdateHasItemsAddedByUser();

                BuildLicenses();
                AppHealth.Current.Telemetry.TrackEditSummaryItemAsync(EditItemActionEnum.Remove).FireAndForget();
            }

            return dependencies;
        }

        private async Task TryRemoveHiddenDependenciesAsync(SavedTemplateViewModel savedTemplate)
        {
            foreach (var identity in savedTemplate.Dependencies)
            {
                var dependency = Features.FirstOrDefault(f => f.Identity == identity.Identity);
                if (dependency == null)
                {
                    dependency = Pages.FirstOrDefault(p => p.Identity == identity.Identity);
                }

                if (dependency != null)
                {
                    // If the template is not hidden we can not remove it because it could be added in wizard
                    if (dependency.IsHidden)
                    {
                        // Look if there are another saved template that depends on it.
                        // For example, if it's added two different chart pages, when remove the first one SampleDataService can not be removed, but if no saved templates use SampleDataService, it can be removed.
                        if (!Features.Any(sf => sf.Dependencies.Any(d => d.Identity == dependency.Identity)) || Pages.Any(p => p.Dependencies.Any(d => d.Identity == dependency.Identity)))
                        {
                            await RemoveAsync(dependency);
                        }
                    }
                }
            }
        }

        private IEnumerable<SavedTemplateViewModel> GetSavedTemplateDependencies(SavedTemplateViewModel savedTemplate)
        {
            List<SavedTemplateViewModel> dependencies = new List<SavedTemplateViewModel>();

            foreach (var page in Pages.Where(p => p.Dependencies.Any(d => d.Identity == savedTemplate.Identity)))
            {
                dependencies.Add(page);
            }

            foreach (var feature in Features.Where(f => f.Dependencies.Any(d => d.Identity == savedTemplate.Identity)))
            {
                dependencies.Add(feature);
            }

            return dependencies;
        }

        private void UpdateHasItemsAddedByUser()
        {
            foreach (var page in Pages)
            {
                if (page.TemplateOrigin == TemplateOrigin.UserSelection)
                {
                    HasItemsAddedByUser = true;
                    return;
                }
            }

            foreach (var feature in Features)
            {
                if (feature.TemplateOrigin == TemplateOrigin.UserSelection)
                {
                    HasItemsAddedByUser = true;
                    return;
                }
            }

            HasItemsAddedByUser = false;
        }

        private bool CanDeletePage(SavedTemplateViewModel page)
        {
            if (page.GenGroup == 0)
            {
                // The page can be removed if it isn't a Settings Page and there are more than one pages added.
                return Pages.Count(p => p.GenGroup == 0) > 1;
            }

            return true;
        }

        public async Task OnDeletePageAsync(SavedTemplateViewModel page)
        {
            if (page.IsReadOnly)
            {
                await ShowReadOnlyNotificationAsync(page.Name);
                return;
            }

            int newIndex = Pages.IndexOf(page) - 1;
            newIndex = newIndex >= 0 ? newIndex : 0;

            var dependencies = await RemoveAsync(page);
            if (dependencies != null && dependencies.Any())
            {
                await ShowDependencyNotificationAsync(page.Name, dependencies.Select(t => t.Name));
            }
            else
            {
                SelectedPage = Pages.ElementAt(newIndex);
                SelectedPage.IsFocused = true;
            }
        }

        public async Task OnDeleteFeatureAsync(SavedTemplateViewModel feature)
        {
            if (feature.IsReadOnly)
            {
                await ShowReadOnlyNotificationAsync(feature.Name);
                return;
            }

            int newIndex = Features.IndexOf(feature) - 1;
            newIndex = newIndex >= 0 ? newIndex : 0;

            var dependencies = await RemoveAsync(feature);
            if (dependencies != null && dependencies.Any())
            {
                await ShowDependencyNotificationAsync(feature.Name, dependencies.Select(t => t.Name));
            }
            else
            {
                SelectedFeature = Features.ElementAt(newIndex);
                SelectedFeature.IsFocused = true;
            }
        }

        private async Task ShowReadOnlyNotificationAsync(string name)
        {
            var message = string.Format(StringRes.NotificationRemoveError_ReadOnly, name);
            var notification = Notification.Warning(message, Category.RemoveTemplateValidation);
            await NotificationsControl.AddNotificationAsync(notification);
        }

        private async Task ShowDependencyNotificationAsync(string name, IEnumerable<string> dependencyNames)
        {
            var dependencyNamesFormated = string.Empty;
            foreach (var dependencyName in dependencyNames.Take(3))
            {
                dependencyNamesFormated += $" **{dependencyName}**,";
            }

            dependencyNamesFormated = dependencyNamesFormated.Remove(dependencyNamesFormated.Length - 1);
            if (dependencyNames.Count() > 3)
            {
                dependencyNamesFormated += "...";
            }

            var message = string.Format(StringRes.NotificationRemoveError_Dependency, name, dependencyNamesFormated);
            var notification = Notification.Warning(message, Category.RemoveTemplateValidation);
            await NotificationsControl.AddNotificationAsync(notification);
        }
    }
}
