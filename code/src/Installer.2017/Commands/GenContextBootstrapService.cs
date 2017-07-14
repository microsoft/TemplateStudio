// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
        public async System.Threading.Tasks.Task GenContextInit()
        {
            if (!GenContext.IsInitialized)
            {
#if DEBUG
                GenContext.Bootstrap(new LocalTemplatesSource(), new VsGenShell());
#else
                GenContext.Bootstrap(new RemoteTemplatesSource(), new VsGenShell());
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
        System.Threading.Tasks.Task GenContextInit();
        TaskAwaiter GetAwaiter();
    }
}
