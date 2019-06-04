// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.Extensions
{
    public static class TemplateTypeExtensions
    {
        private const string _newProjectStepPages = "03Pages";
        private const string _newProjectStepFeatures = "04Features";
        private const string _newProjectStepServices = "05Services";
        private const string _newProjectStepTests = "06Tests";

        public static string GetNewProjectStepId(this TemplateType templateType)
        {
            switch (templateType)
            {
                case TemplateType.Page:
                    return _newProjectStepPages;
                case TemplateType.Feature:
                    return _newProjectStepFeatures;
                case TemplateType.Service:
                    return _newProjectStepServices;
                case TemplateType.Testing:
                    return _newProjectStepTests;
                default:
                    return string.Empty;
            }
        }

        public static string GetNewProjectStepTitle(this TemplateType templateType)
        {
            switch (templateType)
            {
                case TemplateType.Page:
                    return StringRes.NewProjectStepPages;
                case TemplateType.Feature:
                    return StringRes.NewProjectStepFeatures;
                case TemplateType.Service:
                    return StringRes.NewProjectStepServices;
                case TemplateType.Testing:
                    return StringRes.NewProjectStepTesting;
                default:
                    return string.Empty;
            }
        }

        public static string GetStepPageTitle(this TemplateType templateType)
        {
            switch (templateType)
            {
                case TemplateType.Page:
                    return StringRes.AddPagesTitle;
                case TemplateType.Feature:
                    return StringRes.AddFeaturesTitle;
                case TemplateType.Service:
                    return StringRes.AddServiceTitle;
                case TemplateType.Testing:
                    return StringRes.AddTestingTitle;
                default:
                    return string.Empty;
            }
        }
    }
}
