// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class TemplatesStepViewModel : Observable
    {
        private readonly UserSelectionContext _context;
        private readonly TemplateType _templateType;
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ObservableCollection<TemplateGroupViewModel> Groups { get; } = new ObservableCollection<TemplateGroupViewModel>();

        public TemplatesStepViewModel(TemplateType templateType, UserSelectionContext context, string title)
        {
            _templateType = templateType;
            _context = context;
            Title = title;
        }

        public void LoadData()
        {
            Groups.Clear();
            DataService.LoadTemplatesGroups(Groups, _templateType, _context);
        }

        public void ResetData(string projectTypeName, string frameworkName)
        {
            Groups.Clear();
            DataService.LoadTemplatesGroups(Groups, _templateType, _context);
        }

        public void ResetTemplatesCount()
        {
            foreach (var group in Groups)
            {
                foreach (var template in group.Items)
                {
                    template.ResetTemplateCount();
                }
            }
        }
    }
}
