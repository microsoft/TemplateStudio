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

        public event EventHandler<MetadataInfoViewModel> OnProjectTypeChanged;

        public event EventHandler<MetadataInfoViewModel> OnFrameworkChanged;

        public event EventHandler<TemplateInfoViewModel> OnTemplateClicked;

        public event EventHandler<TemplateInfoViewModel> OnTemplateSelected;

        public event EventHandler<SavedTemplateViewModel> OnDeleteTemplateClicked;

        public event EventHandler<KeyEventArgs> OnKeyDown;

        public event EventHandler OnReorderTemplate;

        private EventService()
        {
        }

        public void RaiseOnProjectTypeChanged(MetadataInfoViewModel projectType)
        {
            if (projectType != null)
            {
                OnProjectTypeChanged?.Invoke(this, projectType);
            }
        }

        public void RaiseOnFrameworkChanged(MetadataInfoViewModel framework)
        {
            if (framework != null)
            {
                OnFrameworkChanged?.Invoke(this, framework);
            }
        }

        public void RaiseOnTemplateClicked(TemplateInfoViewModel template)
        {
            if (template != null)
            {
                OnTemplateClicked?.Invoke(this, template);
            }
        }

        public void RaiseOnTemplateSelected(TemplateInfoViewModel template)
        {
            if (template != null)
            {
                OnTemplateSelected?.Invoke(this, template);
            }
        }

        public void RaiseOnDeleteTemplateClicked(SavedTemplateViewModel savedTemplate)
        {
            if (savedTemplate != null)
            {
                OnDeleteTemplateClicked?.Invoke(this, savedTemplate);
            }
        }

        public void RaiseOnReorderTemplate() => OnReorderTemplate?.Invoke(this, EventArgs.Empty);

        public void RaiseOnKeyDown(KeyEventArgs args)
        {
            if (args != null)
            {
                OnKeyDown?.Invoke(this, args);
            }
        }
    }
}
