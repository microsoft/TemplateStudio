Namespace Activation
    ' For more information on application activation see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.vb.md
    Friend MustInherit Class ActivationHandler
        Public MustOverride Function CanHandle(args As Object) As Boolean
        
        Public MustOverride Function HandleAsync(args As Object) As Task
    End Class

    Friend MustInherit Class ActivationHandler(Of T As Class)
        Inherits ActivationHandler

        Protected MustOverride Function HandleInternalAsync(args As T) As Task

        Public Overrides Async Function HandleAsync(args As Object) As Task
            Await HandleInternalAsync(TryCast(args, T))
        End Function

        Public Overrides Function CanHandle(args As Object) As Boolean
            Return TypeOf args Is T AndAlso CanHandleInternal(TryCast(args, T))
        End Function

        Protected Overridable Function CanHandleInternal(args As T) As Boolean
            Return True
        End Function
    End Class
End Namespace
