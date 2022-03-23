// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Templates.Core.Gen
{
    public enum ItemGenerationType
    {
        None,
        Generate,
        GenerateAndMerge,
    }

    public class UserSelection
    {
        public UserSelection(UserSelectionContext context)
        {
            Context = context;
        }

        public UserSelectionContext Context { get; private set; }

        public string HomeName { get; set; }

        public ItemGenerationType ItemGenerationType { get; set; } = ItemGenerationType.None;

        public List<UserSelectionItem> Pages { get; } = new List<UserSelectionItem>();

        public List<UserSelectionItem> Features { get; } = new List<UserSelectionItem>();

        public List<UserSelectionItem> Services { get; } = new List<UserSelectionItem>();

        public List<UserSelectionItem> Testing { get; } = new List<UserSelectionItem>();

        public IEnumerable<UserSelectionItem> Items
        {
            get
            {
                foreach (var page in Pages)
                {
                    yield return page;
                }

                foreach (var feature in Features)
                {
                    yield return feature;
                }

                foreach (var service in Services)
                {
                    yield return service;
                }

                foreach (var testing in Testing)
                {
                    yield return testing;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(Context.Language))
            {
                sb.AppendFormat("Language: '{0}'", Context.Language);
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(Context.ProjectType))
            {
                sb.AppendFormat("ProjectType: '{0}'", Context.ProjectType);
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(Context.FrontEndFramework))
            {
                sb.AppendFormat("Front End Framework: '{0}'", Context.FrontEndFramework);
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(Context.BackEndFramework))
            {
                sb.AppendFormat("Back End Framework: '{0}'", Context.BackEndFramework);
                sb.AppendLine();
            }

            foreach (var property in Context.PropertyBag)
            {
#pragma warning disable CA1308 // Normalize strings to uppercase
                sb.AppendFormat($"{property.Key.ToLowerInvariant()}", property.Value);
#pragma warning restore CA1308 // Normalize strings to uppercase
                sb.AppendLine();
            }

            if (Pages.Any())
            {
                sb.AppendFormat("Pages: '{0}'", string.Join(", ", Pages.Select(p => $"{p.Name} - {p.TemplateId}").ToArray()));
                sb.AppendLine();
            }

            if (Features.Any())
            {
                sb.AppendFormat("Features: '{0}'", string.Join(", ", Features.Select(p => $"{p.Name} - {p.TemplateId}").ToArray()));
                sb.AppendLine();
            }

            if (Services.Any())
            {
                sb.AppendFormat("Services: '{0}'", string.Join(", ", Services.Select(p => $"{p.Name} - {p.TemplateId}").ToArray()));
                sb.AppendLine();
            }

            if (Testing.Any())
            {
                sb.AppendFormat("Testing: '{0}'", string.Join(", ", Testing.Select(p => $"{p.Name} - {p.TemplateId}").ToArray()));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public void Add(UserSelectionItem template, TemplateType templateType)
        {
            switch (templateType)
            {
                case TemplateType.Page:
                    Pages.Add(template);
                    break;
                case TemplateType.Feature:
                    Features.Add(template);
                    break;
                case TemplateType.Service:
                    Services.Add(template);
                    break;
                case TemplateType.Testing:
                    Testing.Add(template);
                    break;
            }
        }
    }
}
