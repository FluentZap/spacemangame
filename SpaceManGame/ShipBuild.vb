Module ShipBuild
    Private Build_Save(10) As Ship
    Private Build_Save_count As Integer
    Private Undo_count As Integer
    Private All_Connected As Boolean        

    'Build ship creation
    Private shipclass, shiptype As Integer    
    Private shipsize As PointI

    'Local
    Private Build_ship As Ship
    Private Exit_Ship_build As Boolean = False
    Private Diagram_texture As Texture

    Private pressedkeys As New HashSet(Of Keys)
    Private mouse_info As New MainForm.mouse_info_type
    Private left_click As Boolean
    Private right_click As Boolean
    Private left_down_point As PointI = Nothing
    Private left_release_point As PointI = Nothing
    Private choice As Integer = -1
    Private choiceType As Tech_type_enum = Tech_type_enum.Menu_item
    Private Build_selection As tech_list_enum = tech_list_enum.Empty    
    Private Redraw_Minimap As Boolean
    Private Validate_rooms As Boolean
    Private Validate_pipelines As Boolean

    'Menu Items
    Private Main_Menu_open As Boolean = False
    Private clear_tiles As Boolean
    Private info_bars As Boolean
    Private tile_grid As Boolean
    Private split_room As Boolean

    'Minimap
    Private MiniMap_Scale As Integer

    'Room Building
    Private selection_start_point As PointI = Nothing
    Private selection_end_point As PointI = Nothing
    Private selection As PointI
    Private old_selection As PointI
    Private Selected_device_Panel As Integer = tech_list_enum.All_rooms
    Private Drawing_room As Boolean
    Private Erasing As Boolean
    Private Erasing_key As Boolean
    Private Selecting_room As Boolean
    Private Room_selection As Integer = -1
    Private Room_selection_color As Color = Color.FromArgb(255, 160, 160, 225)
    Private Buildable As Boolean
    Private Adding_To_Room As Boolean
    Private Adding_To_Room_Id As Integer
    Private Tile_selection As Rectangle = New Rectangle(-1, -1, 0, 0)
    Private Valid_rooms As HashSet(Of Integer) = New HashSet(Of Integer)

    'Room remove
    Private Removeing_room As Boolean
    Private Remove_Room_ID As Integer


    Dim selection_tile_map(,) As Byte
    Dim selection_tile_map2(,) As Byte

    'Dim selection_tile_map(shipsize.x + 2, shipsize.y + 2) As Byte
    'Dim selection_tile_map2(shipsize.x + 2, shipsize.y + 2) As Byte

    Private Selection_color As Color = Color.FromArgb(150, 255, 255, 255)

    'Device building
    Private dt_selection As PointI
    Private Reset_device_list As Boolean = True
    Private Device_rotation As rotate_enum
    Private Device_Flip As flip_enum

    'Piping        
    Private Drawing_Pipe As Boolean
    Private pipeing As Boolean
    Private Pipeline_select As Integer = -1
    Private Text_Input As Boolean
    Private New_Pipe_Menu As Boolean
    Private Pipeline_Name As String = ""
    Private Valid_pipelines As HashSet(Of Integer) = New HashSet(Of Integer)
    'Private Pipeline_dialog As Boolean
    'Private Pipeline_dialog_color_scroll As Integer
    'Private Pipeline_dialog_text_select As Boolean
    'Private Pipeline_dialog_text As String
    'Private Pipeline_dialog_pipe_ID As Integer



    'Info Panel
    Private Info_Select As Ship_Build_Info_enum





    'Center point
    Private Set_Center_point As Boolean
    Private all_menu_items_list As New Dictionary(Of Tech_type_enum, Dictionary(Of Integer, Menu_button))()
    Private atsize As Integer

    Enum Info_Panel_Mode_enum
        Pipeline
        Device
        Room
    End Enum


    'Info Panel
    'Private info_panel_mode As Info_Panel_Mode_enum


    'Menu Items
    Private device_menu As PointI
    Private menu_item As New Dictionary(Of Integer, Menu_button)()
    Private Room_menu_item As New Dictionary(Of Integer, Menu_button)()
    Private Device_menu_item As New Dictionary(Of Integer, Menu_button)()

    'Sub Clear_Ship()
    '    clear_tiles = True
    'End Sub



    Sub Build_save_ship(ByVal Build_ship As Ship)

        Build_Save(Build_Save_count) = Build_ship.Clone
        Undo_count = 0
        Build_Save_count += 1
        If Build_Save_count >= 11 Then Build_Save_count = 0

    End Sub

    Function Ship_build_undo() As Ship
        'If Undo_count < 10 AndAlso Build_Save_count > 0 AndAlso Build_Save(Build_Save_count - 1) IsNot Nothing Then
        'Undo_count += 1
        Build_Save_count -= 1
        If Build_Save_count <= 0 Then Build_Save_count = 10
        'End If
        Return Build_Save(Build_Save_count)
    End Function

    Function Ship_build_redo() As Ship
        If Undo_count > 0 AndAlso Build_Save_count < 10 AndAlso Build_Save(Build_Save_count + 1) IsNot Nothing Then
            Undo_count -= 1
            Build_Save_count += 1
            If Build_Save_count <= 0 Then Build_Save_count = 10
        End If
        Return Build_Save(Build_Save_count)
    End Function

    Enum ship_build_menu_enum
        main_menu_button
        main_menu_panel
        main_menu_new
        main_menu_load
        main_menu_save
        main_menu_exit
        'ship_info
        resouce_cost
        undo
        redo
        mouse_position
        minimap
        device_panel


        remove
        remove_section
        split_room
        rotate_left
        rotate_right
        flip_up
        flip_right
        info_bars
        tile_grid
        set_center_point

        'stats_panel
        'stats_button
        'zoom_in
        'zoom_out
        Pipeline_new
        Pipeline_select
        Pipeline_Textbox

        Info_panel
        Info_Info
        Info_Devices
        Info_Rooms

        New_Pipe_menu_Panel
        New_Pipe_menu_Energy_1
        New_Pipe_menu_Energy_2
        New_Pipe_menu_Energy_3
        New_Pipe_menu_Data_1
        New_Pipe_menu_Data_2
        New_Pipe_menu_Data_3

    End Enum


    Sub Handle_UI()
        'Set choice and relese mouse clicks
        choice = -1        
        left_click = False

        'detect a click
        If mouse_info.get_left_click(left_down_point, left_release_point) Then
            left_click = True
            For Each menu_collection In all_menu_items_list
                For Each Button In menu_collection.Value
                    If Button.Value.enabled = True Then
                        If Button.Value.bounds.Contains(left_down_point.x, left_down_point.y) And Button.Value.bounds.Contains(left_release_point.x, left_release_point.y) Then
                            choice = Button.Key
                            choiceType = menu_collection.Key                            
                            'clear_tiles = True
                        End If
                    End If
                Next
            Next
        End If        

        If Text_Input = False AndAlso New_Pipe_Menu = False Then
            Select Case choiceType
                Case Is = Tech_type_enum.Room 'Handle room buttons                   

                    If choice > -1 Then Erasing = False : Device_rotation = rotate_enum.Zero : Device_Flip = flip_enum.None : split_room = False : pipeing = False

                    If choice >= tech_list_enum.Engineering AndAlso choice <= tech_list_enum.Hydroponics Then
                        'Calculate which room is seleced to build
                        Room_menu_item(choice).clear_color()
                        Selected_device_Panel = choice
                        Build_selection = CType(choice, tech_list_enum)
                        Reset_device_list = True
                    End If
                    If choice = tech_list_enum.All_rooms Then
                        'Calculate which room is seleced to build
                        Room_menu_item(choice).clear_color()
                        Selected_device_Panel = choice
                        Build_selection = tech_list_enum.Empty
                        Reset_device_list = True
                    End If
                    If choice = tech_list_enum.Piping Then
                        Room_menu_item(choice).clear_color()
                        Selected_device_Panel = choice
                        Build_selection = tech_list_enum.Empty
                        Reset_device_list = True
                        Pipeline_select = -1
                        pipeing = True
                    End If

                    If choice = tech_list_enum.Armor Then
                        Room_menu_item(choice).clear_color()
                        Selected_device_Panel = choice
                        Build_selection = tech_list_enum.Empty
                        Reset_device_list = True
                    End If



                Case Is = Tech_type_enum.Menu_item 'Handle Menu item buttons                    
                    'Check for menu open
                    If choice = ship_build_menu_enum.main_menu_button Then
                        If Main_Menu_open = True Then
                            Main_Menu_open = False
                        Else
                            Main_Menu_open = True
                        End If
                    End If

                    If choice = ship_build_menu_enum.main_menu_new Then
                        clear_tiles = True
                        'new_ship_build_menu()
                    End If
                    If choice = ship_build_menu_enum.main_menu_load Then
                        Build_ship = load_ship_schematic()
                        shipsize = Build_ship.shipsize
                        ReDim selection_tile_map(shipsize.x + 2, shipsize.y + 2)
                        set_minimap_Scale()
                        Validate_rooms = True
                        Validate_pipelines = True
                    End If
                    If choice = ship_build_menu_enum.main_menu_save Then
                        save_ship_schematic(Build_ship)
                    End If
                    If choice = ship_build_menu_enum.main_menu_exit Then
                        Exit_Ship_build = True
                    End If


                    If choice = ship_build_menu_enum.undo Then
                        Build_ship = Ship_build_undo()
                    End If

                    If choice = ship_build_menu_enum.redo Then
                        Build_ship = Ship_build_redo()
                    End If


                    If choice = ship_build_menu_enum.remove Then
                        If Erasing = True Then Erasing = False Else Erasing = True
                    End If

                    If choice = ship_build_menu_enum.info_bars Then
                        If info_bars = True Then info_bars = False Else info_bars = True
                    End If

                    If choice = ship_build_menu_enum.tile_grid Then
                        If tile_grid = True Then tile_grid = False Else tile_grid = True
                    End If

                    If choice = ship_build_menu_enum.split_room Then
                        If split_room = True Then split_room = False Else split_room = True
                        If split_room = True Then
                            choiceType = Tech_type_enum.Menu_item
                            'Build_selection = tech_list_enum.Empty
                        End If
                    End If

                    'If choice = ship_build_menu_enum.set_center_point Then
                    'Build_selection = tech_list_enum.Empty
                    'Set_Center_point = True
                    'End If

                    If choice = ship_build_menu_enum.rotate_left Then
                        Select Case Device_rotation
                            Case Is = rotate_enum.Zero
                                Device_rotation = rotate_enum.TwoSeventy
                            Case Is = rotate_enum.Ninty
                                Device_rotation = rotate_enum.Zero
                            Case Is = rotate_enum.OneEighty
                                Device_rotation = rotate_enum.Ninty
                            Case Is = rotate_enum.TwoSeventy
                                Device_rotation = rotate_enum.OneEighty
                        End Select
                    End If

                    If choice = ship_build_menu_enum.rotate_right Then
                        Select Case Device_rotation
                            Case Is = rotate_enum.Zero
                                Device_rotation = rotate_enum.Ninty
                            Case Is = rotate_enum.Ninty
                                Device_rotation = rotate_enum.OneEighty
                            Case Is = rotate_enum.OneEighty
                                Device_rotation = rotate_enum.TwoSeventy
                            Case Is = rotate_enum.TwoSeventy
                                Device_rotation = rotate_enum.Zero
                        End Select
                    End If

                    If choice = ship_build_menu_enum.flip_up Then
                        Select Case Device_Flip
                            Case flip_enum.None
                                Device_Flip = flip_enum.Flip_Y
                            Case flip_enum.Flip_X
                                Device_Flip = flip_enum.Both
                            Case flip_enum.Flip_Y
                                Device_Flip = flip_enum.None
                            Case flip_enum.Both
                                Device_Flip = flip_enum.Flip_X
                        End Select
                    End If

                    If choice = ship_build_menu_enum.flip_right Then
                        Select Case Device_Flip
                            Case flip_enum.None
                                Device_Flip = flip_enum.Flip_X
                            Case flip_enum.Flip_X
                                Device_Flip = flip_enum.None
                            Case flip_enum.Flip_Y
                                Device_Flip = flip_enum.Both
                            Case flip_enum.Both
                                Device_Flip = flip_enum.Flip_Y
                        End Select
                    End If

                    If choice = ship_build_menu_enum.Pipeline_new Then
                        New_Pipe_Menu = True                        
                    End If

                    If choice = ship_build_menu_enum.Pipeline_Textbox Then
                        Text_Input = True
                        left_click = False
                        Pipeline_Name = ""
                    End If



                    If choice = ship_build_menu_enum.Info_Info Then
                        Info_Select = Ship_Build_Info_enum.Info
                    End If

                    If choice = ship_build_menu_enum.Info_Devices Then
                        Info_Select = Ship_Build_Info_enum.Devices
                        'Build_selection = tech_list_enum.Empty
                    End If

                    If choice = ship_build_menu_enum.Info_Rooms Then
                        Info_Select = Ship_Build_Info_enum.Rooms
                        'Build_selection = tech_list_enum.Empty
                    End If


                Case Is = Tech_type_enum.Device 'Handle Device buttons
                    If choice > -1 Then
                        If pipeing = True Then
                            If choice < 1000 Then
                                Pipeline_select = choice
                                Pipeline_Name = Build_ship.pipeline_list(Pipeline_select).Name
                            End If

                            If choice >= 1000 Then
                                'Remove if delete key is pressed
                                Build_ship.pipeline_list.Remove(choice - 1000)
                                For Each Device In Build_ship.device_list
                                    Device.Value.Disconnect_Pipeline(choice - 1000)
                                Next
                                Device_menu_item.Remove(choice - 1000)
                                Device_menu_item.Remove(choice)
                                Reset_device_list = True
                                If Pipeline_select = choice - 1000 Then
                                    For a = choice - 1000 To -1 Step -1
                                        If Build_ship.pipeline_list.ContainsKey(a) Then Pipeline_select = a : Pipeline_Name = Build_ship.pipeline_list(Pipeline_select).Name : Exit For
                                        If a = -1 Then Pipeline_select = -1
                                    Next
                                End If
                            End If

                            'If choice >= 2000 Then
                            'Pipeline_dialog = True
                            'Pipeline_dialog_pipe_ID = choice - 2000
                            'menu_item(ship_build_menu_enum.Pipeline_Dialog).enabled = True
                            'menu_item(ship_build_menu_enum.Pipeline_Dialog_Cancel).enabled = True
                            'menu_item(ship_build_menu_enum.Pipeline_Dialog_Ok).enabled = True
                            'menu_item(ship_build_menu_enum.Pipeline_Dialog_TextBox).enabled = True
                            'End If

                        Else
                            Build_selection = CType(choice, tech_list_enum)
                            Erasing = False : Device_rotation = rotate_enum.Zero : Device_Flip = flip_enum.None : split_room = False
                        End If

                        'If Selected_device_Panel = tech_list_enum.Armor Then
                        'Build_selection = CType(choice, tech_list_enum)
                        'End If



                    End If
            End Select
            If (choice = -1 AndAlso left_click = True) OrElse (choiceType = Tech_type_enum.Menu_item AndAlso Not choice = ship_build_menu_enum.main_menu_button AndAlso choice > -1) Then Main_Menu_open = False

            'open or close menu
            If Main_Menu_open = True Then
                menu_item(ship_build_menu_enum.main_menu_new).enabled = True
                menu_item(ship_build_menu_enum.main_menu_load).enabled = True
                menu_item(ship_build_menu_enum.main_menu_save).enabled = True
                menu_item(ship_build_menu_enum.main_menu_exit).enabled = True
            Else
                menu_item(ship_build_menu_enum.main_menu_new).enabled = False
                menu_item(ship_build_menu_enum.main_menu_load).enabled = False
                menu_item(ship_build_menu_enum.main_menu_save).enabled = False
                menu_item(ship_build_menu_enum.main_menu_exit).enabled = False
            End If

            If New_Pipe_Menu = True Then
                menu_item(ship_build_menu_enum.New_Pipe_menu_Panel).enabled = True
                menu_item(ship_build_menu_enum.New_Pipe_menu_Energy_1).enabled = True
                menu_item(ship_build_menu_enum.New_Pipe_menu_Energy_2).enabled = True
                menu_item(ship_build_menu_enum.New_Pipe_menu_Energy_3).enabled = True
                menu_item(ship_build_menu_enum.New_Pipe_menu_Data_1).enabled = True
                menu_item(ship_build_menu_enum.New_Pipe_menu_Data_2).enabled = True
                menu_item(ship_build_menu_enum.New_Pipe_menu_Data_3).enabled = True
            Else
                menu_item(ship_build_menu_enum.New_Pipe_menu_Panel).enabled = False
                menu_item(ship_build_menu_enum.New_Pipe_menu_Energy_1).enabled = False
                menu_item(ship_build_menu_enum.New_Pipe_menu_Energy_2).enabled = False
                menu_item(ship_build_menu_enum.New_Pipe_menu_Energy_3).enabled = False
                menu_item(ship_build_menu_enum.New_Pipe_menu_Data_1).enabled = False
                menu_item(ship_build_menu_enum.New_Pipe_menu_Data_2).enabled = False
                menu_item(ship_build_menu_enum.New_Pipe_menu_Data_3).enabled = False
            End If
            

            If Pipeline_select > -1 AndAlso pipeing = True AndAlso Info_Select = Ship_Build_Info_enum.Info Then
                menu_item(ship_build_menu_enum.Pipeline_Textbox).enabled = True
                menu_item(ship_build_menu_enum.Pipeline_Textbox).text = Build_ship.pipeline_list(Pipeline_select).Name
            Else
                menu_item(ship_build_menu_enum.Pipeline_Textbox).enabled = False
            End If



            'Screen Scroll/ Safegaurds
            If pressedkeys.Contains(Keys.W) Then view_location_sb.y -= 256 : pressedkeys.Remove(Keys.W)
            If pressedkeys.Contains(Keys.S) Then view_location_sb.y += 256 : pressedkeys.Remove(Keys.S)
            If pressedkeys.Contains(Keys.A) Then view_location_sb.x -= 256 : pressedkeys.Remove(Keys.A)
            If pressedkeys.Contains(Keys.D) Then view_location_sb.x += 256 : pressedkeys.Remove(Keys.D)


            If mouse_info.position.x = 0 Then
                view_location_sb.x -= 1
            ElseIf mouse_info.position.x >= screen_size.x - 1 Then
                view_location_sb.x += 1
            End If
            If mouse_info.position.y = 0 Then
                view_location_sb.y -= 1
            ElseIf mouse_info.position.y >= screen_size.y - 1 Then
                view_location_sb.y += 1
            End If
            'view_location_sb.x = 0
            'view_location_sb.y = 0
            If view_location_sb.x < -(screen_size.x / sb_zoom / 2) Then view_location_sb.x = Convert.ToInt32(-(screen_size.x / sb_zoom / 2))
            If view_location_sb.x > (shipsize.x + 1) * 32 - screen_size.x / sb_zoom / 2 Then view_location_sb.x = Convert.ToInt32((shipsize.x + 1) * 32 - screen_size.x / sb_zoom / 2)
            If view_location_sb.y < -(screen_size.y / sb_zoom / 2) Then view_location_sb.y = Convert.ToInt32(-(screen_size.y / sb_zoom / 2))
            If view_location_sb.y > (shipsize.y + 1) * 32 - screen_size.y / sb_zoom / 2 Then view_location_sb.y = Convert.ToInt32((shipsize.y + 1) * 32 - screen_size.y / sb_zoom / 2)

            If mouse_info.wheel > 0 Then
                If sb_zoom < 4 Then
                    'view_location_sb.x += 1

                    'view_location_sb.x += CInt(screen_size.x / 4) ' CInt(screen_size.x / sb_zoom) \ 4
                    'view_location_sb.y += CInt(sb_zoom * sb_zoom + 1) 'CInt(screen_size.y / sb_zoom) \ 4
                End If
                sb_zoom += 1
            ElseIf mouse_info.wheel < 0 Then
                If sb_zoom > 1 Then
                    'view_location_sb.x -= screen_size.x \ 4
                    'view_location_sb.y -= screen_size.y \ 4
                End If
                sb_zoom -= 1
            End If
            mouse_info.wheel = 0

            If pressedkeys.Contains(Keys.Z) Then sb_zoom -= 1 : pressedkeys.Remove(Keys.Z)
            If pressedkeys.Contains(Keys.X) Then sb_zoom += 1 : pressedkeys.Remove(Keys.X)



            'view_location_personal.x = Ship_List(current_selected_ship_view).GetOfficer.Item(0).GetLocationD.x + 16 - ((screen_size.x / 2) / personal_zoom)
            'view_location_personal.y = Ship_List(current_selected_ship_view).GetOfficer.Item(0).GetLocationD.y + 16 - ((screen_size.y / 2) / personal_zoom)



            If sb_zoom < 1 Then sb_zoom = 1
            If sb_zoom > 4 Then sb_zoom = 4
            'If sb_map_zoom <= 0 Then sb_map_zoom = 4
            'If sb_map_zoom > 32 Then sb_map_zoom = 32

            selection.x = Convert.ToInt32(view_location_sb.x + mouse_info.position.x / sb_zoom) \ 32
            selection.y = Convert.ToInt32(view_location_sb.y + mouse_info.position.y / sb_zoom) \ 32

            If view_location_sb.x + mouse_info.position.x / sb_zoom < 0 Then selection.x -= 1
            If view_location_sb.y + mouse_info.position.y / sb_zoom < 0 Then selection.y -= 1

            If Text_Input = False AndAlso New_Pipe_Menu = False Then

                If pressedkeys.Contains(Keys.ControlKey) Then Erasing = True : Erasing_key = True
                If Erasing_key = True AndAlso pressedkeys.Contains(Keys.ControlKey) = False Then Erasing = False : Erasing_key = False


                If pressedkeys.Contains(Keys.R) Then
                    Select Case Device_rotation
                        Case Is = rotate_enum.Zero
                            Device_rotation = rotate_enum.Ninty
                        Case Is = rotate_enum.Ninty
                            Device_rotation = rotate_enum.OneEighty
                        Case Is = rotate_enum.OneEighty
                            Device_rotation = rotate_enum.TwoSeventy
                        Case Is = rotate_enum.TwoSeventy
                            Device_rotation = rotate_enum.Zero
                    End Select
                    pressedkeys.Remove(Keys.R)
                End If

                If pressedkeys.Contains(Keys.T) Then
                    Select Case Device_rotation
                        Case Is = rotate_enum.Zero
                            Device_rotation = rotate_enum.Ninty
                        Case Is = rotate_enum.Ninty
                            Device_rotation = rotate_enum.OneEighty
                        Case Is = rotate_enum.OneEighty
                            Device_rotation = rotate_enum.TwoSeventy
                        Case Is = rotate_enum.TwoSeventy
                            Device_rotation = rotate_enum.Zero
                    End Select
                    pressedkeys.Remove(Keys.T)
                End If


                If pressedkeys.Contains(Keys.F) Then
                    Select Case Device_Flip
                        Case flip_enum.None
                            Device_Flip = flip_enum.Flip_Y
                        Case flip_enum.Flip_X
                            Device_Flip = flip_enum.Both
                        Case flip_enum.Flip_Y
                            Device_Flip = flip_enum.None
                        Case flip_enum.Both
                            Device_Flip = flip_enum.Flip_X
                    End Select
                    pressedkeys.Remove(Keys.F)
                End If

                If pressedkeys.Contains(Keys.G) Then
                    Select Case Device_Flip
                        Case flip_enum.None
                            Device_Flip = flip_enum.Flip_X
                        Case flip_enum.Flip_X
                            Device_Flip = flip_enum.None
                        Case flip_enum.Flip_Y
                            Device_Flip = flip_enum.Both
                        Case flip_enum.Both
                            Device_Flip = flip_enum.Flip_Y
                    End Select
                    pressedkeys.Remove(Keys.G)
                End If
            End If
        End If

        If New_Pipe_Menu = True Then
            If choiceType = Tech_type_enum.Menu_item Then
                Dim Id As Integer
                If choice >= ship_build_menu_enum.New_Pipe_menu_Energy_1 AndAlso choice <= ship_build_menu_enum.New_Pipe_menu_Data_3 Then
                    'add bounds
                    For a = 0 To 255
                        If Not Build_ship.pipeline_list.ContainsKey(a) Then Id = a : Exit For
                    Next
                    Reset_device_list = True



                    If choice = ship_build_menu_enum.New_Pipe_menu_Energy_1 Then
                        Build_ship.pipeline_list.Add(Id, New ship_pipeline_type(Pipeline_type_enum.Energy, 100, Color.LightBlue, device_tile_type_enum.Pipeline_energy_Small, "Small Energy  " + Id.ToString))
                    End If

                    If choice = ship_build_menu_enum.New_Pipe_menu_Energy_2 Then
                        Build_ship.pipeline_list.Add(Id, New ship_pipeline_type(Pipeline_type_enum.Energy, 200, Color.LightBlue, device_tile_type_enum.Pipeline_energy_Medium, "Medium Energy  " + Id.ToString))
                    End If

                    If choice = ship_build_menu_enum.New_Pipe_menu_Energy_3 Then
                        Build_ship.pipeline_list.Add(Id, New ship_pipeline_type(Pipeline_type_enum.Energy, 300, Color.LightBlue, device_tile_type_enum.Pipeline_energy_Large, "Large Energy  " + Id.ToString))
                    End If



                    If choice = ship_build_menu_enum.New_Pipe_menu_Data_1 Then
                        Build_ship.pipeline_list.Add(Id, New ship_pipeline_type(Pipeline_type_enum.Data, 100, Color.LightGreen, device_tile_type_enum.Pipeline_data_Small, "Small Data  " + Id.ToString))
                    End If

                    If choice = ship_build_menu_enum.New_Pipe_menu_Data_2 Then
                        Build_ship.pipeline_list.Add(Id, New ship_pipeline_type(Pipeline_type_enum.Data, 200, Color.LightGreen, device_tile_type_enum.Pipeline_data_Medium, "Medium Data  " + Id.ToString))
                    End If

                    If choice = ship_build_menu_enum.New_Pipe_menu_Data_3 Then
                        Build_ship.pipeline_list.Add(Id, New ship_pipeline_type(Pipeline_type_enum.Data, 300, Color.LightGreen, device_tile_type_enum.Pipeline_data_Large, "Large Data  " + Id.ToString))
                    End If


                    New_Pipe_Menu = False

                End If
            End If
        End If


        'Text Input
        If Text_Input = True Then
            If pressedkeys.Contains(Keys.Back) AndAlso Pipeline_Name.Length > 0 Then Pipeline_Name = Pipeline_Name.Remove(Pipeline_Name.Length - 1, 1) : pressedkeys.Remove(Keys.Back)

            If Pipeline_Name.Length < 15 Then
                Dim key_temp(pressedkeys.Count - 1) As Keys
                pressedkeys.CopyTo(key_temp)
                For Each key In key_temp
                    If key >= 48 AndAlso key <= 57 OrElse key >= 65 AndAlso key <= 90 Then
                        If pressedkeys.Contains(Keys.ShiftKey) Then
                            Pipeline_Name += ChrW(key).ToString.ToUpper
                        Else
                            Pipeline_Name += ChrW(key).ToString.ToLower
                        End If
                        pressedkeys.Remove(key)
                    End If

                    If key = 32 Then
                        Pipeline_Name += " "
                        pressedkeys.Remove(key)
                    End If
                Next
            End If


            If pressedkeys.Contains(Keys.Enter) OrElse left_click = True Then
                If Not Pipeline_Name = "" Then
                    'Build_ship.pipeline_list(Pipeline_select).Color = Color.Blue
                    Text_Input = False
                    Reset_device_list = True
                End If
            End If

            Build_ship.pipeline_list(Pipeline_select).Name = Pipeline_Name
            all_menu_items_list(Tech_type_enum.Menu_item)(ship_build_menu_enum.Pipeline_Textbox).text = Pipeline_Name
            all_menu_items_list(Tech_type_enum.Menu_item)(ship_build_menu_enum.Pipeline_Textbox).press()
        End If
        'If pressedkeys.Contains(Keys.R) Then If Device_rotation = rotate_enum.Ninty Then Device_rotation = rotate_enum.Zero : pressedkeys.Remove(Keys.R) Else Device_rotation = rotate_enum.Ninty : pressedkeys.Remove(Keys.R)
        'Device_rotation = rotate_enum.Ninty


    End Sub


    Sub Create_build_ship()
        Select Case CType(shipclass, ship_class_enum)
            Case ship_class_enum.corvette
                MiniMap_Scale = 6
                shipsize.x = 30
                shipsize.y = 30
            Case ship_class_enum.fighterT
                MiniMap_Scale = 12
                shipsize.x = 10
                shipsize.y = 15
            Case ship_class_enum.fighterW
                MiniMap_Scale = 12
                shipsize.x = 20
                shipsize.y = 10
            Case ship_class_enum.frigate
                MiniMap_Scale = 4
                shipsize.x = 40
                shipsize.y = 60
            Case ship_class_enum.destroyer
                MiniMap_Scale = 2
                shipsize.x = 99
                shipsize.y = 119
            Case ship_class_enum.cruiser
                MiniMap_Scale = 1
                shipsize.x = 200
                shipsize.y = 250
            Case ship_class_enum.battle_cruiser
                shipsize.x = 300
                shipsize.y = 400
            Case ship_class_enum.carrier
                shipsize.x = 400
                shipsize.y = 600
        End Select
        'Create build ship and tilemap
        Build_ship = New Ship(-1, New PointD(0, 0), CType(shiptype, ship_type_enum), CType(shipclass, ship_class_enum), shipsize, 0)
        For x = 0 To shipsize.x
            For y = 0 To shipsize.y
                Build_ship.SetTile(x, y, New Ship_tile(0, tile_type_enum.empty, 0, 0, 0, walkable_type_enum.Walkable))
            Next y
        Next x
    End Sub



    Sub set_minimap_Scale()
        Select Case Build_ship.shipclass
            Case ship_class_enum.corvette
                MiniMap_Scale = 6
            Case ship_class_enum.fighterW
                MiniMap_Scale = 12
            Case ship_class_enum.fighterT
                MiniMap_Scale = 12
            Case ship_class_enum.frigate
                MiniMap_Scale = 4
            Case ship_class_enum.destroyer
                MiniMap_Scale = 2
            Case ship_class_enum.cruiser
                MiniMap_Scale = 1
            Case ship_class_enum.battle_cruiser
            Case ship_class_enum.carrier
        End Select
        Minimap_Texture = New Texture(d3d_device, (shipsize.x + 1) * MiniMap_Scale, (shipsize.y + 1) * MiniMap_Scale, 4, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default)
    End Sub

    Sub Add_menu_items()
        'Dim Device_list As Dictionary(Of tech_list_enum, tech_list_enum) = New Dictionary(Of Integer, Menu_button)
        'Menu Items
        menu_item.Add(ship_build_menu_enum.resouce_cost, New Menu_button(button_texture_enum.ship_build__ship_info, New Rectangle(32, 0, 256, 32), button_style.DisplayOnly, Color.White, CType(shipclass, ship_class_enum).ToString + " " + "Crew 0/0", Color.Black, d3d_font_enum.SB_small))
        menu_item.Add(ship_build_menu_enum.device_panel, New Menu_button(button_texture_enum.ship_build__device_panel, New Rectangle(screen_size.x - 220, 280, 220, 480), button_style.DisplayOnly))
        menu_item.Add(ship_build_menu_enum.mouse_position, New Menu_button(button_texture_enum.ship_build__ship_info, New Rectangle(screen_size.x - 556, 0, 256, 32), button_style.DisplayOnly, Color.FromArgb(255, 255, 255, 255), "0 / 0", Color.Black, d3d_font_enum.SB_small))

        menu_item.Add(ship_build_menu_enum.minimap, New Menu_button(button_texture_enum.ship_build__minimap, New Rectangle(screen_size.x - 224, 0, 224, 256), button_style.DisplayOnly))

        menu_item.Add(ship_build_menu_enum.info_bars, New Menu_button(button_texture_enum.ship_build__info_bars, New Rectangle(screen_size.x - 256, 0, 32, 32), button_style.Clickable))
        menu_item.Add(ship_build_menu_enum.remove, New Menu_button(button_texture_enum.ship_build__remove, New Rectangle(screen_size.x - 256, 36, 32, 32), button_style.Clickable))
        menu_item.Add(ship_build_menu_enum.remove_section, New Menu_button(button_texture_enum.ship_build__remove_selection, New Rectangle(screen_size.x - 256, 72, 32, 32), button_style.Clickable))
        menu_item.Add(ship_build_menu_enum.split_room, New Menu_button(button_texture_enum.ship_build__split, New Rectangle(screen_size.x - 256, 108, 32, 32), button_style.Clickable))
        menu_item.Add(ship_build_menu_enum.tile_grid, New Menu_button(button_texture_enum.ship_build__tile_grid, New Rectangle(screen_size.x - 292, 0, 32, 32), button_style.Clickable))

        menu_item.Add(ship_build_menu_enum.rotate_left, New Menu_button(button_texture_enum.ship_build__rotate_left, New Rectangle(screen_size.x - 292, 200, 32, 32), button_style.Clickable))
        menu_item.Add(ship_build_menu_enum.rotate_right, New Menu_button(button_texture_enum.ship_build__rotate_right, New Rectangle(screen_size.x - 256, 200, 32, 32), button_style.Clickable))
        menu_item.Add(ship_build_menu_enum.flip_up, New Menu_button(button_texture_enum.ship_build__flip_up, New Rectangle(screen_size.x - 292, 240, 32, 32), button_style.Clickable))
        menu_item.Add(ship_build_menu_enum.flip_right, New Menu_button(button_texture_enum.ship_build__flip_right, New Rectangle(screen_size.x - 256, 240, 32, 32), button_style.Clickable))


        menu_item.Add(ship_build_menu_enum.set_center_point, New Menu_button(button_texture_enum.ship_build__info_bars, New Rectangle(screen_size.x - 266, 206, 32, 32), button_style.Clickable))
        menu_item(ship_build_menu_enum.set_center_point).enabled = False

        menu_item.Add(ship_build_menu_enum.Info_panel, New Menu_button(button_texture_enum.ship_build__description_panel, New Rectangle(0, screen_size.y \ 2 - 250, 300, 500), button_style.DisplayOnly))

        menu_item.Add(ship_build_menu_enum.Info_Info, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(0, screen_size.y \ 2 - 282, 80, 32), button_style.Both, Color.LightBlue, "Info", Color.White, d3d_font_enum.SB_small))
        menu_item.Add(ship_build_menu_enum.Info_Devices, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(80, screen_size.y \ 2 - 282, 80, 32), button_style.Both, Color.Blue, "Devices", Color.White, d3d_font_enum.SB_small))
        menu_item.Add(ship_build_menu_enum.Info_Rooms, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(160, screen_size.y \ 2 - 282, 80, 32), button_style.Both, Color.Green, "Rooms", Color.White, d3d_font_enum.SB_small))


        menu_item.Add(ship_build_menu_enum.Pipeline_new, New Menu_button(button_texture_enum.ship_build__Pipeline_panel, New Rectangle(screen_size.x - 176, 290, 128, 20), button_style.Clickable, Color.LightGreen, "New Pipeline", Color.Black, d3d_font_enum.SB_small))
        menu_item(ship_build_menu_enum.Pipeline_new).enabled = False

        menu_item.Add(ship_build_menu_enum.Pipeline_Textbox, New Menu_button(button_texture_enum.ship_build__menu_item, New Rectangle(70, screen_size.y \ 2 - 240, 160, 40), button_style.Both, Color.LightBlue, "", Color.Black, d3d_font_enum.SB_small))
        menu_item(ship_build_menu_enum.Pipeline_Textbox).enabled = False
        'menu_item.Add(ship_build_menu_enum.Info_panel, New Menu_button(button_texture_enum.ship_build__description_panel, New Rectangle(0, screen_size.y \ 2 - 250, 300, 500), button_style.DisplayOnly))

        menu_item.Add(ship_build_menu_enum.main_menu_button, New Menu_button(button_texture_enum.ship_build__menu_button, New Rectangle(0, 0, 32, 32), button_style.Both))
        menu_item.Add(ship_build_menu_enum.main_menu_new, New Menu_button(button_texture_enum.ship_build__menu_item, New Rectangle(0, 32, 160, 40), button_style.Both, Color.FromArgb(255, 100, 100, 240), "New ship", Color.Black, d3d_font_enum.SB_small)) : menu_item(ship_build_menu_enum.main_menu_new).enabled = False
        menu_item.Add(ship_build_menu_enum.main_menu_load, New Menu_button(button_texture_enum.ship_build__menu_item, New Rectangle(0, 72, 160, 40), button_style.Both, Color.FromArgb(255, 100, 100, 240), "Load ship", Color.Black, d3d_font_enum.SB_small)) : menu_item(ship_build_menu_enum.main_menu_load).enabled = False
        menu_item.Add(ship_build_menu_enum.main_menu_save, New Menu_button(button_texture_enum.ship_build__menu_item, New Rectangle(0, 112, 160, 40), button_style.Both, Color.FromArgb(255, 100, 100, 240), "Save ship", Color.Black, d3d_font_enum.SB_small)) : menu_item(ship_build_menu_enum.main_menu_save).enabled = False
        menu_item.Add(ship_build_menu_enum.main_menu_exit, New Menu_button(button_texture_enum.ship_build__menu_item, New Rectangle(0, 152, 160, 40), button_style.Both, Color.FromArgb(255, 100, 100, 240), "Exit ship build", Color.Black, d3d_font_enum.SB_small)) : menu_item(ship_build_menu_enum.main_menu_exit).enabled = False


        menu_item.Add(ship_build_menu_enum.New_Pipe_menu_Panel, New Menu_button(button_texture_enum.ship_build__minimap, New Rectangle(screen_size.x \ 2, screen_size.y \ 2, 300, 300), button_style.DisplayOnly, Color.FromArgb(255, 255, 255, 255), "Select Pipeline", Color.Black, d3d_font_enum.SB_small)) : menu_item(ship_build_menu_enum.New_Pipe_menu_Panel).enabled = False

        menu_item.Add(ship_build_menu_enum.New_Pipe_menu_Data_1, New Menu_button(button_texture_enum.ship_build__menu_button, New Rectangle(screen_size.x \ 2, screen_size.y \ 2, 32, 32), button_style.Both)) : menu_item(ship_build_menu_enum.New_Pipe_menu_Data_1).enabled = False
        menu_item.Add(ship_build_menu_enum.New_Pipe_menu_Data_2, New Menu_button(button_texture_enum.ship_build__menu_button, New Rectangle(screen_size.x \ 2 + 32, screen_size.y \ 2, 32, 32), button_style.Both)) : menu_item(ship_build_menu_enum.New_Pipe_menu_Data_2).enabled = False
        menu_item.Add(ship_build_menu_enum.New_Pipe_menu_Data_3, New Menu_button(button_texture_enum.ship_build__menu_button, New Rectangle(screen_size.x \ 2 + 64, screen_size.y \ 2, 32, 32), button_style.Both)) : menu_item(ship_build_menu_enum.New_Pipe_menu_Data_3).enabled = False

        menu_item.Add(ship_build_menu_enum.New_Pipe_menu_Energy_1, New Menu_button(button_texture_enum.ship_build__menu_button, New Rectangle(screen_size.x \ 2, screen_size.y \ 2 + 32, 32, 32), button_style.Both)) : menu_item(ship_build_menu_enum.New_Pipe_menu_Energy_1).enabled = False
        menu_item.Add(ship_build_menu_enum.New_Pipe_menu_Energy_2, New Menu_button(button_texture_enum.ship_build__menu_button, New Rectangle(screen_size.x \ 2 + 32, screen_size.y \ 2 + 32, 32, 32), button_style.Both)) : menu_item(ship_build_menu_enum.New_Pipe_menu_Energy_2).enabled = False
        menu_item.Add(ship_build_menu_enum.New_Pipe_menu_Energy_3, New Menu_button(button_texture_enum.ship_build__menu_button, New Rectangle(screen_size.x \ 2 + 64, screen_size.y \ 2 + 32, 32, 32), button_style.Both)) : menu_item(ship_build_menu_enum.New_Pipe_menu_Energy_3).enabled = False


        'menu_item.Add(ship_build_menu_enum.Pipeline_Dialog, New Menu_button(button_texture_enum.ship_build__Pipeline_Dialog_Panel, New Rectangle(screen_size.x \ 2 - 300, screen_size.y \ 2 - 150, 600, 300), button_style.DisplayOnly))
        'menu_item.Add(ship_build_menu_enum.Pipeline_Dialog_Cancel, New Menu_button(button_texture_enum.ship_build__Pipeline_panel, New Rectangle(screen_size.x \ 2 - 300, screen_size.y \ 2 + 150, 128, 20), button_style.Clickable, Color.LightGreen, "Ok", Color.Black, d3d_font_enum.SB_small))
        'menu_item.Add(ship_build_menu_enum.Pipeline_Dialog_Ok, New Menu_button(button_texture_enum.ship_build__Pipeline_panel, New Rectangle(screen_size.x \ 2, screen_size.y \ 2 + 150, 128, 20), button_style.Clickable, Color.LightGreen, "Cancel", Color.Black, d3d_font_enum.SB_small))
        'menu_item.Add(ship_build_menu_enum.Pipeline_Dialog_TextBox, New Menu_button(button_texture_enum.ship_build__Pipeline_panel, New Rectangle(screen_size.x \ 2 - 176, 290, 128, 20), button_style.Clickable, Color.LightGreen, "", Color.Black, d3d_font_enum.SB_small))

        'menu_item(ship_build_menu_enum.Pipeline_Dialog).enabled = False
        'menu_item(ship_build_menu_enum.Pipeline_Dialog_Cancel).enabled = False
        'menu_item(ship_build_menu_enum.Pipeline_Dialog_Ok).enabled = False
        'menu_item(ship_build_menu_enum.Pipeline_Dialog_TextBox).enabled = False

    End Sub

    Sub Add_room_menu_items()

        device_menu.y = 280 + 32
        device_menu.x = screen_size.x - 300
        'Rooms
        Room_menu_item.Add(tech_list_enum.Piping, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, 280, 80, 32), button_style.Clickable, Color.White, "Pipelines", Color.Black, d3d_font_enum.SB_small))

        If Player_Tech.Contains(tech_list_enum.All_rooms) Then Room_menu_item.Add(tech_list_enum.All_rooms, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.LawnGreen, "All Rooms", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        Room_menu_item.Add(tech_list_enum.Armor, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Gray, "Armor", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Airlock) Then Room_menu_item.Add(tech_list_enum.Airlock, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.LightBlue, "Airlock", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Armory) Then Room_menu_item.Add(tech_list_enum.Armory, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Green, "Armory", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Bridge) Then Room_menu_item.Add(tech_list_enum.Bridge, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Red, "Bridge", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Brigg) Then Room_menu_item.Add(tech_list_enum.Brigg, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Gold, "Brigg", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Cargo_Bay) Then Room_menu_item.Add(tech_list_enum.Cargo_Bay, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.White, "Cargo Bay", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Corridor) Then Room_menu_item.Add(tech_list_enum.Corridor, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Gray, "Corridor", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Crew_Quarters) Then Room_menu_item.Add(tech_list_enum.Crew_Quarters, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Green, "Crew Quarters", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Engineering) Then Room_menu_item.Add(tech_list_enum.Engineering, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Orange, "Engineering", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Fighter_Bay) Then Room_menu_item.Add(tech_list_enum.Fighter_Bay, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Red, "Fighter Bay", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Hydroponics) Then Room_menu_item.Add(tech_list_enum.Hydroponics, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Green, "Hydroponics", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Messdeck) Then Room_menu_item.Add(tech_list_enum.Messdeck, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Yellow, "Mess Deck", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Science_Room) Then Room_menu_item.Add(tech_list_enum.Science_Room, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Blue, "Science Room", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Shuttle_Bay) Then Room_menu_item.Add(tech_list_enum.Shuttle_Bay, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Red, "Shuttle Bay", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Sick_Bay) Then Room_menu_item.Add(tech_list_enum.Sick_Bay, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Blue, "Sick Bay", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
        If Player_Tech.Contains(tech_list_enum.Transporter_Room) Then Room_menu_item.Add(tech_list_enum.Transporter_Room, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(device_menu.x, device_menu.y, 80, 32), button_style.Clickable, Color.Yellow, "Transporter Room", Color.Black, d3d_font_enum.SB_small)) : device_menu.y += 32
    End Sub

    Sub Split_room_sub()
        If split_room = True Then

            If selection.x >= 0 AndAlso selection.x <= shipsize.x AndAlso selection.y >= 0 AndAlso selection.y <= shipsize.y Then
                clear_tile_map(shipsize, selection_tile_map2)
                If Build_ship.tile_map(selection.x, selection.y).roomID > 0 Then
                    Adding_To_Room_Id = Build_ship.tile_map(selection.x, selection.y).roomID

                    If Device_rotation = rotate_enum.Zero OrElse Device_rotation = rotate_enum.OneEighty Then
                        Dim ystart As Boolean = False
                        For x = selection.x To 0 Step -1
                            For y = selection.y To 0 Step -1
                                If Build_ship.tile_map(x, y).roomID = Adding_To_Room_Id Then
                                    ystart = True
                                    Build_selection = Build_ship.room_list(Build_ship.tile_map(x, y).roomID).type
                                    selection_tile_map(x + 1, y + 1) = room_sprite_enum.Floor
                                End If
                                If ystart = True AndAlso Not Build_ship.tile_map(x, y + 1).roomID = Adding_To_Room_Id Then y = 0
                            Next
                            For y = 0 To selection.y
                                If Build_ship.tile_map(x, y).roomID = Adding_To_Room_Id Then
                                    ystart = True
                                    Build_selection = Build_ship.room_list(Build_ship.tile_map(x, y).roomID).type
                                    selection_tile_map(x + 1, y + 1) = room_sprite_enum.Floor
                                End If
                                If ystart = True AndAlso Not Build_ship.tile_map(x, y + 1).roomID = Adding_To_Room_Id Then y = selection.y
                            Next
                        Next

                        ystart = False
                        For x = selection.x + 1 To shipsize.x
                            For y = 0 To shipsize.y
                                If Build_ship.tile_map(x, y).roomID = Adding_To_Room_Id Then
                                    ystart = True
                                    Build_selection = Build_ship.room_list(Build_ship.tile_map(x, y).roomID).type
                                    selection_tile_map2(x + 1, y + 1) = room_sprite_enum.Floor
                                End If
                                If ystart = True AndAlso Not Build_ship.tile_map(x, y + 1).roomID = Adding_To_Room_Id Then y = shipsize.y
                            Next
                        Next
                    End If

                End If


                Buildable = set_room_walls(shipsize, selection_tile_map)
                Buildable = set_room_walls(shipsize, selection_tile_map2)
                For x = 0 To shipsize.x
                    For y = 0 To shipsize.y
                        If selection_tile_map2(x + 1, y + 1) < 255 Then
                            selection_tile_map(x + 1, y + 1) = selection_tile_map2(x + 1, y + 1)
                        End If


                    Next
                Next
                'Buildable = False
            End If
        End If

    End Sub

    Sub Erase_Pipeing()
        Dim DoNotRemove As Boolean        
        If Pipeline_select > -1 AndAlso pipeing = True Then
            If selection.x >= 0 AndAlso selection.x <= shipsize.x AndAlso selection.y >= 0 AndAlso selection.y <= shipsize.y AndAlso Build_ship.tile_map(selection.x, selection.y).roomID > 0 AndAlso Build_ship.tile_map(selection.x, selection.y).type < tile_type_enum.empty Then
                If (Erasing = True AndAlso mouse_info.left_down = True) OrElse (Erasing = False AndAlso mouse_info.right_down = True) Then

                    If Build_ship.tile_map(selection.x, selection.y).device_tile IsNot Nothing Then
                        If Build_ship.device_list(Build_ship.tile_map(selection.x, selection.y).device_tile.device_ID).Connected_To_Pipeline(Pipeline_select) Then
                            'Check for Still connected
                            For Each tile In Build_ship.pipeline_list(Pipeline_select).Tiles
                                If Not tile.Key = selection Then
                                    If Build_ship.tile_map(tile.Key.x, tile.Key.y).device_tile IsNot Nothing AndAlso Not Build_ship.tile_map(tile.Key.x, tile.Key.y).device_tile.type = device_tile_type_enum.Empty Then
                                        If Build_ship.tile_map(tile.Key.x, tile.Key.y).device_tile.device_ID = Build_ship.tile_map(selection.x, selection.y).device_tile.device_ID Then
                                            DoNotRemove = True
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If

                    If Build_ship.pipeline_list(Pipeline_select).Tiles.ContainsKey(selection) Then Build_ship.pipeline_list(Pipeline_select).Tiles.Remove(selection)

                    If DoNotRemove = False AndAlso Build_ship.tile_map(selection.x, selection.y).device_tile IsNot Nothing Then _
                    Build_ship.device_list(Build_ship.tile_map(selection.x, selection.y).device_tile.device_ID).Disconnect_Pipeline(Pipeline_select)

                    set_pipe_sprites(Build_ship.pipeline_list)
                    Validate_pipelines = True
                End If
            End If
        End If
    End Sub


    Sub Pipeing_Sub()
        'Piping                
        If pipeing = True AndAlso Pipeline_select > -1 Then
            If selection.x >= 0 AndAlso selection.x <= shipsize.x AndAlso selection.y >= 0 AndAlso selection.y <= shipsize.y AndAlso choice = -1 Then
                If mouse_info.left_down = False Then
                    If Drawing_Pipe = False Then
                        selection_start_point.x = selection.x
                        selection_start_point.y = selection.y
                        Tile_selection.X = selection.x
                        Tile_selection.Y = selection.y
                        Tile_selection.Width = 0 : Tile_selection.Height = 0
                    End If
                Else
                    selection_end_point.x = selection.x
                    selection_end_point.y = selection.y

                    If selection_end_point.x < selection_start_point.x Then
                        Tile_selection.Width = selection_start_point.x - selection_end_point.x
                        Tile_selection.X = selection_end_point.x
                    Else
                        Tile_selection.Width = selection_end_point.x - selection_start_point.x
                        Tile_selection.X = selection_start_point.x
                    End If
                    If selection_end_point.y < selection_start_point.y Then
                        Tile_selection.Height = selection_start_point.y - selection_end_point.y
                        Tile_selection.Y = selection_end_point.y
                    Else
                        Tile_selection.Height = selection_end_point.y - selection_start_point.y
                        Tile_selection.Y = selection_start_point.y
                    End If
                    Drawing_Pipe = True
                End If

                If Tile_selection.Width = 0 AndAlso Tile_selection.Height = 0 AndAlso Drawing_room = True AndAlso mouse_info.left_down = False Then

                End If
            Else
                If left_click = True AndAlso choice = -1 Then Selecting_room = False : Room_selection = -1 : left_click = False
            End If



            'Check for pipe build cancel
            If mouse_info.right_down = True AndAlso mouse_info.left_down = True AndAlso Drawing_Pipe = True Then
                Drawing_Pipe = False
                mouse_info.left_down = False
                clear_tiles = True
            End If

            If Drawing_Pipe = True Then
                If selection.x >= 0 AndAlso selection.x <= shipsize.x AndAlso selection.y >= 0 AndAlso selection.y <= shipsize.y AndAlso choice = -1 Then
                    If Math.Abs(selection_start_point.x - selection_end_point.x) > Math.Abs(selection_start_point.y - selection_end_point.y) Then
                        Device_rotation = rotate_enum.Ninty
                        Dim loop_step As Integer = 1
                        If selection_start_point.x > selection_end_point.x Then loop_step = -1 Else loop_step = 1
                        For x = selection_start_point.x To selection_end_point.x Step loop_step
                            selection_tile_map(x + 1, selection_start_point.y + 1) = 0
                            If Build_ship.tile_map(x, selection_start_point.y).type = 255 Then Buildable = False
                        Next
                    Else
                        Dim loop_step As Integer = 1
                        If selection_start_point.y > selection_end_point.y Then loop_step = -1 Else loop_step = 1
                        Device_rotation = rotate_enum.Zero
                        For y = selection_start_point.y To selection_end_point.y Step loop_step
                            selection_tile_map(selection_start_point.x + 1, y + 1) = 0
                            If Build_ship.tile_map(selection_start_point.x, y).type = 255 Then Buildable = False
                        Next
                    End If
                End If

                If mouse_info.left_down = False Then
                    If Buildable = True Then
                        If Pipeline_select > -1 Then
                            'Build_ship.pipeline_list.Add(Pipeline_select, New Ship.ship_pipeline_type(Pipeline_type_enum.Energy, 100))
                            For x = 0 To shipsize.x
                                For y = 0 To shipsize.y
                                    If selection_tile_map(x + 1, y + 1) < 255 Then
                                        'Need Fixing
                                        If Not Build_ship.pipeline_list(Pipeline_select).Tiles.ContainsKey(New PointI(x, y)) Then
                                            'Add devices to pipeline
                                            Build_ship.pipeline_list(Pipeline_select).Tiles.Add(New PointI(x, y), Pipeline_sprite_enum.horizontal)
                                            If Build_ship.tile_map(x, y).device_tile IsNot Nothing AndAlso Not Build_ship.tile_map(x, y).device_tile.type = device_tile_type_enum.Empty Then

                                                'Build_ship.device_list(Build_ship.tile_map(selection.x, selection.y).device_tile.device_ID)
                                                If Build_ship.device_list(Build_ship.tile_map(x, y).device_tile.device_ID).Contains_Open_Pipeline(Build_ship.pipeline_list(Pipeline_select).Type) Then
                                                    Build_ship.device_list(Build_ship.tile_map(x, y).device_tile.device_ID).Connect_Pipeline(Pipeline_select, Build_ship.pipeline_list(Pipeline_select).Type)
                                                End If

                                            End If

                                        End If
                                    End If
                                Next
                            Next
                        End If
                    End If
                    clear_tiles = True
                    Drawing_Pipe = False
                    Validate_pipelines = True
                    set_pipe_sprites(Build_ship.pipeline_list)

                End If
            End If
        End If


    End Sub

    Sub Device_build()

        'Device Build
        If Build_selection > -1 AndAlso Tech_list(Build_selection).Base_type = Tech_type_enum.Device Then
            If Device_tech_list(Build_selection).Rotatable = False Then Device_rotation = rotate_enum.Zero
            If Device_tech_list(Build_selection).IsDoor = True Then If Device_rotation = rotate_enum.OneEighty Then Device_rotation = rotate_enum.Zero
            If Device_tech_list(Build_selection).IsDoor = True Then If Device_rotation = rotate_enum.TwoSeventy Then Device_rotation = rotate_enum.Ninty

            'Set center point for selection
            Select Case Device_rotation
                Case rotate_enum.Zero
                    dt_selection.x = selection.x - Device_tech_list(Build_selection).center.x
                    dt_selection.y = selection.y - Device_tech_list(Build_selection).center.y
                Case rotate_enum.Ninty
                    dt_selection.x = selection.x - ((Device_tech_list(Build_selection).cmap.Length - 1) - Device_tech_list(Build_selection).center.y)
                    dt_selection.y = selection.y - Device_tech_list(Build_selection).center.x
                Case rotate_enum.OneEighty
                    dt_selection.x = selection.x - ((Device_tech_list(Build_selection).cmap(0).Length - 1) - Device_tech_list(Build_selection).center.x)
                    dt_selection.y = selection.y - ((Device_tech_list(Build_selection).cmap.Length - 1) - Device_tech_list(Build_selection).center.y)
                Case rotate_enum.TwoSeventy
                    dt_selection.x = selection.x - Device_tech_list(Build_selection).center.y
                    dt_selection.y = selection.y - ((Device_tech_list(Build_selection).cmap(0).Length - 1) - Device_tech_list(Build_selection).center.x)
            End Select

            If Device_Flip = flip_enum.Flip_X OrElse Device_Flip = flip_enum.Both Then
            End If

            If Device_Flip = flip_enum.Flip_Y OrElse Device_Flip = flip_enum.Both Then
            End If



            Dim sprite As Integer = 0
            'clear_tiles = True
            'Set Selection Map
            Dim y_start, x_start, y_end, x_end, x_step, y_step As Integer
            Dim colx As Integer = 0
            Dim coly As Integer = 0
            Dim x, y As Integer

            'Y cmap.Length
            'X cmap(0).Length

            Select Case Device_rotation
                Case rotate_enum.Zero
                    y_start = 0 : y_end = Device_tech_list(Build_selection).cmap.Length - 1 : y_step = 1
                    x_start = 0 : x_end = Device_tech_list(Build_selection).cmap(0).Length - 1 : x_step = 1
                Case rotate_enum.Ninty
                    y_start = Device_tech_list(Build_selection).cmap.Length - 1 : y_end = 0 : y_step = -1
                    x_start = 0 : x_end = Device_tech_list(Build_selection).cmap(0).Length - 1 : x_step = 1
                Case rotate_enum.OneEighty
                    y_start = Device_tech_list(Build_selection).cmap.Length - 1 : y_end = 0 : y_step = -1
                    x_start = Device_tech_list(Build_selection).cmap(0).Length - 1 : x_end = 0 : x_step = -1
                Case rotate_enum.TwoSeventy
                    y_start = 0 : y_end = Device_tech_list(Build_selection).cmap.Length - 1 : y_step = 1
                    x_start = Device_tech_list(Build_selection).cmap(0).Length - 1 : x_end = 0 : x_step = -1
            End Select

            'Device_Flip = flip_enum.Flip_X
            If Device_Flip = flip_enum.Flip_X OrElse Device_Flip = flip_enum.Both Then
                Dim xs, xe As Integer
                xs = x_start
                xe = x_end
                x_start = xe
                x_end = xs
                x_step = -x_step
            End If

            If Device_Flip = flip_enum.Flip_Y OrElse Device_Flip = flip_enum.Both Then
                Dim ys, ye As Integer
                ys = y_start
                ye = y_end
                y_start = ye
                y_end = ys
                y_step = -y_step
            End If

            'Set Selection Tile Map
            For row = y_start To y_end Step y_step
                For column = x_start To x_end Step x_step

                    If Device_rotation = rotate_enum.Zero Then x = column : y = row
                    If Device_rotation = rotate_enum.Ninty Then x = row : y = column

                    If Device_rotation = rotate_enum.OneEighty Then x = column : y = row
                    If Device_rotation = rotate_enum.TwoSeventy Then x = row : y = column

                    Dim key As Byte = Device_tech_list(Build_selection).cmap(coly)(colx)
                    colx += 1
                    If colx >= Device_tech_list(Build_selection).cmap(0).Length Then colx = 0 : coly += 1

                    If key >= 1 AndAlso key <= 4 Then
                        If dt_selection.x + x < 0 Then sprite += 1
                        If dt_selection.y + y < 0 Then sprite += 1
                        If dt_selection.x + x > shipsize.x Then sprite += 1

                        If dt_selection.x + x > shipsize.x AndAlso dt_selection.y + y < 0 Then sprite -= 1
                        If dt_selection.x + x < 0 AndAlso dt_selection.y + y < 0 Then sprite -= 1

                        If dt_selection.x + x >= 0 AndAlso dt_selection.x + x <= shipsize.x AndAlso dt_selection.y + y >= 0 AndAlso dt_selection.y + y <= shipsize.y Then
                            selection_tile_map(dt_selection.x + x + 1, dt_selection.y + y + 1) = Convert.ToByte(sprite)
                            sprite += 1
                        End If
                    End If

                    If key = 5 Then
                        If dt_selection.x + x >= 0 AndAlso dt_selection.x + x <= shipsize.x AndAlso dt_selection.y + y >= 0 AndAlso dt_selection.y + y <= shipsize.y Then
                            selection_tile_map(dt_selection.x + x + 1, dt_selection.y + y + 1) = 253
                        End If
                    End If


                    If key >= 6 AndAlso key <= 9 Then
                        If dt_selection.x + x >= 0 AndAlso dt_selection.x + x <= shipsize.x AndAlso dt_selection.y + y >= 0 AndAlso dt_selection.y + y <= shipsize.y Then
                            selection_tile_map(dt_selection.x + x + 1, dt_selection.y + y + 1) = 254
                        End If
                    End If

                    

                Next
            Next




            'check for buildable
            Dim device_to_room_id As Integer = 0

            colx = 0 : coly = 0
            For row = y_start To y_end Step y_step
                For column = x_start To x_end Step x_step

                    If Device_rotation = rotate_enum.Zero Then x = column : y = row
                    If Device_rotation = rotate_enum.Ninty Then x = row : y = column

                    If Device_rotation = rotate_enum.OneEighty Then x = column : y = row
                    If Device_rotation = rotate_enum.TwoSeventy Then x = row : y = column

                    Dim key As Byte = Device_tech_list(Build_selection).cmap(coly)(colx)
                    colx += 1
                    If colx >= Device_tech_list(Build_selection).cmap(0).Length Then colx = 0 : coly += 1


                    If dt_selection.x + x >= 0 AndAlso dt_selection.x + x <= shipsize.x AndAlso dt_selection.y + y >= 0 AndAlso dt_selection.y + y <= shipsize.y Then

                        If key > 0 Then
                            If selection_tile_map(dt_selection.x + x + 1, dt_selection.y + y + 1) = 255 Then
                                'If key <= 4 Then selection_tile_map(dt_selection.x + x + 1, dt_selection.y + y + 1) = 253
                                'If key >= 5 Then selection_tile_map(dt_selection.x + x + 1, dt_selection.y + y + 1) = 254
                            End If
                        End If

                        'Check cmap to tiles
                        Dim tile As Integer = Build_ship.tile_map(dt_selection.x + x, dt_selection.y + y).sprite

                        If key >= 1 AndAlso key <= 5 Then
                            If Build_ship.tile_map(dt_selection.x + x, dt_selection.y + y).device_tile IsNot Nothing Then
                                If Not Build_ship.tile_map(dt_selection.x + x, dt_selection.y + y).device_tile.type = device_tile_type_enum.Device_Base Then
                                    Buildable = False
                                End If
                            End If
                        End If

                        'If Not key = 0 AndAlso Build_ship.tile_map(dt_selection.x + x, dt_selection.y + y).device_tile IsNot Nothing AndAlso Not Build_ship.tile_map(dt_selection.x + x, dt_selection.y + y).device_tile.type = device_tile_type_enum.Restricted Then Buildable = False

                        For Each room In Build_ship.room_list
                            If room.Value.access_point.ContainsKey(New PointI(dt_selection.x + x, dt_selection.y + y)) Then Buildable = False
                        Next

                        Select Case key

                            Case Is = 1, 6, 10 'Check for floor
                                If Not tile = room_sprite_enum.Floor Then Buildable = False
                                device_to_room_id = Build_ship.tile_map(dt_selection.x + x, dt_selection.y + y).roomID
                            Case Is = 2, 7, 11 'Check for walls
                                If tile < 4 OrElse tile > 7 Then Buildable = False
                                device_to_room_id = Build_ship.tile_map(dt_selection.x + x, dt_selection.y + y).roomID
                            Case Is = 3, 8, 12 'Check for empty or device base
                                If Not Build_ship.tile_map(dt_selection.x + x, dt_selection.y + y).type = tile_type_enum.empty AndAlso Not Build_ship.tile_map(dt_selection.x + x, dt_selection.y + y).type = tile_type_enum.Device_Base Then Buildable = False
                                If Build_ship.tile_map(dt_selection.x + x, dt_selection.y + y).type = tile_type_enum.Device_Base Then
                                    If (Build_ship.tile_map(dt_selection.x + x, dt_selection.y + y).device_tile IsNot Nothing AndAlso Not Build_ship.tile_map(dt_selection.x + x, dt_selection.y + y).device_tile.type = device_tile_type_enum.Device_Base) Then Buildable = False
                                End If
                            Case Is = 4, 9, 13 'Check for corner
                                If tile < 0 OrElse tile > 3 Then Buildable = False
                                device_to_room_id = Build_ship.tile_map(dt_selection.x + x, dt_selection.y + y).roomID
                            Case Is = 5 'Check for AP
                                If Not tile = room_sprite_enum.Floor Then Buildable = False
                        End Select
                    Else
                        'check if important tiles are not on the map
                        If Not key = 0 AndAlso key < 5 Then Buildable = False

                    End If
                    'Check if the room type matches the device
                    If device_to_room_id > 1 Then
                        If Not Tech_list(Build_selection).Device_room_type = tech_list_enum.All_rooms AndAlso Not Build_ship.room_list(device_to_room_id).type = Tech_list(Build_selection).Device_room_type Then Buildable = False
                    End If
                Next
            Next



            'build device
            If Buildable = True Then
                If mouse_info.left_down = False AndAlso left_click = True Then
                    Dim id As Integer
                    For id = 0 To 100000
                        If Build_ship.device_list.ContainsKey(id) = False Then Exit For
                    Next
                    sprite = 0
                    Dim tile_list As HashSet(Of PointI) = New HashSet(Of PointI)
                    For y = 0 To shipsize.y
                        For x = 0 To shipsize.x
                            'If Device_tech_list(Build_selection).cmap(y)(x) > 0 Then
                            If selection_tile_map(x + 1, y + 1) < 255 Then

                                If Device_tech_list(Build_selection).IsDoor = True Then
                                    Dim basetile As Ship_tile = Build_ship.tile_map(dt_selection.x, dt_selection.y)
                                    Build_ship.tile_map(x, y) = New Ship_tile(Build_ship.tile_map(x, y).roomID, Build_ship.tile_map(x, y).type, 0, basetile.integrity, room_sprite_enum.Floor, walkable_type_enum.Walkable)
                                End If


                                If selection_tile_map(x + 1, y + 1) < 253 Then
                                    If Build_ship.tile_map(x, y).type = tile_type_enum.empty Then Build_ship.tile_map(x, y) = New Ship_tile(Convert.ToByte(device_to_room_id), tile_type_enum.Device_Base, 0, 0, room_sprite_enum.empty, walkable_type_enum.Walkable)

                                    Build_ship.tile_map(x, y).walkable = walkable_type_enum.HasDevice
                                    If Device_tech_list(Build_selection).IsDoor = True Then Build_ship.tile_map(x, y).walkable = walkable_type_enum.Door
                                    If Device_tech_list(Build_selection).type = device_type_enum.landing_bay Then Build_ship.tile_map(x, y).walkable = walkable_type_enum.Walkable


                                    Build_ship.tile_map(x, y).device_tile = New Device_tile(id, selection_tile_map(x + 1, y + 1), Device_rotation, Device_Flip, 0, Device_tech_list(Build_selection).tile_type)
                                    tile_list.Add(New PointI(x, y))
                                End If

                                'Access point
                                If selection_tile_map(x + 1, y + 1) = 253 Then
                                    'Build_ship.tile_map(x, y).device_tile = New Device_tile(id, selection_tile_map(x + 1, y + 1), Device_rotation, Device_Flip, 0, Device_tech_list(Build_selection).tile_type)
                                    Build_ship.room_list(device_to_room_id).access_point.Add(New PointI(x, y), New access_point_type(False, id))
                                End If

                                'Do not set sprite
                                If selection_tile_map(x + 1, y + 1) = 254 Then
                                    If Build_ship.tile_map(x, y).type = 255 Then
                                        Build_ship.tile_map(x, y) = New Ship_tile(Convert.ToByte(device_to_room_id), tile_type_enum.Device_Base, 0, 0, room_sprite_enum.empty, walkable_type_enum.Walkable)
                                        Build_ship.tile_map(x, y).device_tile = New Device_tile(id, -1, rotate_enum.Zero, flip_enum.None, 0, device_tile_type_enum.Device_Base)
                                    End If
                                    If Build_ship.tile_map(x, y).type < 255 Then
                                        If Build_ship.tile_map(x, y).device_tile Is Nothing Then
                                            Build_ship.tile_map(x, y).device_tile = New Device_tile(id, -1, rotate_enum.Zero, flip_enum.None, 0, device_tile_type_enum.Device_Base)
                                        Else
                                            'Set IdHash for Device Base
                                            If Build_ship.tile_map(x, y).device_tile.type = device_tile_type_enum.Device_Base Then
                                                Build_ship.tile_map(x, y).device_tile.IDhash.Add(id)
                                            End If
                                        End If
                                    End If
                                End If

                            End If

                        Next
                    Next

                    Build_ship.device_list.Add(id, New Ship_device(Device_tech_list(Build_selection).type, Device_tech_list(Build_selection).integrity, Device_tech_list(Build_selection).integrity, 0, Device_tech_list(Build_selection).required_Points, Device_tech_list(Build_selection).pipeline, Build_selection, tile_list))




                    'Set Device Face
                    If Device_Flip = flip_enum.None Then
                        Build_ship.device_list(id).Device_Face = rotate_enum.Zero
                    ElseIf Device_Flip = flip_enum.Flip_X Then
                        Build_ship.device_list(id).Device_Face = rotate_enum.OneEighty
                    End If


                    'If Build_ship.device_list(id).type = device_type_enum.weapon Then
                    Build_ship.device_list(id).Device_Face = Device_rotation
                    'End If


                    'Set device Thrust Direction
                    If Build_ship.device_list(id).type = device_type_enum.engine Then
                        Select Case Build_ship.device_list(id).Device_Face
                            Case Is = rotate_enum.Zero
                                Build_ship.device_list(id).Thrust_Direction = Direction_Enum.Bottom
                            Case Is = rotate_enum.Ninty
                                Build_ship.device_list(id).Thrust_Direction = Direction_Enum.Left
                            Case Is = rotate_enum.OneEighty
                                Build_ship.device_list(id).Thrust_Direction = Direction_Enum.Top
                            Case Is = rotate_enum.TwoSeventy
                                Build_ship.device_list(id).Thrust_Direction = Direction_Enum.Right
                        End Select
                    End If

                    If Build_ship.device_list(id).type = device_type_enum.thruster Then
                        Select Case Build_ship.device_list(id).Device_Face
                            Case Is = rotate_enum.Zero
                                Build_ship.device_list(id).Thrust_Direction = Direction_Enum.Left
                            Case Is = rotate_enum.Ninty
                                Build_ship.device_list(id).Thrust_Direction = Direction_Enum.Top
                            Case Is = rotate_enum.OneEighty
                                Build_ship.device_list(id).Thrust_Direction = Direction_Enum.Right
                            Case Is = rotate_enum.TwoSeventy
                                Build_ship.device_list(id).Thrust_Direction = Direction_Enum.Bottom
                        End Select
                    End If


                    'Set Thrust Point/Weapon point
                    If Device_tech_list(Build_ship.device_list(id).tech_ID).type = device_type_enum.engine OrElse Device_tech_list(Build_ship.device_list(id).tech_ID).type = device_type_enum.thruster OrElse Device_tech_list(Build_ship.device_list(id).tech_ID).type = device_type_enum.weapon Then
                        Dim Active_Point As PointI
                        Select Case Build_ship.device_list(id).Device_Face
                            Case rotate_enum.Zero
                                Active_Point.x = Device_tech_list(Build_ship.device_list(id).tech_ID).Active_Point.x + dt_selection.x
                                Active_Point.y = Device_tech_list(Build_ship.device_list(id).tech_ID).Active_Point.y + dt_selection.y
                            Case rotate_enum.Ninty
                                Active_Point.x = (Device_tech_list(Build_ship.device_list(id).tech_ID).cmap.Length - 1 - Device_tech_list(Build_ship.device_list(id).tech_ID).Active_Point.y) + dt_selection.x
                                Active_Point.y = (Device_tech_list(Build_ship.device_list(id).tech_ID).cmap(0).Length - 1 - Device_tech_list(Build_ship.device_list(id).tech_ID).Active_Point.x) + dt_selection.y
                            Case rotate_enum.OneEighty
                                Active_Point.x = (Device_tech_list(Build_ship.device_list(id).tech_ID).cmap(0).Length - 1 - Device_tech_list(Build_ship.device_list(id).tech_ID).Active_Point.x) + dt_selection.x
                                Active_Point.y = (Device_tech_list(Build_ship.device_list(id).tech_ID).cmap.Length - 1 - Device_tech_list(Build_ship.device_list(id).tech_ID).Active_Point.y) + dt_selection.y
                            Case rotate_enum.TwoSeventy
                                Active_Point.x = Device_tech_list(Build_ship.device_list(id).tech_ID).Active_Point.y + dt_selection.x
                                Active_Point.y = Device_tech_list(Build_ship.device_list(id).tech_ID).Active_Point.x + dt_selection.y
                        End Select
                        Build_ship.device_list(id).Active_Point = Active_Point
                    End If


                    'Set to pipeline
                    For Each tile In Build_ship.device_list(id).tile_list
                        For Each pipe In Build_ship.pipeline_list
                            For Each pipe_tile In pipe.Value.Tiles
                                If pipe_tile.Key = tile Then
                                    If Build_ship.device_list(id).Contains_Open_Pipeline(pipe.Value.Type) Then
                                        Build_ship.device_list(id).Connect_Pipeline(pipe.Key, pipe.Value.Type)
                                    End If
                                End If
                            Next
                        Next
                    Next


                    For Each tile In Build_ship.tile_map
                        If tile.roomID > 0 Then
                            If tile.device_tile IsNot Nothing AndAlso tile.device_tile.device_ID = id Then Build_ship.room_list(tile.roomID).device_list.Add(id)
                        End If
                    Next
                    clear_tiles = True
                    'Build_save_ship(Build_ship)
                End If
            End If
        End If

    End Sub

    Sub Room_Building()
        ' room building
        If Build_selection > -1 AndAlso split_room = False AndAlso Removeing_room = False AndAlso Tech_list(Build_selection).Base_type = Tech_type_enum.Room Then


            'Check for room build cancel
            If mouse_info.right_down = True AndAlso mouse_info.left_down = True AndAlso Drawing_room = True Then
                Drawing_room = False
                mouse_info.left_down = False
                mouse_info.right_down = False
                clear_tiles = True  'clear_tile_map(shipsize, selection_tile_map)
                Buildable = False
                left_click = False
                right_click = False
            End If


            'Start of building room
            'Make Selection and set tiles
            If Drawing_room = True Then

                'Add's other room tiles if adding
                If Adding_To_Room = True Then
                    For x = 0 To shipsize.x
                        For y = 0 To shipsize.y
                            If Build_ship.tile_map(x, y).roomID = Adding_To_Room_Id Then
                                'If Build_ship.tile_map(x, y).device_tile IsNot Nothing Or Build_ship.tile_map(x, y).type = tile_type_enum.empty Then
                                'selection_tile_map(x + 1, y + 1) = 254
                                'Else
                                If Build_ship.tile_map(x, y).type < tile_type_enum.Device_Base Then selection_tile_map(x + 1, y + 1) = room_sprite_enum.Floor
                                'End If
                            End If
                            'If Build_ship.tile_map(x, y).roomID = Adding_To_Room_Id Then selection_tile_map(x + 1, y + 1) = room_sprite_enum.Floor
                        Next
                    Next
                End If

                'Add's room tiles for selection
                For x = Tile_selection.X To Tile_selection.Right
                    For y = Tile_selection.Y To Tile_selection.Bottom
                        selection_tile_map(x + 1, y + 1) = room_sprite_enum.Floor
                        'check for device tile
                        'If Build_ship.tile_map(x, y).device_tile IsNot Nothing Then Buildable = False
                    Next
                Next

                'Checks all tiles in map and sets correct tile          
                If set_room_walls(shipsize, selection_tile_map) = False AndAlso Buildable = True Then Buildable = False                

                'Make New Room
                'Check for overlap                        
                Adding_To_Room_Id = -1
                Adding_To_Room = False

                If Tech_list(Build_selection).Base_type = Tech_type_enum.Armor Then Adding_To_Room_Id = 1 : Adding_To_Room = True

                If Tile_selection.Width >= 1 AndAlso Tile_selection.Height >= 1 Then
                    For x = Tile_selection.X To Tile_selection.Right
                        For y = Tile_selection.Y To Tile_selection.Bottom
                            If selection_tile_map(x + 1, y + 1) < room_sprite_enum.empty Then
                                If Build_ship.tile_map(x, y).roomID > 0 Then
                                    If Adding_To_Room_Id = -1 Then Adding_To_Room_Id = Build_ship.tile_map(x, y).roomID
                                    If Adding_To_Room_Id = Build_ship.tile_map(x, y).roomID AndAlso Build_ship.tile_map(x, y).type = Tech_list(Build_selection).tile_type Then
                                        Adding_To_Room = True
                                    Else
                                        Buildable = False : Adding_To_Room = False
                                    End If
                                End If
                            End If
                        Next
                    Next
                Else
                    Buildable = False
                End If

                'Adding to room
                If Tile_selection.Width >= 1 AndAlso Tile_selection.Height >= 1 AndAlso Buildable = True Then 'Check for size
                    'Selection_color = Color.FromArgb(150, 100, 255, 100)
                    If mouse_info.left_down = False Then
                        If Adding_To_Room = False Then
                            For r = 2 To 255
                                If Build_ship.room_list.ContainsKey(r) = False Then Adding_To_Room_Id = r : Exit For 'Need to put safe guards
                            Next
                        End If
                        Dim walkable As walkable_type_enum
                        'Dim device_tile_save As Device_tile = Nothing
                        For x = 0 To shipsize.x
                            For y = 0 To shipsize.y
                                If selection_tile_map(x + 1, y + 1) < room_sprite_enum.empty Then
                                    'Set Room Tiles
                                    If Tech_list(Build_selection).Base_type = Tech_type_enum.Room Then
                                        If Not selection_tile_map(x + 1, y + 1) = room_sprite_enum.Floor Then walkable = walkable_type_enum.Impassable Else walkable = walkable_type_enum.Walkable
                                        If Build_ship.tile_map(x, y).device_tile Is Nothing Then
                                            Build_ship.tile_map(x, y) = New Ship_tile(Convert.ToByte(Adding_To_Room_Id), CType(Tech_list(Build_selection).tile_type, tile_type_enum), 0, 100, CType(selection_tile_map(x + 1, y + 1), room_sprite_enum), walkable)
                                        End If
                                    End If
                                End If
                            Next
                        Next
                        'Done creating room, Add to room list and clear selection
                        If Adding_To_Room = False Then Build_ship.room_list.Add(Adding_To_Room_Id, New ship_room_type(Build_selection))
                        Drawing_room = False : Adding_To_Room = False
                        clear_tiles = True
                        'Build_save_ship(Build_ship)
                        Validate_rooms = True
                    End If
                Else
                    'Selection_color = Color.FromArgb(150, 255, 100, 100)
                    If mouse_info.left_down = False Then
                        clear_tiles = True  'clear_tile_map(shipsize, selection_tile_map)
                        Drawing_room = False
                    End If
                End If
            End If
        End If

    End Sub


    Sub Box_selection()
        If selection.x >= 0 AndAlso selection.x <= shipsize.x AndAlso selection.y >= 0 AndAlso selection.y <= shipsize.y Then

            If Drawing_room = False AndAlso Removeing_room = False AndAlso pipeing = False Then
                selection_start_point.x = selection.x
                selection_start_point.y = selection.y
                Tile_selection.X = selection.x
                Tile_selection.Y = selection.y
                Tile_selection.Width = 0 : Tile_selection.Height = 0
            End If

            If mouse_info.left_down = True OrElse mouse_info.right_down = True Then

                selection_end_point.x = selection.x
                selection_end_point.y = selection.y

                If selection_end_point.x < selection_start_point.x Then
                    Tile_selection.Width = selection_start_point.x - selection_end_point.x
                    Tile_selection.X = selection_end_point.x
                Else
                    Tile_selection.Width = selection_end_point.x - selection_start_point.x
                    Tile_selection.X = selection_start_point.x
                End If
                If selection_end_point.y < selection_start_point.y Then
                    Tile_selection.Height = selection_start_point.y - selection_end_point.y
                    Tile_selection.Y = selection_end_point.y
                Else
                    Tile_selection.Height = selection_end_point.y - selection_start_point.y
                    Tile_selection.Y = selection_start_point.y
                End If
                If Build_selection > -1 AndAlso Tech_list(Build_selection).Base_type = Tech_type_enum.Room Then
                    If mouse_info.left_down = True AndAlso Removeing_room = False Then Drawing_room = True
                    If mouse_info.right_down = True AndAlso Drawing_room = False Then Removeing_room = True
                End If
                Selecting_room = False
                Room_selection = -1
            End If

        End If



    End Sub


    Sub Room_Remove()
        'room removeing

        'Box selection calculation        
        'Check for room build cancel
        If mouse_info.left_down = True AndAlso mouse_info.right_down = True AndAlso Removeing_room = True Then
            Removeing_room = False
            mouse_info.right_down = False
            mouse_info.left_down = False
            clear_tiles = True
            Buildable = False
            left_click = False
            right_click = False
        End If


        If Removeing_room = True AndAlso Drawing_room = False Then
            'check for overlap and room id
            Remove_Room_ID = -1

            If Tile_selection.Width >= 0 AndAlso Tile_selection.Height >= 0 Then
                For x = Tile_selection.X To Tile_selection.Right
                    For y = Tile_selection.Y To Tile_selection.Bottom
                        If Build_ship.tile_map(x, y).roomID > 0 Then
                            If Remove_Room_ID = -1 Then
                                Remove_Room_ID = Build_ship.tile_map(x, y).roomID
                            Else
                                If Not Remove_Room_ID = Build_ship.tile_map(x, y).roomID Then Buildable = False
                            End If
                        End If                        
                    Next
                Next
            End If


            'add room tiles
            If Remove_Room_ID > -1 Then
                For x = 0 To shipsize.x
                    For y = 0 To shipsize.y
                        If Build_ship.tile_map(x, y).roomID = Remove_Room_ID Then
                            If Build_ship.tile_map(x, y).type < tile_type_enum.Device_Base Then selection_tile_map(x + 1, y + 1) = room_sprite_enum.Floor                            
                        End If
                    Next
                Next
            End If

            'Add's empty tiles to selection
            For x = Tile_selection.X To Tile_selection.Right
                For y = Tile_selection.Y To Tile_selection.Bottom
                    selection_tile_map(x + 1, y + 1) = room_sprite_enum.empty
                Next
            Next

            'Check for device in selection
            For x = Tile_selection.X - 1 To Tile_selection.Right + 1
                For y = Tile_selection.Y - 1 To Tile_selection.Bottom + 1
                    If x >= 0 AndAlso x < Build_ship.shipsize.x AndAlso y >= 0 AndAlso y < Build_ship.shipsize.y Then
                        If Build_ship.tile_map(x, y).device_tile IsNot Nothing Then Buildable = False
                        If Remove_Room_ID > -1 Then
                            If Build_ship.room_list(Remove_Room_ID).access_point.ContainsKey(New PointI(x, y)) Then Buildable = False
                        End If
                    End If
                Next
            Next



            'Checks all tiles in map and sets correct tile
            If set_room_walls(shipsize, selection_tile_map) = False AndAlso Buildable = True Then Buildable = False


            'Adding to room
            If Tile_selection.Width >= 0 AndAlso Tile_selection.Height >= 0 AndAlso Buildable = True Then 'Check for size                
                If mouse_info.right_down = False Then
                    Dim walkable As walkable_type_enum
                    'Dim device_tile_save As Device_tile = Nothing
                    For x = 0 To shipsize.x
                        For y = 0 To shipsize.y
                            If selection_tile_map(x + 1, y + 1) < room_sprite_enum.empty Then
                                'Set Room Tiles
                                Dim tile_type As tile_type_enum = CType(Tech_list(Build_ship.room_list(Remove_Room_ID).type).tile_type, tile_type_enum)

                                If Not selection_tile_map(x + 1, y + 1) = room_sprite_enum.Floor Then walkable = walkable_type_enum.Impassable Else walkable = walkable_type_enum.Walkable
                                If Build_ship.tile_map(x, y).device_tile Is Nothing Then
                                    Build_ship.tile_map(x, y) = New Ship_tile(Convert.ToByte(Remove_Room_ID), tile_type, 0, 100, CType(selection_tile_map(x + 1, y + 1), room_sprite_enum), walkable)
                                End If


                            Else

                                If Build_ship.tile_map(x, y).roomID = Remove_Room_ID Then
                                    If Build_ship.tile_map(x, y).type < tile_type_enum.Device_Base Then
                                        Build_ship.tile_map(x, y) = New Ship_tile(0, tile_type_enum.empty, 0, 0, 0, walkable_type_enum.Impassable)
                                        For Each pipeline In Build_ship.pipeline_list
                                            If pipeline.Value.Tiles.ContainsKey(New PointI(x, y)) Then pipeline.Value.Tiles.Remove(New PointI(x, y))
                                        Next
                                    End If
                                End If

                            End If
                        Next
                    Next
                    'Done removing
                    Removeing_room = False
                    clear_tiles = True
                    Validate_rooms = True
                    Validate_pipelines = True
                    set_pipe_sprites(Build_ship.pipeline_list)
                End If
            Else
                If mouse_info.right_down = False Then
                    clear_tiles = True
                    Removeing_room = False
                End If
            End If
        End If
    End Sub


    Sub Armor_Building()
        ' room building
        If Build_selection > -1 AndAlso Tech_list(Build_selection).Base_type = Tech_type_enum.Armor Then
            'Box selection calculation
            If selection.x >= 0 AndAlso selection.x <= shipsize.x AndAlso selection.y >= 0 AndAlso selection.y <= shipsize.y AndAlso choice = -1 Then
                If mouse_info.left_down = False Then
                    If Drawing_room = False Then
                        selection_start_point.x = selection.x
                        selection_start_point.y = selection.y
                        Tile_selection.X = selection.x
                        Tile_selection.Y = selection.y
                        Tile_selection.Width = 0 : Tile_selection.Height = 0
                    End If
                Else
                    selection_end_point.x = selection.x
                    selection_end_point.y = selection.y

                    If selection_end_point.x < selection_start_point.x Then
                        Tile_selection.Width = selection_start_point.x - selection_end_point.x
                        Tile_selection.X = selection_end_point.x
                    Else
                        Tile_selection.Width = selection_end_point.x - selection_start_point.x
                        Tile_selection.X = selection_start_point.x
                    End If
                    If selection_end_point.y < selection_start_point.y Then
                        Tile_selection.Height = selection_start_point.y - selection_end_point.y
                        Tile_selection.Y = selection_end_point.y
                    Else
                        Tile_selection.Height = selection_end_point.y - selection_start_point.y
                        Tile_selection.Y = selection_start_point.y
                    End If
                    Drawing_room = True
                    Selecting_room = False
                    Room_selection = -1
                End If

                'If Tile_selection.Width = 0 AndAlso Tile_selection.Height = 0 AndAlso Drawing_room = True AndAlso mouse_info.left_down = False Then
                'End If

            Else
                If left_click = True AndAlso choice = -1 Then Selecting_room = False : Room_selection = -1 : left_click = False
            End If


            'Check for room build cancel
            If mouse_info.right_down = True AndAlso mouse_info.left_down = True AndAlso Drawing_room = True Then
                Drawing_room = False
                mouse_info.left_down = False
                clear_tiles = True
            End If


            'Start of building room
            'Make Selection and set tiles
            If Drawing_room = True Then
                'Clears selection tile map
                'clear_tiles = True

                'Add's room tiles for selection
                For x = Tile_selection.X To Tile_selection.Right
                    For y = Tile_selection.Y To Tile_selection.Bottom
                        selection_tile_map(x + 1, y + 1) = room_sprite_enum.Floor
                    Next
                Next

                'Make New Room                                
                For x = Tile_selection.X To Tile_selection.Right
                    For y = Tile_selection.Y To Tile_selection.Bottom
                        If selection_tile_map(x + 1, y + 1) < room_sprite_enum.empty Then
                            If Build_ship.tile_map(x, y).roomID > 1 Then selection_tile_map(x + 1, y + 1) = room_sprite_enum.empty

                        End If
                    Next
                Next

                'Set room
                If Buildable = True Then

                    'Add's all armor tiles to map
                    For x = 0 To shipsize.x
                        For y = 0 To shipsize.y
                            If Build_ship.tile_map(x, y).roomID = 1 Then
                                If Not Build_ship.tile_map(x, y).sprite = room_sprite_enum.blank Then selection_tile_map(x + 1, y + 1) = room_sprite_enum.Floor
                            End If
                        Next
                    Next

                    'Checks all tiles in map and sets correct tile
                    'Buildable = 
                    set_armor_sprites(shipsize, selection_tile_map)


                    If mouse_info.left_down = False Then
                        For x = 0 To shipsize.x
                            For y = 0 To shipsize.y
                                If selection_tile_map(x + 1, y + 1) < room_sprite_enum.empty Then
                                    'Set Room Tiles
                                    If Build_ship.tile_map(x, y).roomID = 1 Then
                                        Build_ship.tile_map(x, y) = New Ship_tile(1, Build_ship.tile_map(x, y).type, 0, 100, CType(selection_tile_map(x + 1, y + 1), room_sprite_enum), walkable_type_enum.Impassable)
                                    Else
                                        Build_ship.tile_map(x, y) = New Ship_tile(1, CType(Tech_list(Build_selection).tile_type, tile_type_enum), 0, 100, CType(selection_tile_map(x + 1, y + 1), room_sprite_enum), walkable_type_enum.Impassable)
                                    End If

                                End If
                            Next
                        Next
                        'Does not creat room                        
                        If Not Build_ship.room_list.ContainsKey(1) Then Build_ship.room_list.Add(1, New ship_room_type(tech_list_enum.ArmorLV1))
                        Drawing_room = False
                        clear_tiles = True
                    End If
                Else
                    If mouse_info.left_down = False Then
                        clear_tiles = True
                        Drawing_room = False
                    End If
                End If
            End If
        End If

    End Sub

    Sub Erasing_Sub()

        Buildable = False
        Dim clear_id As Integer

        If selection.x >= 0 AndAlso selection.x <= shipsize.x AndAlso selection.y >= 0 AndAlso selection.y <= shipsize.y AndAlso Build_ship.tile_map(selection.x, selection.y).roomID > 0 AndAlso Build_ship.tile_map(selection.x, selection.y).type < tile_type_enum.empty Then

            'Needfix
            'Clear room
            If Build_ship.tile_map(selection.x, selection.y).Has_Device_Tile = False Then
                clear_id = Build_ship.tile_map(selection.x, selection.y).roomID
                If clear_id = 1 Then
                    selection_tile_map(selection.x + 1, selection.y + 1) = room_sprite_enum.blank
                Else
                    For x = 0 To Build_ship.shipsize.x
                        For y = 0 To Build_ship.shipsize.y
                            If Build_ship.tile_map(x, y).roomID = clear_id Then selection_tile_map(x + 1, y + 1) = room_sprite_enum.blank
                        Next
                    Next
                End If
            Else
                'Clear Devices
                clear_id = Build_ship.tile_map(selection.x, selection.y).device_tile.device_ID
                For x = 0 To Build_ship.shipsize.x
                    For y = 0 To Build_ship.shipsize.y
                        If Build_ship.tile_map(x, y).Has_Device_Tile AndAlso Build_ship.tile_map(x, y).device_tile.ContainsID(clear_id) Then selection_tile_map(x + 1, y + 1) = room_sprite_enum.blank
                    Next
                Next

            End If


            'Clear
            If left_click = True Then

                'Clear room
                If Build_ship.tile_map(selection.x, selection.y).device_tile Is Nothing AndAlso Build_ship.tile_map(selection.x, selection.y).roomID > 1 Then
                    clear_id = Build_ship.tile_map(selection.x, selection.y).roomID

                    If Build_ship.room_list(clear_id).device_list.Count > 0 Then
                        Dim device_list(Build_ship.room_list(clear_id).device_list.Count - 1) As Integer
                        Build_ship.room_list(clear_id).device_list.CopyTo(device_list, 0)
                        For Each item In device_list
                            Remove_Device(item)
                        Next
                    End If

                    'remove pipelines
                    For x = 0 To Build_ship.shipsize.x
                        For y = 0 To Build_ship.shipsize.y
                            If Build_ship.tile_map(x, y).roomID = clear_id Then
                                Build_ship.tile_map(x, y) = New Ship_tile(0, tile_type_enum.empty, 0, 0, 0, walkable_type_enum.Walkable)
                                Dim removelist As HashSet(Of Integer) = New HashSet(Of Integer)
                                For Each pipe In Build_ship.pipeline_list
                                    'If pipe.Value.Tiles.ContainsKey(New PointI(x, y)) Then pipe.Value.Tiles.Remove(New PointI(x, y))
                                    If pipe.Value.Tiles.ContainsKey(New PointI(x, y)) Then removelist.Add(pipe.Key)
                                Next
                                For Each item In removelist
                                    Build_ship.pipeline_list(item).Tiles.Clear()
                                Next
                            End If
                        Next
                    Next
                    set_pipe_sprites(Build_ship.pipeline_list)
                    Validate_pipelines = True

                    Build_ship.room_list.Remove(clear_id)
                    'Build_save_ship(Build_ship)
                End If



                'Clear device
                If Build_ship.tile_map(selection.x, selection.y).Has_Device_Tile Then
                    clear_id = Build_ship.tile_map(selection.x, selection.y).device_tile.device_ID
                    Remove_Device(clear_id)
                    Build_save_ship(Build_ship)
                End If
            End If



            If mouse_info.left_down = True Then
                'Clear armor
                If Build_ship.tile_map(selection.x, selection.y).roomID = 1 Then
                    Build_ship.tile_map(selection.x, selection.y) = New Ship_tile(0, tile_type_enum.empty, 0, 0, 0, walkable_type_enum.Walkable)

                    For x = 0 To shipsize.x
                        For y = 0 To shipsize.y
                            If Build_ship.tile_map(x, y).roomID = 1 Then
                                If Not Build_ship.tile_map(x, y).sprite = room_sprite_enum.blank Then selection_tile_map(x + 1, y + 1) = room_sprite_enum.Floor
                            End If
                        Next
                    Next

                    set_armor_sprites(shipsize, selection_tile_map)

                    For x = 0 To shipsize.x
                        For y = 0 To shipsize.y
                            If selection_tile_map(x + 1, y + 1) < room_sprite_enum.empty Then
                                'Set Room Tiles
                                If Build_ship.tile_map(x, y).roomID = 1 Then
                                    Build_ship.tile_map(x, y) = New Ship_tile(1, Build_ship.tile_map(x, y).type, 0, 100, CType(selection_tile_map(x + 1, y + 1), room_sprite_enum), walkable_type_enum.Walkable)
                                End If

                            End If
                        Next
                    Next
                    'Does not creat room                        
                    If Not Build_ship.room_list.ContainsKey(1) Then Build_ship.room_list.Add(1, New ship_room_type(tech_list_enum.Armor))
                    Drawing_room = False
                    clear_tiles = True

                End If
            End If


        End If

    End Sub


    Sub Remove_Device(ByVal clear_id As Integer)
        For x = 0 To Build_ship.shipsize.x
            For y = 0 To Build_ship.shipsize.y
                If Build_ship.tile_map(x, y).Has_Device_Tile AndAlso Build_ship.tile_map(x, y).device_tile.ContainsID(clear_id) Then
                    If Build_ship.tile_map(x, y).device_tile.IDhash.Count > 1 Then
                        Build_ship.tile_map(x, y).device_tile.IDhash.Remove(clear_id)
                    Else                        
                        'If Build_ship.tile_map(x, y).type = tile_type_enum.empty Then Build_ship.tile_map(x, y) = New Ship_tile(0, tile_type_enum.empty, 0, 0, room_sprite_enum.empty, walkable_type_enum.Impassable)
                        If Build_ship.tile_map(x, y).type = tile_type_enum.Device_Base Then
                            Build_ship.tile_map(x, y) = New Ship_tile(0, tile_type_enum.empty, 0, 0, room_sprite_enum.empty, walkable_type_enum.Walkable)
                        End If

                        'If Build_ship.tile_map(x, y).type = tile_type_enum.Restricted Then Build_ship.tile_map(x, y) = New Ship_tile(0, tile_type_enum.empty, 0, 0, room_sprite_enum.empty, walkable_type_enum.Walkable)

                        If Device_tech_list(Build_ship.device_list(clear_id).tech_ID).IsDoor = True AndAlso Build_ship.tile_map(x, y).device_tile.sprite > -1 Then
                            If Build_ship.tile_map(x, y).device_tile.rotate = rotate_enum.Zero OrElse Build_ship.tile_map(x, y).device_tile.rotate = rotate_enum.OneEighty Then
                                If y - 1 < 0 OrElse Not Build_ship.tile_map(x, y - 1).roomID = Build_ship.tile_map(x, y).roomID Then
                                    Build_ship.tile_map(x, y).sprite = room_sprite_enum.WallT
                                    Build_ship.tile_map(x, y).walkable = walkable_type_enum.Impassable
                                Else
                                    Build_ship.tile_map(x, y).sprite = room_sprite_enum.WallB
                                    Build_ship.tile_map(x, y).walkable = walkable_type_enum.Impassable
                                End If
                            Else
                                If x - 1 < 0 OrElse Not Build_ship.tile_map(x - 1, y).roomID = Build_ship.tile_map(x, y).roomID Then
                                    Build_ship.tile_map(x, y).sprite = room_sprite_enum.WallL
                                    Build_ship.tile_map(x, y).walkable = walkable_type_enum.Impassable
                                Else
                                    Build_ship.tile_map(x, y).sprite = room_sprite_enum.WallR
                                    Build_ship.tile_map(x, y).walkable = walkable_type_enum.Impassable
                                End If
                            End If
                        End If

                        Build_ship.tile_map(x, y).device_tile = Nothing
                        If Build_ship.tile_map(x, y).sprite = room_sprite_enum.Floor Then Build_ship.tile_map(x, y).walkable = walkable_type_enum.Walkable
                        If Build_ship.tile_map(x, y).type = tile_type_enum.empty Then Build_ship.tile_map(x, y) = New Ship_tile(0, tile_type_enum.empty, 0, 0, room_sprite_enum.empty, walkable_type_enum.Walkable)

                    End If
                End If
            Next
        Next

        'Remove access points
        Dim ap_to_remove As HashSet(Of PointI) = New HashSet(Of PointI)
        For Each room In Build_ship.room_list
            For Each ap In room.Value.access_point
                If ap.Value.Base_Device = clear_id Then ap_to_remove.Add(ap.Key)
            Next
            For Each ap In ap_to_remove
                room.Value.access_point.Remove(ap)
            Next
            ap_to_remove.Clear()
        Next

        For Each room In Build_ship.room_list
            If room.Value.device_list.Contains(clear_id) Then room.Value.device_list.Remove(clear_id)
        Next
        Build_ship.device_list.Remove(clear_id)


    End Sub


    Sub PostUI_Update()

        'Set Selection Colors
        If Buildable = True Then
            Selection_color = Color.FromArgb(150, 100, 255, 100)
        Else
            Selection_color = Color.FromArgb(150, 255, 100, 100)
        End If
        'End of Building room






        'Set device panel color
        If Selected_device_Panel > -1 Then
            menu_item(ship_build_menu_enum.device_panel).color = Room_menu_item(Selected_device_Panel).color
        End If

        'If Tech_list(Build_selection).Base_type = Tech_type_enum.Room Then Room_menu_item(Build_selection).color_add = New Button_color(0, 10, 10, 10)

        If Selected_device_Panel > -1 Then Room_menu_item(Selected_device_Panel).press()


        If Erasing = True Then menu_item(ship_build_menu_enum.remove).press()
        If info_bars = True Then menu_item(ship_build_menu_enum.info_bars).press()
        If tile_grid = True Then menu_item(ship_build_menu_enum.tile_grid).press()
        If split_room = True Then menu_item(ship_build_menu_enum.split_room).press()
        If Set_Center_point = True Then menu_item(ship_build_menu_enum.set_center_point).press()

        If Info_Select = Ship_Build_Info_enum.Info Then menu_item(ship_build_menu_enum.Info_Info).press()
        If Info_Select = Ship_Build_Info_enum.Devices Then menu_item(ship_build_menu_enum.Info_Devices).press()
        If Info_Select = Ship_Build_Info_enum.Rooms Then menu_item(ship_build_menu_enum.Info_Rooms).press()

        If pipeing = True AndAlso Pipeline_select > -1 Then Device_menu_item(Pipeline_select).press()


        If Build_selection > -1 AndAlso Tech_list(Build_selection).Base_type = Tech_type_enum.Device AndAlso Device_tech_list(Build_selection).Rotatable = True Then
            menu_item(ship_build_menu_enum.rotate_left).style = button_style.Both
            menu_item(ship_build_menu_enum.rotate_right).style = button_style.Both
            menu_item(ship_build_menu_enum.rotate_left).reset()
            menu_item(ship_build_menu_enum.rotate_right).reset()
        Else
            menu_item(ship_build_menu_enum.rotate_left).style = button_style.DisplayOnly
            menu_item(ship_build_menu_enum.rotate_left).Adj_color = Set_Brighness(Color.Red, 0.6)
            menu_item(ship_build_menu_enum.rotate_right).style = button_style.DisplayOnly
            menu_item(ship_build_menu_enum.rotate_right).Adj_color = Set_Brighness(Color.Red, 0.6)
        End If


        If Build_selection > -1 AndAlso Tech_list(Build_selection).Base_type = Tech_type_enum.Device AndAlso (Device_tech_list(Build_selection).Flipable = flip_enum.Flip_X OrElse Device_tech_list(Build_selection).Flipable = flip_enum.Both) Then
            menu_item(ship_build_menu_enum.flip_right).style = button_style.Both
            menu_item(ship_build_menu_enum.flip_right).reset()
        Else
            menu_item(ship_build_menu_enum.flip_right).style = button_style.DisplayOnly
            menu_item(ship_build_menu_enum.flip_right).Adj_color = Set_Brighness(Color.Red, 0.6)
        End If


        If Build_selection > -1 AndAlso Tech_list(Build_selection).Base_type = Tech_type_enum.Device AndAlso (Device_tech_list(Build_selection).Flipable = flip_enum.Flip_Y OrElse Device_tech_list(Build_selection).Flipable = flip_enum.Both) Then
            menu_item(ship_build_menu_enum.flip_up).style = button_style.Both
            menu_item(ship_build_menu_enum.flip_up).reset()
        Else
            menu_item(ship_build_menu_enum.flip_up).style = button_style.DisplayOnly
            menu_item(ship_build_menu_enum.flip_up).Adj_color = Set_Brighness(Color.Red, 0.6)
        End If


        menu_item(ship_build_menu_enum.mouse_position).text = selection.x.ToString + " / " + selection.y.ToString + " : " + Tile_selection.Width.ToString + " / " + Tile_selection.Height.ToString

        menu_item(ship_build_menu_enum.resouce_cost).text = CType(shipclass, ship_class_enum).ToString + " " + "Mass " + Build_ship.Mass.ToString + " Tons"

    End Sub


    Sub Set_Center()
        If Set_Center_point = True Then
            If selection.x >= 0 AndAlso selection.x <= shipsize.x AndAlso selection.y >= 0 AndAlso selection.y <= shipsize.y Then
                If left_click = True Then
                    Build_ship.center_point = selection
                    Set_Center_point = False
                    left_click = False
                End If
            End If
        End If

        Dim A, B, C, D As Double
        Dim Tiles As Integer
        Dim Left As Integer = Build_ship.shipsize.x
        Dim Right As Integer = 0
        Dim Top As Integer = Build_ship.shipsize.y
        Dim Bottom As Integer = 0
        Dim Ship_Center As PointI
        Dim Center As PointI
        'Find Center of Ship
        For x = 0 To Build_ship.shipsize.x
            For y = 0 To Build_ship.shipsize.y
                If Build_ship.tile_map(x, y).type < tile_type_enum.Device_Base Then
                    If x < Left Then Left = x
                    If x > Right Then Right = x
                    If y < Top Then Top = y
                    If y > Bottom Then Bottom = y
                End If
            Next
        Next
        Ship_Center.x = ((Right - Left) \ 2) + Left
        Ship_Center.y = ((Bottom - Top) \ 2) + Top
        'Adjust for tiles       
        For x = 0 To Build_ship.shipsize.x
            For y = 0 To Build_ship.shipsize.y
                If Build_ship.tile_map(x, y).type < tile_type_enum.Device_Base Then
                    Tiles += 1
                    If x <= Ship_Center.x AndAlso y <= Ship_Center.y Then A += Tech_list(Build_ship.room_list(Build_ship.tile_map(x, y).roomID).type).weight
                    If x >= Ship_Center.x AndAlso y <= Ship_Center.y Then B += Tech_list(Build_ship.room_list(Build_ship.tile_map(x, y).roomID).type).weight
                    If x <= Ship_Center.x AndAlso y >= Ship_Center.y Then C += Tech_list(Build_ship.room_list(Build_ship.tile_map(x, y).roomID).type).weight
                    If x >= Ship_Center.x AndAlso y >= Ship_Center.y Then D += Tech_list(Build_ship.room_list(Build_ship.tile_map(x, y).roomID).type).weight
                End If
            Next
        Next

        If B = 0 AndAlso D = 0 Then
            Center.x = Ship_Center.x
        Else
            Center.x = Ship_Center.x - (CInt(((A + C) / (B + D))) - 1)
        End If

        If A = 0 AndAlso B = 0 Then
            Center.y = Ship_Center.y
        Else
            Center.y = Ship_Center.y + (CInt(((C + D) / (A + B))) - 1)
        End If


        'Center.x = Ship_Center.x - ((A + C) - (B + D))
        'Center.y = Ship_Center.y - ((A + B) - (C + D))
        Build_ship.center_point = Center



    End Sub

    Sub Set_Mass()

        Dim Mass As Double
        For Each Tile In Build_ship.tile_map
            If Tile.type < tile_type_enum.Device_Base Then
                Mass += Tech_list(Build_ship.room_list(Tile.roomID).type).weight
            End If
        Next
        Build_ship.Mass = Math.Round(Mass, 2)
    End Sub

    Sub Build_Device_List()
        If Reset_device_list = True Then
            menu_item(ship_build_menu_enum.Pipeline_new).enabled = False
            Reset_device_list = False
            device_menu.x = screen_size.x - 200
            device_menu.y = 300
            Device_menu_item.Clear()
            For Each item In Player_Tech
                'Add devices
                If Tech_list(item).Base_type = Tech_type_enum.Device Then
                    If Tech_list(item).Device_room_type = CType(Selected_device_Panel, tech_list_enum) Then
                        Device_menu_item.Add(item, New Menu_button(button_texture_enum.ship_build__menu_button, New Rectangle(device_menu.x, device_menu.y, 32, 32), button_style.Both))
                        Device_menu_item(item).reset()
                        device_menu.x += 40
                        If device_menu.x >= screen_size.x Then device_menu.y += 40 : device_menu.x = screen_size.x - 200
                    End If
                End If
                'Add armors

                If Tech_list(item).Base_type = Tech_type_enum.Armor Then
                    If CType(Selected_device_Panel, tech_list_enum) = tech_list_enum.Armor Then
                        Device_menu_item.Add(item, New Menu_button(button_texture_enum.ship_build__menu_button, New Rectangle(device_menu.x, device_menu.y, 32, 32), button_style.Both))
                        Device_menu_item(item).reset()
                        device_menu.x += 40
                        If device_menu.x >= screen_size.x Then device_menu.y += 40 : device_menu.x = screen_size.x - 200
                    End If
                End If
            Next

            'redraw pipeline menu
            If CType(Selected_device_Panel, tech_list_enum) = tech_list_enum.Piping Then
                device_menu.x = screen_size.x - 176
                device_menu.y = 314
                menu_item(ship_build_menu_enum.Pipeline_new).enabled = True
                For Each pipe In Build_ship.pipeline_list
                    Device_menu_item.Add(pipe.Key, New Menu_button(button_texture_enum.ship_build__Pipeline_panel, New Rectangle(device_menu.x, device_menu.y, 128, 20), button_style.Both, pipe.Value.Color, pipe.Value.Name, Color.White, d3d_font_enum.SB_small))
                    Device_menu_item.Add(pipe.Key + 1000, New Menu_button(button_texture_enum.ship_build__Pipeline_delete, New Rectangle(device_menu.x + 130, device_menu.y, 20, 20), button_style.Both))
                    Device_menu_item(pipe.Key).reset()
                    Device_menu_item(pipe.Key + 1000).reset()
                    device_menu.y += 24
                Next

            End If
        End If
    End Sub


    Sub Check_For_connected_Rooms()

        'Check for connected rooms
        All_Connected = True
        Dim connected = False
        For Each room In Build_ship.room_list
            For x = 0 To shipsize.x
                For y = 0 To shipsize.y
                    If Build_ship.tile_map(x, y).roomID = room.Key Then
                        If x - 1 >= 0 AndAlso Not Build_ship.tile_map(x - 1, y).roomID = room.Key Then connected = True
                        If x + 1 <= shipsize.x AndAlso Not Build_ship.tile_map(x + 1, y).roomID = room.Key Then connected = True
                        If y - 1 >= 0 AndAlso Not Build_ship.tile_map(x, y - 1).roomID = room.Key Then connected = True
                        If y + 1 <= shipsize.y AndAlso Not Build_ship.tile_map(x, y + 1).roomID = room.Key Then connected = True
                    End If
                Next
            Next
            If connected = False AndAlso Build_ship.room_list.Count > 0 Then All_Connected = False
        Next




        Valid_rooms.Clear()

        Dim todo As Queue(Of PointI) = New Queue(Of PointI)
        Dim done As HashSet(Of PointI) = New HashSet(Of PointI)
        Dim tile_count As Integer
        For Each room In Build_ship.room_list
            If room.Key >= 2 Then
                todo.Clear()
                done.Clear()
                tile_count = 0

                For x = 0 To Build_ship.shipsize.x
                    For y = 0 To Build_ship.shipsize.y
                        If Build_ship.tile_map(x, y).roomID >= 2 AndAlso Build_ship.tile_map(x, y).roomID = room.Key Then
                            If todo.Count = 0 Then todo.Enqueue(New PointI(x, y))
                            tile_count += 1
                        End If
                    Next
                Next

                Do Until todo.Count = 0
                    Dim tile As PointI = todo.Dequeue
                    Dim pos As PointI

                    For pos.x = tile.x - 1 To tile.x + 1
                        For pos.y = tile.y - 1 To tile.y + 1
                            If Not pos = tile Then
                                If pos.x >= 0 AndAlso pos.x <= Build_ship.shipsize.x AndAlso pos.y >= 0 AndAlso pos.y <= Build_ship.shipsize.y Then
                                    If Build_ship.tile_map(pos.x, pos.y).roomID = room.Key AndAlso Not done.Contains(pos) AndAlso Not todo.Contains(pos) Then todo.Enqueue(pos)
                                End If
                            End If
                        Next
                    Next
                    done.Add(tile)
                Loop

                If done.Count = tile_count Then Valid_rooms.Add(room.Key)
            End If
        Next

    End Sub


    Sub Check_For_connected_Pipelines()
        Valid_pipelines.Clear()

        Dim todo As Queue(Of PointI) = New Queue(Of PointI)
        Dim done As HashSet(Of PointI) = New HashSet(Of PointI)

        For Each pipeline In Build_ship.pipeline_list
            If pipeline.Value.Tiles.Count > 0 Then
                todo.Clear()
                done.Clear()
                todo.Enqueue(pipeline.Value.Tiles.Keys(0))

                Do Until todo.Count = 0
                    Dim tile As PointI = todo.Dequeue
                    Dim pos As PointI
                    pos = New PointI(tile.x - 1, tile.y)
                    If pos.x >= 0 AndAlso pipeline.Value.Tiles.ContainsKey(pos) AndAlso Not done.Contains(pos) AndAlso Not todo.Contains(pos) Then todo.Enqueue(pos)
                    pos = New PointI(tile.x + 1, tile.y)
                    If pos.x <= Build_ship.shipsize.x AndAlso pipeline.Value.Tiles.ContainsKey(pos) AndAlso Not done.Contains(pos) AndAlso Not todo.Contains(pos) Then todo.Enqueue(pos)
                    pos = New PointI(tile.x, tile.y - 1)
                    If pos.y >= 0 AndAlso pipeline.Value.Tiles.ContainsKey(pos) AndAlso Not done.Contains(pos) AndAlso Not todo.Contains(pos) Then todo.Enqueue(pos)
                    pos = New PointI(tile.x, tile.y + 1)
                    If pos.y <= Build_ship.shipsize.y AndAlso pipeline.Value.Tiles.ContainsKey(pos) AndAlso Not done.Contains(pos) AndAlso Not todo.Contains(pos) Then todo.Enqueue(pos)
                    done.Add(tile)
                Loop

                If done.Count = pipeline.Value.Tiles.Count Then Valid_pipelines.Add(pipeline.Key)
            End If
        Next

    End Sub




    'Main Sub
    Sub new_ship_build_menu()
        shipclass = New_ship_options_menu()


        'Build tools creation / Tech tree        
        'Player_Tech.Add(tech_list_enum.Armor)
        Player_Tech.Add(tech_list_enum.All_rooms)
        Player_Tech.Add(tech_list_enum.Airlock)
        Player_Tech.Add(tech_list_enum.Bridge)
        Player_Tech.Add(tech_list_enum.Corridor)
        Player_Tech.Add(tech_list_enum.Engineering)
        Player_Tech.Add(tech_list_enum.Crew_Quarters)

        Player_Tech.Add(tech_list_enum.Combustion_Engine_MK1)
        Player_Tech.Add(tech_list_enum.Energy_Generator_MK1)
        Player_Tech.Add(tech_list_enum.Projectile_MK1)
        Player_Tech.Add(tech_list_enum.Door_MK1)
        Player_Tech.Add(tech_list_enum.Door_MK2)
        Player_Tech.Add(tech_list_enum.Airlock_MK1)
        Player_Tech.Add(tech_list_enum.Thruster_MK1)

        Player_Tech.Add(tech_list_enum.Bridge_Control_Panel)
        Player_Tech.Add(tech_list_enum.Computer_MK1)

        Player_Tech.Add(tech_list_enum.ArmorLV1)
        Player_Tech.Add(tech_list_enum.ArmorLV2)

        Player_Tech.Add(tech_list_enum.Pipe_Data_100)
        Player_Tech.Add(tech_list_enum.Pipe_Energy_100)

        Player_Tech.Add(tech_list_enum.Landing_Bay_Small)


        Create_build_ship()

        ReDim selection_tile_map(shipsize.x + 2, shipsize.y + 2)
        ReDim selection_tile_map2(shipsize.x + 2, shipsize.y + 2)

        Dim time_current As Long        

        'UI and flow control
        Add_menu_items()
        Add_room_menu_items()
        all_menu_items_list.Add(Tech_type_enum.Menu_item, menu_item)
        all_menu_items_list.Add(Tech_type_enum.Room, Room_menu_item)
        all_menu_items_list.Add(Tech_type_enum.Device, Device_menu_item)

        set_minimap_Scale()

        clear_tile_map(Build_ship.shipsize, selection_tile_map)
        clear_tile_map(Build_ship.shipsize, selection_tile_map2)

        'Set view to center
        view_location_sb.x = Convert.ToInt32((shipsize.x + 1) * 32 / 2 - screen_size.x / sb_zoom / 2)
        view_location_sb.y = Convert.ToInt32((shipsize.y + 1) * 32 / 2 - screen_size.y / sb_zoom / 2)
        Build_ship.center_point.x = shipsize.x \ 2
        Build_ship.center_point.y = shipsize.y \ 2


        'Logic_Time = Current_Time()
        'render_time = Current_Time()

        Do
            If Logic_Time >= 1 Then

                Logic_Time -= 1


                MainForm.getUI(pressedkeys, mouse_info) 'Get Mouse/Keys
                atsize = Convert.ToInt32(32 * sb_zoom)

                'Check Button Selection
                For Each button In all_menu_items_list
                    Button_Selection(button.Value, mouse_info)
                Next



                If Not selection.Equals(old_selection) Then clear_tiles = True
                If clear_tiles = True Then clear_tile_map(Build_ship.shipsize, selection_tile_map) : clear_tiles = False : Redraw_Minimap = True : Set_Center() : Set_Mass()

                Handle_UI()

                If Erasing = False Then
                    Buildable = True
                    Box_selection()
                    Split_room_sub()
                    Pipeing_Sub()
                    Erase_Pipeing()
                    Device_build()
                    Room_Remove()
                    Room_Building()
                    Armor_Building()

                Else
                    If pipeing = True Then
                        Erase_Pipeing()
                    Else
                        Erasing_Sub()
                    End If

                End If


                If Validate_rooms = True Then
                    Check_For_connected_Rooms()
                    Set_Center()
                    Validate_rooms = False
                End If

                If Validate_pipelines = True Then
                    Check_For_connected_Pipelines()
                    Validate_pipelines = False
                End If

                Build_Device_List()
                PostUI_Update()
            End If



            QueryPerformanceCounter(time_current)

            If time_current + refresh_rate >= render_end + render_duration Then
                Logic_Time += Logic_Ratio
                'Render
                render_ship_build()
                'all_menu_items_list, Build_ship, Tile_selection, Build_selection, selection_tile_map, Selection_color, selection, Room_selection, Room_selection_color, Erasing, Device_rotation, info_bars, tile_grid, Device_Flip, Pipeline_select, pipeing, Build_ship.center_point, Redraw_Minimap, Pipeline_dialog, MiniMap_Scale
                Redraw_Minimap = False
                QueryPerformanceCounter(render_end)
                render_duration = render_end - time_current
            End If





            Application.DoEvents()

            If pressedkeys.Contains(Keys.F10) Then pressedkeys.Remove(Keys.F10) : lock_screen()
        Loop While pressedkeys.Contains(Keys.Escape) = False AndAlso Exit_Ship_build = False
        Clean_Up()
        End
    End Sub


    Function set_room_walls(ByVal shipsize As PointI, ByVal selection_tile_map(,) As Byte) As Boolean
        Dim buildable As Boolean = True
        For x = 0 To shipsize.x
            For y = 0 To shipsize.y
                If selection_tile_map(x + 1, y + 1) < 254 Then
                    Dim pos As PointI
                    Dim empty As Integer = 0
                    Dim walls As Integer = 0
                    pos.x = x + 1
                    pos.y = y + 1
                    Dim L, R, T, B As Integer
                    L = selection_tile_map(pos.x - 1, pos.y)
                    T = selection_tile_map(pos.x, pos.y - 1)
                    R = selection_tile_map(pos.x + 1, pos.y)
                    B = selection_tile_map(pos.x, pos.y + 1)
                    If L = 254 Then L = 255
                    If R = 254 Then R = 255
                    If T = 254 Then T = 255
                    If B = 254 Then B = 255

                    If L = room_sprite_enum.empty Then empty = empty + 1
                    If T = room_sprite_enum.empty Then empty = empty + 1
                    If R = room_sprite_enum.empty Then empty = empty + 1
                    If B = room_sprite_enum.empty Then empty = empty + 1
                    If empty = 2 Then 'corner
                        If T = room_sprite_enum.empty AndAlso L = room_sprite_enum.empty Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.CornerTL
                        If T = room_sprite_enum.empty AndAlso R = room_sprite_enum.empty Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.CornerTR
                        If B = room_sprite_enum.empty AndAlso L = room_sprite_enum.empty Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.CornerBL
                        If B = room_sprite_enum.empty AndAlso R = room_sprite_enum.empty Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.CornerBR
                    End If
                    If empty = 1 Then 'wall
                        If T = room_sprite_enum.empty Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.WallT
                        If B = room_sprite_enum.empty Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.WallB
                        If L = room_sprite_enum.empty Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.WallL
                        If R = room_sprite_enum.empty Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.WallR
                    End If
                End If
            Next
        Next
        'Sets angles
        For x = 0 To shipsize.x
            For y = 0 To shipsize.y
                If selection_tile_map(x + 1, y + 1) < 254 Then
                    Dim pos As PointI
                    Dim empty As Integer = 0
                    Dim walls As Integer = 0
                    pos.x = x + 1
                    pos.y = y + 1
                    Dim L, R, T, B As Integer
                    L = selection_tile_map(pos.x - 1, pos.y)
                    T = selection_tile_map(pos.x, pos.y - 1)
                    R = selection_tile_map(pos.x + 1, pos.y)
                    B = selection_tile_map(pos.x, pos.y + 1)

                    If L = 254 Then L = 255
                    If R = 254 Then R = 255
                    If T = 254 Then T = 255
                    If B = 254 Then B = 255

                    If L = room_sprite_enum.empty Then empty = empty + 1
                    If T = room_sprite_enum.empty Then empty = empty + 1
                    If R = room_sprite_enum.empty Then empty = empty + 1
                    If B = room_sprite_enum.empty Then empty = empty + 1
                    If Not L = room_sprite_enum.Floor Then walls = walls + 1
                    If Not T = room_sprite_enum.Floor Then walls = walls + 1
                    If Not R = room_sprite_enum.Floor Then walls = walls + 1
                    If Not B = room_sprite_enum.Floor Then walls = walls + 1
                    If empty = 0 AndAlso walls = 2 Then 'angle
                        If T = room_sprite_enum.WallL OrElse T = room_sprite_enum.CornerTL Then If L = room_sprite_enum.WallT OrElse L = room_sprite_enum.CornerTL Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.AngleBR
                        If T = room_sprite_enum.WallR OrElse T = room_sprite_enum.CornerTR Then If R = room_sprite_enum.WallT OrElse R = room_sprite_enum.CornerTR Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.AngleBL
                        If B = room_sprite_enum.WallL OrElse B = room_sprite_enum.CornerBL Then If L = room_sprite_enum.WallB OrElse L = room_sprite_enum.CornerBL Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.AngleTR
                        If B = room_sprite_enum.WallR OrElse B = room_sprite_enum.CornerBR Then If R = room_sprite_enum.WallB OrElse R = room_sprite_enum.CornerBR Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.AngleTL
                    End If
                End If
            Next
        Next

        'check for unbuildable room
        Dim has_floor As Boolean = False
        For x = 0 To shipsize.x
            For y = 0 To shipsize.y
                If selection_tile_map(x + 1, y + 1) = room_sprite_enum.Floor Then
                    Dim pos As PointI
                    Dim empty As Integer = 0
                    Dim walls As Integer = 0
                    Dim angles As Integer = 0
                    pos.x = x + 1
                    pos.y = y + 1
                    Dim L, R, T, B, TL, TR, BL, BR As Integer
                    L = selection_tile_map(pos.x - 1, pos.y) : T = selection_tile_map(pos.x, pos.y - 1) : R = selection_tile_map(pos.x + 1, pos.y) : B = selection_tile_map(pos.x, pos.y + 1)
                    TL = selection_tile_map(pos.x - 1, pos.y - 1) : TR = selection_tile_map(pos.x + 1, pos.y - 1) : BL = selection_tile_map(pos.x - 1, pos.y + 1) : BR = selection_tile_map(pos.x + 1, pos.y + 1)
                    If L = room_sprite_enum.empty Then empty = empty + 1
                    If T = room_sprite_enum.empty Then empty = empty + 1
                    If R = room_sprite_enum.empty Then empty = empty + 1
                    If B = room_sprite_enum.empty Then empty = empty + 1
                    If Not L = room_sprite_enum.Floor Then walls = walls + 1
                    If Not T = room_sprite_enum.Floor Then walls = walls + 1
                    If Not R = room_sprite_enum.Floor Then walls = walls + 1
                    If Not B = room_sprite_enum.Floor Then walls = walls + 1

                    If L = room_sprite_enum.AngleBL Or L = room_sprite_enum.AngleBR Or L = room_sprite_enum.AngleTL Or L = room_sprite_enum.AngleTR Then walls -= 1 : angles += 1 : L = room_sprite_enum.AngleBL
                    If T = room_sprite_enum.AngleBL Or T = room_sprite_enum.AngleBR Or T = room_sprite_enum.AngleTL Or T = room_sprite_enum.AngleTR Then walls -= 1 : angles += 1 : T = room_sprite_enum.AngleBL
                    If R = room_sprite_enum.AngleBL Or R = room_sprite_enum.AngleBR Or R = room_sprite_enum.AngleTL Or R = room_sprite_enum.AngleTR Then walls -= 1 : angles += 1 : R = room_sprite_enum.AngleBL
                    If B = room_sprite_enum.AngleBL Or B = room_sprite_enum.AngleBR Or B = room_sprite_enum.AngleTL Or B = room_sprite_enum.AngleTR Then walls -= 1 : angles += 1 : B = room_sprite_enum.AngleBL

                    If T = room_sprite_enum.AngleBL AndAlso L = room_sprite_enum.AngleBL AndAlso TL = room_sprite_enum.Floor Then buildable = False
                    If T = room_sprite_enum.AngleBL AndAlso R = room_sprite_enum.AngleBL AndAlso TR = room_sprite_enum.Floor Then buildable = False

                    If B = room_sprite_enum.AngleBL AndAlso L = room_sprite_enum.AngleBL AndAlso BL = room_sprite_enum.Floor Then buildable = False
                    If B = room_sprite_enum.AngleBL AndAlso R = room_sprite_enum.AngleBL AndAlso BR = room_sprite_enum.Floor Then buildable = False

                    If T = room_sprite_enum.WallL OrElse T = room_sprite_enum.WallR OrElse T = room_sprite_enum.CornerTL OrElse T = room_sprite_enum.CornerTR _
                    OrElse B = room_sprite_enum.WallL OrElse B = room_sprite_enum.WallR OrElse B = room_sprite_enum.CornerBL OrElse B = room_sprite_enum.CornerBR Then
                        If L = room_sprite_enum.WallT OrElse L = room_sprite_enum.WallB OrElse L = room_sprite_enum.CornerTL OrElse L = room_sprite_enum.CornerBL _
                        OrElse R = room_sprite_enum.WallT OrElse R = room_sprite_enum.WallB OrElse R = room_sprite_enum.CornerTR OrElse R = room_sprite_enum.CornerBR Then buildable = False
                    End If


                    If empty > 0 Then buildable = False



                    'If walls = 2 AndAlso angles = 1 Then Buildable = False
                    has_floor = True
                End If
            Next
        Next
        If has_floor = False Then buildable = False

        Return buildable
    End Function


    Sub set_armor_sprites(ByVal shipsize As PointI, ByVal selection_tile_map(,) As Byte)
        Dim L, R, T, B As Boolean
        Dim pos As PointI
        For x = 0 To shipsize.x
            For y = 0 To shipsize.y

                If selection_tile_map(x + 1, y + 1) < 254 Then
                    Dim empty As Integer = 0
                    Dim walls As Integer = 0
                    pos.x = x + 1
                    pos.y = y + 1

                    L = False
                    R = False
                    T = False
                    B = False
                    'true is occupied
                    If x - 1 >= 0 AndAlso Build_ship.tile_map(x - 1, y).roomID > 0 Then L = True
                    If x + 1 <= shipsize.x AndAlso Build_ship.tile_map(x + 1, y).roomID > 0 Then R = True
                    If y - 1 >= 0 AndAlso Build_ship.tile_map(x, y - 1).roomID > 0 Then T = True
                    If y + 1 <= shipsize.y AndAlso Build_ship.tile_map(x, y + 1).roomID > 0 Then B = True

                    If selection_tile_map(pos.x - 1, pos.y) < 250 Then L = True
                    If selection_tile_map(pos.x + 1, pos.y) < 250 Then R = True
                    If selection_tile_map(pos.x, pos.y - 1) < 250 Then T = True
                    If selection_tile_map(pos.x, pos.y + 1) < 250 Then B = True

                    If L = False Then empty += 1
                    If T = False Then empty += 1
                    If R = False Then empty += 1
                    If B = False Then empty += 1
                    If empty = 2 Then 'corner
                        If T = False AndAlso L = False Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.CornerTL
                        If T = False AndAlso R = False Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.CornerTR
                        If B = False AndAlso L = False Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.CornerBL
                        If B = False AndAlso R = False Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.CornerBR
                    End If
                    If empty = 1 Then 'wall
                        If T = False Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.WallT
                        If B = False Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.WallB
                        If L = False Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.WallL
                        If R = False Then selection_tile_map(pos.x, pos.y) = room_sprite_enum.WallR
                    End If
                End If
            Next
        Next

    End Sub


    Sub clear_tile_map(ByVal shipsize As PointI, ByVal tile_map(,) As Byte)

        For x = 0 To shipsize.x + 2
            For y = 0 To shipsize.y + 2
                tile_map(x, y) = room_sprite_enum.empty
            Next
        Next

        'Dim new_map(shipsize.x, shipsize.y) As Byte
        'Erase tile_map
        'tile_map = new_map
    End Sub


    Function New_ship_options_menu() As ship_class_enum

        Dim Descriptions(8) As String
        Descriptions(0) = "The corvette class is a highly maneuverable all purpose warship. Most corvettes are used as mid range support vessels. The highly adaptable nature of the class allows the corvette to be used for a wide range of missions from escort to cargo transport. Due to it’s relatively small size it is normally lightly armored. The corvette is considered a more basic vessel, although if outfitted with advanced technology the corvette could take on any mission."
        Descriptions(1) = "Because of it’s small size the fighter is a very versatile class. The fighter is commonly launched from a larger vessel which makes it particularly useful for jobs as a short range strike craft, scout ship, short range transport or for any other scenario where a small quick ship is key to success."
        Descriptions(2) = "The Other class is a highly maneuverable all purpose warship. Most corvettes are used as mid range support vessels. The highly adaptable nature of the class allows the corvette to be used for a wide range of missions from escort to cargo transport. Due to it’s relatively small size it is normally lightly armored. The corvette is considered a more basic vessel, although if outfitted with advanced technology the corvette could take on any mission."
        Descriptions(3) = "The corvette class is a highly maneuverable all purpose warship. Most corvettes are used as mid range support vessels. The highly adaptable nature of the class allows the corvette to be used for a wide range of missions from escort to cargo transport. Due to it’s relatively small size it is normally lightly armored. The corvette is considered a more basic vessel, although if outfitted with advanced technology the corvette could take on any mission."
        Descriptions(4) = "The corvette class is a highly maneuverable all purpose warship. Most corvettes are used as mid range support vessels. The highly adaptable nature of the class allows the corvette to be used for a wide range of missions from escort to cargo transport. Due to it’s relatively small size it is normally lightly armored. The corvette is considered a more basic vessel, although if outfitted with advanced technology the corvette could take on any mission."
        Descriptions(5) = "The corvette class is a highly maneuverable all purpose warship. Most corvettes are used as mid range support vessels. The highly adaptable nature of the class allows the corvette to be used for a wide range of missions from escort to cargo transport. Due to it’s relatively small size it is normally lightly armored. The corvette is considered a more basic vessel, although if outfitted with advanced technology the corvette could take on any mission."
        Descriptions(6) = "The corvette class is a highly maneuverable all purpose warship. Most corvettes are used as mid range support vessels. The highly adaptable nature of the class allows the corvette to be used for a wide range of missions from escort to cargo transport. Due to it’s relatively small size it is normally lightly armored. The corvette is considered a more basic vessel, although if outfitted with advanced technology the corvette could take on any mission."
        Descriptions(7) = "The corvette class is a highly maneuverable all purpose warship. Most corvettes are used as mid range support vessels. The highly adaptable nature of the class allows the corvette to be used for a wide range of missions from escort to cargo transport. Due to it’s relatively small size it is normally lightly armored. The corvette is considered a more basic vessel, although if outfitted with advanced technology the corvette could take on any mission."
        Descriptions(8) = "The corvette class is a highly maneuverable all purpose warship. Most corvettes are used as mid range support vessels. The highly adaptable nature of the class allows the corvette to be used for a wide range of missions from escort to cargo transport. Due to it’s relatively small size it is normally lightly armored. The corvette is considered a more basic vessel, although if outfitted with advanced technology the corvette could take on any mission."
        Dim pressedkeys As New HashSet(Of Keys)
        Dim mouse_info As New MainForm.mouse_info_type
        MainForm.getUI(pressedkeys, mouse_info)

        Dim time_start, time_current, cps, rate As Long
        QueryPerformanceFrequency(cps)
        rate = Convert.ToInt64(cps / 60)

        Dim type As ship_class_enum

        Dim menu_item(10) As Menu_button
        'menu_item(0) = New Menu_button(button_texture_enum.ship_build__options_button, New Rectangle(Convert.ToInt32(screen_size.x / 2 - 375), Convert.ToInt32(screen_size.y / 2 - 200), 250, 40), button_style.Both, Color.White, "Carbon Fiber", Color.White, d3d_font_enum.Big_Button)
        'menu_item(1) = New Menu_button(button_texture_enum.ship_build__options_button, New Rectangle(Convert.ToInt32(screen_size.x / 2 - 125), Convert.ToInt32(screen_size.y / 2 - 200), 250, 40), button_style.Both, Color.White, "Biological", Color.White, d3d_font_enum.Big_Button)
        'menu_item(2) = New Menu_button(button_texture_enum.ship_build__options_button, New Rectangle(Convert.ToInt32(screen_size.x / 2 + 125), Convert.ToInt32(screen_size.y / 2 - 200), 250, 40), button_style.Both, Color.White, "Energy", Color.White, d3d_font_enum.Big_Button)
        Dim pos As New PointI(CInt(screen_size.x / 2 - 280), CInt(screen_size.y / 2 - 280))

        menu_item(0) = New Menu_button(button_texture_enum.ship_build__options_button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Fighter Wide", Color.White, d3d_font_enum.Big_Button) : pos.y += 60
        menu_item(1) = New Menu_button(button_texture_enum.ship_build__options_button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Fighter Tall", Color.White, d3d_font_enum.Big_Button) : pos.y += 60
        menu_item(2) = New Menu_button(button_texture_enum.ship_build__options_button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Corvette", Color.White, d3d_font_enum.Big_Button) : pos.y += 60
        menu_item(3) = New Menu_button(button_texture_enum.ship_build__options_button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Frigate", Color.White, d3d_font_enum.Big_Button) : pos.y += 60
        menu_item(4) = New Menu_button(button_texture_enum.ship_build__options_button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Destroyer", Color.White, d3d_font_enum.Big_Button) : pos.y += 60

        menu_item(5) = New Menu_button(button_texture_enum.ship_build__options_button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Cruiser", Color.White, d3d_font_enum.Big_Button) : pos.y += 60
        menu_item(6) = New Menu_button(button_texture_enum.ship_build__options_button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Battle Cruiser", Color.White, d3d_font_enum.Big_Button) : pos.y += 60
        menu_item(7) = New Menu_button(button_texture_enum.ship_build__options_button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Battleship", Color.White, d3d_font_enum.Big_Button) : pos.y += 60
        menu_item(8) = New Menu_button(button_texture_enum.ship_build__options_button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Carrier", Color.White, d3d_font_enum.Big_Button) : pos.y += 60

        menu_item(9) = New Menu_button(button_texture_enum.ship_build__options_button, New Rectangle(pos.x, pos.y, 250, 40), button_style.Both, Color.White, "Ok", Color.White, d3d_font_enum.Big_Button)

        menu_item(10) = New Menu_button(button_texture_enum.ship_build__description_panel, New Rectangle(Convert.ToInt32(screen_size.x / 2 + 20), Convert.ToInt32(screen_size.y / 2 - 260), 300, 500), button_style.DisplayOnly, Color.White, Descriptions(0), Color.White, d3d_font_enum.SB_small, True, DrawTextFormat.Center + DrawTextFormat.WordBreak)
        Dim left_down_point As PointI = Nothing
        Dim left_release_point As PointI = Nothing
        Dim choice As Integer = 0
        Dim shiptype As Integer = -1
        Dim shipclass As Integer = -1


        Do
            QueryPerformanceCounter(time_start)

            For Each button In menu_item
                If button.bounds.Contains(mouse_info.position.x, mouse_info.position.y) Then
                    button.reset()
                    If mouse_info.left_down Then
                        If button.bounds.Contains(mouse_info.left_down_point.x, mouse_info.left_down_point.y) Then
                            ' button down on the original button
                            button.click()
                        Else
                            ' button down but not on the original button
                            button.Adj_color = Color.Black
                        End If
                    Else
                        ' mouse over
                        button.hilight()
                    End If
                Else
                    ' normal; no scrollover
                    button.reset()
                End If
            Next

            ' detect a click
            If mouse_info.get_left_click(left_down_point, left_release_point) Then
                For i = 0 To menu_item.Length - 1
                    If menu_item(i).bounds.Contains(left_down_point.x, left_down_point.y) And menu_item(i).bounds.Contains(left_release_point.x, left_release_point.y) Then
                        choice = i
                        Exit For
                    End If
                Next
            End If


            Select Case choice
                Case 0
                    type = ship_class_enum.fighterW
                Case 1
                    type = ship_class_enum.fighterT
                Case 2
                    type = ship_class_enum.corvette
                Case 3
                    type = ship_class_enum.frigate
                Case 4
                    type = ship_class_enum.destroyer
                Case 5
                    type = ship_class_enum.cruiser
                Case 6
                    type = ship_class_enum.battle_cruiser
                Case 7
                    type = ship_class_enum.battle_ship
                Case 8
                    type = ship_class_enum.carrier
                Case 9
                    If type > -1 Then Exit Do
            End Select
            If choice >= 0 AndAlso choice <= 7 Then shipclass = choice : menu_item(10).text = Descriptions(choice)
            If shipclass >= 0 Then menu_item(shipclass).press()

            render_new_ship_options(menu_item)
            Application.DoEvents()

            If pressedkeys.Contains(Keys.F10) Then pressedkeys.Remove(Keys.F10) : lock_screen()

            Do
                QueryPerformanceCounter(time_current)
            Loop Until (time_current - time_start) >= rate

        Loop While Not pressedkeys.Contains(Keys.Escape)
        If pressedkeys.Contains(Keys.Escape) Then
            Clean_Up()
            End
        End If
        Return type
    End Function

    Sub set_pipe_sprites(ByVal pipeline_list As Dictionary(Of Integer, ship_pipeline_type))
        For Each pipeline In pipeline_list
            Dim L, R, T, B As Boolean
            Dim tile As Pipeline_sprite_enum
            Dim temp_list As New Dictionary(Of PointI, Pipeline_sprite_enum)()
            For Each pipe In pipeline.Value.Tiles
                tile = Pipeline_sprite_enum.cross
                L = False : R = False : T = False : B = False
                If pipeline.Value.Tiles.ContainsKey(New PointI(pipe.Key.x - 1, pipe.Key.y)) Then L = True
                If pipeline.Value.Tiles.ContainsKey(New PointI(pipe.Key.x + 1, pipe.Key.y)) Then R = True
                If pipeline.Value.Tiles.ContainsKey(New PointI(pipe.Key.x, pipe.Key.y - 1)) Then T = True
                If pipeline.Value.Tiles.ContainsKey(New PointI(pipe.Key.x, pipe.Key.y + 1)) Then B = True

                If T = True AndAlso B = False AndAlso L = False AndAlso R = False Then tile = Pipeline_sprite_enum.EndB
                If T = False AndAlso B = True AndAlso L = False AndAlso R = False Then tile = Pipeline_sprite_enum.EndT
                If T = False AndAlso B = False AndAlso L = True AndAlso R = False Then tile = Pipeline_sprite_enum.EndR
                If T = False AndAlso B = False AndAlso L = False AndAlso R = True Then tile = Pipeline_sprite_enum.EndL

                If T = True AndAlso B = True Then
                    If L = True AndAlso R = False Then tile = Pipeline_sprite_enum.SplitR
                    If R = True AndAlso L = False Then tile = Pipeline_sprite_enum.SplitL
                    If L = True AndAlso R = True Then tile = Pipeline_sprite_enum.cross
                    If L = False AndAlso R = False Then tile = Pipeline_sprite_enum.vertical
                End If
                If L = True AndAlso R = True Then
                    If T = True AndAlso B = False Then tile = Pipeline_sprite_enum.SplitB
                    If B = True AndAlso T = False Then tile = Pipeline_sprite_enum.SplitT
                    If T = True AndAlso B = True Then tile = Pipeline_sprite_enum.cross
                    If T = False AndAlso B = False Then tile = Pipeline_sprite_enum.horizontal
                End If
                If T = False AndAlso B = True AndAlso L = False AndAlso R = True Then tile = Pipeline_sprite_enum.TL
                If T = False AndAlso B = True AndAlso L = True AndAlso R = False Then tile = Pipeline_sprite_enum.TR
                If T = True AndAlso B = False AndAlso L = False AndAlso R = True Then tile = Pipeline_sprite_enum.BL
                If T = True AndAlso B = False AndAlso L = True AndAlso R = False Then tile = Pipeline_sprite_enum.BR

                temp_list.Add(pipe.Key, tile)
            Next
            pipeline.Value.Tiles.Clear()
            For Each item In temp_list
                pipeline.Value.Tiles.Add(item.Key, item.Value)
            Next
        Next
    End Sub



    Sub render_ship_build()
        'ByRef all_menu_item_list As Dictionary(Of Tech_type_enum, Dictionary(Of Integer, Menu_button)), ByRef ship As Ship, ByVal Tile_selection As Rectangle, ByVal Build_selection As tech_list_enum, ByVal selection_tile_map(,) As Byte, ByVal Selection_color As Color, ByVal selection As PointI, ByVal Room_selection As Integer, ByVal Room_selection_color As Color, ByVal erasing As Boolean, ByVal device_rotation As rotate_enum, ByVal info_bars As Boolean, ByVal tile_grid As Boolean, ByVal device_flip As flip_enum, ByVal Pipeline_select As Integer, ByVal pipeing As Boolean, ByVal center_point As PointI, ByRef Redraw_Minimap As Boolean, ByVal Pipeline_dialog As Boolean, ByVal MiniMap_Scale As Integer
        Dim ship_size = Build_ship.GetShipSize
        Dim TileMap(ship_size.x, ship_size.y) As Ship_tile
        Dim pos As PointD
        Dim atsize As Integer = Convert.ToInt32(32 * sb_zoom)

        TileMap = Build_ship.tile_map

        d3d_device.BeginScene()
        d3d_device.Clear(ClearFlags.Target, Color.FromArgb(255, 100, 100, 200), 0, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)


        'd3d_device.TextureState(0).ColorOperation = TextureOperation.Modulate
        'd3d_device.TextureState(0).ColorArgument1 = TextureArgument.TextureColor
        'd3d_device.TextureState(0).ColorArgument2 = TextureArgument.Diffuse


        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)


        Dim scale As Decimal = Convert.ToDecimal(sb_zoom)
        d3d_sprite.Transform = Matrix.Identity * Matrix.Scaling(scale, scale, 0)


        For x = (view_location_sb.x * Convert.ToInt32(sb_zoom) \ atsize) - 1 To (view_location_sb.x * Convert.ToInt32(sb_zoom) \ atsize) + (screen_size.x \ atsize) + 1
            For y = (view_location_sb.y * Convert.ToInt32(sb_zoom) \ atsize) - 1 To (view_location_sb.y * Convert.ToInt32(sb_zoom) \ atsize) + (screen_size.y \ atsize) + 1
                pos.x = ((x * 32) - view_location_sb.x)
                pos.y = ((y * 32) - view_location_sb.y)
                If x >= 0 AndAlso x <= ship_size.x AndAlso y >= 0 AndAlso y <= ship_size.y Then

                    Select Case TileMap(x, y).type

                        'If regular tile
                        Case Is < tile_type_enum.Device_Base
                            If Tech_list(Build_selection).Base_type = Tech_type_enum.Device AndAlso Tech_list(Build_selection).Device_room_type = Build_ship.room_list(TileMap(x, y).roomID).type Then
                                'draw on floor for device build in correct room
                                Draw_Ship_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, Color.White)
                                If TileMap(x, y).device_tile IsNot Nothing AndAlso Not TileMap(x, y).device_tile.type = device_tile_type_enum.Empty Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, TileMap(x, y).device_tile.spriteAni, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                            Else
                                'draw normal / Draw piped
                                If Valid_rooms.Contains(TileMap(x, y).roomID) OrElse TileMap(x, y).roomID = 1 Then
                                    Draw_Ship_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, Color.White)
                                Else
                                    Draw_Ship_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, Color.FromArgb(255, 255, 120, 120))
                                End If

                                If TileMap(x, y).device_tile IsNot Nothing AndAlso Not TileMap(x, y).device_tile.type = device_tile_type_enum.Empty Then

                                    'Draw piped devices
                                    If pipeing = True AndAlso Pipeline_select > -1 Then
                                        If Build_ship.device_list(TileMap(x, y).device_tile.device_ID).Connected_To_Pipeline(Pipeline_select) Then
                                            'Draw connected
                                            'Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, TileMap(x, y).device_tile.spriteAni, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.FromArgb(255, 120, 120, 255))

                                            Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, TileMap(x, y).device_tile.spriteAni, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                                            Draw_Ship_Tile(tile_type_enum.Hull_1, 4, pos, Color.FromArgb(100, 0, 0, 255))

                                            'ElseIf Tech_list(Build_ship.device_list(TileMap(x, y).device_tile.device_ID).tech_ID).device_data.pipeline.ContainsKey(Build_ship.pipeline_list(Pipeline_select).Type) Then

                                        ElseIf Build_ship.device_list(TileMap(x, y).device_tile.device_ID).Contains_Open_Pipeline(Build_ship.pipeline_list(Pipeline_select).Type) Then
                                            'Draw avilable
                                            Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, TileMap(x, y).device_tile.spriteAni, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.FromArgb(255, 120, 255, 120))
                                            Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, TileMap(x, y).device_tile.spriteAni, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                                            Draw_Ship_Tile(tile_type_enum.Hull_1, 4, pos, Color.FromArgb(100, 0, 255, 0))
                                        Else
                                            'draw normal
                                            Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, TileMap(x, y).device_tile.spriteAni, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                                        End If

                                    Else
                                        'draw normal
                                        Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, TileMap(x, y).device_tile.spriteAni, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                                    End If

                                End If
                            End If

                            'If restricted tile
                            'Case Is = tile_type_enum.Restricted
                            'Draw_Ship_Tile(tile_type_enum.Hull_1, 2, pos, Color.White)
                            'If TileMap(x, y).device_tile IsNot Nothing AndAlso Not TileMap(x, y).device_tile.type = device_tile_type_enum.Empty Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, TileMap(x, y).device_tile.spriteAni, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)

                            'If device base
                        Case Is = tile_type_enum.Device_Base
                            Draw_Ship_Tile(tile_type_enum.Hull_1, 2, pos, Color.White)
                            If TileMap(x, y).device_tile IsNot Nothing AndAlso Not TileMap(x, y).device_tile.type = device_tile_type_enum.Empty Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, TileMap(x, y).device_tile.spriteAni, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)

                            'Draw Grid/Blank tiles
                        Case Is = tile_type_enum.empty
                            If tile_grid = True Then
                                If x \ 5 - x / 5 = 0 OrElse y \ 5 - y / 5 = 0 Then
                                    Draw_Ship_Tile(tile_type_enum.Hull_1, 0, pos, Color.Black)
                                Else
                                    Draw_Ship_Tile(tile_type_enum.Hull_1, 0, pos, Color.White)
                                End If
                            Else
                                Draw_Ship_Tile(tile_type_enum.Hull_1, 0, pos, Color.White)
                            End If
                            'If TileMap(x, y).device_tile IsNot Nothing AndAlso Not TileMap(x, y).device_tile.type = device_tile_type_enum.Empty Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)

                    End Select


                    'Draw build selection
                    If Build_selection > -1 AndAlso Erasing = False AndAlso Removeing_room = False AndAlso (Tech_list(Build_selection).Base_type = Tech_type_enum.Room OrElse Tech_list(Build_selection).Base_type = Tech_type_enum.Armor) Then
                        If selection_tile_map(x + 1, y + 1) < room_sprite_enum.empty Then Draw_Ship_Tile(Tech_list(Build_selection).tile_type, selection_tile_map(x + 1, y + 1), pos, Selection_color)
                    End If


                    If Removeing_room = True Then
                        'If selection_tile_map(x + 1, y + 1) < room_sprite_enum.empty Then Draw_Ship_Tile(Tech_list(Build_selection).tile_type, selection_tile_map(x + 1, y + 1), pos, Selection_color)
                        If selection_tile_map(x + 1, y + 1) < room_sprite_enum.empty Then Draw_Ship_Tile(Tech_list(Build_ship.room_list(Remove_Room_ID).type).tile_type, selection_tile_map(x + 1, y + 1), pos, Selection_color)
                    End If


                    'Drawing device
                    If Build_selection > -1 AndAlso Erasing = False AndAlso Tech_list(Build_selection).Base_type = Tech_type_enum.Device Then
                        If selection_tile_map(x + 1, y + 1) < room_sprite_enum.empty AndAlso selection_tile_map(x + 1, y + 1) < 250 Then
                            'Draw device tile                        
                            Draw_Device_Tile(Device_tech_list(Build_selection).tile_type, selection_tile_map(x + 1, y + 1), 0, pos, Device_rotation, Device_Flip, scale, Selection_color)
                            'Draw selection
                            Draw_Ship_Tile(tile_type_enum.Hull_1, 2, pos, Selection_color)
                        Else
                            If selection_tile_map(x + 1, y + 1) = 254 Then
                                Draw_Ship_Tile(tile_type_enum.Hull_1, 2, pos, Selection_color)
                            End If
                            If selection_tile_map(x + 1, y + 1) = 253 Then
                                Draw_Ship_Tile(tile_type_enum.Hull_1, 1, pos, Selection_color)
                            End If
                        End If
                    End If

                    'Drawing Pipeline build
                    If pipeing = True AndAlso Erasing = False AndAlso Pipeline_select > -1 AndAlso selection_tile_map(x + 1, y + 1) < 255 Then
                        Draw_Device_Tile(Build_ship.pipeline_list(Pipeline_select).Tile_Type, selection_tile_map(x + 1, y + 1), 0, pos, Device_rotation, Device_Flip, scale, Selection_color)
                    End If


                    'Eraseing
                    If Erasing = True Then
                        If selection_tile_map(x + 1, y + 1) < room_sprite_enum.empty Then Draw_Ship_Tile(tile_type_enum.Hull_1, 2, pos, Selection_color)
                    End If

                    'Draw cursor selection
                    If Not Tech_list(Build_selection).Base_type = Tech_type_enum.Device AndAlso selection.x = x AndAlso selection.y = y Then Draw_Ship_Tile(tile_type_enum.Hull_1, 4, pos, Color.FromArgb(100, 255, 255, 255))

                End If
            Next
        Next

        'Draw Hard Point
        'For Each item In Build_ship.device_list
        'pos.x = ((item.Value.Active_Point.x * 32) - view_location_sb.x)
        'pos.y = ((item.Value.Active_Point.y * 32) - view_location_sb.y)
        'Draw_Ship_Tile(tile_type_enum.Hull_1, 1, pos, Color.Yellow)
        'Next



        'Draw access points
        For Each room In Build_ship.room_list
            For Each item In room.Value.access_point
                pos.x = ((item.Key.x * 32) - view_location_sb.x)
                pos.y = ((item.Key.y * 32) - view_location_sb.y)
                Draw_Ship_Tile(tile_type_enum.Hull_1, 1, pos, Color.White)
            Next
        Next

        'Draw all piplines
        For Each pipe In Build_ship.pipeline_list
            For Each item In pipe.Value.Tiles
                pos.x = ((item.Key.x * 32) - view_location_sb.x)
                pos.y = ((item.Key.y * 32) - view_location_sb.y)
                If pipeing = True Then
                    If Erasing = True AndAlso item.Key = selection AndAlso pipe.Key = Pipeline_select Then
                        'Draw to erase
                        Draw_Device_Tile(pipe.Value.Tile_Type, item.Value, 0, pos, rotate_enum.Zero, Device_Flip, scale, Color.FromArgb(255, 255, 80, 80))
                        Draw_Ship_Tile(tile_type_enum.Hull_1, 4, pos, Color.FromArgb(100, 255, 0, 0))
                    Else

                        If pipe.Key = Pipeline_select Then
                            'Draw selected pipeline                            
                            If Valid_pipelines.Contains(pipe.Key) Then
                                Draw_Device_Tile(pipe.Value.Tile_Type, item.Value, 0, pos, rotate_enum.Zero, Device_Flip, scale, pipe.Value.Color)
                            Else
                                Draw_Device_Tile(pipe.Value.Tile_Type, item.Value, 0, pos, rotate_enum.Zero, Device_Flip, scale, pipe.Value.Color)
                                Draw_Ship_Tile(tile_type_enum.Hull_1, 2, pos, Color.Red)
                            End If
                        Else
                            'Draw unselected pipeline
                            If Valid_pipelines.Contains(pipe.Key) Then
                                Draw_Device_Tile(pipe.Value.Tile_Type, item.Value, 0, pos, rotate_enum.Zero, Device_Flip, scale, Color.FromArgb(pipe.Value.Color.A - 155, pipe.Value.Color.R, pipe.Value.Color.G, pipe.Value.Color.B))
                            Else
                                Draw_Device_Tile(pipe.Value.Tile_Type, item.Value, 0, pos, rotate_enum.Zero, Device_Flip, scale, Color.FromArgb(pipe.Value.Color.A - 155, pipe.Value.Color.R, pipe.Value.Color.G, pipe.Value.Color.B))
                                Draw_Ship_Tile(tile_type_enum.Hull_1, 2, pos, Color.FromArgb(100, 255, 0, 0))
                            End If

                        End If
                    End If
                Else
                    If Build_selection > -1 AndAlso Tech_list(Build_selection).Base_type = Tech_type_enum.Device AndAlso Device_tech_list(Build_selection).Contains_Pipeline(pipe.Value.Type) Then
                        'Draw correct if building device
                        Draw_Device_Tile(pipe.Value.Tile_Type, item.Value, 0, pos, rotate_enum.Zero, flip_enum.None, scale, pipe.Value.Color)
                    Else
                        'Draw clear                        
                        Draw_Device_Tile(pipe.Value.Tile_Type, item.Value, 0, pos, rotate_enum.Zero, flip_enum.None, scale, Color.FromArgb(pipe.Value.Color.A - 205, pipe.Value.Color.R, pipe.Value.Color.G, pipe.Value.Color.B))
                    End If
                End If
            Next
        Next


        'Draw remove room selection
        If Tile_selection.X >= 0 AndAlso Tile_selection.Right <= ship_size.x AndAlso Tile_selection.Y >= 0 AndAlso Tile_selection.Bottom <= ship_size.y AndAlso Removeing_room = True Then
            For x = Tile_selection.X To Tile_selection.Right
                For y = Tile_selection.Y To Tile_selection.Bottom
                    pos.x = ((x * 32) - view_location_sb.x)
                    pos.y = ((y * 32) - view_location_sb.y)
                    Draw_Ship_Tile(tile_type_enum.Hull_1, 2, pos, Color.White)
                Next
            Next
        End If



        pos.x = ((Build_ship.center_point.x * 32) - view_location_sb.x)
        pos.y = ((Build_ship.center_point.y * 32) - view_location_sb.y)
        Draw_Ship_Tile(tile_type_enum.Hull_1, 3, pos, Color.White)



        'Draw info bars
        If selection.x >= 0 AndAlso selection.x <= ship_size.x AndAlso selection.y >= 0 AndAlso selection.y <= ship_size.y AndAlso info_bars = True Then

            For y = 0 To ship_size.y
                pos.x = ((selection.x * 32) - view_location_sb.x)
                pos.y = ((y * 32) - view_location_sb.y)
                Draw_Ship_Tile(tile_type_enum.Hull_1, 2, pos, Color.FromArgb(100, 100, 255, 100))
            Next
            For x = 0 To ship_size.x
                pos.x = ((x * 32) - view_location_sb.x)
                pos.y = ((selection.y * 32) - view_location_sb.y)
                Draw_Ship_Tile(tile_type_enum.Hull_1, 2, pos, Color.FromArgb(100, 255, 100, 100))
            Next

            pos.x = ((selection.x - 1) * 32) - view_location_sb.x
            pos.y = (selection.y * 32) - view_location_sb.y
            draw_text(selection.x.ToString, New Rectangle(pos.intX, pos.intY, 32, 32), CType(DrawTextFormat.Center + DrawTextFormat.VerticalCenter, DrawTextFormat), Color.FromArgb(255, 255, 0, 0), d3d_font(d3d_font_enum.SB_small))

            pos.x = ((selection.x + 1) * 32) - view_location_sb.x
            draw_text((ship_size.x - selection.x).ToString, New Rectangle(pos.intX, pos.intY, 32, 32), CType(DrawTextFormat.Center + DrawTextFormat.VerticalCenter, DrawTextFormat), Color.FromArgb(255, 255, 0, 0), d3d_font(d3d_font_enum.SB_small))


            pos.x = (selection.x * 32) - view_location_sb.x
            pos.y = ((selection.y - 1) * 32) - view_location_sb.y
            draw_text(selection.y.ToString, New Rectangle(pos.intX, pos.intY, 32, 32), CType(DrawTextFormat.Center + DrawTextFormat.VerticalCenter, DrawTextFormat), Color.FromArgb(255, 0, 255, 0), d3d_font(d3d_font_enum.SB_small))

            pos.y = ((selection.y + 1) * 32) - view_location_sb.y
            draw_text((ship_size.y - selection.y).ToString, New Rectangle(pos.intX, pos.intY, 32, 32), CType(DrawTextFormat.Center + DrawTextFormat.VerticalCenter, DrawTextFormat), Color.FromArgb(255, 0, 255, 0), d3d_font(d3d_font_enum.SB_small))


        End If


        d3d_sprite.Transform = Matrix.Identity

        'd3d_device.TextureState(0).ColorOperation = TextureOperation.Modulate
        'd3d_device.TextureState(0).ColorArgument1 = TextureArgument.TextureColor
        'd3d_device.TextureState(0).ColorArgument2 = TextureArgument.Diffuse


        For Each Menu_collection In all_menu_items_list
            For Each item In Menu_collection.Value
                If item.Value.enabled = True Then
                    d3d_sprite.Draw(button_texture(item.Value.tile_Set), Vector3.Empty, New Vector3(item.Value.bounds.X, item.Value.bounds.Y, 0), item.Value.get_color.ToArgb)
                    If Not item.Value.text = "" Then draw_text(item.Value.text, item.Value.bounds, item.Value.align, item.Value.fontcolor, d3d_font(item.Value.font))

                    'Draw info panel
                    If Menu_collection.Key = Tech_type_enum.Menu_item AndAlso item.Key = ship_build_menu_enum.Info_panel Then
                        If Info_Select = Ship_Build_Info_enum.Info Then
                            render_ShipBuild_Info_panel()
                        End If
                        If Info_Select = Ship_Build_Info_enum.Devices Then
                            render_ShipBuild_Device_panel()
                        End If
                        If Info_Select = Ship_Build_Info_enum.Rooms Then
                            render_ShipBuild_Rooms_panel()
                        End If

                    End If

                End If
            Next
        Next






        'If Tech_list(Build_selection).Base_type = Tech_type_enum.Device  Then
        'End If




        'sb_map_zoom = 32
        'Dim adj_zoom As Single = Convert.ToSingle(1 / sb_map_zoom)
        'd3d_sprite.Transform = Matrix.Identity * Matrix.Scaling(adj_zoom, adj_zoom, 0)
        d3d_sprite.End()

        If Redraw_Minimap = True Then
            render_ship_map(Build_ship, Minimap_Texture, MiniMap_Scale)
            'Render_MiniMap_Texture(ship, Minimap_Texture)
            Redraw_Minimap = False
        End If

        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        d3d_sprite.Transform = Matrix.Identity
        pos.x = screen_size.x - 224 / 2 - (Build_ship.shipsize.x * MiniMap_Scale) / 2

        pos.y = 256 / 2 - (Build_ship.shipsize.y * MiniMap_Scale) / 2
        '256x224        
        d3d_sprite.Draw(Minimap_Texture, Vector3.Empty, New Vector3(pos.intX, pos.intY, 0), Color.White.ToArgb)
        d3d_sprite.End()

        d3d_device.EndScene()

        Try
            d3d_device.Present()
        Catch e As DeviceLostException
            DeviceLost = True
        End Try
        If DeviceLost = True Then AttemptRecovery()

    End Sub


    Sub render_ShipBuild_Info_panel()


        'Draw info Panel
        Dim panel_pos As PointI = New PointI(all_menu_items_list(Tech_type_enum.Menu_item)(ship_build_menu_enum.Info_panel).bounds.Location.X, all_menu_items_list(Tech_type_enum.Menu_item)(ship_build_menu_enum.Info_panel).bounds.Location.Y)
        If Build_selection > -1 Then
            If Tech_list(Build_selection).Base_type = Tech_type_enum.Device Then
                draw_text("= Room", New Rectangle(40, panel_pos.y + 128, 300, 30), CType(4, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
                draw_text("= Wall", New Rectangle(40, panel_pos.y + 160, 300, 30), CType(4, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
                draw_text("= Blank", New Rectangle(40, panel_pos.y + 192, 300, 30), CType(4, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
                Draw_Ship_Tile(tile_type_enum.Hull_1, 5, New PointD(6, panel_pos.y + 128), Color.White)
                Draw_Ship_Tile(tile_type_enum.Hull_1, 6, New PointD(6, panel_pos.y + 160), Color.White)
                Draw_Ship_Tile(tile_type_enum.Hull_1, 7, New PointD(6, panel_pos.y + 192), Color.White)
                draw_text(Tech_list(Build_selection).Name, New Rectangle(panel_pos.x, panel_pos.y, 300, 30), CType(5, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))

                draw_text("Connections", New Rectangle(6, panel_pos.y + 250, 300, 30), DrawTextFormat.Left, Color.White, d3d_font(d3d_font_enum.SB_small))
                Dim y = panel_pos.y + 262
                For Each pipline In Device_tech_list(Build_selection).pipeline
                    draw_text(pipline.Type.ToString + "  " + pipline.Amount.ToString, New Rectangle(6, y, 300, 30), DrawTextFormat.Left, Color.White, d3d_font(d3d_font_enum.SB_small))
                    y += 12
                Next

                'd3d_sprite.Draw(button_texture(), Vector3.Empty, New Vector3(item.bounds.X, item.bounds.Y, 0), item.get_color.ToArgb)

                Draw_device_diagram(Build_selection, New PointI(300 / 2 - (Device_tech_list(Build_selection).cmap(0).Length * 16) \ 2, panel_pos.y + 30))

            End If
            If Tech_list(Build_selection).Base_type = Tech_type_enum.Room Then
                draw_text(Tech_list(Build_selection).Name, New Rectangle(panel_pos.x, panel_pos.y, 300, 30), CType(5, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
                draw_text(Tech_list(Build_selection).Description, New Rectangle(panel_pos.x, panel_pos.y + 60, 300, 30), CType(21, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
            End If
            If Tech_list(Build_selection).Base_type = Tech_type_enum.Armor Then
                draw_text(Tech_list(Build_selection).Name, New Rectangle(panel_pos.x, panel_pos.y, 300, 30), CType(5, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
            End If
        End If

        'Draw armor info
        If Selected_device_Panel = tech_list_enum.Armor Then
            If Build_selection = tech_list_enum.Empty Then
                draw_text("Armor", New Rectangle(panel_pos.x, panel_pos.y, 300, 30), CType(5, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
                draw_text("Armor puts a hard shell between sensative ship componants and the outside. Click an armor, Then draw armor onto ship.", New Rectangle(panel_pos.x, panel_pos.y + 30, 300, 80), CType(21, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
            End If
        End If

        If Selected_device_Panel = tech_list_enum.All_rooms Then
            If Build_selection = tech_list_enum.Empty Then
                draw_text("Devices for all rooms", New Rectangle(panel_pos.x, panel_pos.y, 300, 30), CType(5, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
                draw_text("These devices can be put into any room on the ship. Select a device to add.", New Rectangle(panel_pos.x, panel_pos.y + 30, 300, 80), CType(21, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
            End If
        End If


        If Selected_device_Panel = tech_list_enum.Piping Then
            If Pipeline_select = -1 Then
                draw_text("Pipelines", New Rectangle(panel_pos.x, panel_pos.y, 300, 30), CType(5, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
                draw_text("Pipelines are what connects a ships systems together. Click on the New Pipeline button to add a pipeline. Then select the pipeline for more options.", New Rectangle(panel_pos.x, panel_pos.y + 30, 300, 80), CType(21, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
            End If

        End If


    End Sub




    Sub render_ShipBuild_Device_panel()
        Dim panel_pos As PointI = New PointI(all_menu_items_list(Tech_type_enum.Menu_item)(ship_build_menu_enum.Info_panel).bounds.Location.X, all_menu_items_list(Tech_type_enum.Menu_item)(ship_build_menu_enum.Info_panel).bounds.Location.Y)

        draw_text("Devices", New Rectangle(6, panel_pos.y, 300, 32), CType(4, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))

        Dim y As Integer = panel_pos.y + 32
        For Each Device In Build_ship.device_list


            draw_text(Tech_list(Device.Value.tech_ID).Name, New Rectangle(6, y, 300, 32), CType(4, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
            'Draw_Ship_Tile(tile_type_enum.Hull_1, 5, New PointD(6, panel_pos.y + 128), Color.White)

            y = y + 32
        Next
    End Sub





    Sub render_ShipBuild_Rooms_panel()
        Dim panel_pos As PointI = New PointI(all_menu_items_list(Tech_type_enum.Menu_item)(ship_build_menu_enum.Info_panel).bounds.Location.X, all_menu_items_list(Tech_type_enum.Menu_item)(ship_build_menu_enum.Info_panel).bounds.Location.Y)

        draw_text("Rooms    Eng / Sci", New Rectangle(6, panel_pos.y, 300, 32), CType(4, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
        Dim y As Integer = panel_pos.y + 32
        Dim Eng, Sci As Integer
        For Each room In Build_ship.room_list
            Eng = 0 : Sci = 0
            For Each Device In room.Value.device_list
                Eng += CInt(Build_ship.device_list(Device).required_Points.engineering)
                Sci += CInt(Build_ship.device_list(Device).required_Points.science)
            Next

            draw_text(Tech_list(room.Value.type).Name + "    " + Eng.ToString + "   " + Sci.ToString, New Rectangle(6, y, 300, 32), CType(4, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))
            'Draw_Ship_Tile(tile_type_enum.Hull_1, 5, New PointD(6, panel_pos.y + 128), Color.White)

            y = y + 32
        Next


    End Sub



End Module
