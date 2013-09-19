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

        End Select
        Return Nothing
    End Function
End Module