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
    class Program
    {
        private const string separator = "**********************************************************************";
        private const string argumentNewLine = "\r\n\t\t\t\t   ";

        static void Main(string[] args)
        {
            try
            {
                PrintHeader();
                LocalizationTool tool = new LocalizationTool();
                ToolCommandHandler commandHandler = new ToolCommandHandler();
                commandHandler.SubscribeOnCommand("help", PrintHelp);
                commandHandler.SubscribeOnCommand("gen", tool.GenerateProjectTemplatesAndCommandsHandler);
                commandHandler.SubscribeOnCommand("ext", tool.ExtractLocalizableItems);
                commandHandler.Listen();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                Console.WriteLine("Press any key to quit.");
                Console.ReadKey();
            }
        }

        private static void PrintHeader()
        {
            Console.Clear();
            Console.WriteLine(separator);
            Console.WriteLine("** Windows Template Studio Localization Tool");
            Console.WriteLine("** Copyright (c) 2017 Microsoft Corporation");
            Console.WriteLine(separator);
            Console.WriteLine();
        }

        private static void PrintHelp(ToolCommandInfo commandInfo)
        {
            if (commandInfo.Arguments == null || commandInfo.Arguments.Length == 0)
            {
                Console.WriteLine("For more information on a specific command, type HELP command-name");
                Console.WriteLine("EXIT\tQuits Windows Template Studio Localization Tool.");
                Console.WriteLine("EXT\tExtract localizable items for different cultures.");
                Console.WriteLine("GEN\tGenerates Project Templates for different cultures.");
                Console.WriteLine("HELP\tProvides Help information for Windows Template Studio Localization Tool.");
                Console.WriteLine();
            }
            else
            {
                switch (commandInfo.Arguments[0].ToUpper())
                {
                    case "EXIT":
                        Console.WriteLine("Quits Windows Template Studio Localization Tool.");
                        Console.WriteLine();
                        Console.WriteLine("EXIT");
                        Console.WriteLine();
                        break;
                    case "EXT":
                        Console.WriteLine("Extract localizable items for different cultures.");
                        Console.WriteLine();
                        Console.WriteLine("EXT \"sourceDirectoryPath\" \"destinationDirectoryPath\" \"cultures\"");
                        Console.WriteLine();
                        Console.WriteLine($"\tsourceDirectoryPath\t - path to the folder that contains{argumentNewLine}source files for data extraction{argumentNewLine}(it's root project folder).");
                        Console.WriteLine();
                        Console.WriteLine($"\tdestinationDirectoryPath - path to the folder in which will be{argumentNewLine}saved all extracted items.");
                        Console.WriteLine();
                        Console.WriteLine($"\tcultures\t\t - list of cultures, to extract{argumentNewLine}localizable items for. It's case{argumentNewLine}sensitive (en-us != en-US).{argumentNewLine}Or use \"ALL\" to create for all languages.");
                        Console.WriteLine();
                        Console.WriteLine("Example:");
                        Console.WriteLine();
                        Console.WriteLine("\tEXT \"C:\\Projects\\wts\" \"C:\\MyFolder\\Extracted\" \"de-DE;es-ES;fr-FR\"");
                        Console.WriteLine();
                        break;
                    case "GEN":
                        Console.WriteLine("Generates Project Templates for different cultures.");
                        Console.WriteLine();
                        Console.WriteLine("GEN \"sourceDirectoryPath\" \"destinationDirectoryPath\" \"cultures\"");
                        Console.WriteLine();
                        Console.WriteLine($"\tsourceDirectoryPath\t - path to the folder that contains{argumentNewLine}source files for Project Templates{argumentNewLine}(it's root project folder).");
                        Console.WriteLine();
                        Console.WriteLine($"\tdestinationDirectoryPath - path to the folder in which will be{argumentNewLine}saved all localized Project{argumentNewLine}Templates (parent for CSharp.UWP.{argumentNewLine}2017.Solution directory).");
                        Console.WriteLine();
                        Console.WriteLine($"\tcultures\t\t - list of cultures, to generate{argumentNewLine}Project Templates for. It's case{argumentNewLine}sensitive (en-us != en-US).{argumentNewLine}Or use \"ALL\" to create for all languages.");
                        Console.WriteLine();
                        Console.WriteLine("Example:");
                        Console.WriteLine();
                        Console.WriteLine("\tGEN \"C:\\MyFolder\\wts\" \"C:\\MyFolder\\Generated\\ProjectTemplates\" \"de-DE;es-ES;fr-FR\"");
                        Console.WriteLine();
                        break;
                    case "HELP":
                        Console.WriteLine("Provides Help information for Windows Template Studio Localization Tool.");
                        Console.WriteLine();
                        Console.WriteLine("HELP [command]");
                        Console.WriteLine();
                        Console.WriteLine("\tcommand - displays help information on that command.");
                        Console.WriteLine();
                        break;
                    default:
                        Console.WriteLine("Command unknown.");
                        break;
                }
            }
        }

        private static string PrintArray(string[] array)
        {
            if (array == null || array.Length == 0)
            {
                return "\t\tEmpty or Null...";
            }

            StringBuilder writer = new StringBuilder();

            foreach (string item in array)
            {
                writer.AppendLine("\t\t" + item);
            }

            return writer.ToString();
        }
    }
}
