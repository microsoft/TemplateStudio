Namespace Activation
    Friend Module SchemeActivationConfig
        ' TODO WTS: Add the pages that can be opened from scheme activation in your app here.
        Private ReadOnly _activationPages As Dictionary(Of String, Type) = New Dictionary(Of String, Type)() From
        {
            {"sample", GetType(Views.SchemeActivationSamplePage)}
        }

        Friend Function GetPage(pageKey As String) As Type
            Return _activationPages.FirstOrDefault(Function(p) p.Key = pageKey).Value
        End Function

        Friend Function GetPageKey(pageType As Type) As String
            Return _activationPages.FirstOrDefault(Function(p) p.Value = pageType).Key
        End Function
    End Module
End Namespace