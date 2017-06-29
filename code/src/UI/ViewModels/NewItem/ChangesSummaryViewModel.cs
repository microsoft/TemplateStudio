// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels.Common;

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

        public ICommand MoreDetailsCommand { get; }

        public ChangesSummaryViewModel()
        {
            MoreDetailsCommand = new RelayCommand(OnMoreDetails);
        }

        public void Initialize()
        {
            var output = NewItemGenController.Instance.CompareOutputAndProject();
            var warnings = GenContext.Current.FailedMergePostActions.Select(w => new FailedMergesFileViewModel(w));
            var licenses = MainViewModel.Current.GetActiveTemplate().LicenseTerms;

            FileGroups.Clear();
            FileGroups.Add(new ItemsGroupViewModel<BaseFileViewModel>(StringRes.ChangesSummaryCategoryConflictingFiles, output.ConflictingFiles.Select(cf => new ConflictingFileViewModel(cf)), OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseFileViewModel>(StringRes.ChangesSummaryCategoryFailedMerges, warnings, OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseFileViewModel>(StringRes.ChangesSummaryCategotyModifiedFiles, output.ModifiedFiles.Select(mf => new ModifiedFileViewModel(mf)), OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseFileViewModel>(StringRes.ChangesSummaryCategoryNewFiles, output.NewFiles.Select(nf => new NewFileViewModel(nf)), OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseFileViewModel>(StringRes.ChangesSummaryCategoryUnchangedFiles, output.UnchangedFiles.Select(nf => new UnchangedFileViewModel(nf)), OnItemChanged));

            var group = FileGroups.FirstOrDefault(gr => gr.Templates.Any());
            if (group != null)
            {
                group.SelectedItem = group.Templates.First();
            }
            HasLicenses = licenses != null && licenses.Any();
            if (HasLicenses)
            {
                Licenses.AddRange(licenses.Select(l => new SummaryLicenseViewModel(l)));
            }
        }

        private void OnMoreDetails()
        {
            Process.Start("https://github.com/Microsoft/WindowsTemplateStudio/blob/issue267-rightclick/docs/newitem.md");
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
