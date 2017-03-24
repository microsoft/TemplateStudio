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

using Microsoft.Templates.Wizard.Steps;

namespace Microsoft.Templates.Wizard.Host
{
    public class WizardSteps
    {
        private readonly List<Type> _steps = new List<Type>();
        private int CurrentIndex => _steps.IndexOf(Current);

        public static WizardSteps Project
        {
            get
            {
                var steps = new WizardSteps();

                steps.Add<Steps.ProjectType.ViewModel>();
                steps.Add<Steps.Framework.ViewModel>();
                steps.Add<Steps.Pages.ViewModel>();
                steps.Add<Steps.DevFeatures.ViewModel>();
                steps.Add<Steps.ConsumerFeatures.ViewModel>();
                steps.Add<Steps.Summary.ViewModel>();

                return steps;
            }
        }

        public static WizardSteps Page
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public static WizardSteps Feature
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Type Current { get; private set; }

        public void Add<T>() where T : StepViewModel
        {
            _steps.Add(typeof(T));
        }

        public Type First()
        {
            Current = _steps.FirstOrDefault();
            return Current;
        }

        public Type GoBack()
        {
            if (CanGoBack())
            {
                Current = _steps.ElementAt(CurrentIndex - 1);
                return Current;
            }
            return null;
        }

        public Type GoForward()
        {
            if (CanGoForward())
            {
                Current = _steps.ElementAt(CurrentIndex + 1);
                return Current;
            }
            return null;
        }

        public bool CanGoForward()
        {
            return _steps.Count > CurrentIndex + 1;
        }

        public bool CanGoBack()
        {
            return CurrentIndex > 0;
        }
    }
}
