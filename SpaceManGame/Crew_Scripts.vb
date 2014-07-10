Module Crew_Scripts

    Sub run_crew_scrips(ByVal Crew_List As Dictionary(Of Integer, Crew))

        'Dim crew_temp(crew_list.Keys.Count - 1) As Integer
        ' crew_list.Keys.CopyTo(crew_temp, 0)
        Dim completed As New List(Of Integer)()
        For Each key In Crew_List.Keys
            If Crew_List(key).command_queue.Any Then

                Select Case Crew_List(key).command_queue.First.type
                    Case crew_script_enum.move_to_tile : crew_script_move_to_tile(Crew_List(key))

                    Case crew_script_enum.hold : crew_script_hold(Crew_List(key))

                    Case crew_script_enum.set_room : crew_script_set_room(Crew_List(key), key)

                    Case crew_script_enum.remove_room : crew_script_remove_room(Crew_List(key), key)

                    Case crew_script_enum.open_door : crew_script_open_door(Crew_List(key))



                    Case crew_script_enum.try_working : crew_script_try_working(Crew_List(key), key)
                    Case crew_script_enum.start_working : crew_script_start_working(Crew_List(key), key)


                    Case crew_script_enum.pub_start : crew_script_pub_start(Crew_List(key), key)





                    Case crew_script_enum.transport_buy_goods : crew_script_transport_buy_goods(Crew_List(key), key)
                    Case crew_script_enum.transport_sell_goods : crew_script_transport_sell_goods(Crew_List(key), key)

                    Case crew_script_enum.transport_dropoff_money : crew_script_transport_dropoff_money(Crew_List(key), key)
                    Case crew_script_enum.transport_pickup_exchange_money : crew_script_transport_pickup_exchange_money(Crew_List(key), key)


                    Case crew_script_enum.transport_pickup_goods : crew_script_transport_pickup_goods(Crew_List(key), key)

                    Case crew_script_enum.transport_dropoff_goods : crew_script_transport_dropoff_goods(Crew_List(key), key)
                    Case crew_script_enum.transport_pickup_money : crew_script_transport_pickup_money(Crew_List(key), key)

                    Case crew_script_enum.transport_start : crew_script_transport_start_work(Crew_List(key), key)



                    Case crew_script_enum.builder_build_tile : crew_script_builder_build_tile(Crew_List(key), key)
                    Case crew_script_enum.builder_start_work : crew_script_builder_start_work(Crew_List(key), key)


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



    Public Sub crew_script_try_working(ByVal crew_member As Crew, ByVal id As Integer)
        Dim command As Crew.Command_Try_Work = DirectCast(crew_member.command_queue.First, Crew.Command_Try_Work)
        'Only programed for planets
        Dim planet As Planet = u.Planet_List(crew_member.Location_ID)
        Dim pos As PointI
        pos.x = crew_member.find_tile.x
        pos.y = crew_member.find_tile.y
        If crew_member.region = Officer_location_enum.Planet Then
            'If planet.Building_List(crew_member.WorkBuilding).access_point(command.Ap).Used = False Then
            crew_member.command_queue.First.status = script_status_enum.complete
        End If
        'End If
    End Sub



    Public Sub crew_script_start_working(ByVal crew_member As Crew, ByVal id As Integer)
        Dim command As Crew.Command_Start_Work = DirectCast(crew_member.command_queue.First, Crew.Command_Start_Work)
        'Only programed for planets
        Dim planet As Planet = u.Planet_List(crew_member.Location_ID)
        Dim pos As PointI
        pos.x = crew_member.find_tile.x
        pos.y = crew_member.find_tile.y
        If crew_member.region = Officer_location_enum.Planet Then
            'If planet.Building_List(crew_member.WorkBuilding).access_point(command.Ap).Used = False Then
            'planet.Building_List(crew_member.WorkBuilding).access_point(command.Ap).Used = True
            'planet.Building_List(crew_member.WorkBuilding).access_point(command.Ap).NextUp = False
            'planet.Building_List(crew_member.WorkBuilding).Working_crew_list.Add(id)
            'If planet.Building_List(crew_member.WorkBuilding).Assigned_crew_list.Contains(id) Then planet.Building_List(crew_member.WorkBuilding).Assigned_crew_list.Remove(id)
            'crew_member.command_queue.First.status = script_status_enum.complete
            'End If
        End If
    End Sub




    Public Sub crew_script_pub_start(ByVal crew_member As Crew, ByVal id As Integer)
        Dim command As Crew.Command_Pub_Start = DirectCast(crew_member.command_queue.First, Crew.Command_Pub_Start)
        Dim planet As Planet = u.Planet_List(crew_member.Location_ID)
        Dim pos As PointI
        pos.x = crew_member.find_tile.x
        pos.y = crew_member.find_tile.y

        If crew_member.region = Officer_location_enum.Planet Then
            'If planet.Building_List(crew_member.PubBuilding).Assigned_crew_list.Contains(id) Then
            'planet.Building_List(crew_member.PubBuilding).Assigned_crew_list.Remove(id)
            'planet.Building_List(crew_member.PubBuilding).Working_crew_list.Add(id)
        End If

        If command.Pub_Start = -1 Then
            command.Pub_Start = GST + command.Pub_Time
            If command.Pub_Start > 3000 Then command.Pub_Start -= 3000
        End If


        If Math.Abs(GST - command.Pub_Start) < 1500 Then
            If GST > command.Pub_Start Then
                'planet.Building_List(crew_member.PubBuilding).Working_crew_list.Remove(id)
                'planet.Building_List(crew_member.PubBuilding).access_point(command.Ap).Used = False
                crew_member.command_queue.First.status = script_status_enum.complete
            End If
        End If

        'End If
    End Sub


    Public Sub crew_script_transport_start_work(ByVal crew_member As Crew, ByVal id As Integer)
        Dim command As Crew.Command_Trans_Start = DirectCast(crew_member.command_queue.First, Crew.Command_Trans_Start)
        'Only programed for planets
        Dim planet As Planet = u.Planet_List(crew_member.Location_ID)

        'If crew_member.RemoveWhenDone = False Then planet.Building_List(crew_member.WorkBuilding).Available_Transporters.Add(id) Else crew_member.RemoveWhenDone = False

        crew_member.command_queue.First.status = script_status_enum.complete
    End Sub

    Public Sub crew_script_transport_pickup_money(ByVal crew_member As Crew, ByVal id As Integer)
        Dim command As Crew.Command_Trans_Pickup = DirectCast(crew_member.command_queue.First, Crew.Command_Trans_Pickup)
        'Only programed for planets
        Dim planet As Planet = u.Planet_List(crew_member.Location_ID)
        Dim pos As PointI
        pos.x = crew_member.find_tile.x
        pos.y = crew_member.find_tile.y
        If planet.Item_Point.ContainsKey(pos) AndAlso planet.Item_Point(pos).Item = Item_Enum.CrystalCoin Then
            planet.Item_Point(pos).Amount -= command.Amount
            If planet.Item_Point(pos).Amount < 0 Then command.Amount += planet.Item_Point(pos).Amount : planet.Item_Point(pos).Amount = 0

            If crew_member.Item_List.ContainsKey(Item_Enum.CrystalCoin) Then
                crew_member.Item_List(Item_Enum.CrystalCoin) += command.Amount
            Else
                crew_member.Item_List.Add(Item_Enum.CrystalCoin, command.Amount)
            End If

            crew_member.command_queue.First.status = script_status_enum.complete
        Else
            crew_member.command_queue.First.status = script_status_enum.complete
        End If

    End Sub


    Public Sub crew_script_transport_dropoff_money(ByVal crew_member As Crew, ByVal id As Integer)
        Dim command As Crew.Command_Trans_Dropoff_Money = DirectCast(crew_member.command_queue.First, Crew.Command_Trans_Dropoff_Money)
        'Only programed for planets
        Dim planet As Planet = u.Planet_List(crew_member.Location_ID)
        Dim B As Planet_Building = planet.Building_List(command.ID)

        'Drops off money
        If crew_member.Item_List.ContainsKey(Item_Enum.CrystalCoin) Then
            'For Each Bpoint In B.Item_Slots
            'If Bpoint.Value.Input_Slot = True AndAlso planet.Item_Point.ContainsKey(Bpoint.Key) AndAlso planet.Item_Point(Bpoint.Key).Item = Item_Enum.Refined_Crystal_Piece Then planet.Item_Point(Bpoint.Key).Amount += crew_member.Item_List(Item_Enum.Refined_Crystal_Piece) : crew_member.Item_List(Item_Enum.Refined_Crystal_Piece) = 0 : crew_member.command_queue.First.status = script_status_enum.complete : Exit For
            'Next
        Else
            crew_member.command_queue.First.status = script_status_enum.complete
        End If

    End Sub


    Public Sub crew_script_transport_pickup_exchange_money(ByVal crew_member As Crew, ByVal id As Integer)
        Dim command As Crew.Command_Trans_Pickup_Exchange = DirectCast(crew_member.command_queue.First, Crew.Command_Trans_Pickup_Exchange)
        'Only programed for planets
        Dim planet As Planet = u.Planet_List(crew_member.Location_ID)
        Dim B As Planet_Building = planet.Building_List(command.ID)

        If Not crew_member.Item_List.ContainsKey(Item_Enum.CrystalCoin) Then crew_member.Item_List.Add(Item_Enum.CrystalCoin, 0)

        If planet.Exchange.Pickup_Money(command.ID) = True Then
            crew_member.Item_List(Item_Enum.CrystalCoin) += 1
        Else
            crew_member.command_queue.First.status = script_status_enum.complete
        End If

    End Sub

    Public Sub crew_script_transport_buy_goods(ByVal crew_member As Crew, ByVal id As Integer)
        Dim command As Crew.Command_Trans_Buy = DirectCast(crew_member.command_queue.First, Crew.Command_Trans_Buy)
        'Only programed for planets
        Dim planet As Planet = u.Planet_List(crew_member.Location_ID)
        
        If crew_member.Item_List.ContainsKey(Item_Enum.CrystalCoin) AndAlso crew_member.Item_List(Item_Enum.CrystalCoin) >= 10 Then
            crew_member.Item_List(Item_Enum.CrystalCoin) -= 10

            'If planet.Exchange.Buy_Item(command.Item) = True Then

            If crew_member.Item_List.ContainsKey(command.Item) Then
                crew_member.Item_List(command.Item) += 1
            Else
                crew_member.Item_List.Add(command.Item, 1)
            End If

        Else
            command.status = script_status_enum.complete
        End If
        command.Amount -= 1
        'Else
        command.status = script_status_enum.complete
        'End If


        If crew_member.Item_List(Item_Enum.CrystalCoin) <= 0 Then crew_member.Item_List.Remove(Item_Enum.CrystalCoin) : command.status = script_status_enum.complete
        If command.Amount = 0 Then crew_member.command_queue.First.status = script_status_enum.complete


    End Sub


    Public Sub crew_script_transport_sell_goods(ByVal crew_member As Crew, ByVal id As Integer)
        Dim command As Crew.Command_Trans_Sell = DirectCast(crew_member.command_queue.First, Crew.Command_Trans_Sell)
        'Only programed for planets
        Dim planet As Planet = u.Planet_List(crew_member.Location_ID)
        Dim B As Planet_Building = planet.Building_List(command.ID)


        If crew_member.Item_List.ContainsKey(command.Item) Then
            crew_member.Item_List(command.Item) -= 1
            '            planet.Exchange.List_Item(command.Item, command.ID)
            If crew_member.Item_List(command.Item) = 0 Then crew_member.Item_List.Remove(command.Item) : crew_member.command_queue.First.status = script_status_enum.complete
        End If

        If command.Amount = 0 Then crew_member.command_queue.First.status = script_status_enum.complete

    End Sub


    Public Sub crew_script_transport_pickup_goods(ByVal crew_member As Crew, ByVal id As Integer)
        Dim command As Crew.Command_Trans_Pickup_Goods = DirectCast(crew_member.command_queue.First, Crew.Command_Trans_Pickup_Goods)
        'Only programed for planets
        Dim planet As Planet = u.Planet_List(crew_member.Location_ID)
        Dim B As Planet_Building = planet.Building_List(command.ID)
        Dim pos As PointI
        pos.x = crew_member.find_tile.x
        pos.y = crew_member.find_tile.y

        Dim Bought As Boolean = False
        'For Each ipoint In B.Item_Slots
        'If planet.Item_Point.ContainsKey(ipoint.Key) AndAlso planet.Item_Point(ipoint.Key).Item = command.Item Then
        'If planet.Item_Point(ipoint.Key).Amount >= 1 Then

        'planet.Item_Point(ipoint.Key).Amount -= 1

        If crew_member.Item_List.ContainsKey(command.Item) Then
            crew_member.Item_List(command.Item) += 1
        Else
            crew_member.Item_List.Add(command.Item, 1)
        End If

        command.Amount -= 1
        'If planet.Item_Point(ipoint.Key).Amount = 0 Then planet.Item_Point.Remove(ipoint.Key)
        Bought = True
        'End If
        'End If
        'If Bought = True Then Exit For
        'Next
        If command.Amount = 0 OrElse Bought = False Then crew_member.command_queue.First.status = script_status_enum.complete

    End Sub



    Public Sub crew_script_transport_dropoff_goods(ByVal crew_member As Crew, ByVal id As Integer)
        Dim command As Crew.Command_Trans_Dropoff = DirectCast(crew_member.command_queue.First, Crew.Command_Trans_Dropoff)
        'Only programed for planets
        Dim planet As Planet = u.Planet_List(crew_member.Location_ID)
        Dim B As Planet_Building = planet.Building_List(command.Dropoff_ID)
        Dim pos As PointI
        pos.x = crew_member.find_tile.x
        pos.y = crew_member.find_tile.y

        Dim Dropped As Boolean = False        
        If Not crew_member.Item_List.ContainsKey(command.Item) Then crew_member.command_queue.First.status = script_status_enum.complete : Exit Sub

        'For Each ipoint In B.Item_Slots'

        'If ipoint.Value.Input_Slot = True Then
        'If Not planet.Item_Point.ContainsKey(ipoint.Key) Then planet.Item_Point.Add(ipoint.Key, New Item_Point_Type)
        'If (planet.Item_Point(ipoint.Key).Item = Item_Enum.None OrElse planet.Item_Point(ipoint.Key).Item = command.Item) Then
        'If planet.Item_Point(ipoint.Key).Amount < 100 Then

        'planet.Item_Point(ipoint.Key).Item = command.Item
        'planet.Item_Point(ipoint.Key).Amount += 1

        'crew_member.Item_List(command.Item) -= 1

        'command.Amount -= 1
        'Dropped = True
        'End If
        'End If
        'End If
        'If Dropped = True Then Exit For
        'Next

        If crew_member.Item_List(command.Item) <= 0 Then crew_member.Item_List.Remove(command.Item) : crew_member.command_queue.First.status = script_status_enum.complete


    End Sub







    Public Sub crew_script_builder_build_tile(ByVal crew_member As Crew, ByVal id As Integer)
        Dim command As Crew.Command_Builder_Build_Tile = DirectCast(crew_member.command_queue.First, Crew.Command_Builder_Build_Tile)
        'Only programed for planets
        Dim planet As Planet = u.Planet_List(crew_member.Location_ID)

        If planet.Build_List.ContainsKey(command.BuildingID) AndAlso planet.Build_List(command.BuildingID).Tile_List.ContainsKey(command.Position) AndAlso planet.Build_List(command.BuildingID).Build_Progress > 0 AndAlso Not planet.Build_List(command.BuildingID).Tile_List(command.Position) = 5 Then

            If command.Countdown <= 0 Then
                planet.Build_List(command.BuildingID).Tile_List(command.Position) += CByte(1)
                If planet.Build_List(command.BuildingID).Tile_List(command.Position) >= 5 Then planet.Build_List(command.BuildingID).Tile_List(command.Position) = 5 : planet.Build_List(command.BuildingID).Build_Progress -= 1 : crew_member.command_queue.First.status = script_status_enum.complete
                command.Countdown = command.Time
            Else
                command.Countdown -= 1
            End If

        Else
            crew_member.command_queue.First.status = script_status_enum.complete
        End If

        If planet.Build_List.ContainsKey(command.BuildingID) Then
            If planet.Build_List(command.BuildingID).Build_Progress <= 0 Then
                planet.Build_List(command.BuildingID).Builders -= 1
                planet.Build_List(command.BuildingID).Compleated = True
                crew_member.command_queue.Clear()
                crew_member.command_queue.Enqueue(New Crew.Command_Builder_Start_Work())
            End If
        Else
            crew_member.command_queue.Clear()
            crew_member.command_queue.Enqueue(New Crew.Command_Builder_Start_Work())
        End If

    End Sub


    Public Sub crew_script_builder_start_work(ByVal crew_member As Crew, ByVal id As Integer)
        Dim command As Crew.Command_Builder_Start_Work = DirectCast(crew_member.command_queue.First, Crew.Command_Builder_Start_Work)
        'Only programed for planets
        Dim planet As Planet = u.Planet_List(crew_member.Location_ID)
        planet.MoveCrewTo(crew_member.find_tile, planet.CapitalPoint * 32 + New PointI(16, 16), crew_member)
        planet.Builder_List(id) = -1
        crew_member.command_queue.First.status = script_status_enum.complete
    End Sub


#End Region

End Module
