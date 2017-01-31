using Microsoft.TemplateEngine.Edge.Template;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Templates.Wizard.Resources;

namespace Microsoft.Templates.Wizard.PostActions
{
    public class InsertPartialGenerationPostAction : PostActionBase
	{

		public InsertPartialGenerationPostAction(string name, string description,  IReadOnlyDictionary<string, string> parameters)
			: base(name, description, parameters)
		{
		}

		public override PostActionResult Execute(GenInfo genInfo, TemplateCreationResult result, GenShell shell)
		{
			try
            {
                //Read values from params
                var destinationFileName = GetValueFromParameter("destination.filename");
                var destinationAnchor = GetValueFromParameter("destination.anchor");

                if (string.IsNullOrEmpty(destinationFileName)) return new PostActionResult() { ResultCode = ResultCode.ConfigurationError, Message = PostActionResources.InsertPartialGeneration_EmptyDestinationFileNamePattern.UseParams(Name) };
                if (string.IsNullOrEmpty(destinationAnchor)) return new PostActionResult() { ResultCode = ResultCode.ConfigurationError, Message = PostActionResources.InsertPartialGeneration_EmptyDestinationAnchorPattern.UseParams(Name) };

                var sourceFile = Path.Combine(shell.OutputPath, genInfo.Name, Name + ".txt");

                var pageNamespace = GetValueFromGenParameter(genInfo.Parameters, "PageNamespace");
                string projectPath = GetProjectPathFromNamespace(shell, pageNamespace);

                var destinationFile = Path.Combine(projectPath, destinationFileName);

                if (!File.Exists(sourceFile)) return new PostActionResult() { ResultCode = ResultCode.ContextError, Message = PostActionResources.InsertPartialGeneration_FileNotFoundPattern.UseParams(sourceFile) };
                if (!File.Exists(destinationFile)) return new PostActionResult() { ResultCode = ResultCode.ContextError, Message = PostActionResources.InsertPartialGeneration_FileNotFoundPattern.UseParams(destinationFile) };

                //Read Code to insert
                var postActionCodeLines = File.ReadAllLines(sourceFile);

                //Read destination file, find anchor
                var destinationFileContent = File.ReadAllText(destinationFile);
                var anchorIndex = destinationFileContent.IndexOf(destinationAnchor, StringComparison.Ordinal);

                if (anchorIndex == -1)
                {
                    var postActionCode = postActionCodeLines.Aggregate(new StringBuilder(), (sb, a) => sb.AppendLine(a), sb => sb.ToString());

                    return new PostActionResult()
                    {
                        ResultCode = ResultCode.AnchorNotFound,
                        Message = PostActionResources.InsertPartialGeneration_AnchorNotFoundPattern.UseParams(Name, postActionCode, destinationAnchor, destinationFile)
                    };
                }

                string formattedCode = FormatPostActionCode(postActionCodeLines, destinationFileContent, anchorIndex);

                var nextLineBreakAfterAnchor = destinationFileContent.IndexOf(Environment.NewLine, anchorIndex, StringComparison.Ordinal);
                destinationFileContent = destinationFileContent.Insert(nextLineBreakAfterAnchor, formattedCode);

                //Save
                File.WriteAllText(destinationFile, destinationFileContent);
                File.Delete(sourceFile);

                return new PostActionResult()
                {
                    ResultCode = ResultCode.Success,
                    Message = PostActionResources.InsertPartialGeneration_SuccessPattern.UseParams(Name, formattedCode, destinationAnchor, destinationFile)
                };
            }
            catch (Exception ex)
			{
				return new PostActionResult()
				{
					ResultCode = ResultCode.Error,
					Message = PostActionResources.InsertPartialGeneration_ErrorPattern.UseParams(Name),
					Exception = ex
				};
			}
		}

        private static string GetProjectPathFromNamespace(GenShell shell, string pageNamespace)
        {
            var projectPath = shell.OutputPath;
            foreach (char c in pageNamespace.Where(c => c == '.'))
            {
                projectPath = Directory.GetParent(projectPath).FullName;
            }

            return projectPath;
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
