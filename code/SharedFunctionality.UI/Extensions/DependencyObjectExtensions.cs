// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI
{
    public static class DependencyObjectExtensions
    {
        public static IEnumerable<T> ChildrenOfType<T>(this DependencyObject dObject, bool includeInheritance = false)
            where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(dObject);

            for (int i = 0; i < count; i++)
            {
                var current = VisualTreeHelper.GetChild(dObject, i);

                if (current.GetType().Equals(typeof(T)) || (includeInheritance && current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
                {
                    yield return current as T;
                }

                foreach (var children in current.ChildrenOfType<T>(includeInheritance))
                {
                    yield return children;
                }
            }
        }
    }
}
