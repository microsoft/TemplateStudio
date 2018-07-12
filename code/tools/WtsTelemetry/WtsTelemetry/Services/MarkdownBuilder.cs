using System;
using System.Text;
using WtsTelemetry.Helpers;

namespace WtsTelemetry.Services
{
    public class MarkdownBuilder
    {
        private StringBuilder stringBuilder = new StringBuilder();

        public MarkdownBuilder AddHeader(int year, int month)
        {
            stringBuilder.AppendLine($"# Telemetry for Windows Template Studio - {year}.{month.ToString("D2")}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("As new features and pages roll out, percentages  will adjust.");
            stringBuilder.AppendLine();

            return this;
        }

        public MarkdownBuilder AddTable(string title, string firstColumnName, string json)
        {
            try
            {
                stringBuilder.AppendLine($"## {title}");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"|{firstColumnName}|Percentage|");
                stringBuilder.AppendLine("|:---|:---:|");

                foreach (var value in json.ToQueryData())
                {
                    stringBuilder.AppendLine($"|{value.DisplayName}|{Math.Round(value.Value, 1)}%|");
                }
            }
            catch(Exception ex)
            {
                stringBuilder.AppendLine($"Error to process {title} table.");
                stringBuilder.AppendLine($"Entry data: {json}.");
                stringBuilder.AppendLine($"Exception: {ex.Message}.");
            }

            stringBuilder.AppendLine();
            return this;
        }

        public string GetText() => stringBuilder.ToString();        
    }
}
