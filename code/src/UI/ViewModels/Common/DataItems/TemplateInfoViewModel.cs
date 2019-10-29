// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class TemplateInfoViewModel : BasicInfoViewModel
    {
        private int _count;
        private bool _hasMoreThanOne;
        private bool _hasMoreThanTwo;
        private bool _showAddedText;
        private bool _canBeAdded;
        private string _emptyBackendFramework = string.Empty;

        public TemplateInfo Template { get; }

        public string Group { get; }

        public bool IsGroupExclusiveSelection { get; }

        public int GenGroup { get; }

        public TemplateType TemplateType { get; }

        public bool MultipleInstance { get; }

        public bool ItemNameEditable { get; }

        public int Count
        {
            get => _count;
            private set
            {
                HasMoreThanOne = MultipleInstance && value > 1;
                HasMoreThanTwo = MultipleInstance && value > 2;
                ShowAddedText = !MultipleInstance && value > 0;
                CanBeAdded = MultipleInstance || value == 0;
                if (MultipleInstance || value <= 1)
                {
                    SetProperty(ref _count, value);
                }
            }
        }

        public bool HasMoreThanOne
        {
            private get => _hasMoreThanOne;
            set => SetProperty(ref _hasMoreThanOne, value);
        }

        public bool HasMoreThanTwo
        {
            private get => _hasMoreThanTwo;
            set => SetProperty(ref _hasMoreThanTwo, value);
        }

        public bool ShowAddedText
        {
            private get => _showAddedText;
            set => SetProperty(ref _showAddedText, value);
        }

        public bool CanBeAdded
        {
            private get => _canBeAdded;
            set => SetProperty(ref _canBeAdded, value);
        }

        public TemplateInfoViewModel(TemplateInfo template,  string platform, string projectType, string frameworkName)
        {
            // BasicInfo properties
            Name = template.Name;
            Identity = template.TemplateId;
            Title = template.Name;
            Summary = template.Description;
            Description = template.RichDescription;
            Author = template.Author;
            Version = template.Version;
            Icon = template.Icon;
            Order = template.DisplayOrder;
            IsHidden = template.IsHidden;
            Dependencies = template.Dependencies.Select(d => new TemplateInfoViewModel(d, platform, projectType, frameworkName));
            Requirements = template.Requirements.Select(d => new TemplateInfoViewModel(d, platform, projectType, frameworkName));
            Exclusions = template.Exclusions.Select(d => new TemplateInfoViewModel(d, platform, projectType, frameworkName));
            Licenses = template.Licenses.Select(l => new LicenseViewModel(l));

            // ITemplateInfo properties
            Template = template;
            Group = template.Group;
            IsGroupExclusiveSelection = template.IsGroupExclusiveSelection;
            GenGroup = template.GenGroup;
            TemplateType = template.TemplateType;
            MultipleInstance = template.MultipleInstance;
            ItemNameEditable = template.ItemNameEditable;
            CanBeAdded = MultipleInstance || Count == 0;
        }

        public void IncreaseSelection()
        {
            Count++;
        }

        public void DecreaseSelection()
        {
            Count--;
        }

        public void ResetTemplateCount()
        {
            Count = 0;
        }
    }
}
