// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;

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
        public UserSelection(string projectType, string frontEndFramework, string backEndFramework, string platform, string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentNullException(nameof(language));
            }

            ProjectType = projectType;
            FrontEndFramework = frontEndFramework;
            BackEndFramework = backEndFramework;
            Platform = platform;
            Language = language;
        }

        public string ProjectType { get; set; }

        public string FrontEndFramework { get; set; }

        public string BackEndFramework { get; set; }

        public string HomeName { get; set; }

        public string Platform { get; private set; }

        public string Language { get; private set; }

        public ItemGenerationType ItemGenerationType { get; set; } = ItemGenerationType.None;

        public List<UserSelectionItem> Pages { get; } = new List<UserSelectionItem>();

        public List<UserSelectionItem> Features { get; } = new List<UserSelectionItem>();

        public IEnumerable<UserSelectionItem> PagesAndFeatures
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
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(Language))
            {
                sb.AppendFormat("Language: '{0}'", Language);
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(ProjectType))
            {
                sb.AppendFormat("ProjectType: '{0}'", ProjectType);
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(FrontEndFramework))
            {
                sb.AppendFormat("Front End Framework: '{0}'", FrontEndFramework);
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(BackEndFramework))
            {
                sb.AppendFormat("Back End Framework: '{0}'", BackEndFramework);
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
            }
        }
    }
}
