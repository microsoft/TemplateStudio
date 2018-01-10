Imports Windows.ApplicationModel.DataTransfer
Imports Windows.ApplicationModel.DataTransfer.ShareTarget

Imports Param_ItemNamespace.Helpers

Namespace ViewModels
    Public Class SharedDataWebLinkViewModel
        Inherits SharedDataViewModelBase

        Private _uri As Uri

        Public Property Uri As Uri
            Get
                Return _uri
            End Get
            Set
                [Param_Setter](_uri, value)
            End Set
        End Property

        Public Sub New()
        End Sub

        Public Overrides Async Function LoadDataAsync(shareOperation As ShareOperation) As Task
            Await MyBase.LoadDataAsync(shareOperation)

            PageTitle = "ShareTargetFeature_WebLinkTitle".GetLocalized()
            DataFormat = StandardDataFormats.WebLink
            Uri = Await shareOperation.GetWebLinkAsync()
        End Function
    End Class
End Namespace
