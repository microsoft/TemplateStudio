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

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels.Common;

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class ChangesSummaryViewModel : Observable
    {
        private double _currentPoints = CodeLineViewModel.DefaultFontSize;
        public ObservableCollection<ItemsGroupViewModel<BaseNewItemFileViewModel>> FileGroups { get; } = new ObservableCollection<ItemsGroupViewModel<BaseNewItemFileViewModel>>();

        private BaseNewItemFileViewModel _selectedFile;
        public BaseNewItemFileViewModel SelectedFile
        {
            get => _selectedFile;
            set => SetProperty(ref _selectedFile, value);
        }

        public ICommand UpdateFontSizeCommand { get; }

        public ChangesSummaryViewModel()
        {
            UpdateFontSizeCommand = new RelayCommand<string>(UpdateFontSize);
        }

        public void Initialize()
        {
            var output = NewItemGenController.Instance.CompareOutputAndProject();
            var warnings = GenContext.Current.GenerationWarnings.Select(w => new WarningNewItemFileViewModel(w));
            FileGroups.Clear();
            FileGroups.Add(new ItemsGroupViewModel<BaseNewItemFileViewModel>(StringRes.ChangesSummaryCategoryMergeConflicts, warnings, OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseNewItemFileViewModel>(StringRes.ChangesSummaryCategoryOverwrittenFiles, output.ConflictingFiles.Select(cf => new ConfictingNewItemFileViewModel(cf)), OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseNewItemFileViewModel>(StringRes.ChangesSummaryCategotyModifiedFiles, output.ModifiedFiles.Select(mf => new ModifiedNewItemFileViewModel(mf)), OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseNewItemFileViewModel>(StringRes.ChangesSummaryCategoryNewFiles, output.NewFiles.Select(nf => new AddedNewItemFileViewModel(nf)), OnItemChanged));

            var group = FileGroups.FirstOrDefault(gr => gr.Templates.Any());
            if (group != null)
            {
                group.SelectedItem = group.Templates.First();
            }
        }

        private void OnItemChanged(ItemsGroupViewModel<BaseNewItemFileViewModel> group)
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
        
        private void UpdateFontSize(string mode)
        {
            double points = (mode == "Plus") ? 1 : -1;
            double newPoints = _currentPoints + points;
            if (newPoints > 4 && newPoints < 25)
            {
                foreach (var group in FileGroups)
                {
                    foreach (var file in group.Templates)
                    {
                        file.UpdateFontSize(newPoints);
                    }
                }
                _currentPoints = newPoints;
            }
        }
    }
}
