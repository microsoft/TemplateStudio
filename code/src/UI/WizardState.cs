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
using System.Windows;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI
{
    public class WizardState
    {
        public string ProjectType { get; set; }
        public string Framework { get; set; }
        public List<(string name, ITemplateInfo template)> Pages { get; } = new List<(string name, ITemplateInfo template)>();
        public List<(string name, ITemplateInfo template)> DevFeatures { get; } = new List<(string name, ITemplateInfo template)>();
        public List<(string name, ITemplateInfo template)> ConsumerFeatures { get; } = new List<(string name, ITemplateInfo template)>();

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
                sb.AppendFormat("Pages: '{0}'", string.Join(", ", Pages.Select(p => $"{p.name} - {p.template.Name}").ToArray()));
                sb.AppendLine();
            }

            if (DevFeatures.Any())
            {
                sb.AppendFormat("DevFeatures: '{0}'", string.Join(", ", DevFeatures.Select(p => $"{p.name} - {p.template.Name}").ToArray()));
                sb.AppendLine();
            }

            if (ConsumerFeatures.Any())
            {
                sb.AppendFormat("ConsumerFeatures: '{0}'", string.Join(", ", ConsumerFeatures.Select(p => $"{p.name} - {p.template.Name}").ToArray()));
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
