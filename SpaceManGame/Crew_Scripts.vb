Public Enum crew_script_enum As Byte
    center_on_tile
    move_to_tile
    open_door
    use
    hold
    'Ship
    set_room
    remove_room
    'Planet
    'Customers
    Customer_DoSomething
    Customer_EatDrink
    Customer_Browse_Shops
End Enum

Module Crew_Scripts



    Sub run_crew_scrips(ByVal Crew_List As Dictionary(Of Integer, Crew))

        'Dim crew_temp(crew_list.Keys.Count - 1) As Integer
        ' crew_list.Keys.CopyTo(crew_temp, 0)
        Dim completed As New List(Of Integer)()
        Dim Keys(Crew_List.Keys.Count - 1) As Integer
        Crew_List.Keys.CopyTo(Keys, 0)
        For Each key In Keys
            If Crew_List(key).command_queue.Any Then

                Select Case Crew_List(key).command_queue.First.type
                    Case crew_script_enum.move_to_tile : crew_script_move_to_tile(Crew_List(key))

                    Case crew_script_enum.hold : crew_script_hold(Crew_List(key))

                    Case crew_script_enum.set_room : crew_script_set_room(Crew_List(key), key)

                    Case crew_script_enum.remove_room : crew_script_remove_room(Crew_List(key), key)

                    Case crew_script_enum.open_door : crew_script_open_door(Crew_List(key))

                        'Planet
                    Case crew_script_enum.Customer_DoSomething : crew_script_customer_do_something(Crew_List(key), key)

                    Case crew_script_enum.Customer_EatDrink : crew_script_customer_EatDrink(Crew_List(key), key)

                    Case crew_script_enum.Customer_Browse_Shops : crew_script_customer_Browse_Shops(Crew_List(key), key)

                End Select
                If Crew_List(key).command_queue.First.status = script_status_enum.complete Then Crew_List(key).command_queue.Dequeue()
            End If
        Next

    End Sub

#Region "crew scripts"
    'Must pass byval

    Public Sub crew_script_move_to_tile(ByVal crew_member As Crew)
        Dim command As Crew.Command_move = DirectCast(crew_member.command_queue.First, Crew.Command_move)

        Dim dest As PointD = command.dest
        Dim location As PointD = crew_member.location

        If command.status = script_status_enum.uninvoked Then
            command.status = script_status_enum.running

            If location.x < dest.x Then ' need to go right
                command.dx = 1
            ElseIf location.x > dest.x Then ' need to go left
                command.dx = -1
            Else
                command.dx = 0
            End If
            If location.y < dest.y Then ' need to go down
                command.dy = 1
            ElseIf location.y > dest.y Then ' need to go up
                command.dy = -1
            Else
                command.dy = 0
            End If
            ' go at speed for straight movement
            ' go at sqrt(.5) speed for diagonal
            If command.dx <> 0 AndAlso command.dy <> 0 Then
                command.dx *= DIAGONAL_SPEED_MODIFIER
                command.dy *= DIAGONAL_SPEED_MODIFIER
            End If
        End If

        Dim dx As Double = command.dx * crew_member.speed
        Dim dy As Double = command.dy * crew_member.speed
        Dim x_reached As Boolean = False
        Dim y_reached As Boolean = False


        If command.status <> script_status_enum.complete Then
            If (location.x <= dest.x AndAlso location.x + dx + ACCEPTABLE_MARGIN >= dest.x) _
            OrElse (location.x >= dest.x AndAlso location.x + dx - ACCEPTABLE_MARGIN <= dest.x) Then ' if we have reached the x goal...
                x_reached = True
            End If
            If (location.y <= dest.y AndAlso location.y + dy + ACCEPTABLE_MARGIN >= dest.y) _
            OrElse (location.y >= dest.y AndAlso location.y + dy - ACCEPTABLE_MARGIN <= dest.y) Then ' if we have reached the y goal...
                y_reached = True
            End If

            If x_reached AndAlso y_reached Then

                command.status = script_status_enum.complete
                If dx <> 0.0 AndAlso dy <> 0.0 Then ' moving on a diagonal
                    command.residual_movement = crew_member.speed - Math.Abs(location.x - dest.x) / DIAGONAL_SPEED_MODIFIER
                ElseIf dx = 0.0 AndAlso dy <> 0.0 Then ' moving vertically
                    command.residual_movement = crew_member.speed - Math.Abs(location.y - dest.y)
                ElseIf dx <> 0.0 AndAlso dy = 0.0 Then ' moving horizontally
                    command.residual_movement = crew_member.speed - Math.Abs(location.x - dest.x)
                Else ' not moving? alright then...
                    command.residual_movement = 0.0
                End If
                location.x = dest.x ' ...even it up
                location.y = dest.y
                crew_member.location = location ' write back
                resolve_residual_mvt(crew_member)
                location = crew_member.location ' write in

            Else
                If Not x_reached Then ' if we have not reached the x goal...
                    location.x += dx ' ...add distance
                End If
                If Not y_reached Then ' if we have not reached the y goal...
                    location.y += dy ' ...add distance
                End If
            End If
        End If

        'write back
        crew_member.location = location
        'Console.Write(location.x)
        'Console.Write(", ")
        'Console.Write(location.y)
        'Console.WriteLine()
    End Sub

    Public Sub crew_script_open_door(ByVal crew_member As Crew)
        'Only programed for ships
        Dim command As Crew.Command_Open_Door = DirectCast(crew_member.command_queue.First, Crew.Command_Open_Door)
        If crew_member.region = Officer_location_enum.Ship Then

            If command.Set_open = False Then
                If u.Ship_List(crew_member.Location_ID).open_door(command.Door_location) = True Then command.Set_open = True
            End If

            If u.Ship_List(crew_member.Location_ID).tile_map(command.Door_location.x, command.Door_location.y).walkable = walkable_type_enum.OpenDoor Then
                crew_member.command_queue.First.status = script_status_enum.complete
            End If

        End If
    End Sub

    Public Sub resolve_residual_mvt(ByVal crew As Crew)

        If crew.command_queue.Count >= 2 AndAlso crew.command_queue(1).type = crew_script_enum.move_to_tile Then
            Dim first_command As Crew.Command_move = DirectCast(crew.command_queue.First, Crew.Command_move)
            Dim command As Crew.Command_move = DirectCast(crew.command_queue(1), Crew.Command_move)
            Dim dest As PointD = command.dest
            Dim location As PointD = crew.location

            If command.status = script_status_enum.uninvoked Then
                command.status = script_status_enum.running

                If location.x < dest.x Then ' need to go right
                    command.dx = 1
                ElseIf location.x > dest.x Then ' need to go left
                    command.dx = -1
                Else
                    command.dx = 0
                End If
                If location.y < dest.y Then ' need to go down
                    command.dy = 1
                ElseIf location.y > dest.y Then ' need to go up
                    command.dy = -1
                Else
                    command.dy = 0
                End If
                ' go at speed for straight movement
                ' go at sqrt(.5) speed for diagonal
                If command.dx <> 0 AndAlso command.dy <> 0 Then
                    command.dx *= DIAGONAL_SPEED_MODIFIER
                    command.dy *= DIAGONAL_SPEED_MODIFIER
                End If
            End If

            crew.location.x += command.dx * first_command.residual_movement
            crew.location.y += command.dy * first_command.residual_movement

        End If
    End Sub

    Public Sub crew_script_hold(ByRef crew_member As Crew)
        Dim command As Crew.Command_hold = DirectCast(crew_member.command_queue.First, Crew.Command_hold)
        If command.status = script_status_enum.uninvoked Then
            command.status = script_status_enum.running
        End If
        If command.count < command.hold_length Then
            command.count += 1
        Else
            command.status = script_status_enum.complete
        End If
    End Sub

    Public Sub crew_script_set_room(ByVal crew_member As Crew, ByVal id As Integer)
        'Only programed for ships
        Dim ship As Ship = u.Ship_List(crew_member.Location_ID)
        Dim pos As PointI
        pos.x = crew_member.find_tile.x
        pos.y = crew_member.find_tile.y
        If crew_member.region = Officer_location_enum.Ship Then
            ship.room_list(ship.tile_map(pos.x, pos.y).roomID).working_crew_list.Add(id)
            If ship.room_list(ship.tile_map(pos.x, pos.y).roomID).assigned_crew_list.Contains(id) Then ship.room_list(ship.tile_map(pos.x, pos.y).roomID).assigned_crew_list.Remove(id)
            crew_member.command_queue.First.status = script_status_enum.complete
        End If
    End Sub

    Public Sub crew_script_remove_room(ByRef crew_member As Crew, ByVal id As Integer)
        'Only programed for ships
        Dim ship As Ship = u.Ship_List(crew_member.Location_ID)
        Dim pos As PointI
        If crew_member.region = Officer_location_enum.Ship Then
            pos.x = Convert.ToInt32(crew_member.location.x + 16 * 0.03125)
            pos.y = Convert.ToInt32(crew_member.location.y + 16 * 0.03125)
            ship.room_list(ship.tile_map(pos.x, pos.y).roomID).working_crew_list.Remove(id)

            crew_member.command_queue.First.status = script_status_enum.complete
        End If
    End Sub


    Public Sub crew_script_customer_do_something(ByRef crew_member As Crew, ByVal Id As Integer)
        Dim command As Crew.Command_Customer_Do_Something = DirectCast(crew_member.command_queue.First, Crew.Command_Customer_Do_Something)
        Dim P As Planet = u.Planet_List(crew_member.Location_ID)
        Dim Slot As Slot_Return_Type

        'Go to Pub
        Select Case random(0, 1)
            Case Is = 0
                Slot = P.GetNearistCustomerSlot(building_type_enum.Pub, crew_member.find_tile, Id)
                If Not Slot.ID = -1 Then
                    Dim AdjustedPos As PointI
                    AdjustedPos.x = P.Building_List(Slot.ID).LandRect.X + Slot.Point.x
                    AdjustedPos.y = P.Building_List(Slot.ID).LandRect.Y + Slot.Point.y
                    P.MoveCrewTo(crew_member.find_tile, AdjustedPos, crew_member)
                    crew_member.command_queue.Enqueue(New Crew.Command_Customer_EatDrink(Slot))
                    command.status = script_status_enum.complete
                End If

            Case Is = 1
                Dim Building As building_type_enum
                Select Case random(0, 2)
                    Case Is = 0 : Building = building_type_enum.Mine
                    Case Is = 1 : Building = building_type_enum.Factory
                    Case Is = 2 : Building = building_type_enum.Refinery
                End Select
                Slot = P.GetNearistCustomerSlot(Building, crew_member.find_tile, Id)
                If Not Slot.ID = -1 Then
                    Dim AdjustedPos As PointI
                    AdjustedPos.x = P.Building_List(Slot.ID).LandRect.X + Slot.Point.x
                    AdjustedPos.y = P.Building_List(Slot.ID).LandRect.Y + Slot.Point.y
                    P.MoveCrewTo(crew_member.find_tile, AdjustedPos, crew_member)
                    crew_member.command_queue.Enqueue(New Crew.Command_hold(random(100, 100)))
                    crew_member.command_queue.Enqueue(New Crew.Command_Customer_Browse_Shops(Slot, Building))
                    command.status = script_status_enum.complete
                End If
        End Select
        

    End Sub



    Public Sub crew_script_customer_EatDrink(ByRef crew_member As Crew, ByVal Id As Integer)
        Dim command As Crew.Command_Customer_EatDrink = DirectCast(crew_member.command_queue.First, Crew.Command_Customer_EatDrink)
        Dim P As Planet = u.Planet_List(crew_member.Location_ID)

        If command.Duration = -1 Then
            command.Duration = random(200, 300)
        End If

        If command.Duration > 0 Then
            command.Duration -= 1
            'Need to add buying?
        Else            
            P.Building_List(command.slot.ID).Customer_Slots(command.slot.Point) = -1
            command.status = script_status_enum.complete
            crew_member.command_queue.Enqueue(New Crew.Command_Customer_Do_Something)
        End If
    End Sub


    Public Sub crew_script_customer_Browse_Shops(ByRef crew_member As Crew, ByVal Id As Integer)
        Dim command As Crew.Command_Customer_Browse_Shops = DirectCast(crew_member.command_queue.First, Crew.Command_Customer_Browse_Shops)
        Dim P As Planet = u.Planet_List(crew_member.Location_ID)

        If command.Duration = -1 Then
            command.Duration = random(200, 300)
            If command.BuildingType = building_type_enum.Mine Then GetAiSpeech(crew_member, Speech_Enum.Customer_Browsing)
            If command.BuildingType = building_type_enum.Refinery Then GetAiSpeech(crew_member, Speech_Enum.Customer_Browsing)
            If command.BuildingType = building_type_enum.Factory Then GetAiSpeech(crew_member, Speech_Enum.Customer_Browsing)            
        End If

        If command.Duration > 0 Then
            command.Duration -= 1
            'Need to add buying?
        Else
            P.Building_List(command.slot.ID).Customer_Slots(command.slot.Point) = -1
            command.status = script_status_enum.complete
            crew_member.command_queue.Enqueue(New Crew.Command_Customer_Do_Something)
        End If
    End Sub




#End Region

End Module
