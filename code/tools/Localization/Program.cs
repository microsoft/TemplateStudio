// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;

namespace Localization
{
    class Program
    {
        private const string separator = "**********************************************************************";
        private const string argumentNewLine = "\r\n\t\t\t\t   ";

        [STAThread]
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
                commandHandler.SubscribeOnCommand("verify", tool.VerifyLocalizableItems);
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
                Console.WriteLine("VERIFY\tVerify if exist localizable items for different cultures.");
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
                        Console.WriteLine("EXT \"sourceDirectoryPath\" \"destinationDirectoryPath\" [-c \"commitSHA\"] [-t \"tagName\"]");
                        Console.WriteLine();
                        Console.WriteLine($"\tsourceDirectoryPath\t - path to the folder that contains{argumentNewLine}source files for data extraction{argumentNewLine}(it's root project folder).");
                        Console.WriteLine();
                        Console.WriteLine($"\tdestinationDirectoryPath - path to the folder in which will be{argumentNewLine}saved all extracted items.");
                        Console.WriteLine();
                        Console.WriteLine($"\tcommitSHA (optional)\t - commit from where to look {argumentNewLine}for modified files");
                        Console.WriteLine();
                        Console.WriteLine($"\ttagName (optional)\t - indicate the tag name which marks {argumentNewLine}the commit from where to look {argumentNewLine}for modified files");
                        Console.WriteLine();
                        Console.WriteLine("Example:");
                        Console.WriteLine();
                        Console.WriteLine("\tEXT \"C:\\Projects\\wts\" \"C:\\MyFolder\\Extracted\" -c \"f988be4c0878b2b51976e84ce827fce19cf294bf\"");
                        Console.WriteLine("or");
                        Console.WriteLine("\tEXT \"C:\\Projects\\wts\" \"C:\\MyFolder\\Extracted\" -t \"v1.0\"");
                        Console.WriteLine();
                        break;
                    case "GEN":
                        Console.WriteLine("Generates Project Templates for different cultures.");
                        Console.WriteLine();
                        Console.WriteLine("GEN \"sourceDirectoryPath\" \"destinationDirectoryPath\"");
                        Console.WriteLine();
                        Console.WriteLine($"\tsourceDirectoryPath\t - path to the folder that contains{argumentNewLine}source files for Project Templates{argumentNewLine}(it's root project folder).");
                        Console.WriteLine();
                        Console.WriteLine($"\tdestinationDirectoryPath - path to the folder in which will be{argumentNewLine}saved all localized Project{argumentNewLine}Templates (parent for CSharp.UWP.{argumentNewLine}2017.Solution directory).");
                        Console.WriteLine();
                        Console.WriteLine("Example:");
                        Console.WriteLine();
                        Console.WriteLine("\tGEN \"C:\\MyFolder\\wts\" \"C:\\MyFolder\\Generated\\ProjectTemplates\"");
                        Console.WriteLine();
                        break;
                    case "VERIFY":
                        Console.WriteLine("Verify if exist localizable items for different cultures.");
                        Console.WriteLine();
                        Console.WriteLine("VERIFY \"sourceDirectoryPath\"");
                        Console.WriteLine();
                        Console.WriteLine($"\tsourceDirectoryPath\t - path to the folder that contains{argumentNewLine}source files for verify{argumentNewLine}(it's root project folder).");
                        Console.WriteLine();
                        Console.WriteLine("Example:");
                        Console.WriteLine();
                        Console.WriteLine("\tVERIFY \"C:\\MyFolder\\wts\"");
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
