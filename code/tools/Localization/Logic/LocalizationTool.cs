// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        public void GenerateProjectTemplatesAndCommandsHandler(ToolCommandInfo commandInfo)
        {
            if (commandInfo.Arguments == null || commandInfo.Arguments.Length < 3)
            {
                throw new Exception("Error executing command. Too few arguments.");
            }

            string sourceDirectory = commandInfo.Arguments[0];
            string destinationDirectory = commandInfo.Arguments[1];

            List<string> cultures;

            cultures = commandInfo.Arguments[2].ToUpperInvariant() == "ALL"
                     ? new List<string> { "cs-CZ", "de-DE", "es-ES", "fr-FR", "it-IT", "ja-JP", "ko-KR", "pl-PL", "pt-BR", "ru-RU", "tr-TR", "zh-CN", "zh-TW" }
                     : new List<string>(commandInfo.Arguments[2].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries));

            var csProjectTemplateGenerator = new CSharpProjectTemplateGenerator(sourceDirectory, destinationDirectory);
            csProjectTemplateGenerator.GenerateProjectTemplates(cultures);

            var vbProjectTemplateGenerator = new VisualBasicProjectTemplateGenerator(sourceDirectory, destinationDirectory);
            vbProjectTemplateGenerator.GenerateProjectTemplates(cultures);

            var rightClickCommandGenerator = new RightClickCommandGenerator(sourceDirectory, destinationDirectory);
            rightClickCommandGenerator.GenerateRightClickCommands(cultures);
        }

        public void ExtractLocalizableItems(ToolCommandInfo commandInfo)
        {
            if (commandInfo.Arguments == null || commandInfo.Arguments.Length < 3)
            {
                throw new Exception("Error executing command. Too few arguments.");
            }

            string sourceDirectory = commandInfo.Arguments[0];
            string destinationDirectory = commandInfo.Arguments[1];
            List<string> cultures = new List<string>(commandInfo.Arguments[2].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
            string tagName = commandInfo.Arguments.Length > 3 ? commandInfo.Arguments[3] : string.Empty;
            LocalizableItemsExtractor extractor = new LocalizableItemsExtractor(sourceDirectory, destinationDirectory, cultures, tagName);
            extractor.ExtractVsix();
            extractor.ExtractProjectTemplates();
            extractor.ExtractCommandTemplates();
            extractor.ExtractTemplatePagesAndFeatures();
            extractor.ExtractWtsTemplates();
            extractor.ExtractResourceFiles();
        }
    }
}
