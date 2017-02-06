using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;

namespace Microsoft.Templates.Wizard
{
    public class GenInfo
    {
        public static string FrameworkPrameterName = "framework";
        public string Name { get; set; }
        public ITemplateInfo Template { get; set; }
        public Dictionary<string, string> Parameters { get; } = new Dictionary<string, string>();
        public string GetFramework()
        {
            if (Parameters.ContainsKey(FrameworkPrameterName))
            {
                return Parameters[FrameworkPrameterName];
            }
            return String.Empty;
        }
    }
}
