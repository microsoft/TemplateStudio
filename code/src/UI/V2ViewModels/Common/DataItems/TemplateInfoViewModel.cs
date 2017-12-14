// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.UI.V2ViewModels.Common
{
    public class TemplateInfoViewModel : BasicInfoViewModel
    {
        private string _group;

        public string Group
        {
            get => _group;
            set => SetProperty(ref _group, value);
        }

        public TemplateInfoViewModel(ITemplateInfo template, IEnumerable<ITemplateInfo> dependencies)
        {
            Name = template.Name;
            Title = template.Name;
            Description = template.Description;
            Author = template.Author;
            Order = template.GetDisplayOrder();
            Group = template.GetGroup();
        }
    }
}
