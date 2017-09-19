Imports System.ComponentModel
Imports Windows.UI.Xaml.Controls

Namespace Views
    NotInheritable Class TabbedPivotPagePage
        Inherits Page
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    End Class
End Namespace
