using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;

namespace Microsoft.Templates.Wizard
{
    public class GenInfo
    {
        public static string FrameworkParameterName = "framework";
        public static string UsernameParameterName = "UserName";
        public string Name { get; set; }
        public ITemplateInfo Template { get; set; }
        public Dictionary<string, string> Parameters { get; } = new Dictionary<string, string>();

        public string GetFramework()
        {
            if (Parameters.ContainsKey(FrameworkParameterName))
            {
                return Parameters[FrameworkParameterName];
            }
            return String.Empty;
        }

        public string GetUserName()
        {
            if (Parameters.ContainsKey(UsernameParameterName))
            {
                return Parameters[UsernameParameterName];
            }
            return String.Empty;
        }
    }
}
