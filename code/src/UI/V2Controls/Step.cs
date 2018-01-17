// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.V2Controls
{
    public class Step : Observable
    {
        private bool _completed;

        public int Index { get; }

        public int LayoutIndex => Index + 1;

        public string Title { get; }

        public bool Completed
        {
            get => _completed;
            set => SetProperty(ref _completed, value);
        }

        public Step(int index, string title, bool completed = false)
        {
            Completed = completed;
            Index = index;
            Title = title;
        }
    }
}
