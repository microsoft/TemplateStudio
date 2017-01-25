using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.PostActions
{
	public class InsertPartialGenerationPostAction : PostActionBase
	{

		public InsertPartialGenerationPostAction(string name, string description,  IReadOnlyDictionary<string, string> parameters)
			: base(name, description, parameters)
		{
		}

		public override PostActionExecutionResult Execute(ExecutionContext context)
		{
			//Read values from params
			var sourceFileName = GetValueFromParameter("Source.FileName");
			var destinationFileName = GetValueFromParameter("Destination.FileName");
			var destinationAnchor = GetValueFromParameter("Destination.Anchor");


			//Read Code to insert
			var postActionCodeLines = File.ReadAllLines(Path.Combine(context.PagePath, sourceFileName));

			//Read destination file, find anchor
			var destinationFile = Path.Combine(context.ProjectPath, destinationFileName);
			var destinationFileContent = File.ReadAllText(destinationFile);
			var anchorIndex = destinationFileContent.IndexOf(destinationAnchor, StringComparison.Ordinal);

			if (anchorIndex == -1)
			{
				var postActionCode = postActionCodeLines.Aggregate(new StringBuilder(), (sb, a) => sb.AppendLine(a), sb => sb.ToString());

				return new PostActionExecutionResult()
				{
					ResultCode = ResultCode.AnchorNotFound,
					Message = $"Could not inserted {postActionCode} on anchor {destinationAnchor} in file {destinationFile}. Please copy the code and insert it manually"
				};
			}

			var lastLineBreakBeforeAnchor = destinationFileContent.LastIndexOf(Environment.NewLine, anchorIndex, StringComparison.Ordinal);
			var nextLineBreakAfterAnchor = destinationFileContent.IndexOf(Environment.NewLine, anchorIndex, StringComparison.Ordinal);
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

			destinationFileContent = destinationFileContent.Insert(nextLineBreakAfterAnchor, formattedCode);

			//Save
			File.WriteAllText(destinationFile, destinationFileContent);

			return new PostActionExecutionResult()
			{
				ResultCode = ResultCode.Success,
				Message = $"Successfully inserted code {formattedCode} on anchor {destinationAnchor} in file {destinationFile}"
			};
		}


	}
}
