Namespace Views
    Public Class ShellNavigationItem
        Implements INotifyPropertyChanged
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

        Public Property Label() As String
            Get
                Return m_Label
            End Get
            Set
                m_Label = Value
            End Set
        End Property

        Private m_Label As String

        Public Property Symbol() As Symbol
            Get
                Return m_Symbol
            End Get
            Set
                m_Symbol = Value
            End Set
        End Property

        Private m_Symbol As Symbol

        Public ReadOnly Property SymbolAsChar() As Char
            Get
                Return Convert.ToChar(Symbol)
            End Get
        End Property

        Public Property PageType() As Type
            Get
                Return m_PageType
            End Get
            Set
                m_PageType = Value
            End Set
        End Property

        Private m_PageType As Type

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

        Private _selectedForeground As SolidColorBrush = Nothing
        Public Property SelectedForeground() As SolidColorBrush
            Get
                Return If(_selectedForeground, (InlineAssignHelper(_selectedForeground, GetStandardTextColorBrush())))
            End Get
            Set
                [Set](_selectedForeground, value)
            End Set
        End Property

        Private Sub New(name As String, symbol As Symbol, pageType As Type)
            Me.Label = name
            Me.Symbol = symbol
            Me.PageType = pageType
        End Sub

        Public Shared Function FromType(Of T As Page)(name As String, symbol As Symbol) As ShellNavigationItem
            Return New ShellNavigationItem(name, symbol, GetType(T))
        End Function

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub [Set](Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional propertyName As String = Nothing)
            If Equals(storage, value) Then
              Return
            End If

            storage = value
            OnPropertyChanged(propertyName)
        End Sub

        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function

        Private Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
    End Class
End Namespace
