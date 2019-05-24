'{**
' This code block adds the method `GetGridSampleDataAsync()` to the SampleDataService of your project.
'**}
'{[{
Imports System.Threading.Tasks
'}]}
Namespace Services
    Public Module SampleDataService
        '^^
        '{[{

        ' TODO WTS: Remove this once your grid page is displaying real data.
        Public Async Function GetGridSampleDataAsync() As Task(Of ObservableCollection(Of SampleOrder))
            Await Task.CompletedTask

            Return New ObservableCollection(Of SampleOrder)(AllOrders())
        End Function
        '}]}
    End Module
End Namespace
