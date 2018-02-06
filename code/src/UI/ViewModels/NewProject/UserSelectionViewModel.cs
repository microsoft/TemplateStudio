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
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewProject
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
        private ICommand _movePageUpCommand;
        private ICommand _movePageDownCommand;

        public ObservableCollection<SavedTemplateViewModel> Pages { get; } = new ObservableCollection<SavedTemplateViewModel>();

        public ObservableCollection<SavedTemplateViewModel> Features { get; } = new ObservableCollection<SavedTemplateViewModel>();

        public ObservableCollection<LicenseViewModel> Licenses { get; } = new ObservableCollection<LicenseViewModel>();

        public ICommand MovePageUpCommand => _movePageUpCommand ?? (_movePageUpCommand = new RelayCommand(() => OrderingService.MoveUp(SelectedPage)));

        public ICommand MovePageDownCommand => _movePageDownCommand ?? (_movePageDownCommand = new RelayCommand(() => OrderingService.MoveDown(SelectedPage)));

        public bool HasItemsAddedByUser { get; private set; }

        public SavedTemplateViewModel SelectedPage { get; set; }

        public UserSelectionViewModel()
        {
            EventService.Instance.OnDeleteTemplateClicked += OnRemove;
            EventService.Instance.OnReorderTemplate += OnReorderTemplate;
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

            UpdateHomePage();
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
                    dependencyTemplate = new TemplateInfoViewModel(dependency, _frameworkName);
                }

                Add(TemplateOrigin.Dependency, dependencyTemplate);
            }

            template.IncreaseSelection();
            var savedTemplate = new SavedTemplateViewModel(template, templateOrigin);
            var focus = false;
            if (!IsTemplateAdded(template) || template.MultipleInstance)
            {
                if (!string.IsNullOrEmpty(layoutName))
                {
                    savedTemplate.Name = layoutName;
                }
                else
                {
                    savedTemplate.Name = ValidationService.InferTemplateName(template.Name, template.ItemNameEditable, template.ItemNameEditable);
                    if (savedTemplate.ItemNameEditable)
                    {
                        focus = true;
                    }
                }

                AddToCollection(GetCollection(template.TemplateType), savedTemplate);
                RaiseCollectionChanged(template.TemplateType);
                UpdateHasItemsAddedByUser();

                var userSelection = GetUserSelection();
                var licenses = GenComposer.GetAllLicences(userSelection);
                LicensesService.SyncLicenses(licenses, Licenses);

                // Notiffy Licenses name to update the visibillity on the layout
                OnPropertyChanged(nameof(Licenses));
                if (focus)
                {
                    savedTemplate.Focus();
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

        private void RaiseCollectionChanged(TemplateType templateType)
        {
            // Notify collection name to update the visibillity on the layout
            var collectionName = templateType == TemplateType.Page ? nameof(Pages) : nameof(Features);
            OnPropertyChanged(collectionName);
        }

        public UserSelection GetUserSelection()
        {
            var selection = new UserSelection(_projectTypeName, _frameworkName, _language);

            if (Pages.Any())
            {
                selection.HomeName = Pages.First().Name;
            }

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

        private async Task<SavedTemplateViewModel> RemoveAsync(SavedTemplateViewModel savedTemplate)
        {
            // Look if is there any templates that depends on item
            var dependency = GetSavedTemplateDependency(savedTemplate);
            if (dependency == null)
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
                    await NotificationsControl.Instance.CleanErrorNotificationsAsync(ErrorCategory.NamingValidation);
                }

                RaiseCollectionChanged(savedTemplate.TemplateType);
                var template = MainViewModel.Instance.GetTemplate(savedTemplate.Template);
                template?.DecreaseSelection();

                await TryRemoveHiddenDependenciesAsync(savedTemplate);
                UpdateHasItemsAddedByUser();
                AppHealth.Current.Telemetry.TrackEditSummaryItemAsync(EditItemActionEnum.Remove).FireAndForget();
            }

            return dependency;
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

        private SavedTemplateViewModel GetSavedTemplateDependency(SavedTemplateViewModel savedTemplate)
        {
            SavedTemplateViewModel dependencyItem = null;
            dependencyItem = Pages.FirstOrDefault(p => p.Dependencies.Any(d => d.Identity == savedTemplate.Identity));
            if (dependencyItem == null)
            {
                dependencyItem = Features.FirstOrDefault(f => f.Dependencies.Any(d => d.Identity == savedTemplate.Identity));
            }

            return dependencyItem;
        }

        private async void OnRemove(object sender, SavedTemplateViewModel savedTemplate)
        {
            var dependency = await RemoveAsync(savedTemplate);
            if (dependency != null)
            {
                var message = string.Format(StringRes.NotificationRemoveError_Dependency, savedTemplate.Name, dependency.Name);
                var notification = Notification.Warning(message, Category.RemoveTemplateValidation);
                await NotificationsControl.Instance.AddNotificationAsync(notification);
            }
        }

        private void OnReorderTemplate(object sender, EventArgs e)
        {
            UpdateHomePage();
        }

        private void UpdateHasItemsAddedByUser()
        {
            foreach (var page in Pages)
            {
                if (page.TemplateOrigin != TemplateOrigin.Layout)
                {
                    HasItemsAddedByUser = true;
                    return;
                }
            }

            foreach (var feature in Features)
            {
                if (feature.TemplateOrigin != TemplateOrigin.Layout)
                {
                    HasItemsAddedByUser = true;
                    return;
                }
            }

            HasItemsAddedByUser = false;
        }

        private void UpdateHomePage()
        {
            foreach (var page in Pages)
            {
                page.SetHome(false);
            }

            var home = Pages.FirstOrDefault();
            home?.SetHome(true);
        }
    }
}
