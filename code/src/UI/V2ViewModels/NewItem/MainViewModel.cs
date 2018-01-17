// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.V2ViewModels.Common;
using Microsoft.Templates.UI.V2Views.NewItem;
using Microsoft.Templates.UI.V2Extensions;
using Microsoft.Templates.UI.V2Resources;
using Microsoft.Templates.UI.V2Controls;
using System.Collections.Generic;

namespace Microsoft.Templates.UI.V2ViewModels.NewItem
{
    public class MainViewModel : BaseMainViewModel
    {
        private static MainViewModel _instance;
        private TemplateType _templateType;

        public static MainViewModel Instance => _instance ?? (_instance = new MainViewModel(WizardShell.Current));

        public MainViewModel(WizardShell mainWindow)
            : base(mainWindow)
        {
        }

        public async Task InitializeAsync(TemplateType templateType, string language)
        {
            _templateType = templateType;

            var stringResource = templateType == TemplateType.Page ? StringRes.NewItemTitlePage : StringRes.NewItemTitleFeature;
            WizardStatus.Title = stringResource;
            await InitializeAsync(language);
        }

        protected override void UpdateStep()
        {
            base.UpdateStep();
        }

        protected override void OnCancel()
        {
        }

        protected override Task OnTemplatesAvailableAsync()
        {
            return Task.CompletedTask;
        }

        protected override IEnumerable<Step> GetSteps()
        {
            yield return new Step(0, StringRes.NewItemStepOne, true);
            yield return new Step(1, StringRes.NewItemStepTwo);
        }
    }
}
