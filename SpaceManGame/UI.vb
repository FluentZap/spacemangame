Module UI

    Sub Personal_UI(ByVal rate As Double)
        Dim amount As Double = 1
        If pressedkeys.Contains(Keys.ShiftKey) Then amount = 50
        If pressedkeys.Contains(Keys.Z) Then personal_zoom += 0.2F : pressedkeys.Remove(Keys.Z)
        If pressedkeys.Contains(Keys.X) Then personal_zoom -= 0.2F : pressedkeys.Remove(Keys.X)

        If mouse_info.wheel > 0 Then
            'personal_zoom += Math.Ceiling((mouse_info.wheel / 240) * personal_zoom)
        ElseIf mouse_info.wheel < 0 Then
            'personal_zoom += Math.Ceiling((mouse_info.wheel / 240) * personal_zoom)
        End If
        mouse_info.wheel = 0

        If personal_zoom < 1 Then personal_zoom = 1
        If personal_zoom > 20 Then personal_zoom = 20
        If pressedkeys.Contains(Keys.ShiftKey) Then personal_zoom = 0.125
        'If (pressedkeys.Contains(Keys.W) AndAlso pressedkeys.Contains(Keys.A)) OrElse (pressedkeys.Contains(Keys.W) AndAlso pressedkeys.Contains(Keys.D)) Then amount = Convert.ToSingle(amount * DIAGONAL_SPEED_MODIFIER)
        'If (pressedkeys.Contains(Keys.S) AndAlso pressedkeys.Contains(Keys.A)) OrElse (pressedkeys.Contains(Keys.S) AndAlso pressedkeys.Contains(Keys.D)) Then amount = Convert.ToSingle(amount * DIAGONAL_SPEED_MODIFIER)


        If Officer_List(current_player).region = Officer_location_enum.Ship Then
            'have to be separate
            If pressedkeys.Contains(Keys.W) Then Ship_List(current_selected_ship_view).MoveOfficer(0, New PointD(0, -amount))
            If pressedkeys.Contains(Keys.S) Then Ship_List(current_selected_ship_view).MoveOfficer(0, New PointD(0, amount))
            If pressedkeys.Contains(Keys.A) Then Ship_List(current_selected_ship_view).MoveOfficer(0, New PointD(-amount, 0))
            If pressedkeys.Contains(Keys.D) Then Ship_List(current_selected_ship_view).MoveOfficer(0, New PointD(amount, 0))


            view_location_personal.x = Ship_List(current_selected_ship_view).GetOfficer.Item(0).GetLocationD.x - (screen_size.x / 2 / personal_zoom) + 16
            view_location_personal.y = Ship_List(current_selected_ship_view).GetOfficer.Item(0).GetLocationD.y - (screen_size.y / 2 / personal_zoom) + 16


            If view_location_personal.x < -(screen_size.x / 2) Then view_location_personal.x = -(screen_size.x \ 2)
            If view_location_personal.x > Ship_List.Item(current_selected_ship_view).GetShipSize.x * 32 * personal_zoom - screen_size.x / 2 Then view_location_personal.x = CInt(Ship_List.Item(current_selected_ship_view).GetShipSize.x * 32 * personal_zoom) - screen_size.x \ 2

            If view_location_personal.y < -(screen_size.y / 2) Then view_location_personal.y = -(screen_size.y \ 2)
            If view_location_personal.y > Ship_List.Item(current_selected_ship_view).GetShipSize.y * 32 * personal_zoom - screen_size.y / 2 Then view_location_personal.y = CInt(Ship_List.Item(current_selected_ship_view).GetShipSize.y * 32 * personal_zoom) - screen_size.y \ 2
        End If

        If Officer_List(current_player).region = Officer_location_enum.Planet Then
            'have to be separate
            If pressedkeys.Contains(Keys.W) Then Planet_List(Officer_List(current_player).Location_ID).MoveOfficer(0, New PointD(0, -amount * rate))
            If pressedkeys.Contains(Keys.S) Then Planet_List(Officer_List(current_player).Location_ID).MoveOfficer(0, New PointD(0, amount * rate))
            If pressedkeys.Contains(Keys.A) Then Planet_List(Officer_List(current_player).Location_ID).MoveOfficer(0, New PointD(-amount * rate, 0))
            If pressedkeys.Contains(Keys.D) Then Planet_List(Officer_List(current_player).Location_ID).MoveOfficer(0, New PointD(amount * rate, 0))

            'Planet_List(Officer_List(current_player).Location_ID).MoveOfficer(0, New PointD(amount * rate, 0))
            'Planet_List(Officer_List(1).Location_ID).MoveOfficer(1, New PointD(amount, 0))

            'If pressedkeys.Contains(Keys.W) Then view_location_personal.y -= 1 'CInt(3 * rate)
            'If pressedkeys.Contains(Keys.S) Then view_location_personal.y += 1 'CInt(3 * rate)
            'If pressedkeys.Contains(Keys.A) Then view_location_personal.x -= 1 'CInt(3 * rate)
            'If pressedkeys.Contains(Keys.D) Then view_location_personal.x += 1 'CInt(3 * rate)

            view_location_personal_Last = view_location_personal

            view_location_personal.x = Officer_List(current_player).GetLocationD.x + 16 - CInt((screen_size.x \ 2) / personal_zoom)
            view_location_personal.y = Officer_List(current_player).GetLocationD.y + 16 - CInt((screen_size.y \ 2) / personal_zoom)
        End If



    End Sub




    Sub test_ui()
        If pressedkeys.Contains(Keys.W) Then view_location_internal.y -= 1
        If pressedkeys.Contains(Keys.S) Then view_location_internal.y += 1
        If pressedkeys.Contains(Keys.A) Then view_location_internal.x -= 1
        If pressedkeys.Contains(Keys.D) Then view_location_internal.x += 1


    End Sub


    Sub Internal_UI()
        Dim amount As Double = 1
        Dim right_click_location As PointI
        Dim atsize As Integer = Convert.ToInt32(32 * internal_zoom)

        If mouse_info.left_down = True Then
            Dim selection As PointI
            selection.x = Convert.ToInt32(view_location_internal.x + mouse_info.position.x / internal_zoom) \ 32
            selection.y = Convert.ToInt32(view_location_internal.y + mouse_info.position.y / internal_zoom) \ 32
            If selection.x >= 0 AndAlso selection.x <= Ship_List(0).shipsize.x AndAlso selection.y >= 0 AndAlso selection.y <= Ship_List(0).shipsize.y Then
                Ship_List(0).SetTile(selection, New Ship_tile(0, tile_type_enum.empty, 0, 0, 0, walkable_type_enum.Impassable))
            End If
        End If


        If mouse_info.get_right_click(Nothing, right_click_location) = True Then

            right_click_location.x = Convert.ToInt32(view_location_internal.x + right_click_location.x / internal_zoom) \ 32
            right_click_location.y = Convert.ToInt32(view_location_internal.y + right_click_location.y / internal_zoom) \ 32

            If Ship_List(0).Crew_list(0).command_queue.Any Then
                Ship_List(0).Crew_list(0).command_queue.Clear()
            End If

            Dim pf As New A_star

            pf.set_map(Ship_List(0).tile_map, Ship_List(0).shipsize)

            pf.set_start_end(Ship_List(0).Crew_list(0).find_tile, New PointI(right_click_location.x, right_click_location.y))
            pf.find_path()
            If pf.get_status = pf_status.path_found Then
                Dim list As LinkedList(Of PointI)
                list = pf.get_path()
                Ship_List(0).Crew_list(0).speed = 4
                For Each location In list
                    Ship_List(0).Crew_list(0).command_queue.Enqueue(New Crew.Command_move(New PointD(location.x * 32, location.y * 32)))
                Next
            End If


        End If

        If pressedkeys.Contains(Keys.Add) Then Ship_List(0).ambient__light = Color.FromArgb(Ship_List(0).ambient__light.ToArgb - Color.FromArgb(1, 1, 1, 1).ToArgb)

        If pressedkeys.Contains(Keys.Z) Then internal_zoom = internal_zoom + 1 : pressedkeys.Remove(Keys.Z)
        If pressedkeys.Contains(Keys.X) Then internal_zoom = internal_zoom - 1 : pressedkeys.Remove(Keys.X)

        If internal_zoom < 0.5 Then internal_zoom = 1
        If internal_zoom > 20 Then internal_zoom = 20


        If (pressedkeys.Contains(Keys.W) AndAlso pressedkeys.Contains(Keys.A)) OrElse (pressedkeys.Contains(Keys.W) AndAlso pressedkeys.Contains(Keys.D)) Then amount = Convert.ToSingle(amount * DIAGONAL_SPEED_MODIFIER)
        If (pressedkeys.Contains(Keys.S) AndAlso pressedkeys.Contains(Keys.A)) OrElse (pressedkeys.Contains(Keys.S) AndAlso pressedkeys.Contains(Keys.D)) Then amount = Convert.ToSingle(amount * DIAGONAL_SPEED_MODIFIER)


        'have to be separate
        If pressedkeys.Contains(Keys.W) Then Ship_List(current_selected_ship_view).MoveOfficer(0, New PointD(0, -amount))
        If pressedkeys.Contains(Keys.S) Then Ship_List(current_selected_ship_view).MoveOfficer(0, New PointD(0, amount))
        If pressedkeys.Contains(Keys.A) Then Ship_List(current_selected_ship_view).MoveOfficer(0, New PointD(-amount, 0))
        If pressedkeys.Contains(Keys.D) Then Ship_List(current_selected_ship_view).MoveOfficer(0, New PointD(amount, 0))

        'lock screen
        'current_view_location_internal.x = Convert.ToInt32(Ships(current_selected_ship_view).GetOfficer.Item(0).GetLocation.x + (screen_size.x / 2) + 16)
        'current_view_location_internal.y = Convert.ToInt32(Ships(current_selected_ship_view).GetOfficer.Item(0).GetLocation.y + (screen_size.y / 2) + 16)

        view_location_internal.x = Convert.ToInt32(Ship_List(current_selected_ship_view).GetOfficer.Item(0).GetLocation.x - (screen_size.x / 2 / internal_zoom)) + 16
        view_location_internal.y = Convert.ToInt32(Ship_List(current_selected_ship_view).GetOfficer.Item(0).GetLocation.y - (screen_size.y / 2 / internal_zoom)) + 16






        'current_view_location_internal.x = Convert.ToInt32(Ships(current_selected_ship_view).GetOfficer.Item(0).GetLocation.x)
        'current_view_location_internal.y = Convert.ToInt32(Ships(current_selected_ship_view).GetOfficer.Item(0).GetLocation.y)


        If view_location_internal.x < -(screen_size.x / 2) Then view_location_internal.x = Convert.ToInt32(-(screen_size.x / 2))
        If view_location_internal.x > Ship_List.Item(current_selected_ship_view).GetShipSize.x * 32 * internal_zoom - screen_size.x / 2 Then view_location_internal.x = Convert.ToInt32(Ship_List.Item(current_selected_ship_view).GetShipSize.x * 32 * internal_zoom - screen_size.x / 2)

        If view_location_internal.y < -(screen_size.y / 2) Then view_location_internal.y = Convert.ToInt32(-(screen_size.y / 2))
        If view_location_internal.y > Ship_List.Item(current_selected_ship_view).GetShipSize.y * 32 * internal_zoom - screen_size.y / 2 Then view_location_internal.y = Convert.ToInt32(Ship_List.Item(current_selected_ship_view).GetShipSize.y * 32 * internal_zoom - screen_size.y / 2)


        If pressedkeys.Contains(Keys.Tab) Then current_view = current_view_enum.ship_external : shipexternal_redraw = False : pressedkeys.Remove(Keys.Tab)

    End Sub

    Sub External_UI()
        Dim mouse_info As New MainForm.mouse_info_type
        Dim mouse_click As Boolean
        Dim left_down_point As PointI = Nothing
        Dim left_release_point As PointI = Nothing
        MainForm.getUI(pressedkeys, mouse_info)

        If mouse_info.get_left_click(left_down_point, left_release_point) Then
            mouse_click = True
            For Each Button In view_external_menu_items
                If Button.Value.enabled = True Then
                    If Button.Value.bounds.Contains(left_down_point.x, left_down_point.y) And Button.Value.bounds.Contains(left_release_point.x, left_release_point.y) Then
                        'choice = Button.Key
                        'choiceType = menu_collection.Key
                        'clear_tiles = True
                    End If
                End If
            Next
        End If


        Dim amount As Double = 1
        'Dim right_click_location As PointI
        Dim atsize As Integer = Convert.ToInt32(32 * internal_zoom)


        If pressedkeys.Contains(Keys.Z) Then external_zoom -= 0.25 : pressedkeys.Remove(Keys.Z)
        If pressedkeys.Contains(Keys.X) Then external_zoom += 0.25 : pressedkeys.Remove(Keys.X)

        If mouse_info.wheel > 0 Then
            'external_zoom_End += (mouse_info.wheel / 240) * external_zoom
            external_zoom += (mouse_info.wheel / 240) * external_zoom
        ElseIf mouse_info.wheel < 0 Then
            'external_zoom_End += (mouse_info.wheel / 240) * external_zoom
            external_zoom += (mouse_info.wheel / 240) * external_zoom
        End If
        mouse_info.wheel = 0
        Math.Round(external_zoom, 2)
        If external_zoom > 5 Then external_zoom = 5
        If external_zoom < 0.01 Then external_zoom = 0.01

        'Dim dest_point, dest_release_point As PointI

        If mouse_info.right_down = True Then
            Dim delta As PointD
            delta.x = screen_size.x / 2 - mouse_info.position.x
            delta.y = screen_size.y / 2 - mouse_info.position.y
            Dim rot As Double = Math.Atan2(delta.y, delta.x) - 1.57079633
            If rot < 0 Then rot += PI * 2
            'If rot < -PI Then rot += PI * 2
            'Ships(current_selected_ship_view).target_rotation = rot
            Ship_List(current_selected_ship_view).SetFullTurn(rot)
            mouse_info.right_down = False
        End If


        If pressedkeys.Contains(Keys.T) Then
            'Ships(current_selected_ship_view).SetFullTurn(PI / 2)
            Ship_List(current_selected_ship_view).angular_velocity = 0.1
        End If



        'If pressedkeys.Contains(Keys.W) Then Ships(0).Apply_Force(10, New PointD(13, 18), Direction_Enum.Bottom)
        'If pressedkeys.Contains(Keys.S) Then Ships(0).Apply_Force(10, New PointD(15, 18), Direction_Enum.Top)
        'If pressedkeys.Contains(Keys.A) Then Ships(0).Apply_Force(0.01, New PointD(15, 18), Direction_Enum.Left)
        'If pressedkeys.Contains(Keys.D) Then Ships(0).Apply_Force(0.01, New PointD(15, 18), Direction_Enum.Right)

        If pressedkeys.Contains(Keys.W) Then
            For Each Device In Ship_List(current_selected_ship_view).Engine_Coltrol_Group(Direction_Enum.Bottom)
                Ship_List(current_selected_ship_view).Fire_Engine(Device.Key, 1)
            Next
        End If

        If pressedkeys.Contains(Keys.S) Then
            For Each Device In Ship_List(current_selected_ship_view).Engine_Coltrol_Group(Direction_Enum.Top)
                Ship_List(current_selected_ship_view).Fire_Engine(Device.Key, 1)
            Next
        End If

        If pressedkeys.Contains(Keys.A) Then
            For Each Device In Ship_List(current_selected_ship_view).Engine_Coltrol_Group(Direction_Enum.RotateL)
                If Ship_List(current_selected_ship_view).device_list(Device.Key).type = device_type_enum.thruster Then
                    Ship_List(current_selected_ship_view).Fire_Engine(Device.Key, 1)
                End If
            Next
        End If

        If pressedkeys.Contains(Keys.D) Then
            For Each Device In Ship_List(current_selected_ship_view).Engine_Coltrol_Group(Direction_Enum.RotateR)
                If Ship_List(current_selected_ship_view).device_list(Device.Key).type = device_type_enum.thruster Then
                    Ship_List(current_selected_ship_view).Fire_Engine(Device.Key, 1)
                End If
            Next
        End If


        'Ships(current_selected_ship_view).rotation = Ships(current_selected_ship_view).destanation_rotation

        If pressedkeys.Contains(Keys.Tab) Then current_view = current_view_enum.ship_internal : pressedkeys.Remove(Keys.Tab)


        If Ship_List(current_selected_ship_view).angular_velocity > 0.025 Then Ship_List(current_selected_ship_view).angular_velocity = 0.025
        If Ship_List(current_selected_ship_view).angular_velocity < -0.025 Then Ship_List(current_selected_ship_view).angular_velocity = -0.025

    End Sub

    Sub Planet_UI()
        Dim pressedkeys As HashSet(Of Keys) = Nothing
        Dim mouse_info As MainForm.mouse_info_type = Nothing
        Dim amount As Double = 1
        'Dim right_click_location As PointI
        MainForm.getUI(pressedkeys, mouse_info)
        Dim atsize As Integer = Convert.ToInt32(32 * internal_zoom)

    End Sub

    Sub Starmap_UI()
        Dim amount As Double = 1
        'Dim right_click_location As PointI        
        Dim atsize As Integer = Convert.ToInt32(32 * internal_zoom)


        If mouse_info.position.x = 0 Then
            view_location_star_map.x -= 20
        ElseIf mouse_info.position.x = screen_size.x - 1 Then
            view_location_star_map.x += 20
        End If
        If mouse_info.position.y = 0 Then
            view_location_star_map.y -= 20
        ElseIf mouse_info.position.y = screen_size.y - 1 Then
            view_location_star_map.y += 20
        End If


        If mouse_info.wheel > 0 Then
            star_map_zoom += (mouse_info.wheel / 240) * star_map_zoom
        ElseIf mouse_info.wheel < 0 Then
            star_map_zoom += (mouse_info.wheel / 240) * star_map_zoom
        End If
        mouse_info.wheel = 0


        If pressedkeys.Contains(Keys.Z) Then star_map_zoom -= 1000 : pressedkeys.Remove(Keys.Z)
        If pressedkeys.Contains(Keys.X) Then star_map_zoom += 1000 : pressedkeys.Remove(Keys.X)
        If pressedkeys.Contains(Keys.R) Then star_map_zoom = 0 : star_map_sector = 0
        If star_map_zoom > 59999 Then star_map_zoom = 59999
        If star_map_zoom < 0 Then star_map_zoom = 0


        If mouse_info.get_left_click = True Then
            For Each sector In u.stars
                If mouse_info.position.x - screen_size.x \ 2 > sector.Value.location.x \ Convert.ToInt32(60000 - star_map_zoom) Then
                    If mouse_info.position.x - screen_size.x \ 2 < sector.Value.location.x \ Convert.ToInt32(60000 - star_map_zoom) + 16 Then
                        If mouse_info.position.y - screen_size.y \ 2 > sector.Value.location.y \ Convert.ToInt32(60000 - star_map_zoom) Then
                            If mouse_info.position.y - screen_size.y \ 2 < sector.Value.location.y \ Convert.ToInt32(60000 - star_map_zoom) + 16 Then
                                star_map_sector = sector.Key
                                star_map_zoom = 59550
                            End If
                        End If
                    End If
                End If
            Next


        End If


    End Sub

End Module
