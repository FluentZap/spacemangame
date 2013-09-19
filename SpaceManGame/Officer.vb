<Serializable()> Public Class Officer
    Public Officer_Classes As HashSet(Of Officer_Class) = New HashSet(Of Officer_Class)
    Public Current_Class As Class_List_Enum
    Dim faction As Integer
    Public name As String
    Dim location As PointD

    Dim center_point As PointI

    Public speed As Double
    Public view_range As Integer

    Public region As Officer_location_enum
    Public Location_ID As Integer

    Public input_flages As officer_input_flages

    <Serializable()> Public Structure officer_input_flages
        Dim walking As Boolean
        Dim FaceLeft As Move_Direction
        Dim FaceUp As Move_Direction
    End Structure


    <Serializable()> Structure sprite_list
        Dim Head_Sprite As character_sprite_enum
        Dim Torso_Sprite As character_sprite_enum
        Dim Left_Arm_Sprite As character_sprite_enum
        Dim Right_Arm_Sprite As character_sprite_enum
        Dim Left_Leg_Sprite As character_sprite_enum
        Dim Right_Leg_Sprite As character_sprite_enum

        Dim Head_SpriteSet As character_sprite_set_enum
        Dim Torso_SpriteSet As character_sprite_set_enum
        Dim Left_Arm_SpriteSet As character_sprite_set_enum
        Dim Right_Arm_SpriteSet As character_sprite_set_enum
        Dim Left_Leg_SpriteSet As character_sprite_set_enum
        Dim Right_Leg_SpriteSet As character_sprite_set_enum

        Sub New(ByVal Sprite_Set As character_sprite_set_enum, ByVal Sprite As character_sprite_enum)
            Me.Head_Sprite = Sprite
            Me.Torso_Sprite = Sprite
            Me.Left_Arm_Sprite = Sprite
            Me.Right_Arm_Sprite = Sprite
            Me.Left_Leg_Sprite = Sprite
            Me.Right_Leg_Sprite = Sprite

            Me.Head_SpriteSet = Sprite_Set
            Me.Torso_SpriteSet = Sprite_Set
            Me.Left_Arm_SpriteSet = Sprite_Set
            Me.Right_Arm_SpriteSet = Sprite_Set
            Me.Left_Leg_SpriteSet = Sprite_Set
            Me.Right_Leg_SpriteSet = Sprite_Set

        End Sub

        Sub New(ByVal Head_Sprite As character_sprite_enum, ByVal Torso_Sprite As character_sprite_enum, ByVal Left_Arm_Sprite As character_sprite_enum, ByVal Right_Arm_Sprite As character_sprite_enum, ByVal Left_Leg_Sprite As character_sprite_enum, ByVal Right_Leg_Sprite As character_sprite_enum, ByVal Head_SpriteSet As character_sprite_set_enum, ByVal Torso_SpriteSet As character_sprite_set_enum, ByVal Left_Arm_SpriteSet As character_sprite_set_enum, ByVal Right_Arm_SpriteSet As character_sprite_set_enum, ByVal Left_Leg_priteSet As character_sprite_set_enum, ByVal Right_Leg_SpriteSet As character_sprite_set_enum)
            Me.Head_Sprite = Head_Sprite
            Me.Torso_Sprite = Torso_Sprite
            Me.Left_Arm_Sprite = Left_Arm_Sprite
            Me.Right_Arm_Sprite = Right_Arm_Sprite
            Me.Left_Leg_Sprite = Left_Leg_Sprite
            Me.Right_Leg_Sprite = Right_Leg_Sprite

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

    Sub New(ByVal faction As Integer, ByVal Name As String, ByVal Region As Officer_location_enum, ByVal Location_Id As Integer, ByVal location As PointD, ByVal speed As Double, ByVal Sprite As sprite_list)
        Me.name = Name
        Me.region = Region
        Me.Location_ID = Location_Id
        Me.location = location
        Me.speed = speed
        Me.sprite = Sprite
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

    Function GetCurrentClass() As Officer_Class
        For Each item In Officer_Classes
            If item.ClassID = Current_Class Then Return item
        Next
        Return New Officer_Class(0, 0, 0)
    End Function


End Class
