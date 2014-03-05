<Serializable()> Public Class Personality_Type






End Class


<Serializable()> Public Class Officer
    Public Officer_Classes As HashSet(Of Officer_Class) = New HashSet(Of Officer_Class)
    Public Current_Class As Class_List_Enum

    Public Buff_List As HashSet(Of Buff_List_Enum) = New HashSet(Of Buff_List_Enum)
    Public Ability_List As HashSet(Of Ability_List_Enum) = New HashSet(Of Ability_List_Enum)

    Public Skill_List As HashSet(Of Skill_List_Enum) = New HashSet(Of Skill_List_Enum)

    Public Ani_Index As Integer
    Public Ani_Step As Integer
    Public Ani_Current As Basic_Animation
    Public Current_Animation As Unit_Animation_Name_Enum = Unit_Animation_Name_Enum.None

    Dim faction As Integer
    Public name As String
    Public location As PointD

    Dim center_point As PointI

    Public speed As Double
    Public StrafeSpeed As Double

    Public view_range As Integer

    Public region As Officer_location_enum
    Public Location_ID As Integer

    Public input_flages As New officer_input_flages

    Public Fuel As Integer
    Public FuelMax As Integer
    Public Energy As Integer
    Public EnergyMax As Integer
    Public Health As New Actor_Stats_Class(10, 10, 10)


    Class Actor_Stats_Class
        Public Endurace As Byte
        Public Agility As Byte
        Public Perception As Byte


        Public Head As Byte
        Public Torso As Byte
        Public LeftArm As Byte
        Public RightArm As Byte
        Public LeftLeg As Byte
        Public RightLeg As Byte
        Public HeadM As Byte
        Public TorsoM As Byte
        Public LeftArmM As Byte
        Public RightArmM As Byte
        Public LeftLegM As Byte
        Public RightLegM As Byte

        Sub New(ByVal Endurance As Byte, ByVal Agility As Byte, ByVal Perception As Byte)
            Dim Health As Byte = 5
            Head = CByte(Health + 5)
            Torso = CByte(Health + 10)
            LeftArm = Health
            RightArm = Health
            LeftLeg = Health
            RightLeg = Health
            HeadM = CByte(Health + 5)
            TorsoM = CByte(Health + 10)
            LeftArmM = Health
            RightArmM = Health
            LeftLegM = Health
            RightLegM = Health
        End Sub

        Sub Damage_All_Limbs(ByVal A As Byte)
            If A > Head Then Head = 0 Else Head -= A
            If A > Torso Then Torso = 0 Else Torso -= A
            If A > LeftArm Then LeftArm = 0 Else LeftArm -= A
            If A > RightArm Then RightArm = 0 Else RightArm -= A
            If A > LeftLeg Then LeftLeg = 0 Else LeftLeg -= A
            If A > RightLeg Then RightLeg = 0 Else RightLeg -= A
        End Sub




    End Class


    <Serializable()> Public Class officer_input_flages
        Public walking As Boolean
        Public MoveX As Move_Direction
        Public MoveY As Move_Direction
        Public Facing As Move_Direction

        Sub New()
            Me.Facing = Move_Direction.Down
        End Sub

    End Class



    <Serializable()> Public Class Equipment_Class
        'Public Chest As 

    End Class




    <Serializable()> Structure sprite_list
        Dim Sprite As Integer

        Dim Head_SpriteSet As character_sprite_set_enum
        Dim Torso_SpriteSet As character_sprite_set_enum
        Dim Left_Arm_SpriteSet As character_sprite_set_enum
        Dim Right_Arm_SpriteSet As character_sprite_set_enum
        Dim Left_Leg_SpriteSet As character_sprite_set_enum
        Dim Right_Leg_SpriteSet As character_sprite_set_enum

        Sub New(ByVal Sprite_Set As character_sprite_set_enum, ByVal Sprite As Integer)
            Me.Sprite = Sprite

            Me.Head_SpriteSet = Sprite_Set
            Me.Torso_SpriteSet = Sprite_Set
            Me.Left_Arm_SpriteSet = Sprite_Set
            Me.Right_Arm_SpriteSet = Sprite_Set
            Me.Left_Leg_SpriteSet = Sprite_Set
            Me.Right_Leg_SpriteSet = Sprite_Set

        End Sub

        Sub New(ByVal Sprite As Integer, ByVal Head_SpriteSet As character_sprite_set_enum, ByVal Torso_SpriteSet As character_sprite_set_enum, ByVal Left_Arm_SpriteSet As character_sprite_set_enum, ByVal Right_Arm_SpriteSet As character_sprite_set_enum, ByVal Left_Leg_priteSet As character_sprite_set_enum, ByVal Right_Leg_SpriteSet As character_sprite_set_enum)
            Me.Sprite = Sprite
            Me.Head_SpriteSet = Head_SpriteSet
            Me.Torso_SpriteSet = Torso_SpriteSet
            Me.Left_Arm_SpriteSet = Left_Arm_SpriteSet
            Me.Right_Arm_SpriteSet = Right_Arm_SpriteSet
            Me.Left_Leg_SpriteSet = Left_Leg_SpriteSet
            Me.Right_Leg_SpriteSet = Right_Leg_SpriteSet
        End Sub

    End Structure


    Public sprite As sprite_list
    'Attributes

    Sub New(ByVal faction As Integer, ByVal Name As String, ByVal Region As Officer_location_enum, ByVal Location_Id As Integer, ByVal location As PointD, ByVal speed As Double, ByVal StrafeSpeed As Double, ByVal Sprite As sprite_list)
        Me.name = Name
        Me.region = Region
        Me.Location_ID = Location_Id
        Me.location = location
        Me.speed = speed
        Me.StrafeSpeed = StrafeSpeed
        Me.sprite = Sprite
        Me.Officer_Classes.Add(New Officer_Class(Class_List_Enum.Engineer, random(0, 99), CByte(random(0, 40))))
        Me.Officer_Classes.Add(New Officer_Class(Class_List_Enum.Security, random(0, 99), CByte(random(0, 40))))
        Me.Officer_Classes.Add(New Officer_Class(Class_List_Enum.Scientist, random(0, 99), CByte(random(0, 40))))
        Me.Officer_Classes.Add(New Officer_Class(Class_List_Enum.Aviator, random(0, 99), CByte(random(0, 40))))
        Me.Officer_Classes(Class_List_Enum.Engineer).Skill_Points = 2
        Me.Officer_Classes(Class_List_Enum.Security).Skill_Points = 5
        'Me.Officer_Classes(Class_List_Enum.Scientist).Skill_Points = 5
        Recalculate_Abilities_Buffs()
    End Sub

    Sub Move(ByVal Vector As PointD)
        Me.location.x += Vector.x
        Me.location.y += Vector.y
    End Sub



    Sub SetLocation(ByVal Vector As PointD)
        Me.location.x = Vector.x
        Me.location.y = Vector.y
    End Sub


    Function GetLocation() As PointI
        Return location.PointI
    End Function

    Function GetLocationD() As PointD
        Return location
    End Function

    Function find_tile() As PointI
        Dim pos As PointI
        pos.x = Convert.ToInt32(location.x + TILE_SIZE / 2) \ TILE_SIZE
        pos.y = Convert.ToInt32(location.y + TILE_SIZE / 2) \ TILE_SIZE

        Return pos
    End Function


    Function find_rect() As Rectangle
        Dim rect As Rectangle
        rect.X = location.intX + 8
        rect.Y = location.intY + 27
        rect.Width = 15
        rect.Height = 4
        Return rect
    End Function


    Function GetClass(ByVal Class_Name As Class_List_Enum) As Officer_Class
        For Each item In Officer_Classes
            If item.ClassID = Class_Name Then Return item
        Next
        Return Nothing
    End Function


    Function GetClassByNumber(ByVal Number As Integer) As Officer_Class
        Dim a As Integer = 0
        For Each item In Officer_Classes
            If a = Number + 4 Then Return item
            a += 1
        Next
        Return Nothing
    End Function


    Function GetCurrentClass() As Officer_Class
        For Each item In Officer_Classes
            If item.ClassID = Current_Class Then Return item
        Next
        Return New Officer_Class(0, 0, 0)
    End Function

    Function Get_Sprite() As Integer
        Return sprite.Sprite
    End Function








    Sub Update_Sprite()
        'Set correct animation
        Set_Correct_Animation()
        'Progress Animation
        If Ani_Index > Ani_Current.Frame.Count - 1 Then
            If Ani_Current.RepeatFrame > -1 Then
                Ani_Index = Ani_Current.RepeatFrame : Ani_Step = 0
            Else
                sprite.Sprite = Ani_Current.Frame(Ani_Current.Hold_Index).Sprite
                Ani_Current.Finished = True
            End If
        End If
        If Ani_Index <= Ani_Current.Frame.Count - 1 Then
            sprite.Sprite = Ani_Current.Frame(Ani_Index).Sprite
            Ani_Step += 1
            If Ani_Step >= Ani_Current.Frame(Ani_Index).Duration Then Ani_Index += 1 : Ani_Step = 0
        End If
    End Sub



    Sub Recalculate_Abilities_Buffs()

        'Me.Officer_Classes(Current_Class).Ability_List.Add(Ability_List_Enum.Mage__Fireball)
        'Me.Officer_Classes(Current_Class).Buff_List.Add(Buff_List_Enum.Buffs)
        Dim Tree As New Class_Tech_Tree(Current_Class)
        For Each item In Tree.Skills

            If (item.Value.Inherited = True AndAlso Officer_Classes(Current_Class).Level >= item.Value.Req_Level) OrElse Skill_List.Contains(item.Key) Then
                If Not Skill_List.Contains(item.Key) Then Skill_List.Add(item.Key)
                For Each a In item.Value.Ability_List
                    If Not Ability_List.Contains(a) Then Ability_List.Add(a)
                Next

                For Each b In item.Value.Buff_List
                    If Not Buff_List.Contains(b) Then Buff_List.Add(b)
                Next

            End If

        Next



    End Sub



    Sub Set_Correct_Animation()
        If Current_Animation = Unit_Animation_Name_Enum.None Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Stand_Down)
        If input_flages.walking = True Then

            If input_flages.Facing = Move_Direction.Up Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Up)
            If input_flages.Facing = Move_Direction.Down Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Down)
            If input_flages.Facing = Move_Direction.Left Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Left)
            If input_flages.Facing = Move_Direction.Right Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Right)

        Else
            If input_flages.MoveX = Move_Direction.None AndAlso input_flages.MoveY = Move_Direction.None Then
                If Current_Animation = Unit_Animation_Name_Enum.Basic_Walk_Left Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Stand_Left)
                If Current_Animation = Unit_Animation_Name_Enum.Basic_Walk_Right Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Stand_Right)
                If Current_Animation = Unit_Animation_Name_Enum.Basic_Walk_Up Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Stand_Up)
                If Current_Animation = Unit_Animation_Name_Enum.Basic_Walk_Down Then set_Animation(Unit_Animation_Name_Enum.Basic_Walk_Stand_Down)
            End If
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
