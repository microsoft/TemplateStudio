// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.SharedResources;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class ChangesSummaryViewModel : Observable
    {
        private bool _doNotMerge;
        private bool _isDoNotMergeEnabled;
        private NewItemFileViewModel _selected;

        public bool DoNotMerge
        {
            get => _doNotMerge;
            set => SetProperty(ref _doNotMerge, value);
        }

        public bool IsDoNotMergeEnabled
        {
            get => _isDoNotMergeEnabled;
            set => SetProperty(ref _isDoNotMergeEnabled, value);
        }

        public NewItemFileViewModel Selected
        {
            get => _selected;
            private set => SetProperty(ref _selected, value);
        }

        public ObservableCollection<ItemsGroupViewModel<NewItemFileViewModel>> FileGroups { get; } = new ObservableCollection<ItemsGroupViewModel<NewItemFileViewModel>>();

        public ChangesSummaryViewModel()
        {
        }

        public void Initialize(NewItemGenerationResult output)
        {
            var warnings = GenContext.Current.FailedMergePostActions.Where(w => w.MergeFailureType == MergeFailureType.FileNotFound || w.MergeFailureType == MergeFailureType.LineNotFound);
            var failedStyleMerges = GenContext.Current.FailedMergePostActions.Where(w => w.MergeFailureType == MergeFailureType.KeyAlreadyDefined);

            FileGroups.Clear();
            AddGroup(new ItemsGroupViewModel<NewItemFileViewModel>(Resources.ChangesSummaryGroupWarningFiles, warnings.Select(f => NewItemFileViewModel.WarningFile(f))));
            AddGroup(new ItemsGroupViewModel<NewItemFileViewModel>(Resources.ChangesSummaryGroupConflictingStylesFiles, failedStyleMerges.Select(f => NewItemFileViewModel.ConflictingStylesFile(f))));
            AddGroup(new ItemsGroupViewModel<NewItemFileViewModel>(Resources.ChangesSummaryGroupConflictingFiles, output.ConflictingFiles.Select(f => NewItemFileViewModel.ConflictingFile(f))));
            AddGroup(new ItemsGroupViewModel<NewItemFileViewModel>(Resources.ChangesSummaryGroupModifiedFiles, output.ModifiedFiles.Select(f => NewItemFileViewModel.ModifiedFile(f))));
            AddGroup(new ItemsGroupViewModel<NewItemFileViewModel>(Resources.ChangesSummaryGroupNewFiles, output.NewFiles.Select(f => NewItemFileViewModel.NewFile(f))));
            AddGroup(new ItemsGroupViewModel<NewItemFileViewModel>(Resources.ChangesSummaryGroupUnchangedFiles, output.UnchangedFiles.Select(f => NewItemFileViewModel.UnchangedFile(f))));

            SelectFile(FileGroups.First().Items.First());
        }

        public void SelectFile(NewItemFileViewModel file)
        {
            foreach (var group in FileGroups)
            {
                group.CleanSelected();
            }

            Selected = file;
            Selected.IsSelected = true;
        }

        public void ClearSelected() => Selected = null;

        private void AddGroup(ItemsGroupViewModel<NewItemFileViewModel> group)
        {
            if (group.Items.Any())
            {
                FileGroups.Add(group);
            }
        }
    }
}
