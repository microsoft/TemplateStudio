using Microsoft.Templates.Wizard.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.Host
{
    public class WizardSteps
    {
        private readonly List<Type> _steps = new List<Type>();
        private int CurrentIndex => _steps.IndexOf(Current);

        public Type Current { get; private set; }

        public void Add<T>() where T : StepPage
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
