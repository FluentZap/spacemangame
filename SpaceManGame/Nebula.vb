Public Class Nebula
    Public points As List(Of PointI)
    Public type As nebula_type_enum
    Public StormLevel As Integer



    Sub New(ByVal points As List(Of PointI), ByVal type As nebula_type_enum, ByVal StormLevel As Integer)
        Me.points = points
        Me.type = type
        Me.StormLevel = StormLevel
    End Sub

End Class
