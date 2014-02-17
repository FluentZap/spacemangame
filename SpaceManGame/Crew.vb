Public Enum Work_Shift_Enum As Byte
    Morning
    Mid
    Night
End Enum

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




    Public Faction As Integer 'Civilian is 0 Player faction is 1
    Public location As PointD
    Public Sprite As Integer
    Public SpriteSet As character_sprite_set_enum
    Public buffs As buffs_enum
    Public speed As Double

    Public region As Officer_location_enum
    Public Location_ID As Integer


    'Ship
    Public no_ship_duty As Boolean
    Public working As Boolean
    Dim Points As crew_resource_type

    'Planet
    Public HomeBuilding As Integer = -1
    Public HomeSpace As Integer
    Public WorkBuilding As Integer = -1
    Public WorkShift As Work_Shift_Enum
    Public Worker_Type As Worker_Type_Enum
    Public BankBuilding As Integer
    Public PubBuilding As Integer
    Public Wealth As Integer


    'Public command_Queue As New LinkedList(Of crew_command_script)

    Public command_queue As New Queue(Of Crew_Command_script)
    'Public command_list As LinkedList(Of 
    Public Health As Officer.Actor_Stats_Class

    Public Fuel As Integer
    Public FuelMax As Integer
    Public Energy As Integer
    Public EnergyMax As Integer
    Public Perception As Integer

    Public Ani_Index As Integer
    Public Ani_Step As Integer
    Public Ani_Current As Basic_Animation
    Public Current_Animation As Unit_Animation_Name_Enum = Unit_Animation_Name_Enum.None

    Sub New(ByVal Faction As Integer, ByRef location As PointD, ByVal Location_ID As Integer, ByVal Region As Officer_location_enum, ByVal speed As Double, ByVal sprite_set As character_sprite_set_enum, ByVal sprite As character_sprite_enum, ByVal points As crew_resource_type)
        Me.Faction = Faction
        Me.location = location
        Me.Location_ID = Location_ID
        Me.region = Region
        Me.speed = speed
        Me.SpriteSet = sprite_set
        Me.Sprite = sprite
        Me.Points = points
        Me.Health = New Officer.Actor_Stats_Class(10, 10, 10)
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



    Function Get_Sprite() As Integer
        Return Sprite
    End Function


    Sub Update_Sprite()
        'Set correct animation
        Set_Correct_Animation()
        'Progress Animation
        If Ani_Index > Ani_Current.Frame.Count - 1 Then
            If Ani_Current.RepeatFrame > -1 Then
                Ani_Index = Ani_Current.RepeatFrame : Ani_Step = 0
            Else
                Sprite = Ani_Current.Frame(Ani_Current.Hold_Index).Sprite
                Ani_Current.Finished = True
            End If
        End If
        If Ani_Index <= Ani_Current.Frame.Count - 1 Then
            Sprite = Ani_Current.Frame(Ani_Index).Sprite
            Ani_Step += 1
            If Ani_Step >= Ani_Current.Frame(Ani_Index).Duration Then Ani_Index += 1 : Ani_Step = 0
        End If
    End Sub




    Sub Set_Correct_Animation()
        If Current_Animation = Unit_Animation_Name_Enum.None Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Stand_Up)
        If command_queue.Count > 0 Then

            If command_queue.Peek.type = crew_script_enum.move_to_tile Then
                Dim command As Crew.Command_move = DirectCast(command_queue.First, Crew.Command_move)
                If Math.Abs(location.x - command.dest.x) > Math.Abs(location.y - command.dest.y) Then
                    If location.x <= command.dest.x Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Right)
                    If location.x > command.dest.x Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Left)
                Else
                    If location.y <= command.dest.y Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Down)
                    If location.y > command.dest.y Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Up)
                End If
            End If

            If command_queue.Peek.type = crew_script_enum.hold Then
                set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Stand_Down)
            End If
        Else
            set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Stand_Down)
        End If

    End Sub


    Sub set_Animation(ByVal Animation As Unit_Animation_Name_Enum)
        If Not Animation = Current_Animation Then
            Ani_Index = 0
            Ani_Step = 0
            Ani_Current = Get_Animation(Animation)
            Current_Animation = Animation
        End If
    End Sub


End Class
