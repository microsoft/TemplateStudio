// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.Templates.UI.Services
{
#pragma warning disable SA1306 // Field names must begin with lower-case letter
#pragma warning disable SA1516 // Elements must be separated by blank line
#pragma warning disable SA1502 // Element must not be on a single line
    public static class WindowsTemplateStudioColors
    {
        public static readonly Guid Category = new Guid("9c63f223-700a-4418-8a5a-cd450f05f593");

        private static ThemeResourceKey _CardBackgroundDefaultColorKey;
        private static ThemeResourceKey _CardBackgroundDefaultBrushKey;
        public static ThemeResourceKey CardBackgroundDefaultColorKey { get { return _CardBackgroundDefaultColorKey ?? (_CardBackgroundDefaultColorKey = new ThemeResourceKey(Category, "CardBackgroundDefault", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey CardBackgroundDefaultBrushKey { get { return _CardBackgroundDefaultBrushKey ?? (_CardBackgroundDefaultBrushKey = new ThemeResourceKey(Category, "CardBackgroundDefault", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _CardBackgroundDisabledColorKey;
        private static ThemeResourceKey _CardBackgroundDisabledBrushKey;
        public static ThemeResourceKey CardBackgroundDisabledColorKey { get { return _CardBackgroundDisabledColorKey ?? (_CardBackgroundDisabledColorKey = new ThemeResourceKey(Category, "CardBackgroundDisabled", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey CardBackgroundDisabledBrushKey { get { return _CardBackgroundDisabledBrushKey ?? (_CardBackgroundDisabledBrushKey = new ThemeResourceKey(Category, "CardBackgroundDisabled", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _CardBackgroundHoverColorKey;
        private static ThemeResourceKey _CardBackgroundHoverBrushKey;
        public static ThemeResourceKey CardBackgroundHoverColorKey { get { return _CardBackgroundHoverColorKey ?? (_CardBackgroundHoverColorKey = new ThemeResourceKey(Category, "CardBackgroundHover", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey CardBackgroundHoverBrushKey { get { return _CardBackgroundHoverBrushKey ?? (_CardBackgroundHoverBrushKey = new ThemeResourceKey(Category, "CardBackgroundHover", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _CardBackgroundSelectedColorKey;
        private static ThemeResourceKey _CardBackgroundSelectedBrushKey;
        public static ThemeResourceKey CardBackgroundSelectedColorKey { get { return _CardBackgroundSelectedColorKey ?? (_CardBackgroundSelectedColorKey = new ThemeResourceKey(Category, "CardBackgroundSelected", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey CardBackgroundSelectedBrushKey { get { return _CardBackgroundSelectedBrushKey ?? (_CardBackgroundSelectedBrushKey = new ThemeResourceKey(Category, "CardBackgroundSelected", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _CardBorderDefaultColorKey;
        private static ThemeResourceKey _CardBorderDefaultBrushKey;
        public static ThemeResourceKey CardBorderDefaultColorKey { get { return _CardBorderDefaultColorKey ?? (_CardBorderDefaultColorKey = new ThemeResourceKey(Category, "CardBorderDefault", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey CardBorderDefaultBrushKey { get { return _CardBorderDefaultBrushKey ?? (_CardBorderDefaultBrushKey = new ThemeResourceKey(Category, "CardBorderDefault", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _CardBorderDisabledColorKey;
        private static ThemeResourceKey _CardBorderDisabledBrushKey;
        public static ThemeResourceKey CardBorderDisabledColorKey { get { return _CardBorderDisabledColorKey ?? (_CardBorderDisabledColorKey = new ThemeResourceKey(Category, "CardBorderDisabled", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey CardBorderDisabledBrushKey { get { return _CardBorderDisabledBrushKey ?? (_CardBorderDisabledBrushKey = new ThemeResourceKey(Category, "CardBorderDisabled", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _CardBorderHoverColorKey;
        private static ThemeResourceKey _CardBorderHoverBrushKey;
        public static ThemeResourceKey CardBorderHoverColorKey { get { return _CardBorderHoverColorKey ?? (_CardBorderHoverColorKey = new ThemeResourceKey(Category, "CardBorderHover", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey CardBorderHoverBrushKey { get { return _CardBorderHoverBrushKey ?? (_CardBorderHoverBrushKey = new ThemeResourceKey(Category, "CardBorderHover", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _CardBorderSelectedColorKey;
        private static ThemeResourceKey _CardBorderSelectedBrushKey;
        public static ThemeResourceKey CardBorderSelectedColorKey { get { return _CardBorderSelectedColorKey ?? (_CardBorderSelectedColorKey = new ThemeResourceKey(Category, "CardBorderSelected", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey CardBorderSelectedBrushKey { get { return _CardBorderSelectedBrushKey ?? (_CardBorderSelectedBrushKey = new ThemeResourceKey(Category, "CardBorderSelected", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _CardDescriptionTextColorKey;
        private static ThemeResourceKey _CardDescriptionTextBrushKey;
        public static ThemeResourceKey CardDescriptionTextColorKey { get { return _CardDescriptionTextColorKey ?? (_CardDescriptionTextColorKey = new ThemeResourceKey(Category, "CardDescriptionText", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey CardDescriptionTextBrushKey { get { return _CardDescriptionTextBrushKey ?? (_CardDescriptionTextBrushKey = new ThemeResourceKey(Category, "CardDescriptionText", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _CardFooterTextColorKey;
        private static ThemeResourceKey _CardFooterTextBrushKey;
        public static ThemeResourceKey CardFooterTextColorKey { get { return _CardFooterTextColorKey ?? (_CardFooterTextColorKey = new ThemeResourceKey(Category, "CardFooterText", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey CardFooterTextBrushKey { get { return _CardFooterTextBrushKey ?? (_CardFooterTextBrushKey = new ThemeResourceKey(Category, "CardFooterText", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _CardIconColorKey;
        private static ThemeResourceKey _CardIconBrushKey;
        public static ThemeResourceKey CardIconColorKey { get { return _CardIconColorKey ?? (_CardIconColorKey = new ThemeResourceKey(Category, "CardIcon", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey CardIconBrushKey { get { return _CardIconBrushKey ?? (_CardIconBrushKey = new ThemeResourceKey(Category, "CardIcon", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _CardTitleTextColorKey;
        private static ThemeResourceKey _CardTitleTextBrushKey;
        public static ThemeResourceKey CardTitleTextColorKey { get { return _CardTitleTextColorKey ?? (_CardTitleTextColorKey = new ThemeResourceKey(Category, "CardTitleText", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey CardTitleTextBrushKey { get { return _CardTitleTextBrushKey ?? (_CardTitleTextBrushKey = new ThemeResourceKey(Category, "CardTitleText", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _ChangesSummaryDetailFileHeaderColorKey;
        private static ThemeResourceKey _ChangesSummaryDetailFileHeaderBrushKey;
        private static ThemeResourceKey _ChangesSummaryDetailFileHeaderTextColorKey;
        private static ThemeResourceKey _ChangesSummaryDetailFileHeaderTextBrushKey;
        public static ThemeResourceKey ChangesSummaryDetailFileHeaderColorKey { get { return _ChangesSummaryDetailFileHeaderColorKey ?? (_ChangesSummaryDetailFileHeaderColorKey = new ThemeResourceKey(Category, "ChangesSummaryDetailFileHeader", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey ChangesSummaryDetailFileHeaderBrushKey { get { return _ChangesSummaryDetailFileHeaderBrushKey ?? (_ChangesSummaryDetailFileHeaderBrushKey = new ThemeResourceKey(Category, "ChangesSummaryDetailFileHeader", ThemeResourceKeyType.BackgroundBrush)); } }
        public static ThemeResourceKey ChangesSummaryDetailFileHeaderTextColorKey { get { return _ChangesSummaryDetailFileHeaderTextColorKey ?? (_ChangesSummaryDetailFileHeaderTextColorKey = new ThemeResourceKey(Category, "ChangesSummaryDetailFileHeader", ThemeResourceKeyType.ForegroundColor)); } }
        public static ThemeResourceKey ChangesSummaryDetailFileHeaderTextBrushKey { get { return _ChangesSummaryDetailFileHeaderTextBrushKey ?? (_ChangesSummaryDetailFileHeaderTextBrushKey = new ThemeResourceKey(Category, "ChangesSummaryDetailFileHeader", ThemeResourceKeyType.ForegroundBrush)); } }

        private static ThemeResourceKey _DeleteTemplateIconColorKey;
        private static ThemeResourceKey _DeleteTemplateIconBrushKey;
        public static ThemeResourceKey DeleteTemplateIconColorKey { get { return _DeleteTemplateIconColorKey ?? (_DeleteTemplateIconColorKey = new ThemeResourceKey(Category, "DeleteTemplateIcon", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey DeleteTemplateIconBrushKey { get { return _DeleteTemplateIconBrushKey ?? (_DeleteTemplateIconBrushKey = new ThemeResourceKey(Category, "DeleteTemplateIcon", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _GridHeadingBackgroundColorKey;
        private static ThemeResourceKey _GridHeadingBackgroundBrushKey;
        public static ThemeResourceKey GridHeadingBackgroundColorKey { get { return _GridHeadingBackgroundColorKey ?? (_GridHeadingBackgroundColorKey = new ThemeResourceKey(Category, "GridHeadingBackground", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey GridHeadingBackgroundBrushKey { get { return _GridHeadingBackgroundBrushKey ?? (_GridHeadingBackgroundBrushKey = new ThemeResourceKey(Category, "GridHeadingBackground", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _GridHeadingHoverBackgroundColorKey;
        private static ThemeResourceKey _GridHeadingHoverBackgroundBrushKey;
        public static ThemeResourceKey GridHeadingHoverBackgroundColorKey { get { return _GridHeadingHoverBackgroundColorKey ?? (_GridHeadingHoverBackgroundColorKey = new ThemeResourceKey(Category, "GridHeadingHoverBackground", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey GridHeadingHoverBackgroundBrushKey { get { return _GridHeadingHoverBackgroundBrushKey ?? (_GridHeadingHoverBackgroundBrushKey = new ThemeResourceKey(Category, "GridHeadingHoverBackground", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _GridHeadingHoverTextColorKey;
        private static ThemeResourceKey _GridHeadingHoverTextBrushKey;
        public static ThemeResourceKey GridHeadingHoverTextColorKey { get { return _GridHeadingHoverTextColorKey ?? (_GridHeadingHoverTextColorKey = new ThemeResourceKey(Category, "GridHeadingHoverText", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey GridHeadingHoverTextBrushKey { get { return _GridHeadingHoverTextBrushKey ?? (_GridHeadingHoverTextBrushKey = new ThemeResourceKey(Category, "GridHeadingHoverText", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _GridHeadingTextColorKey;
        private static ThemeResourceKey _GridHeadingTextBrushKey;
        public static ThemeResourceKey GridHeadingTextColorKey { get { return _GridHeadingTextColorKey ?? (_GridHeadingTextColorKey = new ThemeResourceKey(Category, "GridHeadingText", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey GridHeadingTextBrushKey { get { return _GridHeadingTextBrushKey ?? (_GridHeadingTextBrushKey = new ThemeResourceKey(Category, "GridHeadingText", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _GridLineColorKey;
        private static ThemeResourceKey _GridLineBrushKey;
        public static ThemeResourceKey GridLineColorKey { get { return _GridLineColorKey ?? (_GridLineColorKey = new ThemeResourceKey(Category, "GridLine", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey GridLineBrushKey { get { return _GridLineBrushKey ?? (_GridLineBrushKey = new ThemeResourceKey(Category, "GridLine", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _HeaderTextColorKey;
        private static ThemeResourceKey _HeaderTextBrushKey;
        public static ThemeResourceKey HeaderTextColorKey { get { return _HeaderTextColorKey ?? (_HeaderTextColorKey = new ThemeResourceKey(Category, "HeaderText", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey HeaderTextBrushKey { get { return _HeaderTextBrushKey ?? (_HeaderTextBrushKey = new ThemeResourceKey(Category, "HeaderText", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _HeaderTextSecondaryColorKey;
        private static ThemeResourceKey _HeaderTextSecondaryBrushKey;
        public static ThemeResourceKey HeaderTextSecondaryColorKey { get { return _HeaderTextSecondaryColorKey ?? (_HeaderTextSecondaryColorKey = new ThemeResourceKey(Category, "HeaderTextSecondary", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey HeaderTextSecondaryBrushKey { get { return _HeaderTextSecondaryBrushKey ?? (_HeaderTextSecondaryBrushKey = new ThemeResourceKey(Category, "HeaderTextSecondary", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _HyperlinkColorKey;
        private static ThemeResourceKey _HyperlinkBrushKey;
        public static ThemeResourceKey HyperlinkColorKey { get { return _HyperlinkColorKey ?? (_HyperlinkColorKey = new ThemeResourceKey(Category, "Hyperlink", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey HyperlinkBrushKey { get { return _HyperlinkBrushKey ?? (_HyperlinkBrushKey = new ThemeResourceKey(Category, "Hyperlink", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _HyperlinkDisabledColorKey;
        private static ThemeResourceKey _HyperlinkDisabledBrushKey;
        public static ThemeResourceKey HyperlinkDisabledColorKey { get { return _HyperlinkDisabledColorKey ?? (_HyperlinkDisabledColorKey = new ThemeResourceKey(Category, "HyperlinkDisabled", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey HyperlinkDisabledBrushKey { get { return _HyperlinkDisabledBrushKey ?? (_HyperlinkDisabledBrushKey = new ThemeResourceKey(Category, "HyperlinkDisabled", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _HyperlinkHoverColorKey;
        private static ThemeResourceKey _HyperlinkHoverBrushKey;
        public static ThemeResourceKey HyperlinkHoverColorKey { get { return _HyperlinkHoverColorKey ?? (_HyperlinkHoverColorKey = new ThemeResourceKey(Category, "HyperlinkHover", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey HyperlinkHoverBrushKey { get { return _HyperlinkHoverBrushKey ?? (_HyperlinkHoverBrushKey = new ThemeResourceKey(Category, "HyperlinkHover", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _ListItemDisabledTextColorKey;
        private static ThemeResourceKey _ListItemDisabledTextBrushKey;
        public static ThemeResourceKey ListItemDisabledTextColorKey { get { return _ListItemDisabledTextColorKey ?? (_ListItemDisabledTextColorKey = new ThemeResourceKey(Category, "ListItemDisabledText", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey ListItemDisabledTextBrushKey { get { return _ListItemDisabledTextBrushKey ?? (_ListItemDisabledTextBrushKey = new ThemeResourceKey(Category, "ListItemDisabledText", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _ListItemMouseOverColorKey;
        private static ThemeResourceKey _ListItemMouseOverBrushKey;
        private static ThemeResourceKey _ListItemMouseOverTextColorKey;
        private static ThemeResourceKey _ListItemMouseOverTextBrushKey;
        public static ThemeResourceKey ListItemMouseOverColorKey { get { return _ListItemMouseOverColorKey ?? (_ListItemMouseOverColorKey = new ThemeResourceKey(Category, "ListItemMouseOver", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey ListItemMouseOverBrushKey { get { return _ListItemMouseOverBrushKey ?? (_ListItemMouseOverBrushKey = new ThemeResourceKey(Category, "ListItemMouseOver", ThemeResourceKeyType.BackgroundBrush)); } }
        public static ThemeResourceKey ListItemMouseOverTextColorKey { get { return _ListItemMouseOverTextColorKey ?? (_ListItemMouseOverTextColorKey = new ThemeResourceKey(Category, "ListItemMouseOver", ThemeResourceKeyType.ForegroundColor)); } }
        public static ThemeResourceKey ListItemMouseOverTextBrushKey { get { return _ListItemMouseOverTextBrushKey ?? (_ListItemMouseOverTextBrushKey = new ThemeResourceKey(Category, "ListItemMouseOver", ThemeResourceKeyType.ForegroundBrush)); } }

        private static ThemeResourceKey _NewItemFileStatusConflictingFileColorKey;
        private static ThemeResourceKey _NewItemFileStatusConflictingFileBrushKey;
        public static ThemeResourceKey NewItemFileStatusConflictingFileColorKey { get { return _NewItemFileStatusConflictingFileColorKey ?? (_NewItemFileStatusConflictingFileColorKey = new ThemeResourceKey(Category, "NewItemFileStatusConflictingFile", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey NewItemFileStatusConflictingFileBrushKey { get { return _NewItemFileStatusConflictingFileBrushKey ?? (_NewItemFileStatusConflictingFileBrushKey = new ThemeResourceKey(Category, "NewItemFileStatusConflictingFile", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _NewItemFileStatusConflictingStylesFileColorKey;
        private static ThemeResourceKey _NewItemFileStatusConflictingStylesFileBrushKey;
        public static ThemeResourceKey NewItemFileStatusConflictingStylesFileColorKey { get { return _NewItemFileStatusConflictingStylesFileColorKey ?? (_NewItemFileStatusConflictingStylesFileColorKey = new ThemeResourceKey(Category, "NewItemFileStatusConflictingStylesFile", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey NewItemFileStatusConflictingStylesFileBrushKey { get { return _NewItemFileStatusConflictingStylesFileBrushKey ?? (_NewItemFileStatusConflictingStylesFileBrushKey = new ThemeResourceKey(Category, "NewItemFileStatusConflictingStylesFile", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _NewItemFileStatusModifiedFileColorKey;
        private static ThemeResourceKey _NewItemFileStatusModifiedFileBrushKey;
        public static ThemeResourceKey NewItemFileStatusModifiedFileColorKey { get { return _NewItemFileStatusModifiedFileColorKey ?? (_NewItemFileStatusModifiedFileColorKey = new ThemeResourceKey(Category, "NewItemFileStatusModifiedFile", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey NewItemFileStatusModifiedFileBrushKey { get { return _NewItemFileStatusModifiedFileBrushKey ?? (_NewItemFileStatusModifiedFileBrushKey = new ThemeResourceKey(Category, "NewItemFileStatusModifiedFile", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _NewItemFileStatusNewFileColorKey;
        private static ThemeResourceKey _NewItemFileStatusNewFileBrushKey;
        public static ThemeResourceKey NewItemFileStatusNewFileColorKey { get { return _NewItemFileStatusNewFileColorKey ?? (_NewItemFileStatusNewFileColorKey = new ThemeResourceKey(Category, "NewItemFileStatusNewFile", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey NewItemFileStatusNewFileBrushKey { get { return _NewItemFileStatusNewFileBrushKey ?? (_NewItemFileStatusNewFileBrushKey = new ThemeResourceKey(Category, "NewItemFileStatusNewFile", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _NewItemFileStatusUnchangedFileColorKey;
        private static ThemeResourceKey _NewItemFileStatusUnchangedFileBrushKey;
        public static ThemeResourceKey NewItemFileStatusUnchangedFileColorKey { get { return _NewItemFileStatusUnchangedFileColorKey ?? (_NewItemFileStatusUnchangedFileColorKey = new ThemeResourceKey(Category, "NewItemFileStatusUnchangedFile", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey NewItemFileStatusUnchangedFileBrushKey { get { return _NewItemFileStatusUnchangedFileBrushKey ?? (_NewItemFileStatusUnchangedFileBrushKey = new ThemeResourceKey(Category, "NewItemFileStatusUnchangedFile", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _NewItemFileStatusWarningFileColorKey;
        private static ThemeResourceKey _NewItemFileStatusWarningFileBrushKey;
        public static ThemeResourceKey NewItemFileStatusWarningFileColorKey { get { return _NewItemFileStatusWarningFileColorKey ?? (_NewItemFileStatusWarningFileColorKey = new ThemeResourceKey(Category, "NewItemFileStatusWarningFile", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey NewItemFileStatusWarningFileBrushKey { get { return _NewItemFileStatusWarningFileBrushKey ?? (_NewItemFileStatusWarningFileBrushKey = new ThemeResourceKey(Category, "NewItemFileStatusWarningFile", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _SavedTemplateBackgroundHoverColorKey;
        private static ThemeResourceKey _SavedTemplateBackgroundHoverBrushKey;
        public static ThemeResourceKey SavedTemplateBackgroundHoverColorKey { get { return _SavedTemplateBackgroundHoverColorKey ?? (_SavedTemplateBackgroundHoverColorKey = new ThemeResourceKey(Category, "SavedTemplateBackgroundHover", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey SavedTemplateBackgroundHoverBrushKey { get { return _SavedTemplateBackgroundHoverBrushKey ?? (_SavedTemplateBackgroundHoverBrushKey = new ThemeResourceKey(Category, "SavedTemplateBackgroundHover", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _SectionDividerColorKey;
        private static ThemeResourceKey _SectionDividerBrushKey;
        public static ThemeResourceKey SectionDividerColorKey { get { return _SectionDividerColorKey ?? (_SectionDividerColorKey = new ThemeResourceKey(Category, "SectionDivider", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey SectionDividerBrushKey { get { return _SectionDividerBrushKey ?? (_SectionDividerBrushKey = new ThemeResourceKey(Category, "SectionDivider", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _SelectedItemActiveColorKey;
        private static ThemeResourceKey _SelectedItemActiveBrushKey;
        private static ThemeResourceKey _SelectedItemActiveTextColorKey;
        private static ThemeResourceKey _SelectedItemActiveTextBrushKey;
        public static ThemeResourceKey SelectedItemActiveColorKey { get { return _SelectedItemActiveColorKey ?? (_SelectedItemActiveColorKey = new ThemeResourceKey(Category, "SelectedItemActive", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey SelectedItemActiveBrushKey { get { return _SelectedItemActiveBrushKey ?? (_SelectedItemActiveBrushKey = new ThemeResourceKey(Category, "SelectedItemActive", ThemeResourceKeyType.BackgroundBrush)); } }
        public static ThemeResourceKey SelectedItemActiveTextColorKey { get { return _SelectedItemActiveTextColorKey ?? (_SelectedItemActiveTextColorKey = new ThemeResourceKey(Category, "SelectedItemActive", ThemeResourceKeyType.ForegroundColor)); } }
        public static ThemeResourceKey SelectedItemActiveTextBrushKey { get { return _SelectedItemActiveTextBrushKey ?? (_SelectedItemActiveTextBrushKey = new ThemeResourceKey(Category, "SelectedItemActive", ThemeResourceKeyType.ForegroundBrush)); } }

        private static ThemeResourceKey _SelectedItemInactiveColorKey;
        private static ThemeResourceKey _SelectedItemInactiveBrushKey;
        private static ThemeResourceKey _SelectedItemInactiveTextColorKey;
        private static ThemeResourceKey _SelectedItemInactiveTextBrushKey;
        public static ThemeResourceKey SelectedItemInactiveColorKey { get { return _SelectedItemInactiveColorKey ?? (_SelectedItemInactiveColorKey = new ThemeResourceKey(Category, "SelectedItemInactive", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey SelectedItemInactiveBrushKey { get { return _SelectedItemInactiveBrushKey ?? (_SelectedItemInactiveBrushKey = new ThemeResourceKey(Category, "SelectedItemInactive", ThemeResourceKeyType.BackgroundBrush)); } }
        public static ThemeResourceKey SelectedItemInactiveTextColorKey { get { return _SelectedItemInactiveTextColorKey ?? (_SelectedItemInactiveTextColorKey = new ThemeResourceKey(Category, "SelectedItemInactive", ThemeResourceKeyType.ForegroundColor)); } }
        public static ThemeResourceKey SelectedItemInactiveTextBrushKey { get { return _SelectedItemInactiveTextBrushKey ?? (_SelectedItemInactiveTextBrushKey = new ThemeResourceKey(Category, "SelectedItemInactive", ThemeResourceKeyType.ForegroundBrush)); } }

        private static ThemeResourceKey _TemplateInfoPageDescriptionColorKey;
        private static ThemeResourceKey _TemplateInfoPageDescriptionBrushKey;
        public static ThemeResourceKey TemplateInfoPageDescriptionColorKey { get { return _TemplateInfoPageDescriptionColorKey ?? (_TemplateInfoPageDescriptionColorKey = new ThemeResourceKey(Category, "TemplateInfoPageDescription", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey TemplateInfoPageDescriptionBrushKey { get { return _TemplateInfoPageDescriptionBrushKey ?? (_TemplateInfoPageDescriptionBrushKey = new ThemeResourceKey(Category, "TemplateInfoPageDescription", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _WindowBorderColorKey;
        private static ThemeResourceKey _WindowBorderBrushKey;
        public static ThemeResourceKey WindowBorderColorKey { get { return _WindowBorderColorKey ?? (_WindowBorderColorKey = new ThemeResourceKey(Category, "WindowBorder", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey WindowBorderBrushKey { get { return _WindowBorderBrushKey ?? (_WindowBorderBrushKey = new ThemeResourceKey(Category, "WindowBorder", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _WindowPanelColorKey;
        private static ThemeResourceKey _WindowPanelBrushKey;
        private static ThemeResourceKey _WindowPanelTextColorKey;
        private static ThemeResourceKey _WindowPanelTextBrushKey;
        public static ThemeResourceKey WindowPanelColorKey { get { return _WindowPanelColorKey ?? (_WindowPanelColorKey = new ThemeResourceKey(Category, "WindowPanel", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey WindowPanelBrushKey { get { return _WindowPanelBrushKey ?? (_WindowPanelBrushKey = new ThemeResourceKey(Category, "WindowPanel", ThemeResourceKeyType.BackgroundBrush)); } }
        public static ThemeResourceKey WindowPanelTextColorKey { get { return _WindowPanelTextColorKey ?? (_WindowPanelTextColorKey = new ThemeResourceKey(Category, "WindowPanel", ThemeResourceKeyType.ForegroundColor)); } }
        public static ThemeResourceKey WindowPanelTextBrushKey { get { return _WindowPanelTextBrushKey ?? (_WindowPanelTextBrushKey = new ThemeResourceKey(Category, "WindowPanel", ThemeResourceKeyType.ForegroundBrush)); } }

        private static ThemeResourceKey _WizardFooterColorKey;
        private static ThemeResourceKey _WizardFooterBrushKey;
        public static ThemeResourceKey WizardFooterColorKey { get { return _WizardFooterColorKey ?? (_WizardFooterColorKey = new ThemeResourceKey(Category, "WizardFooter", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey WizardFooterBrushKey { get { return _WizardFooterBrushKey ?? (_WizardFooterBrushKey = new ThemeResourceKey(Category, "WizardFooter", ThemeResourceKeyType.BackgroundBrush)); } }

        private static ThemeResourceKey _WizardFooterTextColorKey;
        private static ThemeResourceKey _WizardFooterTextBrushKey;
        public static ThemeResourceKey WizardFooterTextColorKey { get { return _WizardFooterTextColorKey ?? (_WizardFooterTextColorKey = new ThemeResourceKey(Category, "WizardFooterText", ThemeResourceKeyType.BackgroundColor)); } }
        public static ThemeResourceKey WizardFooterTextBrushKey { get { return _WizardFooterTextBrushKey ?? (_WizardFooterTextBrushKey = new ThemeResourceKey(Category, "WizardFooterText", ThemeResourceKeyType.BackgroundBrush)); } }
    }
#pragma warning restore SA1306 // Field names must begin with lower-case letter
#pragma warning restore SA1516 // Elements must be separated by blank line
#pragma warning restore SA1502 // Element must not be on a single line
}
