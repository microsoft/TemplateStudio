using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.Host
{
    public class WizardContext : ObservableBase
    {
        public TemplatesRepository TemplatesRepository { get; }
        public GenShell Shell { get; }
        private Dictionary<Type, object> State { get; } = new Dictionary<Type, object>();

        public WizardContext(TemplatesRepository templatesRepository, GenShell shell)
        {
            TemplatesRepository = templatesRepository;
            Shell = shell;
        }

        private bool _canGoForward;
        public bool CanGoForward
        {
            get => _canGoForward;
            set => SetProperty(ref _canGoForward, value);
        }

        public IEnumerable<GenInfo> GetSelection()
        {
            var selection = new List<GenInfo>();

            foreach (var stepState in State)
            {
                if (stepState.Value is IEnumerable<GenInfo> stepTemplates)
                {
                    selection.AddRange(stepTemplates);
                }
                else if (stepState.Value is GenInfo stepTemplate)
                {
                    selection.Add(stepTemplate);
                }
            }

            return selection;
        }

        public TOut GetState<T, TOut>() where T : Steps.StepViewModel
        {
            if (State.ContainsKey(typeof(T)) && State[typeof(T)] is TOut)
            {
                return (TOut)State[typeof(T)];
            }
            return default(TOut);
        }

        public void SetState<T>(T instance, object state) where T : Steps.StepViewModel
        {
            if (instance == null)
            {
                return;
            }
            var type = instance.GetType();

            if (State.ContainsKey(type))
            {
                State[type] = state;
            }
            else
            {
                State.Add(type, state);
            }
        }

        public void ClearState<T>(T instance)
        {
            if (instance == null)
            {
                return;
            }
            var type = instance.GetType();

            if (State.ContainsKey(type))
            {
                State.Remove(type);
            }
        }
    }
}
