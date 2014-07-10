Public Class Planet_Building
    Public Owner As Integer
    Public BuildingRect As New Dictionary(Of Integer, Rectangle)()
    Public LandRect As Rectangle
    Public Type As building_type_enum
    Public Generated As Boolean

    Public Customer_Slots As New Dictionary(Of PointI, Integer)()
    Public Guard_Slots As New Dictionary(Of PointI, Integer)()
    Public Worker_Slots As New Dictionary(Of PointI, Integer)()

    Public Damaged As Boolean = False
    Public Building_Level As Byte
    Public Items As New Dictionary(Of Item_Enum, Integer)()    

    Sub New(ByVal Owner As Integer, ByVal LandRect As Rectangle, ByVal Type As building_type_enum)
        Me.Owner = Owner
        Me.LandRect = LandRect
        Me.Type = Type
    End Sub

    Sub New(ByVal Owner As Integer, ByVal Type As building_type_enum)
        Me.Owner = Owner
        Me.Type = Type
    End Sub



    Function Get_Item(ByVal Item As Item_Enum) As Integer
        If Items.ContainsKey(Item) Then Return Items(Item)
        Return 0
    End Function

    Sub Add_Item(ByVal Item As Item_Enum, ByVal Amount As Integer)
        If Not Items.ContainsKey(Item) Then Items.Add(Item, 0)
        Items(Item) += Amount
    End Sub
    Sub Remove_Item(ByVal Item As Item_Enum, ByVal Amount As Integer)
        If Not Items.ContainsKey(Item) Then Exit Sub
        Items(Item) -= Amount
        If Items(Item) <= 0 Then Items.Remove(Item)
    End Sub
End Class
