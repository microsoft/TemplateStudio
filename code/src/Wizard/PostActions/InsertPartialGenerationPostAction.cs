using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Templates.Wizard.PostActions
{
	public class InsertPartialGenerationPostAction : PostActionBase
	{

		public InsertPartialGenerationPostAction(string name, string description,  IReadOnlyDictionary<string, string> parameters)
			: base(name, description, parameters)
		{
		}

		public override PostActionResult Execute(ExecutionContext context)
		{
			try
			{
				//Read values from params
				var sourceFileName = GetValueFromParameter("Source.FileName");
				var destinationFileName = GetValueFromParameter("Destination.FileName");
				var destinationAnchor = GetValueFromParameter("Destination.Anchor");

				if (string.IsNullOrEmpty(sourceFileName)) return new PostActionResult() { ResultCode = ResultCode.ConfigurationError, Message=$"{sourceFileName} should not be empty"};
				if (string.IsNullOrEmpty(destinationFileName)) return new PostActionResult() { ResultCode = ResultCode.ConfigurationError, Message = $"{destinationFileName} should not be empty" };
				if (string.IsNullOrEmpty(destinationAnchor)) return new PostActionResult() { ResultCode = ResultCode.ConfigurationError, Message = $"{destinationAnchor} should not be empty" };

				var sourceFile = Path.Combine(context.PagePath, sourceFileName);
				var destinationFile = Path.Combine(context.ProjectPath, destinationFileName);

				if (!File.Exists(sourceFile)) return new PostActionResult() { ResultCode = ResultCode.ContextError, Message = $"{sourceFile} was not found" };
				if (!File.Exists(destinationFile)) return new PostActionResult() { ResultCode = ResultCode.ContextError, Message = $"{destinationFile} was not found" };

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
						Message = $"Could not inserted {postActionCode} on anchor {destinationAnchor} in file {destinationFile}. Please copy the code and insert it manually"
					};
				}

				string formattedCode = FormatPostActionCode(postActionCodeLines, destinationFileContent, anchorIndex);

				var nextLineBreakAfterAnchor = destinationFileContent.IndexOf(Environment.NewLine, anchorIndex, StringComparison.Ordinal);
				destinationFileContent = destinationFileContent.Insert(nextLineBreakAfterAnchor, formattedCode);

				//Save
				File.WriteAllText(destinationFile, destinationFileContent);

				return new PostActionResult()
				{
					ResultCode = ResultCode.Success,
					Message = $"Successfully inserted code {formattedCode} on anchor {destinationAnchor} in file {destinationFile}"
				};
			}
			catch (Exception ex)
			{
				return new PostActionResult()
				{
					ResultCode = ResultCode.Error,
					Message = "Error in insert partial generation",
					Details = ex.Message
				};
			}
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
