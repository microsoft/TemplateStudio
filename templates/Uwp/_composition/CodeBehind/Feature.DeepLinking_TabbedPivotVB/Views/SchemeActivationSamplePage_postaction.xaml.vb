'{[{
Imports System.Threading.Tasks
Imports Param_ItemNamespace.Helpers
'}]}
Namespace Views
    Public NotInheritable Partial Class SchemeActivationSamplePage
        Inherits Page
        Implements INotifyPropertyChanged
'{[{
        Implements IPivotActivationPage

        Public Async Function OnPivotActivatedAsync(parameters As Dictionary(Of String, String)) As Task Implements IPivotActivationPage.OnPivotActivatedAsync
            Initialize(parameters)
            Await Task.CompletedTask
        End Function
'}]}
    End Class
End Namespace