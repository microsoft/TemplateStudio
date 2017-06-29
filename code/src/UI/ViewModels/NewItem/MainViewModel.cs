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

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Views.NewItem;
using Microsoft.Templates.UI.Generation;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class MainViewModel : BaseMainViewModel
    {
        public static MainViewModel Current;
        public MainView MainView;

        // Configuration
        public TemplateType ConfigTemplateType;
        public string ConfigFramework;
        public string ConfigProjectType;
        public NewItemSetupViewModel NewItemSetup { get; private set; } = new NewItemSetupViewModel();
        public ChangesSummaryViewModel ChangesSummary { get; private set; } = new ChangesSummaryViewModel();

        public MainViewModel(MainView mainView) : base(mainView)
        {
            MainView = mainView;
            Current = this;
        }

        public async Task InitializeAsync(TemplateType templateType)
        {
            var configInfo = ProjectConfigInfo.ReadProjectConfiguration();

            ConfigTemplateType = templateType;
            ConfigProjectType = configInfo.ProjectType;
            ConfigFramework = configInfo.Framework;
            SetNewItemSetupTitle();
            await BaseInitializeAsync();
        }

        public void SetNewItemSetupTitle() => Title = string.Format(StringRes.NewItemTitle_SF, ConfigTemplateType.ToString().ToLower());

        public void SetChangesSummaryTitle()
        {
            var template = GetActiveTemplate();
            if (template.IsItemNameEditable)
            {
                Title = string.Format(StringRes.ChangesSummaryTitle_SF, NewItemSetup.ItemName, template.TemplateType.ToString().ToLower());
            }
            else
            {
                Title = string.Format(StringRes.ChangesSummaryTitle_SF, template.Name, template.TemplateType.ToString().ToLower());
            }
        }

        protected override void OnCancel()
        {
            MainView.DialogResult = false;
            MainView.Result = null;
            MainView.Close();
        }

        protected override void OnClose()
        {
            MainView.DialogResult = true;
            MainView.Result = null;
            MainView.Close();
        }

        protected override void OnGoBack()
        {
            base.OnGoBack();
            NewItemSetup.Initialize(false);
            HasOverlayBox = true;
            ChangesSummary.HasLicenses = false;
            ChangesSummary.Licenses.Clear();
            SetNewItemSetupTitle();
            CleanStatus();
        }
        protected override async void OnNext()
        {
            base.OnNext();
            NewItemSetup.EditionVisibility = System.Windows.Visibility.Collapsed;
            MainView.Result = CreateUserSelection();
            NewItemGenController.Instance.CleanupTempGeneration();
            await NewItemGenController.Instance.GenerateNewItemAsync(ConfigTemplateType, MainView.Result);
            NavigationService.Navigate(new ChangesSummaryView());
            SetChangesSummaryTitle();
            HasOverlayBox = false;
        }
        protected override void OnFinish(string parameter)
        {
            MainView.Result.ItemGenerationType = ChangesSummary.DoNotMerge ? ItemGenerationType.Generate : ItemGenerationType.GenerateAndMerge;
            base.OnFinish(parameter);
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
        protected override void OnNewTemplatesAvailable()
        {
            _canGoBack = false;
            _templatesReady = false;
            FinishCommand.OnCanExecuteChanged();
            BackCommand.OnCanExecuteChanged();
            ShowFinishButton = false;
            EnableGoForward();
            NavigationService.Navigate(new NewItemSetupView());
            NewItemSetup.Initialize(true);
        }
        protected override UserSelection CreateUserSelection()
        {
            var userSelection = new UserSelection()
            {
                Framework = ConfigFramework,
                ProjectType = ConfigProjectType,
                HomeName = string.Empty
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
