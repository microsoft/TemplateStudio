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

        public static WizardSteps Project
        {
            get
            {
                var steps = new WizardSteps();

                steps.Add<Steps.ProjectTypeStep.ProjectTypeStepPage>();
                steps.Add<Steps.ProjectsStep.ProjectsStepPage>();
                steps.Add<Steps.PagesStep.PagesStepPage>();
                steps.Add<Steps.SummaryStep.SummaryStepPage>();

                return steps;
            }
        }

        public static WizardSteps Page
        {
            get
            {
                var steps = new WizardSteps();

                steps.Add<Steps.PagesStep.PagesStepPage>();

                return steps;
            }
        }

        public static WizardSteps Feature
        {
            get
            {
                //TODO: NOTHING TO ADD FOR THE MOMENT
                var steps = new WizardSteps();

                return steps;
            }
        }

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
