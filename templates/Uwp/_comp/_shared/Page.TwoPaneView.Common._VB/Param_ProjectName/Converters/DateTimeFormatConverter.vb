Namespace Converters
    Public Class DateTimeFormatConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
            Try
                Dim dt As DateTime = CType(value, DateTime)

                If parameter IsNot Nothing Then
                    Return dt.ToString(parameter.ToString())
                End If
            Catch ex As InvalidCastException
            End Try

            Return value
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
            If value IsNot Nothing Then
                Return DateTime.Parse(value.ToString())
            End If

            Return Nothing
        End Function
    End Class
End Namespace
