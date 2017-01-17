using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public static class ITemplateInfoExtensions
    {
        public static TemplateType GetTemplateType(this ITemplateInfo ti)
        {
            string type;
            if (ti.Tags != null && ti.Tags.TryGetValue("type", out type))
            {
                if (type.Equals("project", StringComparison.OrdinalIgnoreCase))
                {
                    return TemplateType.Project;
                }
                else if (type.Equals("item", StringComparison.OrdinalIgnoreCase))
                {
                    return TemplateType.Page;
                }
            }
            return TemplateType.Unspecified; 
        }
    }
}
