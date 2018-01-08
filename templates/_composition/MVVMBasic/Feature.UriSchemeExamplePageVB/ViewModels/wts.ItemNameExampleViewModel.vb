Imports Param_ItemNamespace.Helpers

Namespace ViewModels
    ' TODO WTS: This class exists purely as part of the example of how to launch a specific page in response to a protocol launch and pass it a value. It is expected that you will delete this class once you have changed the handling of a protocol launch to meet your needs and redirected to another of your pages.
    Public Class wts.ItemNameExampleViewModel
        Inherits Observable

        ' This property is just for displaying the passed in value
        Private _secret As String

        Public Property Secret As String
            Get
                Return _secret
            End Get

            Set
                [Set](_secret, Value)
            End Set
        End Property
    End Class
End Namespace
