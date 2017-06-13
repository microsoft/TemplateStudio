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
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Views.NewItem;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class MainViewModel : BaseMainViewModel
    {
        public static MainViewModel Current;
        public MainView MainView;

        //Configuration
        public TemplateType ConfigTemplateType;
        public string ConfigFramework;
        public string ConfigProjectType;
        public string LastSelectedTemplateIdentity;
        public NewItemSetupViewModel NewItemSetup { get; private set; } = new NewItemSetupViewModel();
        public ChangesSummaryViewModel ChangesSummary { get; private set; } = new ChangesSummaryViewModel();

        public MainViewModel(MainView mainView) : base(mainView)
        {
            MainView = mainView;
            Current = this;
        }

        public async Task InitializeAsync(TemplateType templateType)
        {
            ConfigTemplateType = templateType;
            var projectConfiguration = NewItemGenController.Instance.ReadProjectConfiguration();
            ConfigProjectType = projectConfiguration.ProjectType;
            ConfigFramework = projectConfiguration.Framework;
            Title = String.Format(StringRes.NewItemTitle_SF, ConfigTemplateType.ToString().ToLower());
            await BaseInitializeAsync();
        }

        protected override void OnCancel()
        {
            MainView.DialogResult = false;
            MainView.Result = null;
            MainView.Close();
        }
        protected override void OnGoBack()
        {
            base.OnGoBack();
            NewItemSetup.Initialize(false);
        }
        protected override async void OnNext()
        {
            base.OnNext();
            NewItemSetup.EditionVisibility = System.Windows.Visibility.Collapsed;
            MainView.Result = CreateUserSelection();
            await NewItemGenController.Instance.GenerateNewItemAsync(MainView.Result);
            NavigationService.Navigate(new ChangesSummaryView());
            Title = StringRes.ChangesSummaryTitle;
        }
        protected override void OnFinish(string parameter)
        {
            ItemGenerationType itemGenerationType;
            if (Enum.TryParse(parameter, out itemGenerationType))
            {
                MainView.Result.ItemGenerationType = itemGenerationType;
                base.OnFinish(parameter);
            }
        }

        public TemplateInfoViewModel GetActiveTemplate()
        {
            var activeGroup = NewItemSetup.TemplateGroups.FirstOrDefault(gr => gr.SelectedItem != null);
            if (activeGroup != null)
            {
                return activeGroup.SelectedItem as TemplateInfoViewModel;
            }
            return null;
        }

        protected override void OnTemplatesAvailable() => NewItemSetup.Initialize(true);
        protected override UserSelection CreateUserSelection()
        {
            var userSelection = new UserSelection()
            {
                Framework = ConfigFramework,
                ProjectType = ConfigProjectType,
                HomeName = String.Empty
            };
            var template = GetActiveTemplate();
            if (template != null)
            {
                var dependencies = GenComposer.GetAllDependencies(template.Template, ConfigFramework);

                userSelection.Pages.Clear();
                userSelection.Features.Clear();

                AddTemplate(userSelection, NewItemSetup.ItemName, template.Template, ConfigTemplateType);

                foreach (var dependencyTemplate in dependencies)
                {
                    AddTemplate(userSelection, dependencyTemplate.GetDefaultName(), dependencyTemplate, dependencyTemplate.GetTemplateType());
                }

                if (String.IsNullOrEmpty(LastSelectedTemplateIdentity))
                {
                    LastSelectedTemplateIdentity = template.Identity;
                }
                else if (LastSelectedTemplateIdentity != template.Identity)
                {
                    NewItemGenController.Instance.CleanupTempGeneration();
                    LastSelectedTemplateIdentity = template.Identity;
                }
            }
            return userSelection;
        }

        private void AddTemplate(UserSelection userSelection, string name, ITemplateInfo template, TemplateType templateType)
        {
            if (templateType == TemplateType.Page)
            {
                userSelection.Pages.Add((name, template));
            }
            else if (templateType == TemplateType.Feature)
            {
                userSelection.Features.Add((name, template));
            }
        }
    }
}
