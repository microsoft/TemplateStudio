// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization
{
    internal class LocalizationTool
    {
        public LocalizationTool()
        {
        }

        public void GenerateProjectTemplatesHandler(ToolCommandInfo commandInfo)
        {
            if (commandInfo.Arguments == null || commandInfo.Arguments.Length < 3)
            {
                throw new Exception("Error executing command. Too few arguments.");
            }
            string sourceDirectory = commandInfo.Arguments[0];
            string destinationDirectory = commandInfo.Arguments[1];
            List<string> cultures = new List<string>(commandInfo.Arguments[2].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
            ProjectTemplateGenerator projectTemplateGenerator = new ProjectTemplateGenerator(sourceDirectory, destinationDirectory);
            foreach (string culture in cultures)
            {
                projectTemplateGenerator.GenerateProjectTemplate(culture);
            }
        }

        public void ExtractLocalizableItems(ToolCommandInfo commandInfo)
        {
            if (commandInfo.Arguments == null || commandInfo.Arguments.Length < 3)
            {
                throw new Exception("Error executing command. Too few arguments.");
            }
            string sourceDirectory = commandInfo.Arguments[0];
            string destinationDirectory = commandInfo.Arguments[1];
            List<string> cultures = new List<string>(commandInfo.Arguments[2].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
            LocalizableItemsExtractor extractor = new LocalizableItemsExtractor(sourceDirectory, destinationDirectory);
            extractor.ExtractVsix(cultures);
            foreach (string culture in cultures)
            {
                extractor.ExtractProjectTemplate(culture);
            }
            extractor.ExtractTemplateEngineTemplates(cultures);
            extractor.ExtractWtsTemplates(cultures);
            extractor.ExtractResourceFiles(cultures);
            // TemplateEngine Json
            // TemplateEngine Md
            // Wts Json
            // Wts Md
        }
    }
}
