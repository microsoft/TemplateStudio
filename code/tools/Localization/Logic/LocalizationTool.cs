// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Localization.Options;

namespace Localization
{
    internal class LocalizationTool
    {
        private List<string> cultures = new List<string> { "cs-CZ", "de-DE", "es-ES", "fr-FR", "it-IT", "ja-JP", "ko-KR", "pl-PL", "pt-BR", "ru-RU", "tr-TR", "zh-CN", "zh-TW" };

        public LocalizationTool()
        {
        }

        public void GenerateProjectTemplatesAndCommandsHandler(GenerationOptions options)
        {
            if (CanOverwriteDirectory(options.DestinationDirectory))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Console.WriteLine("\nGenerate C# project templates.");
                var csProjectTemplateGenerator = new CSharpProjectTemplateGenerator(options.SourceDirectory, options.DestinationDirectory);
                csProjectTemplateGenerator.GenerateProjectTemplates(cultures);

                Console.WriteLine("\nGenerate VB project templates.");
                var vbProjectTemplateGenerator = new VisualBasicProjectTemplateGenerator(options.SourceDirectory, options.DestinationDirectory);
                vbProjectTemplateGenerator.GenerateProjectTemplates(cultures);

                Console.WriteLine("\nGenerate right click commands.");
                var rightClickCommandGenerator = new RightClickCommandGenerator(options.SourceDirectory, options.DestinationDirectory);
                rightClickCommandGenerator.GenerateRightClickCommands(cultures);

                Console.WriteLine("End");
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                Console.WriteLine(string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
            }
        }

        public void ExtractLocalizableItems(ExtractOptions options)
        {
            if (CanOverwriteDirectory(options.DestinationDirectory))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Console.WriteLine("\nGet original localization files");
                var originalExtractPath = GetOriginalLocalizationFiles(options);

                Console.WriteLine("\nGet actual localization files");
                var actualExtractPath = GetActualLocalizationFiles(options);

                var sourceDir = options.ActualSourceDirectory;
                var destDir = Path.Combine(options.DestinationDirectory, Routes.DiffExtractDirectory);

                var validator = new ValidateLocalizableExtractor(originalExtractPath, actualExtractPath);
                var extractor = new LocalizableItemsExtractor(sourceDir, destDir, validator, cultures);

                Console.WriteLine("\nExtract vsix");
                extractor.ExtractVsix();

                Console.WriteLine("Extract project templates");
                extractor.ExtractProjectTemplates();

                Console.WriteLine("Extract command templates");
                extractor.ExtractCommandTemplates();

                Console.WriteLine("Extract template pages");
                extractor.ExtractTemplatePages();

                Console.WriteLine("Extract template features");
                extractor.ExtractTemplateFeatures();

                Console.WriteLine("Extract project types");
                extractor.ExtractWtsProjectTypes();

                Console.WriteLine("Extract project frameworks");
                extractor.ExtractWtsFrameworks();

                Console.WriteLine("Extract resources");
                extractor.ExtractResourceFiles();

                Console.WriteLine("End");
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                Console.WriteLine(string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
            }
        }

        public bool VerifyLocalizableItems(VerifyOptions options)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var verificator = new LocalizableItemsVerificator(options.SourceDirectory, cultures);
            bool result = verificator.VerificateAllFiles();

            Console.WriteLine("End");
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            Console.WriteLine(string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));

            return result;
        }

        public void MergeLocalizableItems(MergeOptions options)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

           // merge files
            Console.WriteLine("End");
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            Console.WriteLine(string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
        }

        private bool CanOverwriteDirectory(string destDirectory)
        {
            if (!Directory.Exists(destDirectory) || !Directory.EnumerateFileSystemEntries(destDirectory).Any())
            {
                return true;
            }

            Console.WriteLine("\nTarget directory is not empty. Existing files will be overwritten. Continue? (Y/N)");
            return Console.ReadLine().ToUpperInvariant() == "Y";
        }

        private string GetOriginalLocalizationFiles(ExtractOptions options)
        {
            var extractPath = Path.Combine(options.DestinationDirectory, Routes.OriginalExtractDirectory);
            var extractor = new OriginalLocalizableItemsExtractor(options.OriginalSourceDirectory, extractPath);
            extractor.Extract();

            return extractPath;
        }

        private string GetActualLocalizationFiles(ExtractOptions options)
        {
            var extractPath = Path.Combine(options.DestinationDirectory, Routes.ActualExtractDirectory);
            var extractor = new OriginalLocalizableItemsExtractor(options.ActualSourceDirectory, extractPath);
            extractor.Extract();

            return extractPath;
        }
    }
}
