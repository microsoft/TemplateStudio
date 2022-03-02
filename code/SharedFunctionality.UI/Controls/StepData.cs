// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Controls
{
    public class StepData : Selectable
    {
        private bool _completed;
        private string _index;

        public string Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        public bool Completed
        {
            get => _completed;
            set => SetProperty(ref _completed, value);
        }

        public Func<object> GetPage { get; private set; }

        public string Id { get; private set; }

        public string Title { get; private set; }

        private StepData(bool isSelected = false)
            : base(isSelected)
        {
        }

        public static StepData MainStep(string stepId, string index, string title, Func<object> getPage, bool completed = false, bool isSelected = false)
        {
            return new StepData(isSelected)
            {
                Id = stepId,
                Index = index,
                Title = title,
                GetPage = getPage,
                Completed = completed,
            };
        }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case StepData step:
                    return Index == step.Index;
                case int index:
                    return Index.Equals(index);
                case Type type:
                    return type.Equals(GetPage().GetType());
                default:
                    return base.Equals(obj);
            }
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
