using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WtsTelemetry
{
    public static class Queries
    {
        public static string GetProjectData = @"
let queryTable = customEvents
| where timestamp >= ago(30d)
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
