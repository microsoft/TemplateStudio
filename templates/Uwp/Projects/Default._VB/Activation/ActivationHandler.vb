Namespace Activation
    ' For more information on understanding and extending activation flow see
    ' https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/activation.md
    Friend MustInherit Class ActivationHandler
        Public MustOverride Function CanHandle(args As Object) As Boolean

        Public MustOverride Function HandleAsync(args As Object) As Task
    End Class

    ' Extend this class to implement new ActivationHandlers
    Friend MustInherit Class ActivationHandler(Of T As Class)
        Inherits ActivationHandler

        ' Override this method to add the activation logic in your activation handler
        Protected MustOverride Function HandleInternalAsync(args As T) As Task

        Public Overrides Async Function HandleAsync(args As Object) As Task
            Await HandleInternalAsync(TryCast(args, T))
        End Function

        Public Overrides Function CanHandle(args As Object) As Boolean
            ' CanHandle checks the args is of type you have configured
            Return TypeOf args Is T AndAlso CanHandleInternal(TryCast(args, T))
        End Function

        ' You can override this method to add extra validation on activation args
        ' to determine if your ActivationHandler should handle this activation args
        Protected Overridable Function CanHandleInternal(args As T) As Boolean
            Return True
        End Function
    End Class
End Namespace
