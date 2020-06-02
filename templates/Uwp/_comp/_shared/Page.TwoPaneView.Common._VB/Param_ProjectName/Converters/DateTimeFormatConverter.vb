Imports Windows.UI.Xaml.Data

Namespace Converters
    Public Class DateTimeFormatConverter
        Inherits IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object
            Dim dt As DateTime = TryCast(value, DateTime)

            If dt IsNot Nothing AndAlso parameter IsNot Nothing Then
                Return dt.ToString(parameter.ToString())
            End If

            Return value
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object
            If value IsNot Nothing Then
                Return DateTime.Parse(value.ToString())
            End If

            Return default(DateTime)
        End Function
    End Class
End Namespace
