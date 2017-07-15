Imports Param_ItemNamespace.Helpers
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Media

Namespace ViewModels
    Public Class ShellNavigationItem
        Inherits Observable

        Public Property Label As String

        Public Property PageType As Type

        Public Property Symbol As Symbol

        Public ReadOnly Property SymbolAsChar() As Char
            Get
                Return Convert.ToChar(Symbol)
            End Get
        End Property

        Private _iconElement As IconElement = Nothing
        Public ReadOnly Property Icon() As IconElement
            Get
                Dim foregroundBinding = New Binding() With {
                    .Source = Me,
                    .Path = New PropertyPath("SelectedForeground"),
                    .Mode = BindingMode.OneWay,
                    .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                }

                If _iconElement IsNot Nothing Then
                    BindingOperations.SetBinding(_iconElement, IconElement.ForegroundProperty, foregroundBinding)

                    Return _iconElement
                End If

                Dim fontIcon = New FontIcon() With {
                    .FontSize = 16,
                    .Glyph = SymbolAsChar.ToString()
                }

                BindingOperations.SetBinding(fontIcon, FontIcon.ForegroundProperty, foregroundBinding)

                Return fontIcon
            End Get
        End Property

        Private _isSelected As Boolean

        Private _selectedVis As Visibility = Visibility.Collapsed

        Public Property SelectedVis() As Visibility
            Get
                Return _selectedVis
            End Get
            Set
                [Set](_selectedVis, value)
            End Set
        End Property

        Private _selectedForeground As SolidColorBrush = Nothing

        Public Property SelectedForeground() As SolidColorBrush
            Get
                Return If(_selectedForeground, (InlineAssignHelper(_selectedForeground, GetStandardTextColorBrush())))
            End Get
            Set
                [Set](_selectedForeground, value)
            End Set
        End Property

        Public Property IsSelected() As Boolean
            Get
                Return _isSelected
            End Get
            Set
                [Set](_isSelected, value)
                SelectedVis = If(value, Visibility.Visible, Visibility.Collapsed)
                SelectedForeground = If(value, TryCast(Application.Current.Resources("SystemControlForegroundAccentBrush"), SolidColorBrush), GetStandardTextColorBrush())
            End Set
        End Property

        Private Function GetStandardTextColorBrush() As SolidColorBrush
            Dim brush = TryCast(Application.Current.Resources("SystemControlForegroundBaseHighBrush"), SolidColorBrush)

            Return brush
        End Function

        Private Sub New(name As String, symbol As Symbol, pageType As Type)
            Me.Label = name
            Me.Symbol = symbol
            Me.PageType = pageType
        End Sub

        Private Sub New(name As String, icon As IconElement, pageType As Type)
            Me.Label = name
            Me._iconElement = icon
            Me.PageType = pageType
        End Sub

        Public Shared Function FromType(Of T As Page)(name As String, symbol As Symbol) As ShellNavigationItem
            Return New ShellNavigationItem(name, symbol, GetType(T))
        End Function

        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class
End Namespace
