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

using System.Windows;

namespace Microsoft.Templates.Core.Gen
{
    public abstract class GenShell
    {
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

        public abstract string GetVsCultureInfo();

        public abstract string GetVsVersion();

        public abstract string GetVsEdition();

        public virtual void RestorePackages()
        {
        }

        public virtual void CollapseSolutionItems()
        {
        }

        

        public virtual void RefreshProject()
        {

        }
    }
}
