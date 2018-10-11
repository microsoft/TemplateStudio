Namespace Helpers
    Friend Module PivotItemExtensions
        <Extension()>
        Function GetPage(Of T As Class)(pivotItem As PivotItem) As T
            Dim frame = TryCast(pivotItem.Content, Frame)
            If frame IsNot Nothing Then
                Dim element = TryCast(frame.Content, T)
                If element IsNot Nothing Then
                    Return element
                End If
            End If

            Return Nothing
        End Function

        <Extension()>
        Function IsOfViewModelName(pivotItem As PivotItem, viewModelName As String) As Boolean
            Dim frame = TryCast(pivotItem.Content, Frame)
            If frame IsNot Nothing Then
                Dim page = TryCast(frame.Content, Page)
                If page IsNot Nothing Then
                    If page.DataContext.GetType().FullName = viewModelName Then
                        Return True
                    End If
                End If
            End If

            Return False
        End Function
    End Module
End Namespace