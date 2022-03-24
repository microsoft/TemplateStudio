Imports Microsoft.Toolkit.Mvvm.ComponentModel

Namespace ViewModels
    ' TODO WTS: Remove this sample page when/if it's not needed.
    ' This page is an sample of how to launch a specific page in response to a protocol launch and pass it a value.
    ' It is expected that you will delete this page once you have changed the handling of a protocol launch to meet
    ' your needs and redirected to another of your pages.
    Public Class SchemeActivationSampleViewModel
        Inherits ObservableObject

        Public ReadOnly Property Parameters As ObservableCollection(Of String) = New ObservableCollection(Of String)()

        Public Sub Initialize(parameters As Dictionary(Of String, String))
            Me.Parameters.Clear()
            Dim ticks As Long = Nothing
            For Each param In parameters

                If param.Key = "ticks" AndAlso Long.TryParse(param.Value, ticks) Then
                    Dim dateTime = New DateTime(ticks)
                    Me.Parameters.Add($"{param.Key}: {dateTime}")
                Else
                    Me.Parameters.Add($"{param.Key}: {param.Value}")
                End If
            Next
        End Sub
    End Class
End Namespace
