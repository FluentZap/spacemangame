<Serializable()> Public Class Officer
    Dim faction As Integer
    Dim name As String
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
        Dim Left_Leg_priteSet As character_sprite_set_enum
        Dim Right_Leg_SpriteSet As character_sprite_set_enum


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
            Me.Left_Leg_priteSet = Left_Leg_priteSet
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

End Class
