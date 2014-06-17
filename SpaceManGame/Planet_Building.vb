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


Public Class Worker_Slots_Type
    Public ID As Integer
    Public Type As Worker_Type_Enum
    Sub New(ByVal ID As Integer, ByVal Type As Worker_Type_Enum)
        Me.ID = ID
        Me.Type = Type
    End Sub
End Class


Public Class Tennent_Slots_Type
    Public CrewID As Integer
    Public SlotID As Integer
    Sub New(ByVal CrewID As Integer, ByVal SlotID As Integer)
        Me.CrewID = CrewID
        Me.SlotID = SlotID
    End Sub
End Class


Public Class Worker_Slots
    Public Slots As New HashSet(Of Worker_Slots_Type)()

    Sub New(ByVal Transporters As Integer, ByVal Workers As Integer, ByVal Gaurds As Integer)
        For x = 1 To Transporters
            Slots.Add(New Worker_Slots_Type(-1, Worker_Type_Enum.Transporter))
        Next
        For x = 1 To Workers
            Slots.Add(New Worker_Slots_Type(-1, Worker_Type_Enum.Worker))
        Next
        For x = 1 To Gaurds
            'Slots.Add(New Worker_Slots_Type(-1, Worker_Type_Enum.Gaurd))
        Next
    End Sub

    Function Free_Worker_Space() As Integer
        Dim Count As Integer
        For Each item In Slots
            If item.ID = -1 AndAlso Not item.Type = Worker_Type_Enum.Tennents Then Count += 1
        Next
        Return Count
    End Function

End Class


Public Class Tennet_Slots
    Public Slots As New HashSet(Of Tennent_Slots_Type)()
    Sub New(ByVal Tennent_Slots As Integer)
        For x = 1 To Tennent_Slots
            Slots.Add(New Tennent_Slots_Type(-1, x - 1))
        Next
    End Sub
End Class


Public Class Planet_Building
    Public Owner As Integer
    Public BuildingRect As New Dictionary(Of Integer, Rectangle)()
    Public LandRect As Rectangle
    Public Type As building_type_enum
    Public Work_Slots As Worker_Slots
    Public Tennent_Slots As Tennet_Slots

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


    Sub New(ByVal Owner As Integer, ByVal LandRect As Rectangle, ByVal Type As building_type_enum, ByVal Work_Slots As Worker_Slots, ByVal Tennent_Slots As Tennet_Slots)
        Me.Owner = Owner
        Me.LandRect = LandRect
        Me.Type = Type
        Me.Work_Slots = Work_Slots
        Me.Tennent_Slots = Tennent_Slots
    End Sub

    Sub New(ByVal Owner As Integer, ByVal LandRect As Rectangle, ByVal Type As building_type_enum, ByVal Work_Slots As Worker_Slots)
        Me.Owner = Owner
        Me.LandRect = LandRect
        Me.Type = Type
        Me.Work_Slots = Work_Slots
        Me.Tennent_Slots = New Tennet_Slots(0)
    End Sub

End Class
