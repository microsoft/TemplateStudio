using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Wizard;
using Microsoft.Templates.Core;
using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.Templates.Wizard.Dialog
{

    public class AddProjectResult
    {
        public ITemplateInfo ProjectTemplate { get; set; }
        public AddFeaturesResult Features { get; set; }

        public AddProjectResult()
        {
            Features = new AddFeaturesResult();
        }
        public string Summary()
        {
            StringBuilder sb = new StringBuilder();
            if (ProjectTemplate != null)
            {
                sb.AppendLine($"Project Formula: {ProjectTemplate.Name}");
                sb.AppendLine();
            }
            if(Features != null)
            {
                sb.AppendLine($"Features Selected:");
                sb.AppendLine(Features.Summary());
            }
            return sb.ToString();
        }
    }
}
