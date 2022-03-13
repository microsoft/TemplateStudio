Imports System.IO
Imports System.Runtime.CompilerServices

Namespace Helpers
    Module StreamExtensions
        <Extension>
        Function ToBase64String(stream As Stream) As String
            Using memoryStream = New MemoryStream()
                stream.CopyTo(memoryStream)
                Return Convert.ToBase64String(memoryStream.ToArray())
            End Using
        End Function
    End Module
End Namespace
