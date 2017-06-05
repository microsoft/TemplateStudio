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
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.UI.Views.NewItem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class SyncNewItemViewModel : Observable
    {
        private readonly SyncNewItemView _newItemView;

        public SyncNewItemViewModel(SyncNewItemView newItemView)
        {
            _newItemView = newItemView;
        }

        public ObservableCollection<string> GenerationWarnings { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> NewFiles { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> ConflictingFiles { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> ModifiedFiles { get; } = new ObservableCollection<string>();

        public ICommand OkCommand => new RelayCommand(SaveAndClose);

       
        public ICommand CancelCommand => new RelayCommand(_newItemView.Close);

        public async Task InitializeAsync()
        {
            var result = GenController.CompareOutputAndProject();

            NewFiles.AddRange(result.NewFiles);
            ModifiedFiles.AddRange(result.ModifiedFiles);
            ConflictingFiles.AddRange(result.ConflictingFiles);
            GenerationWarnings.AddRange(GenContext.Current.GenerationWarnings.Select(w => w.Description + w.ExtendedInfo));
            await Task.CompletedTask;
        }

        

       

        private void SaveAndClose()
        {
            _newItemView.DialogResult = true;
            _newItemView.Result = true;

            _newItemView.Close();
        }

    }
}
