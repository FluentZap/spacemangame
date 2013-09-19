Module MainMenu

    Public Function main_menu() As Integer

        Dim choice As Integer = -1


        Dim pressedkeys As New HashSet(Of Keys)
        Dim mouse_info As New MainForm.mouse_info_type

        Dim menu_item(6) As Menu_button

        'menu_item(0) = New Menu_button(button_texture_enum.main_menu__newgame, New Rectangle(Convert.ToInt32(screen_size.x / 2 - 125), Convert.ToInt32(screen_size.y / 2 + 80), 250, 40), button_style.Both)
        'menu_item(1) = New Menu_button(button_texture_enum.main_menu__loadgame, New Rectangle(Convert.ToInt32(screen_size.x / 2 - 125), Convert.ToInt32(screen_size.y / 2 + 160), 250, 40), button_style.Both)
        'menu_item(2) = New Menu_button(button_texture_enum.main_menu__quit, New Rectangle(Convert.ToInt32(screen_size.x / 2 - 125), Convert.ToInt32(screen_size.y / 2 + 320), 250, 40), button_style.Both)
        Dim pos As PointI = New PointI(screen_size.x / 2 - 125, screen_size.y / 2)
        menu_item(0) = New Menu_button(button_texture_enum.main_menu__button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Personal", Color.White, d3d_font_enum.Big_Button) : pos.y += 40
        menu_item(1) = New Menu_button(button_texture_enum.main_menu__button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Ship Build", Color.White, d3d_font_enum.Big_Button) : pos.y += 40
        menu_item(2) = New Menu_button(button_texture_enum.main_menu__button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Star Map", Color.White, d3d_font_enum.Big_Button) : pos.y += 40
        menu_item(3) = New Menu_button(button_texture_enum.main_menu__button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "External", Color.White, d3d_font_enum.Big_Button) : pos.y += 40
        menu_item(4) = New Menu_button(button_texture_enum.main_menu__button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Weapon Control", Color.White, d3d_font_enum.Big_Button) : pos.y += 40
        menu_item(5) = New Menu_button(button_texture_enum.main_menu__button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Level up screen", Color.White, d3d_font_enum.Big_Button) : pos.y += 40
        menu_item(6) = New Menu_button(button_texture_enum.main_menu__button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Personal", Color.White, d3d_font_enum.Big_Button) : pos.y += 40


        Dim left_down_point As PointI = Nothing
        Dim left_release_point As PointI = Nothing

        MainForm.getUI(pressedkeys, mouse_info)

        Dim time_start, time_current, cps, rate As Long
        QueryPerformanceFrequency(cps)
        rate = Convert.ToInt64(cps / 60)

        Do
            QueryPerformanceCounter(time_start)
            render_main_menu(menu_item)
            Application.DoEvents()

            ' do the selection/highliting
            calculate_button_selection(menu_item, mouse_info)

            ' detect a click
            If mouse_info.get_left_click(left_down_point, left_release_point) Then
                For i = 0 To 6
                    If menu_item(i).bounds.Contains(left_down_point.x, left_down_point.y) And menu_item(i).bounds.Contains(left_release_point.x, left_release_point.y) Then
                        choice = i
                        Exit For
                    End If
                Next
            End If

            If pressedkeys.Contains(Keys.Escape) Then
                choice = 2
            End If

            If pressedkeys.Contains(Keys.F10) Then pressedkeys.Remove(Keys.F10) : lock_screen()

            Do
                QueryPerformanceCounter(time_current)
            Loop Until (time_current - time_start) >= rate

        Loop While choice = -1

        Return choice
    End Function

End Module
