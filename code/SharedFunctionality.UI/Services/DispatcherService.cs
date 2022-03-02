// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Threading;

namespace Microsoft.Templates.UI.Services
{
    public static class DispatcherService
    {
        public static void BeginInvoke(Action action)
        {
#pragma warning disable VSTHRD001 // Avoid legacy threading switching APIs
            _ = Application.Current.Dispatcher.BeginInvoke(action, DispatcherPriority.ContextIdle, null);
#pragma warning restore VSTHRD001 // Avoid legacy threading switching APIs
        }
    }
}
