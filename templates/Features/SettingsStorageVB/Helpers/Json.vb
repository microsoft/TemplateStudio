Imports Newtonsoft.Json

Namespace Helpers
    Public NotInheritable Class Json
        Private Sub New()
        End Sub
        Public Shared Async Function ToObjectAsync(Of T)(value As String) As Task(Of T)
              Return Await Task.Run(Of T)(Function() 
              Return JsonConvert.DeserializeObject(Of T)(value)
            End Function)
        End Function

        Public Shared Async Function StringifyAsync(value As Object) As Task(Of String)
                Return Await Task.Run(Of String)(Function() 
                Return JsonConvert.SerializeObject(value)
            End Function)
        End Function
    End Class
End Namespace
