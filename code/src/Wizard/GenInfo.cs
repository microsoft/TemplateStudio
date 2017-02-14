using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;

namespace Microsoft.Templates.Wizard
{
    public class GenInfo
    {
        public string Name { get; set; }
        public ITemplateInfo Template { get; set; }
        public Dictionary<string, string> Parameters { get; } = new Dictionary<string, string>();

        public string GetFramework()
        {
            if (Parameters.ContainsKey(GenParams.Framework))
            {
                return Parameters[GenParams.Framework];
            }
            return String.Empty;
        }

        public string GetUserName()
        {
            if (Parameters.ContainsKey(GenParams.Username))
            {
                return Parameters[GenParams.Username];
            }
            return String.Empty;
        }
    }
}
