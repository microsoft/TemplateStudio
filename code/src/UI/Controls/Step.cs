// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Controls
{
    public class Step : Selectable
    {
        private bool _completed;

        public int Index { get; }

        public string Title { get; }

        public Func<object> GetPage { get; }

        public bool Completed
        {
            get => _completed;
            set => SetProperty(ref _completed, value);
        }

        public Step(int index, string title, Func<object> getPage, bool completed = false, bool isSelected = false)
            : base(isSelected)
        {
            Index = index;
            Title = title;
            GetPage = getPage;
            Completed = completed;
        }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case int index:
                    return Index.Equals(index);
                case Type type:
                    return type.Equals(GetPage().GetType());
                default:
                    return base.Equals(obj);
            }
        }

        public override int GetHashCode() => base.GetHashCode();

        public bool IsPrevious(int index) => Index <= index;
    }
}
