Public Module Device_Animations


    Public Enum Special_animation_action_enum
        None
        Hold_door
        Open_door
        Close_door
    End Enum

    Public Class Device_Animation_class
        Public tick As Integer
        Public Finished As Boolean = False
        Public frame() As ani_frame_type
        Public current_frame As Integer
        Public special_action As Special_animation_action_enum

        Sub New(ByVal animation As Animation_set_Enum, Optional ByVal special_action As Special_animation_action_enum = Special_animation_action_enum.None, Optional ByVal speed As Double = 1)
            frame = Get_animation_set(animation)
            Me.special_action = special_action
            If speed <> 1 Then
                For Each item In frame
                    item.duration = CByte(item.duration * speed)
                Next
            End If
        End Sub

        Function advance() As Byte            
            If current_frame < frame.Length Then
                If tick >= frame(current_frame).duration Then
                    current_frame += 1
                    tick = 0
                End If
                tick += 1
            Else
                Finished = True
            End If
            If current_frame = frame.Length Then
                Return frame(current_frame - 1).sprite_set
            Else
                Return frame(current_frame).sprite_set
            End If

        End Function

    End Class


    Class ani_frame_type
        Public duration As Integer
        Public sprite_set As Byte
        Sub New(ByVal duration As Integer, ByVal sprite_set As Byte)
            Me.duration = duration
            Me.sprite_set = sprite_set
        End Sub
    End Class


    Enum Animation_set_Enum
        Door_MK1_open
        Door_MK1_close
        Door_MK1_hold

        Door_MK2_open
        Door_MK2_close
        Door_MK2_hold


        Airlock_MK1_open
        Airlock_MK1_close
        Airlock_MK1_hold


    End Enum


    Function Get_animation_set(ByVal Animation_set As Animation_set_Enum) As ani_frame_type()
        Select Case Animation_set
            Case Is = Animation_set_Enum.Door_MK1_open
                Dim ani_set(11) As ani_frame_type
                For a As Byte = 0 To 11
                    ani_set(a) = New ani_frame_type(1, CByte(a + 1))
                Next a
                Return ani_set
            Case Is = Animation_set_Enum.Door_MK1_close
                Dim ani_set(11) As ani_frame_type
                For a As Byte = 0 To 11
                    ani_set(11 - a) = New ani_frame_type(1, CByte(a))
                Next a
                Return ani_set
            Case Is = Animation_set_Enum.Door_MK1_hold
                Dim ani_set(0) As ani_frame_type
                ani_set(0) = New ani_frame_type(100, 12)
                Return ani_set


            Case Is = Animation_set_Enum.Door_MK2_open
                Dim ani_set(8) As ani_frame_type
                For a As Byte = 0 To 8
                    ani_set(a) = New ani_frame_type(3, CByte(a + 1))
                Next a
                Return ani_set
            Case Is = Animation_set_Enum.Door_MK2_close
                Dim ani_set(8) As ani_frame_type
                For a As Byte = 0 To 8
                    ani_set(8 - a) = New ani_frame_type(3, CByte(a))
                Next a
                Return ani_set
            Case Is = Animation_set_Enum.Door_MK2_hold
                Dim ani_set(0) As ani_frame_type
                ani_set(0) = New ani_frame_type(100, 9)
                Return ani_set


            Case Is = Animation_set_Enum.Airlock_MK1_open
                Dim ani_set(19) As ani_frame_type
                For a As Byte = 0 To 19
                    ani_set(a) = New ani_frame_type(3, CByte(a + 1))
                Next a
                Return ani_set
            Case Is = Animation_set_Enum.Airlock_MK1_close
                Dim ani_set(19) As ani_frame_type
                For a As Byte = 0 To 19
                    ani_set(19 - a) = New ani_frame_type(3, CByte(a))
                Next a
                Return ani_set
            Case Is = Animation_set_Enum.Airlock_MK1_hold
                Dim ani_set(0) As ani_frame_type
                ani_set(0) = New ani_frame_type(100, 19)
                Return ani_set


        End Select
        Return Nothing
    End Function
End Module











Public Module Unit_Animations


    Class Animation_Frame
        Public Sprite As Integer
        Public Duration As Integer
        Sub New(ByVal Sprite As Integer, ByVal Duration As Integer)
            Me.Sprite = Sprite
            Me.Duration = Duration
        End Sub
    End Class

    Class Basic_Animation
        Public Frame() As Animation_Frame
        Public Finished As Boolean = False
        Public RepeatFrame As Integer = -1
        Public Hold_Index As Integer
    End Class




    Enum Unit_Animation_Name_Enum
        None
        Basic_Walk_Stand_Left
        Basic_Walk_Stand_Right
        Basic_Walk_Stand_Up
        Basic_Walk_Stand_Down
        Basic_Walk_Left
        Basic_Walk_Right
        Basic_Walk_Up
        Basic_Walk_Down
    End Enum



    Function Get_Animation(ByVal Animation_Name As Unit_Animation_Name_Enum) As Basic_Animation
        Dim Ani As New Basic_Animation
        Select Case Animation_Name
            '---------------Basic--------------
            Case Is = Unit_Animation_Name_Enum.Basic_Walk_Stand_Left
                Dim frames(0) As Animation_Frame
                frames(0) = New Animation_Frame(10, 0)
                Ani.Frame = frames
                Ani.Hold_Index = 0
            Case Is = Unit_Animation_Name_Enum.Basic_Walk_Stand_Right
                Dim frames(0) As Animation_Frame
                frames(0) = New Animation_Frame(15, 0)
                Ani.Frame = frames
                Ani.Hold_Index = 0
            Case Is = Unit_Animation_Name_Enum.Basic_Walk_Stand_Up
                Dim frames(0) As Animation_Frame
                frames(0) = New Animation_Frame(5, 0)
                Ani.Frame = frames
                Ani.Hold_Index = 0
            Case Is = Unit_Animation_Name_Enum.Basic_Walk_Stand_Down
                Dim frames(0) As Animation_Frame
                frames(0) = New Animation_Frame(0, 0)
                Ani.Frame = frames
                Ani.Hold_Index = 0
            Case Is = Unit_Animation_Name_Enum.Basic_Walk_Left
                Dim frames(3) As Animation_Frame
                frames(0) = New Animation_Frame(11, 15)
                frames(1) = New Animation_Frame(12, 15)
                frames(2) = New Animation_Frame(13, 15)
                frames(3) = New Animation_Frame(14, 15)
                Ani.Frame = frames
                Ani.RepeatFrame = 0
            Case Is = Unit_Animation_Name_Enum.Basic_Walk_Right
                Dim frames(3) As Animation_Frame
                frames(0) = New Animation_Frame(16, 15)
                frames(1) = New Animation_Frame(17, 15)
                frames(2) = New Animation_Frame(18, 15)
                frames(3) = New Animation_Frame(19, 15)
                Ani.Frame = frames
                Ani.RepeatFrame = 0
            Case Is = Unit_Animation_Name_Enum.Basic_Walk_Up
                Dim frames(3) As Animation_Frame
                frames(0) = New Animation_Frame(6, 15)
                frames(1) = New Animation_Frame(7, 15)
                frames(2) = New Animation_Frame(8, 15)
                frames(3) = New Animation_Frame(9, 14)
                Ani.Frame = frames
                Ani.RepeatFrame = 0
            Case Is = Unit_Animation_Name_Enum.Basic_Walk_Down
                Dim frames(3) As Animation_Frame
                frames(0) = New Animation_Frame(1, 15)
                frames(1) = New Animation_Frame(2, 15)
                frames(2) = New Animation_Frame(3, 15)
                frames(3) = New Animation_Frame(4, 15)
                Ani.Frame = frames
                Ani.RepeatFrame = 0
        End Select


        Return Ani
    End Function


    
End Module



Module External_Animations
    Class Ship_Basic_Animation
        Public Sprite As Effect_Sprite_Enum
        Public Frame() As Animation_Frame
        Public Finished As Boolean = False
        Public RepeatFrame As Integer = -1        
    End Class

    Enum Effect_Sprite_Enum
        Engine_Mk1
    End Enum

End Module