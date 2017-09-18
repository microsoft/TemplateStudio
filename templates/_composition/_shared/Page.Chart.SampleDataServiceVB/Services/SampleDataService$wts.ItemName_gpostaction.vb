'{**
' This code block adds the method `GetChartSampleData()` to the SampleDataService of your project.
'**}
'{[{
Imports System.Collections.ObjectModel
Imports System.Linq
'}]}

Namespace Param_ItemNamespace.Services
    Public Module SampleDataService
        '^^
        '{[{

        ' TODO WTS: Remove this once your chart page is displaying real data
        Public Function GetChartSampleData() As ObservableCollection(Of DataPoint)
            Dim data = AllOrders().[Select](Function(o) New DataPoint() With {
                Key .Category = o.Company,
                Key .Value = o.OrderTotal
            }).OrderBy(Function(dp) dp.Category)

            Return New ObservableCollection(Of DataPoint)(data)
        End Function
        '}]}
    End Module
End Namespace