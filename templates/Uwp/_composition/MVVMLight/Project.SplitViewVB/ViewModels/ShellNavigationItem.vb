Imports GalaSoft.MvvmLight

Namespace ViewModels
    Public NotInheritable Class ShellNavigationItem
        Inherits ViewModelBase

        Public Property Label As String

        Public Property ViewModelName As String

        Public Property Symbol As Symbol

        Public ReadOnly Property SymbolAsChar As Char
            Get
                Return Convert.ToChar(Symbol)
            End Get
        End Property

        Private ReadOnly _iconElement As IconElement = Nothing

        Public ReadOnly Property Icon As IconElement
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

                BindingOperations.SetBinding(fontIcon, IconElement.ForegroundProperty, foregroundBinding)

                Return fontIcon
            End Get
        End Property

        Private _selectedVis As Visibility = Visibility.Collapsed

        Public Property SelectedVis As Visibility
            Get
                Return _selectedVis
            End Get
            Set
                [Set](_selectedVis, value)
            End Set
        End Property

        Private _isSelected As Boolean

        Public Property IsSelected As Boolean
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
            Dim brush = TryCast(Application.Current.Resources("ThemeControlForegroundBaseHighBrush"), SolidColorBrush)

            Return brush
        End Function

        Private _selectedForeground As SolidColorBrush = Nothing

        Public Property SelectedForeground As SolidColorBrush
            Get
                Return If(_selectedForeground, (InlineAssignHelper(_selectedForeground, GetStandardTextColorBrush())))
            End Get
            Set
                [Set](_selectedForeground, value)
            End Set
        End Property

        Public Sub New(label As String, symbol As Symbol, viewModelName As String)
            Me.New(label, viewModelName)
            Me.Symbol = symbol
        End Sub

        Public Sub New(label As String, icon As IconElement, viewModelName As String)
            Me.New(label, viewModelName)
            Me._iconElement = icon
        End Sub

        Private Sub New(label As String, viewModelName As String)
            Me.Label = label
            Me.ViewModelName = viewModelName
        End Sub

        Public Overrides Function ToString() As String
            Return Label
        End Function

        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function

    End Class
End Namespace
