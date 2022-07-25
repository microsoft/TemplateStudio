// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.ViewModels.NewProject;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class BaseSelectionViewModel
    {
        /*
        Initialize
        UpdateHasItemsAddedByUser
        AllTemplates
        GetUserSelection
         */

        public static IEnumerable<string> GetNames(ObservableCollection<UserSelectionGroup> groups)
        {
            var names = new List<string>();
            groups.ToList().ForEach(g => names.AddRange(g.GetNames()));
            return names;
        }

        public static IEnumerable<string> GetPageNames(ObservableCollection<UserSelectionGroup> groups) => groups.First(g => g.TemplateType == TemplateType.Page).GetNames(p => p.ItemNameEditable);
        public static ObservableCollection<SavedTemplateViewModel> GetCollection(TemplateType templateType, ObservableCollection<UserSelectionGroup> groups)
        {
            return groups.First(g => g.TemplateType == templateType).Items;
        }
        public static bool IsTemplateAdded(TemplateInfoViewModel template, ObservableCollection<UserSelectionGroup> groups) => GetCollection(template.TemplateType, groups).Any(t => t.Equals(template));

        public static UserSelectionGroup GetGroup(TemplateType templateType, ObservableCollection<UserSelectionGroup> groups) => groups.First(t => t.TemplateType == templateType);

        public static void AddToGroup(TemplateType templateType, SavedTemplateViewModel savedTemplate, ObservableCollection<UserSelectionGroup> groups)
        {
            bool GenGroupEqual(SavedTemplateViewModel st) => st.GenGroup == savedTemplate.GenGroup;
            bool GenGroupPrevious(SavedTemplateViewModel st) => st.GenGroup < savedTemplate.GenGroup;

            int index = 0;
            var group = GetGroup(templateType, groups);
            if (group.Items.Any(GenGroupEqual))
            {
                index = group.Items.IndexOf(group.Items.Last(GenGroupEqual)) + 1;
            }
            else if (group.Items.Any())
            {
                index = group.Items.IndexOf(group.Items.Last(GenGroupPrevious)) + 1;
            }

            group.Insert(index, savedTemplate);
        }
        public static UserSelection GetUserSelection(ObservableCollection<UserSelectionGroup> groups, UserSelectionContext _context) // creates user selection list
        {
            var selection = new UserSelection(_context);
            var pages = groups.First(g => g.TemplateType == TemplateType.Page).Items;
            selection.HomeName = pages.FirstOrDefault()?.Name ?? string.Empty;
            selection.Pages.AddRange(pages.Select(i => i.ToUserSelectionItem()));

            var features = groups.First(g => g.TemplateType == TemplateType.Feature).Items;
            selection.Features.AddRange(features.Select(i => i.ToUserSelectionItem()));

            var services = groups.First(g => g.TemplateType == TemplateType.Service).Items;
            selection.Services.AddRange(services.Select(i => i.ToUserSelectionItem()));

            var tests = groups.First(g => g.TemplateType == TemplateType.Testing).Items;
            selection.Testing.AddRange(tests.Select(i => i.ToUserSelectionItem()));
            return selection;
        }
    }
}
