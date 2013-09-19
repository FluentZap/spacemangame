Public Class Ship_Weapon    
    Public Fire_Mode As Weapon_fire_mode_enum
    Public Range As Integer    

    Public Fire_Rate As Double
    Public Damage As Integer
    Public Damage_Radius As Integer

    Public Projectile_Speed As Double

    Sub New(ByVal Fire_Mode As Weapon_fire_mode_enum, ByVal Range As Integer, ByVal Fire_Rate As Integer, ByVal Damage As Integer, ByVal Damage_Radius As Integer, ByVal Projectile_Speed As Double)
        Me.Fire_Mode = Fire_Mode
        Me.Range = Range
        Me.Fire_Rate = Fire_Rate
        Me.Damage = Damage
        Me.Damage_Radius = Damage_Radius
        Me.Projectile_Speed = Projectile_Speed
    End Sub

End Class
