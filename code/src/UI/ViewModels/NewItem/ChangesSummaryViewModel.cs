using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Resources;

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
            FileGroups.Add(new ItemsGroupViewModel<BaseNewItemFileViewModel>(StringRes.ChangesSummaryWarningsTitle, warnings, OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseNewItemFileViewModel>(StringRes.ChangesSummaryConflictingFilesTitle, output.ConflictingFiles.Select(cf => new ConfictingNewItemFileViewModel(cf.Name)), OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseNewItemFileViewModel>(StringRes.ChangesSummaryModifiedFilesTitle, output.ModifiedFiles.Select(mf => new ModifiedNewItemFileViewModel(mf.Name)), OnItemChanged));
            FileGroups.Add(new ItemsGroupViewModel<BaseNewItemFileViewModel>(StringRes.ChangesSummaryNewFilesTitle, output.NewFiles.Select(nf => new AddedNewItemFileViewModel(nf.Name)), OnItemChanged));

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
