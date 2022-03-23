// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Mvvm;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class CompositionToolViewModel : Observable
    {
        private NewItemFileViewModel _selectedFile;
        private bool _isSelectedFileVisible;

        public NewItemFileViewModel SelectedFile
        {
            get => _selectedFile;
            set => SetProperty(ref _selectedFile, value);
        }

        public bool IsSelectedFileVisible
        {
            get => _isSelectedFileVisible;
            set => SetProperty(ref _isSelectedFileVisible, value);
        }

        public CompositionToolViewModel()
        {
        }

        public ObservableCollection<GenGroup> GenInfoGroups { get; } = new ObservableCollection<GenGroup>();

        public void Initialize(UserSelection userSelection)
        {
            var groups = GenComposer.Compose(userSelection)
                .GroupBy(genInfo => genInfo.Name)
                .Select(genGroup => new GenGroup(genGroup.Key, genGroup));
            GenInfoGroups.Clear();
            GenInfoGroups.AddRange(groups);
        }

        public void SelectItem(object selectedItem)
        {
            switch (selectedItem)
            {
                case CompositionFile file:
                    SelectedFile = NewItemFileViewModel.CompositionToolFile(file.Path);
                    IsSelectedFileVisible = true;
                    return;
                case GenInfoComposition composition:
                    SelectedFile = NewItemFileViewModel.CompositionToolFile(composition.TemplatePath);
                    IsSelectedFileVisible = true;
                    break;
                default:
                    IsSelectedFileVisible = false;
                    break;
            }
        }
    }
}
