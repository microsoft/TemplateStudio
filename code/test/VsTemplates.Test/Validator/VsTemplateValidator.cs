using System.Linq;
using System.Xml;
using VsTemplates.Test.Models;

namespace VsTemplates.Test.Validator
{
    public class VsTemplateValidator
    {
        public static VerifierResultTestModel VerifyTemplateId(string filePath)
        {
            var success = false;
            string message = "TemplateID not defined or defined more than once";

            var xml = new XmlDocument();
            xml.Load(filePath);
            
            var templateIdNodes = xml.GetElementsByTagName("TemplateID");
            if (templateIdNodes.Count == 1)
            {
                var templateId = templateIdNodes.Item(0)?.InnerText;
                if (!string.IsNullOrEmpty(templateId))
                {
                    success = templateId.EndsWith("WTS.local");
                    message = success ? string.Empty : "TemplateID not ends with WTS.local";
                }
                else
                {
                    message = "TemplateID not found";
                }
            }

            return new VerifierResultTestModel(success, message);
        }

        public static VerifierResultTestModel VerifyTemplateName(string filePath)
        {
            var success = false;
            string message = "Name not defined or defined more than once";

            var xml = new XmlDocument();
            xml.Load(filePath);

            var templateIdNodes = xml.GetElementsByTagName("Name");
            if (templateIdNodes.Count == 1)
            {
                var templateId = templateIdNodes.Item(0)?.InnerText;
                if (!string.IsNullOrEmpty(templateId))
                {
                    success = templateId.EndsWith("; local)") || templateId.EndsWith("; local) PREVIEW");
                    message = success ? string.Empty : "Template Name not ends with ; local)";
                }
                else
                {
                    message = "Template Name not found";
                }
            }

            return new VerifierResultTestModel(success, message);
        }

        public static VerifierResultTestModel VerifyProjectTemplatesIncludeWtsTag(string filePath)
        {
            var success = false;
            string message = "ProjectTypeTag not defined";

            var xml = new XmlDocument();
            xml.Load(filePath);

            var templateIdNodes = xml.GetElementsByTagName("ProjectTypeTag");

            if (templateIdNodes.Count > 0)
            {
                if (templateIdNodes.Cast<XmlNode>().Any(n => n.InnerText == "Windows Template Studio"))
                {
                    success = true;
                }
                else
                {
                    message = "ProjectTypeTag does not incude -Windows Template Studio-";
                }
            }
            
            return new VerifierResultTestModel(success, message);
        }
    }
}
