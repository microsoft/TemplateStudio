// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Templates.Core
{
    public static class ProgrammingLanguages
    {
        public const string CSharp = "C#";

        public const string VisualBasic = "VisualBasic";

        public const string Cpp = "C++";

        public const string Any = "Any";

        public static IEnumerable<string> GetAllLanguages()
        {
            yield return ProgrammingLanguages.CSharp;
            yield return ProgrammingLanguages.Cpp;
            yield return ProgrammingLanguages.VisualBasic;
            yield return ProgrammingLanguages.Any;
        }

        public static string GetShortProgrammingLanguage(string language)
        {
            switch (language)
            {
                case CSharp:
                    return "CS";
                case Cpp:
                    return "Cpp";
                case VisualBasic:
                    return "VB";
                default:
                    return language;
            }
        }

        public static bool IsValidLanguage(string language, string platform)
        {
            bool isValid = false;

            // Validate that the language inputted is valid.
            foreach (string lang in GetAllLanguages())
            {
                if (lang.Equals(language, StringComparison.OrdinalIgnoreCase))
                {
                    isValid = true;
                }
            }

            bool isUwpInvalidLanguage = language != null ? platform.Equals(Platforms.Uwp, StringComparison.OrdinalIgnoreCase)
                                        && language.Equals(Any, StringComparison.OrdinalIgnoreCase)
                                        : true;
            bool isWebInvalidLanguage = language != null ? platform.Equals(Platforms.Web, StringComparison.OrdinalIgnoreCase)
                                        && !language.Equals(Any, StringComparison.OrdinalIgnoreCase)
                                        : true;
            if (isUwpInvalidLanguage || isWebInvalidLanguage)
            {
                // Validity is false if either are true since this is an invalid language + platform combo.
                isValid = false;
            }

            return isValid;
        }
    }
}
