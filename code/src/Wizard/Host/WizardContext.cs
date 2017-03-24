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

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Wizard.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Templates.Wizard.Host
{
    public class WizardState
    {
        public string ProjectType { get; set; }
        public string Framework { get; set; }
        public List<(string name, string templateName)> Pages { get; } = new List<(string name, string templateName)>();
        public List<(string name, string templateName)> DevFeatures { get; } = new List<(string name, string templateName)>();
        public List<(string name, string templateName)> ConsumerFeatures { get; } = new List<(string name, string templateName)>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(ProjectType))
            {
                sb.AppendFormat("ProjectType: '{0}'", ProjectType);
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(Framework))
            {
                sb.AppendFormat("Framework: '{0}'", Framework);
                sb.AppendLine();
            }

            if (Pages.Any())
            {
                sb.AppendFormat("Pages: '{0}'", string.Join(", ", Pages.Select(p => $"{p.name} - {p.templateName}").ToArray()));
                sb.AppendLine();
            }

            if (DevFeatures.Any())
            {
                sb.AppendFormat("DevFeatures: '{0}'", string.Join(", ", DevFeatures.Select(p => $"{p.name} - {p.templateName}").ToArray()));
                sb.AppendLine();
            }

            if (ConsumerFeatures.Any())
            {
                sb.AppendFormat("ConsumerFeatures: '{0}'", string.Join(", ", ConsumerFeatures.Select(p => $"{p.name} - {p.templateName}").ToArray()));
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

    public class WizardContext : Observable
    {
        public Window Host { get; set; }

        public WizardState State { get; } = new WizardState();

        private bool _canGoForward;
        public bool CanGoForward
        {
            get => _canGoForward;
            set => SetProperty(ref _canGoForward, value);
        }

        public bool ResetSelection()
        {
            var resetSelectionResult = MessageBox.Show(WizardHostResources.ResetSelection, WizardHostResources.ResetSelectionTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return resetSelectionResult == MessageBoxResult.Yes;
        }
    }
}
