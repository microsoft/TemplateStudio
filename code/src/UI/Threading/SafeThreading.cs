// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Shell;

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
                JoinableTaskContext context = new JoinableTaskContext(System.Threading.Thread.CurrentThread);
                JoinableTaskCollection collection = context.CreateCollection();
                JoinableTaskFactory = context.CreateFactory(collection);
            }
        }
    }
}
