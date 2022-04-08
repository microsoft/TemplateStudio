// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Casing
{
    public class TextCasing
    {
        public string Key { get; set; }

        public CasingType Type { get; set; }

#pragma warning disable CA1308 // Normalize strings to uppercase
        public string ParameterName => $"ts.{Key}.casing.{Type.ToString().ToLowerInvariant()}";
#pragma warning restore CA1308 // Normalize strings to uppercase

        public string Transform(string value)
        {
            switch (Type)
            {
                case CasingType.Camel:
                    return value.ToCamelCase();
                case CasingType.Kebab:
                    return value.ToKebabCase();
                case CasingType.Snake:
                    return value.ToSnakeCase();
                case CasingType.Pascal:
                    return value.ToPascalCase();
                case CasingType.Lower:
                    return value.ToLowerCase();
                default:
                    return value;
            }
        }
    }
}
