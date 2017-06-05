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
using Microsoft.Templates.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Templates.UI.ViewModels
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
            CheckForChangedFiles();
            GenerationWarnings.AddRange(GenContext.Current.GenerationWarnings.Select(w => w.Description + w.ExtendedInfo));
            await Task.CompletedTask;
        }

        public void CheckForChangedFiles()
        {
            var files = Directory
                .EnumerateFiles(GenContext.Current.OutputPath, "*", SearchOption.AllDirectories)
                .Where(f => !Regex.IsMatch(f, MergePostAction.PostactionRegex) && !Regex.IsMatch(f, MergePostAction.FailedPostactionRegex))
                .ToList();

            foreach (var file in files)
            {
                var destFilePath = file.Replace(GenContext.Current.OutputPath, GenContext.Current.ProjectPath);
                if (!File.Exists(destFilePath))
                {
                    NewFiles.Add(file.Replace(GenContext.Current.OutputPath, String.Empty));
                }
                else
                {
                    if (GenContext.Current.MergeFilesFromProject.Contains(destFilePath))
                    {
                        if (!FilesAreEqual(file, destFilePath))
                        {
                            ModifiedFiles.Add(file.Replace(GenContext.Current.ProjectPath, String.Empty));
                        }
                    }
                    else
                    {
                        if (!FilesAreEqual(file, destFilePath))
                        {
                            ConflictingFiles.Add(destFilePath.Replace(GenContext.Current.ProjectPath, String.Empty));
                        }
                    }
                }
            }
        }

        private static bool FilesAreEqual(string file, string destFilePath)
        {
            return File.ReadAllBytes(file).SequenceEqual(File.ReadAllBytes(destFilePath));
        }

        private void SaveAndClose()
        {
            _newItemView.DialogResult = true;
            _newItemView.Result = true;

            _newItemView.Close();
        }

    }
}
