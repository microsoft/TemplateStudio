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
        private readonly string DataByCategoryQuery = @"
let startDatetime = startofmonth(datetime({0}-{1}-01));
let endDatetime = endofmonth(datetime({0}-{1}-01));
let queryTable = customEvents
| where timestamp between(startDatetime .. endDatetime)
| extend eventName = iif(itemType == 'customEvent',name,'')
| where eventName == '{2}'
| where customDimensions.WtsCategory == '{4}';
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

        public string Projects(string platform) => string.Format(DataByCategoryQuery, year, month, "WtsProjectGen", "WtsProjectType", platform);

        public string Frameworks(string platform) => string.Format(DataByCategoryQuery, year, month, "WtsProjectGen", "WtsFramework", platform);

        public string Pages(string platform) => string.Format(DataByCategoryQuery, year, month, "WtsPageGen", "WtsTemplateName", platform);

        public string Features(string platform) => string.Format(DataByCategoryQuery, year, month, "WtsFeatureGen", "WtsTemplateName", platform);

        public string Services(string platform) => string.Format(DataByCategoryQuery, year, month, "WtsServiceGen", "WtsTemplateName", platform);

        public string Testing(string platform) => string.Format(DataByCategoryQuery, year, month, "WtsTestingGen", "WtsTemplateName", platform);

        public string EntryPoints => string.Format(DataQuery, year, month, "WtsWizard", "WtsWizardType");

        public string Languages => string.Format(DataQuery, year, month, "WtsProjectGen", "WtsLanguage");
    }
}
