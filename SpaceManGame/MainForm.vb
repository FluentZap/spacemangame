Public Class MainForm
    ' made this a class instead of struct so it can be passed by reference
    Public Class mouse_info_type
        Public left_down As Boolean = False
        Public middle_down As Boolean = False
        Public right_down As Boolean = False

        Public position As PointI = New PointI(0, 0)

        Private left_click As Boolean = False
        Public left_down_point As New PointI(0, 0)
        Public left_release_point As New PointI(0, 0)

        Private right_click As Boolean = False
        Public right_down_point As New PointI(0, 0)
        Public right_release_point As New PointI(0, 0)


        Public wheel As Integer

        Public Function get_left_click() As Boolean
            If left_click Then
                left_click = False
                Return True
            Else
                Return False
            End If
        End Function
        Public Function get_left_click(ByRef dest_down_point As PointI, ByRef dest_release_point As PointI) As Boolean
            If left_click Then
                dest_down_point = left_down_point
                dest_release_point = left_release_point
                left_click = False
                Return True
            Else
                Return False
            End If
        End Function
        Public Sub set_left_click(ByVal value As Boolean)
            left_click = value
        End Sub

        Public Function get_right_click() As Boolean
            If right_click Then
                right_click = False
                Return True
            Else
                Return False
            End If
        End Function
        Public Function get_right_click(ByRef dest_down_point As PointI, ByRef dest_release_point As PointI) As Boolean
            If right_click Then
                dest_down_point = right_down_point
                dest_release_point = right_release_point
                right_click = False
                Return True
            Else
                Return False
            End If
        End Function
        Public Sub set_right_click(ByVal value As Boolean)
            right_click = value
        End Sub

    End Class

    Private pressedkeys As New HashSet(Of Keys)
    Private mouse_info As New mouse_info_type


    Private Sub MainForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If pressedkeys.Contains(e.KeyCode) Then
        Else
            pressedkeys.Add(e.KeyCode)            
        End If
    End Sub

    Private Sub MainForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If pressedkeys.Contains(e.KeyCode) Then
            pressedkeys.Remove(e.KeyCode)
        End If
    End Sub

    Sub getUI(ByRef pressedkeys As HashSet(Of Keys), ByRef mouse_info As mouse_info_type)
        pressedkeys = Me.pressedkeys
        mouse_info = Me.mouse_info        
    End Sub

    Private Sub MainForm_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        Screen_Lock = False
    End Sub



    Private Sub MainForm_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            mouse_info.left_down = True
            mouse_info.left_down_point.x = e.Location.X
            mouse_info.left_down_point.y = e.Location.Y
        ElseIf e.Button = Windows.Forms.MouseButtons.Middle Then
            mouse_info.middle_down = True
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            mouse_info.right_down = True
            mouse_info.right_down_point.x = e.Location.X
            mouse_info.right_down_point.y = e.Location.Y
        End If
        d3d_device.ShowCursor(True)
    End Sub

    Private Sub MainForm_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter
        'Me.Cursor = New Cursor(Application.StartupPath + "\data\textures\cursor\cur1.cur")
        'Cursor.Hide()
    End Sub

    Private Sub MainForm_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        'Cursor.Show()
    End Sub

    Private Sub MainForm_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        mouse_info.position.x = e.Location.X
        mouse_info.position.y = e.Location.Y
        d3d_device.ShowCursor(True)
    End Sub

    Private Sub MainForm_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            mouse_info.left_down = False
            mouse_info.set_left_click(True)
            mouse_info.left_release_point.x = e.Location.X
            mouse_info.left_release_point.y = e.Location.Y
        ElseIf e.Button = Windows.Forms.MouseButtons.Middle Then
            mouse_info.middle_down = False
        ElseIf  e.Button = Windows.Forms.MouseButtons.Right Then 
            mouse_info.right_down = False
            mouse_info.set_right_click(True)
            mouse_info.right_release_point.x = e.Location.X
            mouse_info.right_release_point.y = e.Location.Y
        End If

        d3d_device.ShowCursor(True)
    End Sub

    Private Sub MainForm_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        Me.mouse_info.wheel = e.Delta
    End Sub

End Class
