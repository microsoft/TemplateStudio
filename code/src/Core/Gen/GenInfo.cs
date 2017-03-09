using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;

namespace Microsoft.Templates.Core.Gen
{
    public class GenInfo
    {
        public string Name { get; set; }
        public ITemplateInfo Template { get; set; }
        public Dictionary<string, string> Parameters { get; } = new Dictionary<string, string>();

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
