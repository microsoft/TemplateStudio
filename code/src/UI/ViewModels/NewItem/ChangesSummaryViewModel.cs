// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class ChangesSummaryViewModel : Observable
    {
        public ObservableCollection<ItemsGroupViewModel<BaseFileViewModel>> FileGroups { get; } = new ObservableCollection<ItemsGroupViewModel<BaseFileViewModel>>();
        public ObservableCollection<SummaryLicenseViewModel> Licenses { get; } = new ObservableCollection<SummaryLicenseViewModel>();

        private BaseFileViewModel _selectedFile;
        public BaseFileViewModel SelectedFile
        {
            get => _selectedFile;
            set => SetProperty(ref _selectedFile, value);
        }

        private bool _hasLicenses;
        public bool HasLicenses
        {
            get => _hasLicenses;
            set => SetProperty(ref _hasLicenses, value);
        }

        private bool _doNotMerge;
        public bool DoNotMerge
        {
            get => _doNotMerge;
            set => SetProperty(ref _doNotMerge, value);
        }

        private bool _hasChangesToApply;
        public bool HasChangesToApply
        {
            get => _hasChangesToApply;
            set => SetProperty(ref _hasChangesToApply, value);
        }

        public ICommand MoreDetailsCommand { get; }
        public ICommand UpdateFontSizeCommand { get; }

        public ChangesSummaryViewModel()
        {
            MoreDetailsCommand = new RelayCommand(OnMoreDetails);
            UpdateFontSizeCommand = new RelayCommand<string>(OnUpdateFontSize);
        }

        public void Initialize(NewItemGenerationResult output)
        {
            var warnings = GenContext.Current.FailedMergePostActions
                .Where(w => w.MergeFailureType == MergeFailureType.FileNotFound || w.MergeFailureType == MergeFailureType.LineNotFound)
                .Select(w => new FailedMergesFileViewModel(w));
            var failedStyleMerges = GenContext.Current.FailedMergePostActions
                .Where(w => w.MergeFailureType == MergeFailureType.KeyAlreadyDefined)
                .Select(w => new FailedStyleMergesFileViewModel(w));
            HasChangesToApply = output.HasChangesToApply;

            FileGroups.Clear();
            FileGroups.Add(new ItemsGroupViewModel<BaseFileViewModel>(StringRes.ChangesSummaryCategoryConflictingFiles, output.ConflictingFiles.Select(cf => new ConflictingFileViewModel(cf)), OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseFileViewModel>(StringRes.ChangesSummaryCategoryFailedMerges, warnings, OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseFileViewModel>(StringRes.ChangesSummaryCategoryFailedStyleMerges, failedStyleMerges, OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseFileViewModel>(StringRes.ChangesSummaryCategotyModifiedFiles, output.ModifiedFiles.Select(mf => new ModifiedFileViewModel(mf)), OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseFileViewModel>(StringRes.ChangesSummaryCategoryNewFiles, output.NewFiles.Select(nf => new NewFileViewModel(nf)), OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseFileViewModel>(StringRes.ChangesSummaryCategoryUnchangedFiles, output.UnchangedFiles.Select(nf => new UnchangedFileViewModel(nf)), OnItemChanged));

            var licenses = new List<TemplateLicense>();
            MainViewModel.Current.MainView.Result.Pages.ForEach(f => licenses.AddRange(f.template.GetLicenses()));
            MainViewModel.Current.MainView.Result.Features.ForEach(f => licenses.AddRange(f.template.GetLicenses()));
            HasLicenses = licenses != null && licenses.Any();
            if (HasLicenses)
            {
                Licenses.AddRange(licenses.Select(l => new SummaryLicenseViewModel(l)));
            }

            var group = FileGroups.FirstOrDefault(gr => gr.Templates.Any());
            if (group != null)
            {
                group.SelectedItem = group.Templates.First();
            }

            MainViewModel.Current.UpdateCanFinish(true);
            MainViewModel.Current.WizardStatus.IsLoading = false;
        }

        public void ResetSelection()
        {
            FileGroups.Clear();
            Licenses.Clear();
            HasLicenses = false;
            SelectedFile = null;
        }

        private void OnMoreDetails() => Process.Start($"{Configuration.Current.GitHubDocsUrl}newitem.md");
        private void OnUpdateFontSize(string points)
        {
            foreach (var group in FileGroups)
            {
                foreach (var template in group.Templates)
                {
                    template.CodeFontSize += double.Parse(points);
                }
            }
        }

        private void OnItemChanged(ItemsGroupViewModel<BaseFileViewModel> group)
        {
            foreach (var item in FileGroups)
            {
                if (item.Name != group.Name)
                {
                    item.CleanSelected();
                }
            }

            SelectedFile = group.SelectedItem;
        }
    }
}
