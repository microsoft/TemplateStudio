// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Gen
{
    public static class UserSelectionContextExtensions
    {
        private const string AppModelKey = "appmodel";

        public static void AddAppModel(this UserSelectionContext context, string appModel)
            => context.AddProperty(AppModelKey, appModel);

        public static string GetAppModel(this UserSelectionContext context)
            => context.GetProperty(AppModelKey);

        private static void AddProperty(this UserSelectionContext context, string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (context.PropertyBag.ContainsKey(key))
            {
                context.PropertyBag.Remove(key);
            }

            context.PropertyBag.Add(key, value);
        }

        private static string GetProperty(this UserSelectionContext context, string key)
        {
            if (context.PropertyBag.TryGetValue(key, out var value))
            {
                return value;
            }

            return string.Empty;
        }
    }
}
