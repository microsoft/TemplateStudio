using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Templates.UI
{
    public static class DependencyObjectExtensions
    {
        public static IEnumerable<T> ChildrenOfType<T>(this DependencyObject dObject, bool includeInheritance = false) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(dObject);

            for (int i = 0; i < count; i++)
            {
                var current = VisualTreeHelper.GetChild(dObject, i);

                if ((current.GetType()).Equals(typeof(T)) || (includeInheritance && current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
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
