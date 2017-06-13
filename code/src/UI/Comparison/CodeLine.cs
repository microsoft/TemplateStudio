namespace Microsoft.Templates.UI.Comparison
{
    public enum LineStatus { Default = 0, New = 1, Deleted = 2 };

    public class CodeLine
    {
        public LineStatus Status { get; set; }
        public int Number { get; set; }
        public string Text { get; set; }

        public CodeLine(int number, string text, LineStatus status = LineStatus.Default)
        {
            Number = number;
            Text = text;
            Status = status;
        }
    }
}
