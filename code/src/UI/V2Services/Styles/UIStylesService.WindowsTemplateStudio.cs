// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI.V2Services
{
    public partial class UIStylesService
    {
        public static readonly DependencyProperty NotificationInformationTextProperty = DependencyProperty.Register("NotificationInformationText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NotificationInformationText
        {
            get { return (Brush)GetValue(NotificationInformationTextProperty); }
            set { SetValue(NotificationInformationTextProperty, value); }
        }

        public static readonly DependencyProperty NotificationInformationBackgroundProperty = DependencyProperty.Register("NotificationInformationBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NotificationInformationBackground
        {
            get { return (Brush)GetValue(NotificationInformationBackgroundProperty); }
            set { SetValue(NotificationInformationBackgroundProperty, value); }
        }

        public static readonly DependencyProperty NotificationInformationIconProperty = DependencyProperty.Register("NotificationInformationIcon", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NotificationInformationIcon
        {
            get { return (Brush)GetValue(NotificationInformationIconProperty); }
            set { SetValue(NotificationInformationIconProperty, value); }
        }

        public static readonly DependencyProperty NotificationWarningTextProperty = DependencyProperty.Register("NotificationWarningText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NotificationWarningText
        {
            get { return (Brush)GetValue(NotificationWarningTextProperty); }
            set { SetValue(NotificationWarningTextProperty, value); }
        }

        public static readonly DependencyProperty NotificationWarningBackgroundProperty = DependencyProperty.Register("NotificationWarningBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NotificationWarningBackground
        {
            get { return (Brush)GetValue(NotificationWarningBackgroundProperty); }
            set { SetValue(NotificationWarningBackgroundProperty, value); }
        }

        public static readonly DependencyProperty NotificationWarningIconProperty = DependencyProperty.Register("NotificationWarningIcon", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NotificationWarningIcon
        {
            get { return (Brush)GetValue(NotificationWarningIconProperty); }
            set { SetValue(NotificationWarningIconProperty, value); }
        }

        public static readonly DependencyProperty NotificationErrorTextProperty = DependencyProperty.Register("NotificationErrorText", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NotificationErrorText
        {
            get { return (Brush)GetValue(NotificationErrorTextProperty); }
            set { SetValue(NotificationErrorTextProperty, value); }
        }

        public static readonly DependencyProperty NotificationErrorBackgroundProperty = DependencyProperty.Register("NotificationErrorBackground", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NotificationErrorBackground
        {
            get { return (Brush)GetValue(NotificationErrorBackgroundProperty); }
            set { SetValue(NotificationErrorBackgroundProperty, value); }
        }

        public static readonly DependencyProperty NotificationErrorIconProperty = DependencyProperty.Register("NotificationErrorIcon", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NotificationErrorIcon
        {
            get { return (Brush)GetValue(NotificationErrorIconProperty); }
            set { SetValue(NotificationErrorIconProperty, value); }
        }

        public static readonly DependencyProperty DeleteTemplateIconProperty = DependencyProperty.Register("DeleteTemplateIcon", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush DeleteTemplateIcon
        {
            get { return (Brush)GetValue(DeleteTemplateIconProperty); }
            set { SetValue(DeleteTemplateIconProperty, value); }
        }

        public static readonly DependencyProperty SavedTemplateBackgroundHoverProperty = DependencyProperty.Register("SavedTemplateBackgroundHover", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush SavedTemplateBackgroundHover
        {
            get { return (Brush)GetValue(SavedTemplateBackgroundHoverProperty); }
            set { SetValue(SavedTemplateBackgroundHoverProperty, value); }
        }

        public static readonly DependencyProperty NewItemFileStatusNewFileProperty = DependencyProperty.Register("NewItemFileStatusNewFile", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NewItemFileStatusNewFile
        {
            get { return (Brush)GetValue(NewItemFileStatusNewFileProperty); }
            set { SetValue(NewItemFileStatusNewFileProperty, value); }
        }

        public static readonly DependencyProperty NewItemFileStatusModifiedFileProperty = DependencyProperty.Register("NewItemFileStatusModifiedFile", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NewItemFileStatusModifiedFile
        {
            get { return (Brush)GetValue(NewItemFileStatusModifiedFileProperty); }
            set { SetValue(NewItemFileStatusModifiedFileProperty, value); }
        }

        public static readonly DependencyProperty NewItemFileStatusConflictingFileProperty = DependencyProperty.Register("NewItemFileStatusConflictingFile", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NewItemFileStatusConflictingFile
        {
            get { return (Brush)GetValue(NewItemFileStatusConflictingFileProperty); }
            set { SetValue(NewItemFileStatusConflictingFileProperty, value); }
        }

        public static readonly DependencyProperty NewItemFileStatusConflictingStylesFileProperty = DependencyProperty.Register("NewItemFileStatusConflictingStylesFile", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NewItemFileStatusConflictingStylesFile
        {
            get { return (Brush)GetValue(NewItemFileStatusConflictingStylesFileProperty); }
            set { SetValue(NewItemFileStatusConflictingStylesFileProperty, value); }
        }

        public static readonly DependencyProperty NewItemFileStatusWarningFileProperty = DependencyProperty.Register("NewItemFileStatusWarningFile", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NewItemFileStatusWarningFile
        {
            get { return (Brush)GetValue(NewItemFileStatusWarningFileProperty); }
            set { SetValue(NewItemFileStatusWarningFileProperty, value); }
        }

        public static readonly DependencyProperty NewItemFileStatusUnchangedFileProperty = DependencyProperty.Register("NewItemFileStatusUnchangedFile", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush NewItemFileStatusUnchangedFile
        {
            get { return (Brush)GetValue(NewItemFileStatusUnchangedFileProperty); }
            set { SetValue(NewItemFileStatusUnchangedFileProperty, value); }
        }

        public static readonly DependencyProperty DialogInfoIconProperty = DependencyProperty.Register("DialogInfoIcon", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush DialogInfoIcon
        {
            get { return (Brush)GetValue(DialogInfoIconProperty); }
            set { SetValue(DialogInfoIconProperty, value); }
        }

        public static readonly DependencyProperty DialogErrorIconProperty = DependencyProperty.Register("DialogErrorIcon", typeof(Brush), typeof(UIStylesService), new PropertyMetadata(null));

        public Brush DialogErrorIcon
        {
            get { return (Brush)GetValue(DialogErrorIconProperty); }
            set { SetValue(DialogErrorIconProperty, value); }
        }
    }
}
