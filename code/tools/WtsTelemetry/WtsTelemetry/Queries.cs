namespace WtsTelemetry
{
    public static class Queries
    {
        public static string GetProjectData = @"
let startDatetime = startofmonth(datetime({0}-{1}-01));
let endDatetime = endofmonth(datetime({0}-{1}-01));
let queryTable = customEvents
| where timestamp between(startDatetime .. endDatetime)
| extend eventName = iif(itemType == 'customEvent',name,'')
| where eventName == 'WtsProjectGen';
queryTable
| extend wtsProjectType = tostring(customDimensions['WtsProjectType'])
| summarize items = sum(itemCount) by wtsProjectType
| extend total = toscalar(queryTable | count)
| extend percentage = todouble(items)/toscalar(queryTable | count)*100
| order by percentage desc
";
    }
}
