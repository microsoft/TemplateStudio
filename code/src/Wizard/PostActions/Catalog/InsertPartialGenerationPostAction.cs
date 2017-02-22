using Microsoft.TemplateEngine.Edge.Template;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Templates.Wizard.Resources;
using System.Configuration;

namespace Microsoft.Templates.Wizard.PostActions.Catalog
{
    public class InsertPartialGenerationPostAction : PostActionBase
    {

        public InsertPartialGenerationPostAction(string name, string description, IReadOnlyDictionary<string, string> parameters)
            : base(name, description, parameters)
        {
        }

        public override PostActionResult Execute(string outputPath, GenInfo genInfo, TemplateCreationResult result, GenShell shell)
        {
            //Read values from params
            var destinationFileName = GetValueFromParameter("destination.filename");
            var destinationAnchor = GetValueFromParameter("destination.anchor");

            if (string.IsNullOrEmpty(destinationFileName))
            {
                throw new ConfigurationErrorsException(PostActionResources.InsertPartialGeneration_EmptyDestinationFileNamePattern.UseParams(Name));
            }

            if (string.IsNullOrEmpty(destinationAnchor))
            {
                throw new ConfigurationErrorsException(PostActionResources.InsertPartialGeneration_EmptyDestinationAnchorPattern.UseParams(Name));
            }

            //TODO: REVIEW WITH SIBILLE
            var sourceFile = Directory.EnumerateFiles(outputPath, Name + ".txt", SearchOption.AllDirectories).FirstOrDefault();
            if (string.IsNullOrEmpty(sourceFile))
            {
                return new PostActionResult
                {
                    ResultCode = ResultCode.NotExecuted
                };
            }

            var postActionCodeLines = ReadPostActionCode(sourceFile);
            var destinationFiles = Directory.EnumerateFiles(Path.GetDirectoryName(sourceFile), destinationFileName, SearchOption.TopDirectoryOnly);

            foreach (var destinationFile in destinationFiles)
            {
                //var destinationFile = Path.Combine(outputPath, destinationFileName);
                var destinationFileContent = ReadDestinationFileContent(destinationFile);

                var anchorIndex = destinationFileContent.IndexOf(destinationAnchor, StringComparison.Ordinal);
                if (anchorIndex == -1)
                {
                    //TODO: REVIEW THIS
                    //var postActionCode = postActionCodeLines.Aggregate(new StringBuilder(), (sb, a) => sb.AppendLine(a), sb => sb.ToString());

                    //return new PostActionResult()
                    //{
                    //    ResultCode = ResultCode.AnchorNotFound,
                    //    Message = PostActionResources.InsertPartialGeneration_AnchorNotFoundPattern.UseParams(Name, postActionCode, destinationAnchor, destinationFile)
                    //};
                }
                else
                {
                    string formattedCode = FormatPostActionCode(postActionCodeLines, destinationFileContent, anchorIndex);

                    if (destinationFileContent.Contains(formattedCode))
                    {
                        //Delete source file
                        File.Delete(sourceFile);

                        return new PostActionResult()
                        {
                            ResultCode = ResultCode.Success
                        };
                    }

                    var nextLineBreakAfterAnchor = destinationFileContent.IndexOf(Environment.NewLine, anchorIndex, StringComparison.Ordinal);
                    destinationFileContent = destinationFileContent.Insert(nextLineBreakAfterAnchor, formattedCode);

                    //Save
                    File.WriteAllText(destinationFile, destinationFileContent);
                }
            }

            //Delete source file
            File.Delete(sourceFile);

            return new PostActionResult()
            {
                //TODO: REVIEW THIS
                ResultCode = ResultCode.Success,
                //Message = PostActionResources.InsertPartialGeneration_SuccessPattern.UseParams(Name, formattedCode, destinationAnchor, destinationFile)
            };
        }

        public static bool CleanUpAnchors(ref string fileContent)
        {
            string[] anchorTexts = { "//PostActionAnchor", "<!--PostActionAnchor", "<!-- PostActionAnchor" };
            var modified = false;

            foreach (var anchorText in anchorTexts)
            {
                var anchorIndex = 0;
                //Search the whole file, until nothing else is found
                while (anchorIndex != -1)
                {
                    anchorIndex = fileContent.IndexOf(anchorText, StringComparison.Ordinal);
                    if (anchorIndex != -1)
                    {
                        var nextLineBreakAfterAnchor = fileContent.IndexOf(Environment.NewLine, anchorIndex, StringComparison.Ordinal);

                        fileContent = fileContent.Remove(anchorIndex, nextLineBreakAfterAnchor - anchorIndex);
                        modified = true;
                    }
                }
            }

            return modified;
        }

        private static string ReadDestinationFileContent(string destinationFile)
        {
            if (File.Exists(destinationFile))
            {
                //Read destination file, find anchor
                return File.ReadAllText(destinationFile);

            }
            else
            {
                return string.Empty;
            }
        }

        private static string[] ReadPostActionCode(string sourceFile)
        {
            if (!File.Exists(sourceFile))
            {
                return null;
                //throw new ConfigurationErrorsException(PostActionResources.InsertPartialGeneration_FileNotFoundPattern.UseParams(sourceFile));
            }

            //Read Code to insert
            var postActionCodeLines = File.ReadAllLines(sourceFile);
            return postActionCodeLines;
        }


        private static string FormatPostActionCode(string[] postActionCodeLines, string destinationFileContent, int anchorIndex)
        {
            var lastLineBreakBeforeAnchor = destinationFileContent.LastIndexOf(Environment.NewLine, anchorIndex, StringComparison.Ordinal);
            var leadingChars = destinationFileContent.Skip(lastLineBreakBeforeAnchor + Environment.NewLine.Length).TakeWhile(char.IsWhiteSpace).ToList();

            var leadingTabs = leadingChars.Count(c => c == '\t');
            var leadingWhiteSpaces = leadingChars.Count(c => c != '\t');

            //Add linebreaks and leading whitespaces/tabs to code
            var formattedCode = Environment.NewLine;
            foreach (var line in postActionCodeLines)
            {
                var formattedCodeLine = line.PadLeft(line.Length + leadingWhiteSpaces);
                formattedCodeLine = string.Concat(new string('\t', leadingTabs), formattedCodeLine);
                formattedCode = string.Concat(formattedCode, formattedCodeLine, Environment.NewLine);
            }

            return formattedCode;
        }
    }
}
