Public Class Planet_Building
    Public Owner As Integer
    Public BuildingRect As HashSet(Of Rectangle) = New HashSet(Of Rectangle)
    Public LandRect As Rectangle
    Public Type As building_type_enum

    Public Workers As Dictionary(Of Integer, Crew) = New Dictionary(Of Integer, Crew)

    Public Working_crew_list As HashSet(Of Integer) = New HashSet(Of Integer)
    Public Assigned_crew_list As HashSet(Of Integer) = New HashSet(Of Integer)

    Public Damaged As Boolean = False
    Public access_point As Dictionary(Of PointI, Boolean) = New Dictionary(Of PointI, Boolean)
    Public Building_Level As Byte


    Sub New(ByVal Owner As Integer, ByVal LandRect As Rectangle, ByVal Type As building_type_enum)
        Me.Owner = Owner
        Me.LandRect = LandRect        
        Me.Type = Type
    End Sub

End Class
