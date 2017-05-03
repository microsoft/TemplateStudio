// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Collections.Generic;
using System.Reflection;
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
