Public Class Planet_Building
    Public Owner As Integer
    Public Rect As Rectangle
    Public Type As building_type_enum

    Sub New(ByVal Owner As Integer, ByVal Rect As Rectangle, ByVal Type As building_type_enum)
        Me.Owner = Owner
        Me.Rect = Rect
        Me.Type = type
    End Sub

End Class
