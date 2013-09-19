Public Class Projectile
    Public Location As PointD
    Public vector_velocity As PointD
    Public Rotation As Double
    Public Warhead_Type As Integer
    Public Damage As Integer = 1
    Public Life As Integer
    Public Second_Stage_Life As Integer

    Sub New(ByVal Location As PointD, ByVal Vector_velocity As PointD, ByVal Rotation As Double, ByVal Life As Integer)
        Me.Location = Location
        Me.vector_velocity = Vector_velocity
        Me.Rotation = Rotation
        Me.Life = Life
    End Sub

    Sub New(ByVal Location As PointD, ByVal Vector_velocity As PointD, ByVal Rotation As Double, ByVal Life As Integer, ByVal Second_Stage_Life As Integer)
        Me.Location = Location
        Me.vector_velocity = Vector_velocity
        Me.Rotation = Rotation
        Me.Life = Life
        Me.Second_Stage_Life = Second_Stage_Life
    End Sub



End Class
