// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Services;

namespace Microsoft.Templates.UI.V2ViewModels.Common
{
    public class TemplateInfoViewModel : BasicInfoViewModel
    {
        private int _count;
        private bool _hasMoreThanOne;
        private bool _hasMoreThanTwo;

        public string Group { get; }

        public TemplateType TemplateType { get; }

        public bool MultipleInstance { get; }

        public bool ItemNameEditable { get; }

        public ICommand ItemClickCommand { get; }

        public int Count
        {
            get => _count;
            set
            {
                SetProperty(ref _count, value);
                HasMoreThanOne = value > 1;
                HasMoreThanTwo = value > 2;
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

        public TemplateInfoViewModel(ITemplateInfo template, IEnumerable<ITemplateInfo> dependencies)
        {
            // BasicInfo properties
            Name = template.Name;
            Title = template.Name;
            Description = template.Description;
            Author = template.Author;
            Icon = template.GetIcon();
            Order = template.GetDisplayOrder();

            // ITemplateInfo properties
            Group = template.GetGroup();
            TemplateType = template.GetTemplateType();
            MultipleInstance = template.GetMultipleInstance();
            ItemNameEditable = template.GetItemNameEditable();
            ItemClickCommand = new RelayCommand(OnItemClick);
        }

        private void OnItemClick() => EventService.Instance.RaiseOnTemplateClicked(this);

        public void UpdateSelection(SavedTemplateViewModel newTemplate)
        {
            Count++;
        }

        public void ResetUserSelection()
        {
            Count = 0;
        }
    }
}
