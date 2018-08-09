namespace WtsTelemetry.Services
{
    public class QueryService
    {
        private readonly string DataQuery = @"
let startDatetime = startofmonth(datetime({0}-{1}-01));
let endDatetime = endofmonth(datetime({0}-{1}-01));
let queryTable = customEvents
| where timestamp between(startDatetime .. endDatetime)
| extend eventName = iif(itemType == 'customEvent',name,'')
| where eventName == '{2}';
queryTable
| extend itemName = tostring(customDimensions['{3}'])
| summarize items = sum(itemCount) by itemName
| extend total = toscalar(queryTable | count)
| extend percentage = todouble(items)/toscalar(queryTable | count)*100
| order by percentage desc
";

        private readonly int year;
        private readonly int month;

        public QueryService(int year, int month)
        {
            this.year = year;
            this.month = month;
        }

        public string Projects => string.Format(DataQuery, year, month, "WtsProjectGen", "WtsProjectType");

        public string Frameworks => string.Format(DataQuery, year, month, "WtsProjectGen", "WtsFramework");

        public string Pages => string.Format(DataQuery, year, month, "WtsPageGen", "WtsTemplateName");

        public string Features => string.Format(DataQuery, year, month, "WtsFeatureGen", "WtsTemplateName");

        public string EntryPoints => string.Format(DataQuery, year, month, "WtsWizard", "WtsWizardType");
    }
}
