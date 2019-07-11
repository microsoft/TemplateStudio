'{**
' This code block adds the method `GetContentGridDataAsync()` to the SampleDataService of your project.
'**}
Namespace Services
    Public Module SampleDataService
'{[{
        Private _allOrders As IEnumerable(Of SampleOrder)
'}]}
        Public Function AllOrders() As IEnumerable(Of SampleOrder)
        End Function

        Public Function AllCompanies() As IEnumerable(Of SampleCompany)
        End Function
'^^
'{[{

        ' TODO WTS: Remove this once your ContentGrid page is displaying real data.
        Public Async Function GetContentGridDataAsync() As Task(Of IEnumerable(Of SampleOrder))
            If _allOrders Is Nothing Then
                _allOrders = AllOrders()
            End If

            Await Task.CompletedTask

            Return _allOrders
        End Function
'}]}
    End Module
End Namespace
