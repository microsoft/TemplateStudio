using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI.Comparison
{
    public static class ComparisonService
    {
        public static IEnumerable<CodeLine> FromPath(string path)
        {
            if (File.Exists(path))
            {
                int lineNumber = 0;
                foreach (var line in File.ReadAllLines(path))
                {
                    yield return new CodeLine(++lineNumber, line);
                }
            }
        }

        public static IEnumerable<CodeLine> CompareFiles(IEnumerable<CodeLine> currentFileLines, IEnumerable<CodeLine> newFileLines)
        {
            var result = new List<CodeLine>();
            int positionCurrentLine = 1;
            int positionNewLine = 1;

            int currentCount = currentFileLines.Count();
            int newCount = newFileLines.Count();

            int lineNumber = 1;

            while (positionCurrentLine <= currentCount || positionNewLine <= newCount)
            {
                CodeLine currentLine = currentFileLines.FirstOrDefault(l => l.Number == positionCurrentLine);
                CodeLine newLine = newFileLines.FirstOrDefault(l => l.Number == positionNewLine);

                if (currentLine == null)
                {
                    // Finish to cover current file. Pendding lines in new file are new.
                    result.Add(new CodeLine(++lineNumber, newLine.Text, LineStatus.New));
                    positionNewLine++;
                }
                else if (newLine == null)
                {
                    // Finish to cover new file. Pendding lines in current file are removed.
                    result.Add(new CodeLine(++lineNumber, currentLine.Text, LineStatus.Deleted));
                    positionCurrentLine++;
                }
                else if (currentLine.Text == newLine.Text)
                {
                    // Current line matches with new line.
                    result.Add(new CodeLine(++lineNumber, currentLine.Text));
                    positionNewLine++;
                    positionCurrentLine++;
                }
                else
                {
                    var nextMatchLine = newFileLines.FirstOrDefault(l => l.Number > positionNewLine && l.Text == currentLine.Text);
                    if (nextMatchLine == null)
                    {
                        // Current line not found in new file lines. This line has been deleted.
                        result.Add(new CodeLine(++lineNumber, currentLine.Text, LineStatus.Deleted));
                        positionCurrentLine++;
                    }
                    else
                    {
                        // New line found before current line.
                        result.Add(new CodeLine(++lineNumber, newLine.Text, LineStatus.New));
                        positionNewLine++;
                    }
                }
            }
            return result;
        }
    }
}
