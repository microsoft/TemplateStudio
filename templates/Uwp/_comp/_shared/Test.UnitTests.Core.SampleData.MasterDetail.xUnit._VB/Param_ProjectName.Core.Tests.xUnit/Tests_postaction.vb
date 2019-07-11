'{[{
Imports System.Linq
Imports System.Threading.Tasks
Imports Param_RootNamespace.Core.Services
'}]}

Public class Tests
    '^^
    '{[{

    ' TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
    ' This test serves only as a demonstration of testing functionality in the Core library.
    <Fact>
    Public Async Function EnsureSampleDataServiceReturnsMasterDetailDataAsync() As Task
        Dim actual = Await SampleDataService.GetMasterDetailDataAsync()

        Assert.NotEmpty(actual)
    End Function
    '}]}
End Class
