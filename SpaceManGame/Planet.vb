Public Class Item_Point_Type
    Public Amount As Integer
    Public Item As Item_Enum
    Public Base_Building As Integer = -1

    Sub New(ByVal Amount As Integer, ByVal item As Item_Enum)
        Me.Amount = Amount
        Me.Item = item
    End Sub

    Sub New(ByVal Base_Building As Integer)
        Me.Base_Building = Base_Building
    End Sub

    Sub New()
    End Sub
End Class

Public Class Planet
    Public orbits_planet As Boolean
    Public orbit_point As Integer
    Public orbit_distance As Integer
    Public radius As Integer
    Public population As Integer
    Public theta As Double
    Public size As PointI
    Public landed_ships As Dictionary(Of Integer, PointI) = New Dictionary(Of Integer, PointI)

    Public Animation_Glow As Single
    Public Animation_Glow_subtract As Boolean
    Public Block_Map As HashSet(Of PointI) = New HashSet(Of PointI)
    Public Resource_Points As Dictionary(Of PointI, Boolean) = New Dictionary(Of PointI, Boolean) 'Is true is resource point is taken
    Public Building_List As Dictionary(Of Integer, Planet_Building) = New Dictionary(Of Integer, Planet_Building)

    Public Item_Point As Dictionary(Of PointI, Item_Point_Type) = New Dictionary(Of PointI, Item_Point_Type)

    Private path_find As A_star

    'trade route
    'available resources
    'regions
    Public tile_map(,) As Planet_tile
    'Dim crew_list As Crew_list

    Public type As planet_type_enum
    'ambient settings


    Public tech As HashSet(Of planet_tech_list_enum)
    Public special_tech As Planet_special_tech_enum

    Public crew_list As Dictionary(Of Integer, Crew) = New Dictionary(Of Integer, Crew)
    Public officer_list As Dictionary(Of Integer, Officer) = New Dictionary(Of Integer, Officer)


    Public Projectiles As HashSet(Of Projectile) = New HashSet(Of Projectile)

    Sub New(ByVal type As planet_type_enum, ByVal size As PointI, ByVal orbit_point As Integer, ByVal orbit_distance As Integer, ByVal orbits_planet As Boolean, ByVal theta_offset As Double)
        Me.type = type
        ReDim Me.tile_map(size.x, size.y)
        Me.size = size
        Me.orbit_point = orbit_point
        Me.orbit_distance = orbit_distance
        Me.orbits_planet = orbits_planet
        Me.theta = theta_offset
    End Sub




    Sub MoveOfficer(ByVal Id As Integer, ByRef vector As PointD)
        Dim Pos(3) As PointD
        vector.x *= officer_list(Id).speed
        vector.y *= officer_list(Id).speed

        '0:7,1
        '0:24,1
        '0:7,31
        '0:24,31

        'top left
        Pos(0).x = Math.Floor((officer_list(Id).GetLocation.x + 8 + vector.x) * 0.03125)
        Pos(0).y = Math.Floor((officer_list(Id).GetLocation.y + 27 + vector.y) * 0.03125)
        'top right
        Pos(1).x = Math.Floor((officer_list(Id).GetLocation.x + 23 + vector.x) * 0.03125)
        Pos(1).y = Math.Floor((officer_list(Id).GetLocation.y + 27 + vector.y) * 0.03125)
        'bottom left
        Pos(2).x = Math.Floor((officer_list(Id).GetLocation.x + 8 + vector.x) * 0.03125)
        Pos(2).y = Math.Floor((officer_list(Id).GetLocation.y + 31 + vector.y) * 0.03125)
        'bottom right
        Pos(3).x = Math.Floor((officer_list(Id).GetLocation.x + 23 + vector.x) * 0.03125)
        Pos(3).y = Math.Floor((officer_list(Id).GetLocation.y + 31 + vector.y) * 0.03125)

        Dim b As Integer = 0
        For a = 0 To 3
            If Pos(a).x >= 0 And Pos(a).x <= size.x And Pos(a).y >= 0 And Pos(a).y <= size.y Then
                If tile_map(Convert.ToInt32(Pos(a).x), Convert.ToInt32(Pos(a).y)).walkable = walkable_type_enum.Walkable Then
                    b = b + 1
                End If
            End If
        Next
        If b = 4 Then
            officer_list(Id).Move(vector)
        ElseIf Not b = 0 Then
            If vector.x < 0 Then officer_list(Id).SetLocation(New PointD(Convert.ToInt32(Pos(0).x * 32 + 32 - 8), officer_list(Id).GetLocation.y))
            If vector.x > 0 Then officer_list(Id).SetLocation(New PointD(Convert.ToInt32(Pos(1).x * 32 - 32 + 8), officer_list(Id).GetLocation.y))
            If vector.y < 0 Then officer_list(Id).SetLocation(New PointD(officer_list(Id).GetLocation.x, Convert.ToInt32(Pos(0).y * 32 + 32 - 27)))
            If vector.y > 0 Then officer_list(Id).SetLocation(New PointD(officer_list(Id).GetLocation.x, Convert.ToInt32(Pos(2).y * 32 - 32)))
        End If
    End Sub



    Sub populate()
        populate_planet(Me)
        Load_Pathfinding()
    End Sub


    Public Sub DoEvents()
        Process_crew()
        Process_Work()

        Update_Officers()

        Handle_Projectiles()
        run_crew_scrips(crew_list)
        Run_animations()

    End Sub


    Sub Process_Work()
        If GSTFrequency = 0 Then
            If GST Mod 10 = 0 Then
                For Each buiding In Building_List
                    If buiding.Value.Type = building_type_enum.Mine Then
                        For Each CrewID In buiding.Value.Working_crew_list
                            crew_list(CrewID).Wealth += 1
                        Next

                        For a = 1 To buiding.Value.Working_crew_list.Count
                            For Each ipoint In Item_Point
                                If (ipoint.Value.Item = Item_Enum.None OrElse ipoint.Value.Item = Item_Enum.Crystal) AndAlso ipoint.Value.Base_Building = buiding.Key Then
                                    If ipoint.Value.Amount < 100 Then
                                        ipoint.Value.Item = Item_Enum.Crystal
                                        ipoint.Value.Amount += 1
                                        Exit For
                                    End If
                                End If
                            Next
                        Next

                    End If
                Next

            End If


        End If
    End Sub


    Sub Process_crew()
        If GSTFrequency = 0 Then
            'Send Crew to work
            'Send Crew to bank if needed
            'Send Crew to Pub
            'Send Crew home
            'al la repeto


            'Send to work
            If GST = 800 Then
                Send_Crew_ToWork(Work_Shift_Enum.Morning)
            ElseIf GST = 1800 Then
                Send_Crew_ToWork(Work_Shift_Enum.Mid)
            ElseIf GST = 2800 Then
                Send_Crew_ToWork(Work_Shift_Enum.Night)
            End If



            'Send crew home
            If GST = 1000 Then
                Send_Crew_Home(Work_Shift_Enum.Night)
            ElseIf GST = 2000 Then
                Send_Crew_Home(Work_Shift_Enum.Morning)
            ElseIf GST = 3000 Then
                Send_Crew_Home(Work_Shift_Enum.Mid)
            End If


        End If
    End Sub


    Sub Send_Crew_ToWork(ByVal WorkShift As Work_Shift_Enum)
        Dim work_Point As PointI
        Dim Found_AP As Boolean = False

        For Each Crew In crew_list
            'Not already working/going to work
            If Not Building_List(Crew.Value.WorkBuilding).Working_crew_list.Contains(Crew.Key) AndAlso Not Building_List(Crew.Value.WorkBuilding).Assigned_crew_list.Contains(Crew.Key) Then
                If Crew.Value.WorkBuilding > -1 Then



                    If Crew.Value.WorkShift = WorkShift Then
                        'Find AP in building
                        For Each AP In Building_List(Crew.Value.WorkBuilding).access_point
                            If AP.Value.NextUp = False Then
                                work_Point = AP.Key
                                Found_AP = True
                                Exit For
                            End If
                        Next
                        'If Open access point is found send crew to work
                        If Found_AP = True Then
                            Found_AP = False
                            'Set NextUp to work to true
                            Building_List(Crew.Value.WorkBuilding).access_point(work_Point).NextUp = True
                            'Pathfind
                            If Not tile_map(Crew.Value.find_tile.x, Crew.Value.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit For
                            Dim tile As PointI = Crew.Value.find_tile

                            path_find.set_start_end(tile, work_Point)
                            path_find.find_path()

                            If path_find.get_status = pf_status.path_found Then
                                If Crew.Value.command_queue.Any Then
                                    Crew.Value.command_queue.Clear()
                                End If
                                Building_List(Crew.Value.WorkBuilding).Assigned_crew_list.Add(Crew.Key)
                                Dim list As LinkedList(Of PointI)
                                list = path_find.get_path

                                'Need to add wait for finish
                                Crew.Value.command_queue.Clear()
                                For Each dest In list
                                    If dest = work_Point Then
                                        Crew.Value.command_queue.Enqueue(New Crew.Command_Try_Work(work_Point))
                                    End If
                                    Crew.Value.command_queue.Enqueue(New Crew.Command_move(New PointD(dest.x * 32, dest.y * 32)))
                                Next
                                Crew.Value.command_queue.Enqueue(New Crew.Command_Start_Work(work_Point))

                            End If
                        End If
                    End If
                End If
            End If
        Next

    End Sub



    Sub Send_Crew_Home(ByVal WorkShift As Work_Shift_Enum)
        Dim Home_Point As PointI
        Dim Home_rect As Rectangle

        For Each Crew In crew_list
            If Crew.Value.HomeBuilding > -1 Then
                If Crew.Value.WorkShift = WorkShift Then
                    Home_rect = Building_List(Crew.Value.HomeBuilding).BuildingRect(Crew.Value.HomeSpace)
                    Home_Point.x = CInt(Home_rect.X + Home_rect.Width / 2)
                    Home_Point.y = CInt(Home_rect.Y + Home_rect.Height / 2)
                    'Find AP in building                    
                    'Pathfind

                    If Not tile_map(Crew.Value.find_tile.x, Crew.Value.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit For
                    Dim tile As PointI = Crew.Value.find_tile

                    Crew.Value.command_queue.Clear()
                    Dim End_Point As PointI
                    End_Point = Send_Crew_Errands(Crew.Value, Crew.Key)

                    path_find.set_start_end(End_Point, Home_Point)
                    path_find.find_path()

                    If path_find.get_status = pf_status.path_found Then
                        If Building_List(Crew.Value.WorkBuilding).Working_crew_list.Contains(Crew.Key) Then Building_List(Crew.Value.WorkBuilding).Working_crew_list.Remove(Crew.Key)
                        If Building_List(Crew.Value.WorkBuilding).access_point.ContainsKey(tile) Then Building_List(Crew.Value.WorkBuilding).access_point(tile).Used = False
                        Dim list As LinkedList(Of PointI)
                        list = path_find.get_path

                        'Need to add sleep and stuuuuf


                        For Each dest In list
                            Crew.Value.command_queue.Enqueue(New Crew.Command_move(New PointD(dest.x * 32, dest.y * 32)))
                        Next
                    End If


                End If
            End If
        Next

    End Sub


    Function Send_Crew_Errands(ByVal Crew As Crew, ByVal ID As Integer) As PointI

        If Not tile_map(Crew.find_tile.x, Crew.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit Function
        Dim tile As PointI = Crew.find_tile

        Dim GotoBank As Boolean
        Dim Pub_Time As Integer
        'If Crew.Wealth > 500 Then GotoBank = True

        Dim Found_Bank_AP As Boolean
        Dim Bank_Point As PointI

        If GotoBank = True Then
            For Each AP In Building_List(Crew.BankBuilding).access_point
                If AP.Value.Used = False Then Bank_Point = AP.Key : Found_Bank_AP = True : Exit For
            Next

            If Found_Bank_AP = True Then
                Found_Bank_AP = False
                Building_List(Crew.BankBuilding).access_point(Bank_Point).Used = True

                path_find.set_start_end(tile, Bank_Point)
                path_find.find_path()

                If path_find.get_status = pf_status.path_found Then
                    If Crew.command_queue.Any Then
                        Crew.command_queue.Clear()
                    End If

                    Building_List(Crew.BankBuilding).Assigned_crew_list.Add(ID)
                    Dim list As LinkedList(Of PointI)
                    list = path_find.get_path

                    'Need to add wait for finish
                    Crew.command_queue.Clear()
                    For Each dest In list
                        Crew.command_queue.Enqueue(New Crew.Command_move(New PointD(dest.x * 32, dest.y * 32)))
                    Next
                    Crew.command_queue.Enqueue(New Crew.Command_Pub_Start(Bank_Point, Pub_Time))
                    Return Bank_Point
                End If
            End If
        End If



        Pub_Time = random(100, 200)


        Dim Found_AP As Boolean
        Dim Pub_Point As PointI
        For Each AP In Building_List(Crew.PubBuilding).access_point
            If AP.Value.Used = False Then Pub_Point = AP.Key : Found_AP = True : Exit For
        Next

        If Found_AP = True Then
            Found_AP = False
            'Set NextUp to work to true
            Building_List(Crew.PubBuilding).access_point(Pub_Point).Used = True
            'Pathfind
            path_find.set_start_end(tile, Pub_Point)
            path_find.find_path()


            If path_find.get_status = pf_status.path_found Then
                If Crew.command_queue.Any Then
                    Crew.command_queue.Clear()
                End If

                Building_List(Crew.PubBuilding).Assigned_crew_list.Add(ID)
                Dim list As LinkedList(Of PointI)
                list = path_find.get_path

                'Need to add wait for finish
                Crew.command_queue.Clear()
                For Each dest In list
                    Crew.command_queue.Enqueue(New Crew.Command_move(New PointD(dest.x * 32, dest.y * 32)))
                Next
                Crew.command_queue.Enqueue(New Crew.Command_Pub_Start(Pub_Point, Pub_Time))
                Return Pub_Point
            End If
        End If







    End Function


    Sub Load_Pathfinding()
        path_find = New A_star
        path_find.set_map(tile_map, size)

    End Sub






    Sub Run_animations()
        For Each item In officer_list
            item.Value.Update_Sprite()
        Next

        For Each item In crew_list
            item.Value.Update_Sprite()
        Next

        If Animation_Glow_subtract = True Then
            Animation_Glow -= 0.001F
        Else
            Animation_Glow += 0.001F
        End If

        If Animation_Glow >= 1 Then Animation_Glow = 1 : Animation_Glow_subtract = True
        If Animation_Glow <= 0.8 Then Animation_Glow = 0.8 : Animation_Glow_subtract = False

    End Sub

    Sub Update_Officers()
        For Each item In officer_list
            If item.Value.input_flages.walking = True Then
                Select Case item.Value.input_flages.Facing
                    Case Is = Move_Direction.Left
                        If item.Value.input_flages.MoveX = Move_Direction.Left Then MoveOfficer(item.Key, New PointD(-1, 0))
                        If item.Value.input_flages.MoveX = Move_Direction.Right Then MoveOfficer(item.Key, New PointD(0.2, 0))
                        If item.Value.input_flages.MoveY = Move_Direction.Up Then MoveOfficer(item.Key, New PointD(0, -DIAGONAL_SPEED_MODIFIER))
                        If item.Value.input_flages.MoveY = Move_Direction.Down Then MoveOfficer(item.Key, New PointD(0, DIAGONAL_SPEED_MODIFIER))
                    Case Is = Move_Direction.Right
                        If item.Value.input_flages.MoveX = Move_Direction.Left Then MoveOfficer(item.Key, New PointD(-0.2, 0))
                        If item.Value.input_flages.MoveX = Move_Direction.Right Then MoveOfficer(item.Key, New PointD(1, 0))
                        If item.Value.input_flages.MoveY = Move_Direction.Up Then MoveOfficer(item.Key, New PointD(0, -DIAGONAL_SPEED_MODIFIER))
                        If item.Value.input_flages.MoveY = Move_Direction.Down Then MoveOfficer(item.Key, New PointD(0, DIAGONAL_SPEED_MODIFIER))
                    Case Is = Move_Direction.Up
                        If item.Value.input_flages.MoveX = Move_Direction.Left Then MoveOfficer(item.Key, New PointD(-DIAGONAL_SPEED_MODIFIER, 0))
                        If item.Value.input_flages.MoveX = Move_Direction.Right Then MoveOfficer(item.Key, New PointD(DIAGONAL_SPEED_MODIFIER, 0))
                        If item.Value.input_flages.MoveY = Move_Direction.Up Then MoveOfficer(item.Key, New PointD(0, -1))
                        If item.Value.input_flages.MoveY = Move_Direction.Down Then MoveOfficer(item.Key, New PointD(0, 0.2))
                    Case Is = Move_Direction.Down
                        If item.Value.input_flages.MoveX = Move_Direction.Left Then MoveOfficer(item.Key, New PointD(-DIAGONAL_SPEED_MODIFIER, 0))
                        If item.Value.input_flages.MoveX = Move_Direction.Right Then MoveOfficer(item.Key, New PointD(DIAGONAL_SPEED_MODIFIER, 0))
                        If item.Value.input_flages.MoveY = Move_Direction.Up Then MoveOfficer(item.Key, New PointD(0, -0.2))
                        If item.Value.input_flages.MoveY = Move_Direction.Down Then MoveOfficer(item.Key, New PointD(0, 1))

                End Select
            End If


        Next
    End Sub


    Sub Handle_Projectiles()
        Dim remove_List As List(Of Projectile) = New List(Of Projectile)
        For Each item In Me.Projectiles
            item.Location.x += item.vector_velocity.x
            item.Location.y += item.vector_velocity.y
            item.Life -= 1
            If item.Life <= 0 Then
                If item.Second_Stage_Life <= 0 Then
                    remove_List.Add(item)
                Else
                    item.Life = item.Second_Stage_Life
                    item.Second_Stage_Life = 0
                    Dim vec As PointD

                    vec.x = Math.Cos(item.Rotation - 1.57079633)
                    vec.y = Math.Sin(item.Rotation - 1.57079633)

                    item.vector_velocity.x += vec.x * 20
                    item.vector_velocity.y += vec.y * 20
                End If
            End If


            Dim tile As PointI = New PointI(item.Location.intX \ 32, item.Location.intY \ 32)
            If tile.x >= 0 AndAlso tile.x <= size.x AndAlso tile.y >= 0 AndAlso tile.y <= size.y Then
                If (Not tile_map(tile.x, tile.y).walkable = walkable_type_enum.Walkable) AndAlso (Not tile_map(tile.x, tile.y).walkable = walkable_type_enum.OpenDoor) Then
                    Detonate_Local_Projectile(item)
                    remove_List.Add(item)
                End If
            End If

        Next

        For Each item In remove_List
            Me.Projectiles.Remove(item)
        Next

    End Sub


    Sub Detonate_Local_Projectile(ByVal Pro As Projectile)
        Dim contact_Point As PointI = New PointI(Pro.Location.intX \ 32, Pro.Location.intY \ 32)
        'tile_map(contact_Point.x, contact_Point.y).color = Color.Red

        For y = contact_Point.y - 2 To contact_Point.y + 2
            For x = contact_Point.x - 2 To contact_Point.x + 2
                If x >= 0 AndAlso x <= size.x AndAlso y >= 0 AndAlso y <= size.y Then
                    'tile_map(x, y).color = Color.Red

                    For Each Crew In crew_list
                        If Crew.Value.find_tile = New PointI(x, y) Then
                            Crew.Value.Health.Damage_All_Limbs(3)
                        End If

                    Next

                End If
            Next
        Next

    End Sub




    Function GetOfficer() As Dictionary(Of Integer, Officer)
        Return Me.officer_list
    End Function










End Class