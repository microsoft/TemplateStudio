'{[{
Imports Param_RootNamespace.Core.Services
'}]}

Public class Tests
    '^^
    '{[{

    ' Remove or update this once your app is using real data and not the SampleDataService.
    ' This test serves only as a demonstration of testing functionality in the Core library.
    <Fact>
    Public Async Function EnsureSampleDataServiceReturnsListDetailDataAsync() As Task
        Dim actual = Await SampleDataService.GetListDetailDataAsync()

        Assert.NotEmpty(actual)
    End Function
    '}]}
End Class
