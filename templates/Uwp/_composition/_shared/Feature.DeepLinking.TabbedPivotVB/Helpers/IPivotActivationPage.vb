Imports System.Collections.Generic
Imports System.Threading.Tasks

Namespace Helpers
    Interface IPivotActivationPage
        Function OnPivotActivatedAsync(parameters As Dictionary(Of String, String)) As Task
    End Interface
End Namespace
