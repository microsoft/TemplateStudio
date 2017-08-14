// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.UI.VisualStudio;

namespace Microsoft.Templates.Extension.Commands
{
    public class GenContextBootstrapService : ISGenContextBootstrapService, IGenContextBootstrapService
    {
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider serviceProvider;
        public GenContextBootstrapService(Microsoft.VisualStudio.Shell.IAsyncServiceProvider provider)
        {
            serviceProvider = provider;
        }
        public async System.Threading.Tasks.Task GenContextInit(GenShell shell, string language)
        {
            if (GenContext.InitializedLanguage != language)
            {
#if DEBUG
                GenContext.Bootstrap(new LocalTemplatesSource(), shell, language);
#else
                GenContext.Bootstrap(new RemoteTemplatesSource(), shell, language);
#endif
                await GenContext.ToolBox.Repo.SynchronizeAsync();
            }
        }

        public TaskAwaiter GetAwaiter()
        {
            return default(TaskAwaiter);
        }
    }

    public interface ISGenContextBootstrapService
    {
    }

    public interface IGenContextBootstrapService
    {
        System.Threading.Tasks.Task GenContextInit(GenShell shell, string language);
        TaskAwaiter GetAwaiter();
    }
}
