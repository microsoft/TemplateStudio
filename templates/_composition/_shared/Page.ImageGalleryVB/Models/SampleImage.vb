Namespace Models
    Public Class SampleImage
        Public Property ID() As String
            Get
                Return m_ID
            End Get
            Set
                m_ID = Value
            End Set
        End Property
        Private m_ID As String

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set
                m_Name = Value
            End Set
        End Property
        Private m_Name As String

        Public Property Source() As String
            Get
                Return m_Source
            End Get
            Set
                m_Source = Value
            End Set
        End Property
        Private m_Source As String
    End Class
End Namespace
