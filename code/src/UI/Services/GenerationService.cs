// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.UI.Services
{
    public class GenerationService
    {
        private readonly DialogService _dialogService = DialogService.Instance;

        private static readonly Lazy<GenerationService> _instance = new Lazy<GenerationService>(() => new GenerationService());

        public static GenerationService Instance => _instance.Value;

        private GenerationService()
        {
        }

        public async Task GenerateProjectAsync(UserSelection userSelection)
        {
            try
            {
                await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex, userSelection.ToString());
                GenContext.ToolBox.Shell.CloseSolution();
                GenContext.ToolBox.Shell.CancelWizard(false);
            }
        }

        public async Task GenerateNewItemAsync(TemplateType templateType, UserSelection userSelection)
        {
            try
            {
                await NewItemGenController.Instance.UnsafeGenerateNewItemAsync(templateType, userSelection);
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex, userSelection.ToString());
                GenContext.ToolBox.Shell.CancelWizard(false);
            }
        }

        public void FinishGeneration(UserSelection userSelection)
        {
            try
            {
                NewItemGenController.Instance.UnsafeFinishGeneration(userSelection);
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex, userSelection.ToString());
                GenContext.ToolBox.Shell.CancelWizard(false);
            }
        }
    }
}
