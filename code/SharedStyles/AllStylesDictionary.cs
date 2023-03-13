// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.UI.Styles
{
    public static class AllStylesDictionary
    {
        // This allows the use of the VSEmulator with needing a specific config option
        public static bool UseEmulator { get; set; } = false;

        public static ResourceDictionary GetMergeDictionary()
        {
            ResourceDictionary result = new ResourceDictionary();

            var lib = UseEmulator ? "VSEmulator" : TemplateStudioProject.AssemblyName;

            result.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"/{lib};component/Styles/TemplateStudioStyles.xaml", UriKind.RelativeOrAbsolute), });

            return result;
        }
    }
}
