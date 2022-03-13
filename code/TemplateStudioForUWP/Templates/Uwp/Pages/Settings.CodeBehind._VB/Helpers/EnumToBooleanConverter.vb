Namespace Helpers
    Public Class EnumToBooleanConverter
        Implements IValueConverter

        Public Property EnumType As Type

        Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
            If TypeOf parameter Is String Then
                If Not [Enum].IsDefined(EnumType, value) Then
                    Throw New ArgumentException("value must be an Enum!")
                End If

                Dim enumValue = [Enum].Parse(EnumType, parameter.ToString())

                Return enumValue.Equals(value)
            End If

            Throw New ArgumentException("parameter must be an Enum name!")
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
            Dim enumString = TryCast(parameter, String)
            If enumString IsNot Nothing Then
                Return [Enum].Parse(EnumType, enumString)
            End If

            Throw New ArgumentException("parameter must be an Enum name!")
        End Function
    End Class
End Namespace
