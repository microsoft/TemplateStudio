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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class TemplateInfoViewModel : CommonInfoViewModel
    {
        public ITemplateInfo Template { get; set; }
        public bool IsItemNameEditable { get; set; }
        public string DefaultName { get; set; }
        public string Group { get; set; }
        public string Identity { get; set; }
        public TemplateType TemplateType { get; set; }

        public ObservableCollection<DependencyInfoViewModel> DependencyItems { get; } = new ObservableCollection<DependencyInfoViewModel>();

        public TemplateInfoViewModel(ITemplateInfo template, IEnumerable<ITemplateInfo> dependencies)
        {
            Template = template;
            IsItemNameEditable = template.GetItemNameEditable();
            DefaultName = template.GetDefaultName();
            Group = template.GetGroup();
            Icon = template.GetIcon();
            Name = template.Name;
            Author = template.Author;
            Order = template.GetOrder();
            Summary = template.Description;
            Identity = template.Identity;
            Version = template.GetVersion();
            TemplateType = template.GetTemplateType();
            Description = template.GetRichDescription();
            DependencyItems.AddRange(dependencies.Select(d => new DependencyInfoViewModel(new TemplateInfoViewModel(d, GenComposer.GetAllDependencies(d, MainViewModel.Current.ConfigFramework)))));
            LicenseTerms = template.GetLicenses();
        }

        public override string ToString()
        {
            return $"{Name} - {Summary}";
        }
    }
}
