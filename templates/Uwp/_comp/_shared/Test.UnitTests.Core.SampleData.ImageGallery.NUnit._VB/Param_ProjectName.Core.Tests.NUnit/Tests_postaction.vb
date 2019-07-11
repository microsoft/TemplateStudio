'{[{
Imports Param_RootNamespace.Core.Services
'}]}

Public Class Tests
    '^^
    '{[{

    ' TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
    ' This test serves only as a demonstration of testing functionality in the Core library.
    <Test>
    Public Async Function EnsureSampleDataServiceReturnsImageGalleryDataAsync() As Task
        Dim actual = Await SampleDataService.GetImageGalleryDataAsync("ms-appx:///Assets")

        Assert.AreNotEqual(0, actual.Count)
    End Function
    '}]}
End Class
