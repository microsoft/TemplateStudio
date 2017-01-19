using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Templates.Wizard.Dialog
{
    public class AddFeaturesResult
    {
        public List<ITemplateInfo> Developer { get; set; }
        public List<ITemplateInfo> Customer { get; set; }

        public AddFeaturesResult()
        {
            Developer = new List<ITemplateInfo>();
            Customer = new List<ITemplateInfo>();
        }

        internal string Summary()
        {
            StringBuilder sb = new StringBuilder();
            if (Developer != null & Developer.Count > 0)
            {
                sb.AppendLine($"* Developer * ");
                foreach (var devFeature in Developer)
                {
                    sb.AppendLine($"\t{devFeature.Name}");
                }
                sb.AppendLine();
            }
            if (Customer != null & Customer.Count > 0)
            {
                sb.AppendLine($"* Customer * ");
                foreach (var custFeature in Customer)
                {
                    sb.AppendLine($"\t{custFeature.Name}");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}