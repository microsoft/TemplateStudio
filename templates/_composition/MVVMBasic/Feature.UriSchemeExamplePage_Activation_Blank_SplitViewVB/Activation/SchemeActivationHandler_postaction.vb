Namespace Activation
    Friend Class SchemeActivationHandler
        Inherits ActivationHandler(Of ProtocolActivatedEventArgs)
        '{[{

        ' By default, this handler expects URIs of the format 'wtsapp:sample?secret={value}'
        Protected Overrides Async Function HandleInternalAsync(args As ProtocolActivatedEventArgs) As Task
            If args.Uri.AbsolutePath.ToLowerInvariant.Equals("sample") Then
                Dim secret = "<<I-HAVE-NO-SECRETS>>"

                Try
                    If args.Uri.Query IsNot Nothing Then
                        ' The following will extract the secret value and pass it to the page. Alternatively, you could pass all or some of the Uri.
                        Dim decoder As New Windows.Foundation.WwwFormUrlDecoder(args.Uri.Query)

                        secret = decoder.GetFirstValueByName("secret")
                    End If
                    ' NullReferenceException if the URI doesn't contain a query
                    ' ArgumentException if the query doesn't contain a param called 'secret'
                Catch ex As Exception
                End Try

                ' It's also possible to have logic here to navigate to different pages. e.g. if you have logic based on the URI used to launch
                NavigationService.Navigate(GetType(Views.wts.ItemNameExamplePage), secret)
            ElseIf args.PreviousExecutionState <> ApplicationExecutionState.Running Then
                ' If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                NavigationService.Navigate(GetType(Views.Param_HomeNamePage))
            End If

            Await Task.CompletedTask
        End Function
        '}]}
    End Class
End Namespace
