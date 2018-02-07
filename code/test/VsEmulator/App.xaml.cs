// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;

namespace Microsoft.Templates.VsEmulator
{
    public partial class App : Application
    {
        public App()
        {
        }

        public static void LoadDarkTheme()
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            MergeDictionary("VsTheme.xaml");
            MergeDictionary("Dark_CommonControls.xaml");
            MergeDictionary("Dark_CommonDocument.xaml");
            MergeDictionary("Dark_Environment.xaml");
            MergeDictionary("Dark_InfoBar.xaml");
            MergeDictionary("Dark_ThemedDialog.xaml");
            MergeDictionary("Dark_WindowsTemplateStudio.xaml");
        }

        public static void LoadLightTheme()
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            MergeDictionary("VsTheme.xaml");
            MergeDictionary("Light_CommonControl.xaml");
            MergeDictionary("Light_CommonDocument.xaml");
            MergeDictionary("Light_Environment.xaml");
            MergeDictionary("Light_InfoBar.xaml");
            MergeDictionary("Light_ThemedDialog.xaml");
            MergeDictionary("Light_WindowsTemplateStudio.xaml");
        }

        private static void MergeDictionary(string dictionaryName)
        {
            var rd = new ResourceDictionary();
            var stringUrl = $"/VsEmulator;component/Styles/{dictionaryName}";
            rd.Source = new Uri(stringUrl, UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(rd);
        }
    }
}
