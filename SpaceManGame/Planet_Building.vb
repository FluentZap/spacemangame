Public Enum BAP_Type
    Worker
    Transporter
    Customer
End Enum


Public Class Building_Access_Point_Type
    Public Used As Boolean = False
    Public NextUp As Boolean = False
    Public Type As BAP_Type

    Sub New(ByVal Type As BAP_Type)
        Me.Type = Type
    End Sub

End Class


Public Class Item_Slots_Type
    Public Input_Slot As Boolean = False
    'Public NextUp As Boolean = False
    Sub New(ByVal Input_Slot As Boolean)
        Me.Input_Slot = Input_Slot
    End Sub
End Class



Public Class Planet_Building
    Public Owner As Integer
    Public BuildingRect As New Dictionary(Of Integer, Rectangle)()
    Public LandRect As Rectangle
    Public Type As building_type_enum

    Public Workers As New Dictionary(Of Integer, Crew)()

    Public Working_crew_list As HashSet(Of Integer) = New HashSet(Of Integer)
    Public Assigned_crew_list As HashSet(Of Integer) = New HashSet(Of Integer)

    Public Damaged As Boolean = False
    Public access_point As New Dictionary(Of PointI, Building_Access_Point_Type)()

    Public Item_Slots As New Dictionary(Of PointI, Item_Slots_Type)()
    Public Building_Level As Byte
    Public Item_Build_Progress As Integer

    Public Available_Transporters As New HashSet(Of Integer)()
    Public Buying_Items As New HashSet(Of Item_Enum)()

    Public PickupPoint As PointI
    Public Resource_Credit As Integer


    Sub New(ByVal Owner As Integer, ByVal LandRect As Rectangle, ByVal Type As building_type_enum)
        Me.Owner = Owner
        Me.LandRect = LandRect
        Me.Type = Type
    End Sub


End Class
