// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Core.Extensions
{
    public static class TemplateTypeExtensions
    {
        public static bool IsItemTemplate(this TemplateType templateType)
        {
            switch (templateType)
            {
                case TemplateType.Page:
                case TemplateType.Feature:
                case TemplateType.Service:
                case TemplateType.Testing:
                    return true;
                default:
                    return false;
            }
        }

        public static WizardTypeEnum? GetWizardType(this TemplateType templateType)
        {
            switch (templateType)
            {
                case TemplateType.Page:
                    return WizardTypeEnum.AddPage;
                case TemplateType.Feature:
                    return WizardTypeEnum.AddFeature;
                case TemplateType.Service:
                    return WizardTypeEnum.AddService;
                case TemplateType.Testing:
                    return WizardTypeEnum.AddTesting;
                default:
                    return null;
            }
        }

        public static NewItemType? GetNewItemType(this TemplateType templateType)
        {
            switch (templateType)
            {
                case TemplateType.Page:
                    return NewItemType.Page;
                case TemplateType.Feature:
                    return NewItemType.Feature;
                case TemplateType.Service:
                    return NewItemType.Service;
                case TemplateType.Testing:
                    return NewItemType.Testing;
                default:
                    return null;
            }
        }
    }
}
