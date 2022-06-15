// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class ErrorDialogViewModel : BaseDialogViewModel
    {
        private string _showDetails;

        public ErrorDialogViewModel(Exception ex)
        {
            Title = Resources.ErrorDialogTitle;
            Description = ex.Message;
            ErrorStackTrace = ex.ToString();
        }

        public string ErrorStackTrace { get; set; }

        public string ShowDetails
        {
            get => _showDetails;
            set => SetProperty(ref _showDetails, value);
        }
    }
}
