// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2Services
{
    public class EventService
    {
        private static EventService _instance;

        public static EventService Instance => _instance ?? (_instance = new EventService());

        public event EventHandler<SavedTemplateViewModel> OnDeleteTemplateClicked;

        public event EventHandler<SavedTemplateViewModel> OnSavedTemplateFocused;

        public event EventHandler<MetadataInfoViewModel> OnProjectTypeChange;

        public event EventHandler<MetadataInfoViewModel> OnFrameworkChange;

        public event EventHandler OnReorderTemplate;

        private EventService()
        {
        }

        public void RaiseOnDeleteTemplateClicked(SavedTemplateViewModel savedTemplate)
        {
            if (savedTemplate != null)
            {
                OnDeleteTemplateClicked?.Invoke(this, savedTemplate);
            }
        }

        public void RaiseOnReorderTemplate() => OnReorderTemplate?.Invoke(this, EventArgs.Empty);

        public void RaiseOnProjectTypeChange(MetadataInfoViewModel projectType) => OnProjectTypeChange?.Invoke(this, projectType);

        public void RaiseOnFrameworkChange(MetadataInfoViewModel framework) => OnFrameworkChange?.Invoke(this, framework);

        public void RaiseOnSavedTemplateFocused(SavedTemplateViewModel template) => OnSavedTemplateFocused?.Invoke(this, template);
    }
}
