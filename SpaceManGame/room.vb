﻿<Serializable()> Public Class crew_resource_type
    Public engineering As Integer
    Public science As Integer

    Sub New(ByVal Eng As Integer, ByVal Sci As Integer)
        engineering = Eng
        science = Sci
    End Sub

    Sub New()
    End Sub
End Class

<Serializable()> Public Class access_point_type
    Public Manned As Boolean
    Public Base_Device As Integer

    Sub New(ByVal Manned As Boolean, ByVal Base_Device As Integer)
        Me.Manned = Manned
        Me.Base_Device = Base_Device
    End Sub


End Class



<Serializable()> Public Class room_type
    Public type As tech_list_enum
    Public required_crew_resources As New crew_resource_type
    Public available_crew_resources As New crew_resource_type
    Public efficiency As New crew_resource_type    

    Public working_crew_list As HashSet(Of Integer) = New HashSet(Of Integer)
    Public assigned_crew_list As HashSet(Of Integer) = New HashSet(Of Integer)

    Public device_list As HashSet(Of Integer) = New HashSet(Of Integer)
    Public access_point As Dictionary(Of PointI, access_point_type) = New Dictionary(Of PointI, access_point_type)


End Class
