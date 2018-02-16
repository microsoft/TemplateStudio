// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Services
{
    public class EventService
    {
        private static EventService _instance;

        public static EventService Instance => _instance ?? (_instance = new EventService());

        public event EventHandler<string> OnSavedTemplateFocused;

        private EventService()
        {
        }

        public void RaiseOnSavedTemplateFocused(string templateName) => OnSavedTemplateFocused?.Invoke(this, templateName);
    }
}
