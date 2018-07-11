using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WtsTelemetry.Services
{
    public class MarkdownBuilder
    {
        private StringBuilder stringBuilder = new StringBuilder();

        public MarkdownBuilder AddHeader(int year, int month)
        {
            stringBuilder.AppendLine($"# Telemetry for Windows Template Studio - {year}.{month}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("As new features and pages roll out, percentages  will adjust.");
            stringBuilder.AppendLine();

            return this;
        }

        public MarkdownBuilder AddTable(string title, string firstColumnName, string json)
        {
            try
            {
                var data = GetQueryData(json);

                stringBuilder.AppendLine($"## {title}");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"|{firstColumnName}|Percentage|");
                stringBuilder.AppendLine("|:---|:---:|");

                foreach (var value in data)
                {
                    stringBuilder.AppendLine($"|{value.Name}|{Math.Round(value.Value, 1)}%|");
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

        private IEnumerable<QueryData> GetQueryData(string jsonData)
        {
            var jobject = JObject.Parse(jsonData);

            return jobject["tables"][0]["rows"]
                .Children()
                .Select(c =>
                    new QueryData
                    {
                        Name = (string)c[0],
                        Value = (double)c[3]
                    });
        }
    }
}
