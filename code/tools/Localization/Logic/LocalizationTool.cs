// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization
{
    internal class LocalizationTool
    {
        private List<string> cultures = new List<string> { "cs-CZ", "de-DE", "es-ES", "fr-FR", "it-IT", "ja-JP", "ko-KR", "pl-PL", "pt-BR", "ru-RU", "tr-TR", "zh-CN", "zh-TW" };
        public LocalizationTool()
        {
        }

        public void GenerateProjectTemplatesAndCommandsHandler(ToolCommandInfo commandInfo)
        {
            if (commandInfo.Arguments == null || commandInfo.Arguments.Length < 2)
            {
                throw new Exception("Error executing command. Too few arguments.");
            }

            string sourceDirectory = commandInfo.Arguments[0];
            string destinationDirectory = commandInfo.Arguments[1];

            var csProjectTemplateGenerator = new CSharpProjectTemplateGenerator(sourceDirectory, destinationDirectory);
            csProjectTemplateGenerator.GenerateProjectTemplates(cultures);

            var vbProjectTemplateGenerator = new VisualBasicProjectTemplateGenerator(sourceDirectory, destinationDirectory);
            vbProjectTemplateGenerator.GenerateProjectTemplates(cultures);

            var rightClickCommandGenerator = new RightClickCommandGenerator(sourceDirectory, destinationDirectory);
            rightClickCommandGenerator.GenerateRightClickCommands(cultures);
        }

        public void ExtractLocalizableItems(ToolCommandInfo commandInfo)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (commandInfo.Arguments == null || commandInfo.Arguments.Length < 2)
            {
                throw new Exception("Error executing command. Too few arguments.");
            }
            string sourceDirectory = commandInfo.Arguments[0];
            string destinationDirectory = commandInfo.Arguments[1];
            ValidateLocalizableExtractor validator = GetValidateExtractor(sourceDirectory, commandInfo.Arguments);
            LocalizableItemsExtractor extractor = new LocalizableItemsExtractor(sourceDirectory, destinationDirectory, cultures, validator);

            Console.WriteLine("Extract vsix");
            extractor.ExtractVsix();

            Console.WriteLine("Extract project templates");
            extractor.ExtractProjectTemplates();

            Console.WriteLine("Extract command templates");
            extractor.ExtractCommandTemplates();

            Console.WriteLine("Extract template pages and features");
            extractor.ExtractTemplatePagesAndFeatures();

            Console.WriteLine("Extract project types and frameworks");
            extractor.ExtractWtsTemplates();

            Console.WriteLine("Extract resources");
            extractor.ExtractResourceFiles();

            Console.WriteLine("End");
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            Console.WriteLine(string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
        }

        private ValidateLocalizableExtractor GetValidateExtractor(string repository, string[] arguments)
        {
            string extractorType = arguments.Length > 2 ? arguments[2].ToLowerInvariant() : string.Empty;
            string extractorParameter = arguments.Length > 3 ? arguments[3] : string.Empty;

            var extractorMode = ExtractorMode.All;
            switch (extractorType)
            {
                case "c":
                    extractorMode = ExtractorMode.Commit;
                    break;
                case "t":
                    extractorMode = ExtractorMode.TagName;
                    break;
            }

            return new ValidateLocalizableExtractor(repository, extractorMode, extractorParameter);
        }
    }
}
