'{[{
Imports Param_RootNamespace.Core.Services
'}]}

Public Class Tests
    '^^
    '{[{

    ' TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
    ' This test serves only as a demonstration of testing functionality in the Core library.
    <Test>
    Public Async Sub EnsureSampleDataServiceReturnsChartDataAsync()
        Dim actual = await SampleDataService.GetChartSampleDataAsync()

        Assert.IsNotEmpty(actual)
    End Sub
    '}]}
End Class
