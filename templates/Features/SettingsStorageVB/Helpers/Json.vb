Imports Newtonsoft.Json

Namespace Helpers
    Public Module Json
        Public Async Function ToObjectAsync(Of T)(value As String) As Task(Of T)
              Return Await Task.Run(Function() 
              Return JsonConvert.DeserializeObject(Of T)(value)
            End Function)
        End Function

        Public Async Function StringifyAsync(value As Object) As Task(Of String)
                Return Await Task.Run(Function() 
                Return JsonConvert.SerializeObject(value)
            End Function)
        End Function
    End Module
End Namespace
