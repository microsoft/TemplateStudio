using Microsoft.Templates.Core.Gen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class BackgroundTaskPostAction : PostAction
    {
        private const string backgroundTaskFolder = "BackgroundTask";
        private const string backgroundTaskServiceFileName = "BackgroundTaskService.cs";
        private const string Anchor = "//BACKGROUNDTASK_ANCHOR";
        private const string backgroundTasksToAddFileName = "*AddBackgroundTask.txt";

        public override void Execute()
        {
            var backgroundServiceFile = Path.Combine(GenContext.Current.OutputPath, backgroundTaskFolder, backgroundTaskServiceFileName);
            var backgroundServiceContent = File.ReadAllText(backgroundServiceFile);

            var anchorIndex = backgroundServiceContent.IndexOf(Anchor);

            if (anchorIndex == -1)
            {
                //TODO: REVIEW THIS
                throw new Exception("Anchor not found");
            }
            var nextLineAfterAnchor = backgroundServiceContent.IndexOf(Environment.NewLine, anchorIndex, StringComparison.Ordinal) + Environment.NewLine.Length;

            var backgroundTasks = GetBackgroundTasksToAdd(GenContext.Current.OutputPath);

            foreach (var backgroundTaskToAdd in backgroundTasks)
            {
                backgroundServiceContent = backgroundServiceContent.Insert(nextLineAfterAnchor, FormatCode(backgroundTaskToAdd.Content, backgroundServiceContent, anchorIndex));
                File.Delete(backgroundTaskToAdd.Name);
            }

            //Delete anchor
            backgroundServiceContent = backgroundServiceContent.Replace(Anchor, String.Empty);
            File.WriteAllText(backgroundServiceFile, backgroundServiceContent);
        }

        private List<(string Name, string Content)> GetBackgroundTasksToAdd(string projectPath)
        {
            return Directory.EnumerateFiles(Path.Combine(projectPath, backgroundTaskFolder), backgroundTasksToAddFileName, SearchOption.TopDirectoryOnly).
                Select(f => (Name: f, Content: File.ReadAllText(f))).ToList();
        }


        private static string FormatCode(string backgroundTaskToAdd, string destinationFileContent, int anchorIndex)
        {
            var lastLineBreakBeforeAnchor = destinationFileContent.LastIndexOf(Environment.NewLine, anchorIndex, StringComparison.Ordinal);
            var leadingChars = destinationFileContent.Skip(lastLineBreakBeforeAnchor + Environment.NewLine.Length).TakeWhile(char.IsWhiteSpace).ToList();

            var leadingTabs = leadingChars.Count(c => c == '\t');
            var leadingWhiteSpaces = leadingChars.Count(c => c != '\t');


            backgroundTaskToAdd = string.Concat(new string('\t', leadingTabs), backgroundTaskToAdd.PadLeft(backgroundTaskToAdd.Length + leadingWhiteSpaces));

            return backgroundTaskToAdd;
        }
    }
}
