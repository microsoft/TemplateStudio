Namespace Activation
    Friend Module SchemeActivationConfig
        ' TODO WTS: Add the pages that can be opened from scheme activation in your app here.
        Private ReadOnly _activationViewModels As Dictionary(Of String, String) = New Dictionary(Of String, String)() From
        {
            {"sample", GetType(ViewModels.SchemeActivationSampleViewModel).FullName}
        }

        Friend Function GetViewModelName(viewModelKey As String) As String
            Return _activationViewModels.FirstOrDefault(Function(p) p.Key = viewModelKey).Value
        End Function

        Friend Function GetViewModelKey(viewModelName As String) As String
            Return _activationViewModels.FirstOrDefault(Function(p) p.Value = viewModelName).Key
        End Function
    End Module
End Namespace