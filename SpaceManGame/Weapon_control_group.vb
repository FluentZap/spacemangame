Public Class Weapon_control_group

    Enum Ship_target_priority_enum
        Fighter
        Corvette
        Frigate
        Capital
    End Enum

    Public Name As String
    Public Connected_Weapons As HashSet(Of Integer) = New HashSet(Of Integer)
    Public Target As Integer
    Public Target_Priority As List(Of Ship_target_priority_enum)


    Sub New(ByVal Name As String)
        Me.Name = Name



    End Sub





End Class
