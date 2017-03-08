using Microsoft.Templates.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Templates.Core
{
    public abstract class GenShell
    {
        private static GenShell _instance;

        private static readonly Lazy<GenShell> _current = new Lazy<GenShell>(GetInstance, true);

        private static GenShell GetInstance()
        {
            if (_instance == null)
            {
                throw new Exception("GenShell is not initialized");
            }

            return _instance;
        }

        public static GenShell Current => _current.Value;

        public static void Initialize(GenShell instance)
        {
            _instance = instance;
        }

        private GenSolution _contextInfo;
        public GenSolution ContextInfo
        {
            get
            {
                if (_contextInfo == null)
                {
                    throw new Exception("Context is not initialized");
                }
                return _contextInfo;
            }
            set
            {
                _contextInfo = value;
            }
        }

        protected abstract string GetActiveProjectName();
        protected abstract string GetActiveProjectPath();
        protected abstract string GetSelectedItemPath();

        public abstract bool SetActiveConfigurationAndPlatform(string configurationName, string platformName);
        public abstract void ShowStatusBarMessage(string message);
        public abstract void AddProjectToSolution(string projectFullPath);
        public abstract void AddItems(params string[] itemsFullPath);
        public abstract void SaveSolution(string solutionFullPath);
        public abstract string GetActiveNamespace();
        public abstract void ShowTaskList();
        public abstract void ShowModal(Window dialog);
        public abstract void CancelWizard(bool back = true);
        public abstract void WriteOutput(string data);
        public abstract void CloseSolution();
    }
}
