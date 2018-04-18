Imports System.Collections.Concurrent

Namespace Helpers
    Friend NotInheritable Class Singleton(Of T As New)
        Private Sub New()
        End Sub

        Private Shared ReadOnly _instances As New ConcurrentDictionary(Of Type, T)()

        Public Shared ReadOnly Property Instance As T
            Get
                Return _instances.GetOrAdd(GetType(T), Function(t) New T())
            End Get
        End Property
    End Class
End Namespace
