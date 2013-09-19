<Serializable()> Public Class Crew

#Region "Crew Script Classes"
    <Serializable()> Public MustInherit Class Crew_Command_script
        Public status As script_status_enum = script_status_enum.uninvoked
        Public type As crew_script_enum
    End Class

    <Serializable()> Public Class Command_move
        Inherits Crew_Command_script
        Public dest As PointD
        Public dx As Double
        Public dy As Double
        Public residual_movement As Double

        Sub New(ByRef dest As PointD)
            type = crew_script_enum.move_to_tile
            Me.dest = dest
            residual_movement = Nothing
        End Sub

    End Class


    <Serializable()> Public Class Command_set_room
        Inherits Crew_Command_script
        Sub New()
            type = crew_script_enum.set_room
        End Sub
    End Class


    <Serializable()> Public Class Command_hold
        Inherits Crew_Command_script
        Public hold_length As Integer
        Public count As Integer

        Sub New(ByVal length As Integer)
            hold_length = length
            count = 0
            type = crew_script_enum.hold
        End Sub
    End Class

    <Serializable()> Public Class Command_Open_Door
        Inherits Crew_Command_script
        Public Door_location As PointI
        Public Set_open As Boolean = False

        Sub New(ByVal Door_location As PointI)
            Me.Door_location = Door_location
            type = crew_script_enum.open_door
        End Sub
    End Class

#End Region

    Public Faction As Integer
    Public location As PointD
    Public HpMax As Integer
    Public Hp As Integer
    Public Sprite As character_sprite_enum
    Public SpriteSet As character_sprite_set_enum
    Public buffs As buffs_enum
    Public speed As Double
    Public no_ship_duty As Boolean
    Public working As Boolean

    Dim Points As crew_resource_type

    'Public command_Queue As New LinkedList(Of crew_command_script)

    Public command_queue As New Queue(Of Crew_Command_script)
    'Public command_list As LinkedList(Of 


    Sub New(ByVal Faction As Integer, ByRef location As PointD, ByVal speed As Double, ByVal hp As Integer, ByVal sprite_set As character_sprite_set_enum, ByVal sprite As character_sprite_enum, ByVal points As crew_resource_type)
        Me.Faction = Faction
        Me.location = location
        Me.speed = speed
        Me.Hp = hp
        Me.SpriteSet = sprite_set
        Me.Sprite = sprite
        Me.Points = points
    End Sub

    Function find_tile() As PointI
        Dim pos As PointI
        pos.x = Convert.ToInt32(location.x) \ TILE_SIZE
        pos.y = Convert.ToInt32(location.y) \ TILE_SIZE

        Return pos
    End Function


    Function find_rect() As Rectangle
        Dim rect As Rectangle
        rect.X = location.intX
        rect.Y = location.intY
        rect.Width = 32
        rect.Height = 32        
        Return rect
    End Function


    Function Get_points() As crew_resource_type
        Return Points
    End Function


End Class
