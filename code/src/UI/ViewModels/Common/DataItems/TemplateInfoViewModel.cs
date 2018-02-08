// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Services;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class TemplateInfoViewModel : BasicInfoViewModel
    {
        private int _count;
        private bool _hasMoreThanOne;
        private bool _hasMoreThanTwo;
        private bool _showAddedText;
        private bool _canBeAdded;

        public ITemplateInfo Template { get; }

        public string Group { get; }

        public int GenGroup { get; }

        public TemplateType TemplateType { get; }

        public bool MultipleInstance { get; }

        public bool ItemNameEditable { get; }

        public int Count
        {
            get => _count;
            private set
            {
                SetProperty(ref _count, value);
                HasMoreThanOne = value > 1;
                HasMoreThanTwo = value > 2;
                ShowAddedText = !MultipleInstance && Count > 0;
                CanBeAdded = MultipleInstance || Count == 0;
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

        public TemplateInfoViewModel(ITemplateInfo template, string frameworkName)
        {
            // BasicInfo properties
            Name = template.Name;
            Identity = template.Identity;
            Title = template.Name;
            Summary = template.Description;
            Description = template.GetRichDescription();
            Author = template.Author;
            Version = template.GetVersion();
            Icon = template.GetIcon();
            Order = template.GetDisplayOrder();
            IsHidden = template.GetIsHidden();
            var dependencies = GenComposer.GetAllDependencies(template, frameworkName);
            Dependencies = dependencies.Select(d => new TemplateInfoViewModel(d, frameworkName));
            Licenses = template.GetLicenses().Select(l => new LicenseViewModel(l));

            // ITemplateInfo properties
            Template = template;
            Group = template.GetGroup();
            GenGroup = template.GetGenGroup();
            TemplateType = template.GetTemplateType();
            MultipleInstance = template.GetMultipleInstance();
            ItemNameEditable = template.GetItemNameEditable();
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
