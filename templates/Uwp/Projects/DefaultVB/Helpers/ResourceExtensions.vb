Imports Windows.ApplicationModel.Resources

Namespace Helpers
    Friend Module ResourceExtensions
        Private _resLoader As New ResourceLoader()

        <Extension>
        Public Function GetLocalized(resourceKey As String) As String
            Return _resLoader.GetString(resourceKey)
        End Function
    End Module
End Namespace
