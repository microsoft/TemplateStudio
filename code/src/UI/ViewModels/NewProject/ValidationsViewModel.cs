// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Resources;
using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class ValidationsViewModel
    {
        public ValidationsViewModel()
        {
        }

        public IEnumerable<string> Names
        {
            get
            {
                var names = new List<string>();
                MainViewModel.Current.ProjectTemplates.SavedPages.ToList().ForEach(spg => names.AddRange(spg.Select(sp => sp.ItemName)));
                names.AddRange(MainViewModel.Current.ProjectTemplates.SavedFeatures.Select(sf => sf.ItemName));
                return names;
            }
        }

        public void ValidateNewTemplateName(TemplateInfoViewModel template)
        {
            var validators = new List<Validator>()
            {
                new ExistingNamesValidator(Names),
                new ReservedNamesValidator()
            };
            if (template.CanChooseItemName)
            {
                validators.Add(new DefaultNamesValidator());
            }
            var validationResult = Naming.Validate(template.NewTemplateName, validators);

            template.IsValidName = validationResult.IsValid;
            template.ErrorMessage = string.Empty;

            if (!template.IsValidName)
            {
                template.ErrorMessage = StringRes.ResourceManager.GetString($"ValidationError_{validationResult.ErrorType}");

                if (string.IsNullOrWhiteSpace(template.ErrorMessage))
                {
                    template.ErrorMessage = StringRes.UndefinedErrorString;
                }
                MainViewModel.Current.SetValidationErrors(template.ErrorMessage);
                throw new Exception(template.ErrorMessage);
            }
            MainViewModel.Current.CleanStatus(true);
        }

        public void ValidateCurrentTemplateName(SavedTemplateViewModel item)
        {
            if (item.NewItemName != item.ItemName)
            {
                var validators = new List<Validator>()
                {
                    new ExistingNamesValidator(Names),
                    new ReservedNamesValidator()
                };
                if (item.CanChooseItemName)
                {
                    validators.Add(new DefaultNamesValidator());
                }
                var validationResult = Naming.Validate(item.NewItemName, validators);

                item.IsValidName = validationResult.IsValid;
                item.ErrorMessage = string.Empty;

                if (!item.IsValidName)
                {
                    item.ErrorMessage = StringRes.ResourceManager.GetString($"ValidationError_{validationResult.ErrorType}");

                    if (string.IsNullOrWhiteSpace(item.ErrorMessage))
                    {
                        item.ErrorMessage = StringRes.UndefinedErrorString;
                    }
                    MainViewModel.Current.SetValidationErrors(item.ErrorMessage);
                    throw new Exception(item.ErrorMessage);
                }
            }
            MainViewModel.Current.CleanStatus(true);
        }

        public string InferItemNameForNewTemplate(TemplateInfoViewModel template)
        {
            var validators = new List<Validator>()
            {
                new ReservedNamesValidator(),
                new DefaultNamesValidator()
            };
            if (template.CanChooseItemName)
            {
                validators.Add(new ExistingNamesValidator(Names));
            }
            return Naming.Infer(template.Template.GetDefaultName(), validators);
        }

        public bool IsValidRename(string newItemName, bool canChooseItemName)
        {
            var validators = new List<Validator>()
            {
                new ExistingNamesValidator(Names),
                new ReservedNamesValidator()
            };
            if (canChooseItemName)
            {
                validators.Add(new DefaultNamesValidator());
            }
            return Naming.Validate(newItemName, validators).IsValid;
        }
    }
}
