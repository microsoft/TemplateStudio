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

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Views.NewItem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class NewItemViewModel : Observable
    {
        private readonly NewItemView _newItemView;


        private string _contextFramework;

        public string ContextFramework
        {
            get { return _contextFramework; }
            set
            {
                SetProperty(ref _contextFramework, value);
                LoadTemplates();
            }
        }

        private string _contextProjectType;

        public string ContextProjectType
        {
            get { return _contextProjectType; }
            set
            {
                SetProperty(ref _contextProjectType, value);
                LoadFrameworks();
            }
        }

        public List<(string Name, ITemplateInfo Template)> SavedTemplates { get; } = new List<(string Name, ITemplateInfo Template)>();

        public NewItemViewModel(NewItemView newItemView)
        {
            _newItemView = newItemView;
        }

        public ICommand OkCommand => new RelayCommand(SaveAndClose);
        public ICommand CancelCommand => new RelayCommand(_newItemView.Close);

        public ObservableCollection<string> ProjectTypes { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Frameworks { get; } = new ObservableCollection<string>();

        public ObservableCollection<ITemplateInfo> Templates { get; } = new ObservableCollection<ITemplateInfo>();

        private bool _enableContextSelection;
        public bool EnableContextSelection
        {
            get { return _enableContextSelection; }
            set
            {
                SetProperty(ref _enableContextSelection, value);
            }
        }

        private ITemplateInfo _templateSelected;
        public ITemplateInfo TemplateSelected
        {
            get { return _templateSelected; }
            set
            {
                SetProperty(ref _templateSelected, value);
                if (value != null)
                {
                    var validators = new List<Validator>()
                    {
                        new ReservedNamesValidator()   
                    };
                    if (_templateSelected.GetItemNameEditable())
                    {
                        validators.Add(new DefaultNamesValidator());
                    }
                    ItemName = Naming.Infer(_templateSelected.GetDefaultName(), validators);
                }
            }
        }


        private string _itemName;
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                SetProperty(ref _itemName, value);
            }
        }


        public async Task InitializeAsync()
        {
            await GenContext.ToolBox.Repo.SynchronizeAsync();

            var projectConfig = ReadProjectConfiguration();

            ProjectTypes.AddRange(GenContext.ToolBox.Repo.GetProjectTypes().Select(f => f.Name));
            if (!string.IsNullOrEmpty(projectConfig.ProjectType))
            {
                EnableContextSelection = false;
                ContextProjectType = projectConfig.ProjectType;
            }
            else
            {
                ContextProjectType = ProjectTypes.FirstOrDefault();
                EnableContextSelection = true;
            }
            if (!string.IsNullOrEmpty(projectConfig.Framework))
            {
                ContextFramework = projectConfig.Framework;
            }


            await Task.CompletedTask;
        }

        private static (string ProjectType, string Framework) ReadProjectConfiguration()
        {
            var path = Path.Combine(GenContext.Current.ProjectPath, "Package.appxmanifest");
            var manifest = XElement.Load(path);

            var metadata = manifest.Descendants().FirstOrDefault(e => e.Name.LocalName == "Metadata");
            var projectType = metadata?.Descendants().FirstOrDefault(m => m.Attribute("Name").Value == "projectType")?.Attribute("Value")?.Value;
            var framework = metadata?.Descendants().FirstOrDefault(m => m.Attribute("Name").Value == "framework")?.Attribute("Value")?.Value;

            return (projectType, framework);
        }

        private void LoadTemplates()
        {
            var pageTemplates = GenContext.ToolBox.Repo.Get(t =>
                                                    (t.GetTemplateType() == TemplateType.Page || t.GetTemplateType() == TemplateType.Feature)
                                                  && t.GetFrameworkList().Contains(_contextFramework)
                                                  && t.GetRightClickEnabled());
            Templates.Clear();
            Templates.AddRange(pageTemplates);

            if (Templates.Any())
            {
                TemplateSelected = Templates.FirstOrDefault();
            }
        }

        private void LoadFrameworks()
        {
            var projectFrameworks = GenComposer.GetSupportedFx(_contextProjectType);
            var targetFrameworks = GenContext.ToolBox.Repo.GetFrameworks()
                                                                .Where(m => projectFrameworks.Contains(m.Name))
                                                                .Select(f => f.Name)
                                                                .ToList();

            Frameworks.Clear();
            Frameworks.AddRange(targetFrameworks);
            if (ContextFramework == null)
            {
                ContextFramework = Frameworks.FirstOrDefault();
            }
        }

        private void SaveAndClose()
        {
            _newItemView.DialogResult = true;
            _newItemView.Result = CreateUserSelection();

            _newItemView.Close();
        }

        private UserSelection CreateUserSelection()
        {
            var userSelection = new UserSelection()
            {
                ProjectType = _contextProjectType,
                Framework = _contextFramework,
                HomeName = ""
            };
            AddTemplate(userSelection, TemplateSelected);
            var dependencies = GenComposer.GetAllDependencies(TemplateSelected, _contextFramework);
            foreach (var dependency in dependencies)
            {
                AddTemplate(userSelection, dependency);
            }

            return userSelection;
        }

        private void AddTemplate(UserSelection userSelection, ITemplateInfo template)
        {
            if (template.GetTemplateType() == TemplateType.Page)
            {
                userSelection.Pages.Add((ItemName, template));
            }
            else
            {
                userSelection.Features.Add((ItemName, template));
            }
        }
    }
}
