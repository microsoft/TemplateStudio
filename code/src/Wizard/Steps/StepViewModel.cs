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
using System.Windows.Controls;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.ViewModels;

namespace Microsoft.Templates.Wizard.Steps
{
    public abstract class StepViewModel : Observable
    {
        protected WizardContext Context { get; }

        public abstract string PageTitle { get; }

        public StepViewModel(WizardContext context)
        {
            Context = context;
            Context.CanGoForward = true;
        }

        //TODO: MAKE THIS METHOD TRULY ASYNC
        public abstract Task InitializeAsync();
        public abstract void SaveState();

        protected abstract Page GetPageInternal();

        public Page GetPage()
        {
            var page = GetPageInternal();

            page.DataContext = this;
            page.Loaded += async (sender, e) =>
            {
                await InitializeAsync();
            };

            return page;
        }
    }
}
