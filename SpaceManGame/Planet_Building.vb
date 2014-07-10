Public Class Slot_Return_Type
    Public Point As PointI
    Public ID As Integer
    Sub New(ByVal Point As PointI, ByVal Id As Integer)
        Me.Point = Point
        Me.ID = Id
    End Sub

    Sub New()
        Me.ID = -1
    End Sub

End Class

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

    Function EmptyCustomerSlots() As Integer
        Dim Empty As Integer
        For Each slot In Customer_Slots
            If slot.Value = -1 Then Empty += 1
        Next
        Return Empty
    End Function

    Function GetEmptyCustomerSlot() As PointI
        Dim Empty As PointI
        For Each slot In Customer_Slots
            If slot.Value = -1 Then Empty = slot.Key : Return Empty
        Next
        Return Nothing
    End Function


    Function GetWorker() As Integer        
        For Each slot In Worker_Slots
            If Not slot.Value = -1 Then Return slot.Value
        Next
        Return -1
    End Function


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
