// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.Templates.UI.Threading
{
    public static class SafeThreading
    {
        public static JoinableTaskFactory JoinableTaskFactory { get; set; }

        static SafeThreading()
        {
            try
            {
                JoinableTaskFactory = ThreadHelper.JoinableTaskFactory;
            }
            catch (NullReferenceException)
            {
                JoinableTaskContext context = ThreadHelper.JoinableTaskContext;
                JoinableTaskCollection collection = context.CreateCollection();
                JoinableTaskFactory = context.CreateFactory(collection);
            }
        }
    }
}
