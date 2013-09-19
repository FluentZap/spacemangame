Module Pathfinder

    Private Const DEFAULT_WIDTH As Integer = 127 '255
    Private Const DEFAULT_HEIGHT As Integer = 127 '255

    Public Sub pf_demo()
        Dim ship_size As New PointI(127, 127)
        Dim m0 As New Ship(0, New PointD(0, 0), ship_type_enum.carbon_fiber, ship_class_enum.corvette, ship_size, 0)
        For x = 0 To ship_size.X
            For y = 0 To ship_size.Y
                m0.SetTile(x, y, New Ship_tile(0, tile_type_enum.Corridor_1, 0, 0, room_sprite_enum.Floor, walkable_type_enum.Walkable))
            Next y
        Next x
        m0.tile_map(0, 9).walkable = walkable_type_enum.Impassable
        m0.tile_map(1, 9).walkable = walkable_type_enum.Impassable
        m0.tile_map(2, 9).walkable = walkable_type_enum.Impassable
        m0.tile_map(3, 9).walkable = walkable_type_enum.Impassable
        m0.tile_map(4, 9).walkable = walkable_type_enum.Impassable
        m0.tile_map(5, 9).walkable = walkable_type_enum.Impassable
        m0.tile_map(6, 9).walkable = walkable_type_enum.Impassable
        m0.tile_map(7, 9).walkable = walkable_type_enum.Impassable
        Dim m1 As New Ship(0, New PointD(0, 0), ship_type_enum.carbon_fiber, ship_class_enum.corvette, ship_size, 0)  'impassable, worst case
        For x = 0 To ship_size.X
            For y = 0 To ship_size.Y
                m1.SetTile(x, y, New Ship_tile(0, tile_type_enum.Corridor_1, 0, 0, room_sprite_enum.Floor, walkable_type_enum.Walkable))
            Next y
        Next x
        m1.tile_map(4, 13).walkable = walkable_type_enum.Impassable
        m1.tile_map(4, 14).walkable = walkable_type_enum.Impassable
        m1.tile_map(4, 12).walkable = walkable_type_enum.Impassable
        m1.tile_map(2, 12).walkable = walkable_type_enum.Impassable
        m1.tile_map(2, 13).walkable = walkable_type_enum.Impassable
        m1.tile_map(2, 14).walkable = walkable_type_enum.Impassable
        m1.tile_map(3, 12).walkable = walkable_type_enum.Impassable
        m1.tile_map(3, 14).walkable = walkable_type_enum.Impassable
        Dim m2 As New Ship(0, New PointD(0, 0), ship_type_enum.carbon_fiber, ship_class_enum.corvette, ship_size, 0)
        For x = 0 To ship_size.X
            For y = 0 To ship_size.Y
                m2.SetTile(x, y, New Ship_tile(0, tile_type_enum.Corridor_1, 0, 0, room_sprite_enum.Floor, walkable_type_enum.Walkable))
            Next y
        Next x
        Dim m3 As New Ship(0, New PointD(0, 0), ship_type_enum.carbon_fiber, ship_class_enum.corvette, ship_size, 0) ' passable
        For x = 0 To ship_size.X
            For y = 0 To ship_size.Y
                m3.SetTile(x, y, New Ship_tile(0, tile_type_enum.Corridor_1, 0, 0, room_sprite_enum.Floor, walkable_type_enum.Walkable))
            Next y
        Next x
        m3.tile_map(4, 13).walkable = walkable_type_enum.Impassable
        m3.tile_map(4, 14).walkable = walkable_type_enum.Impassable
        m3.tile_map(4, 12).walkable = walkable_type_enum.Impassable
        m3.tile_map(2, 12).walkable = walkable_type_enum.Impassable
        m3.tile_map(2, 13).walkable = walkable_type_enum.Impassable
        m3.tile_map(2, 14).walkable = walkable_type_enum.Impassable
        m3.tile_map(3, 12).walkable = walkable_type_enum.Impassable
        Dim m4 = New Ship(0, New PointD(0, 0), ship_type_enum.carbon_fiber, ship_class_enum.corvette, ship_size, 0)
        For x = 0 To ship_size.X
            For y = 0 To ship_size.Y
                m4.SetTile(x, y, New Ship_tile(0, tile_type_enum.Corridor_1, 0, 0, room_sprite_enum.Floor, walkable_type_enum.Walkable))
            Next y
        Next x
        For i = 0 To ship_size.X ' create a full wall of impassable tiles on map
            m4.tile_map(i, 9).walkable = walkable_type_enum.Impassable
        Next

        Dim ships(4) As Ship
        ships(0) = m0
        ships(1) = m1
        ships(2) = m2
        ships(3) = m3
        ships(4) = m4

        Dim start As New PointI(3, 3)
        Dim goal As New PointI(3, 13)

        Dim pf As New A_star(use_d_star:=False)

        pf.set_map(ships(3).tile_map, ship_size)
        Dim stopwatch As New System.Diagnostics.Stopwatch
        stopwatch.Start()

        pf.set_start_end(start, goal)
        pf.find_path()

        stopwatch.Stop()
        Console.WriteLine(stopwatch.ElapsedMilliseconds)

        If pf.get_status = pf_status.path_found Then
            Console.WriteLine("Path Found")
            Dim list As LinkedList(Of PointI)
            list = pf.get_path()
            For Each vertex In list
                Console.WriteLine(Str(vertex.X) + ", " + Str(vertex.Y))
            Next
        ElseIf pf.get_status = pf_status.no_path Then
            Console.WriteLine("No Path Possible")
        ElseIf pf.get_status = pf_status.pending Then
            Console.WriteLine("Ready to begin search")
        ElseIf pf.get_status = pf_status.searching Then
            Console.WriteLine("Search in progress")
        ElseIf pf.get_status = pf_status.no_map Then
            Console.WriteLine("Map not set")
        ElseIf pf.get_status = pf_status.no_goal Then
            Console.WriteLine("No start/goal set")
        End If
    End Sub


    Private Class Edge
        Public vertex As Vertex
        Public cost As Integer
    End Class

    Private Class Vertex
        Public x As Integer
        Public y As Integer
        Public walkable As Boolean
        Public edge_list As New List(Of Edge)
        Public h As Integer
        Public g As Integer
        Public f As Integer
        Public parent As Vertex

        Public Sub set_all(ByRef source_tile As Tile, ByVal map_x As Integer, ByVal map_y As Integer)
            'Get's x/y from location on tile map instead of vector
            Me.x = map_x
            Me.y = map_y
            If source_tile.walkable = walkable_type_enum.Walkable OrElse source_tile.walkable = walkable_type_enum.Door OrElse source_tile.walkable = walkable_type_enum.OpenDoor Then
                walkable = True
            Else
                walkable = False
            End If
            parent = Nothing
        End Sub

    End Class

    Private Class Open_list
        ' this class needs to be tweaked for best performance
        ' unsure of tie-breaking method

        ' priority stack is a dictionary of stacks of vertices keyed by f-value. vertices with the same f-value will be in the same
        ' stack. The most recently entered vertex will be on top of the stack (tie-breaking feature)
        Dim priority_stack As New Dictionary(Of Integer, LinkedList(Of Vertex))
        ' hash table contains the vertices. This provides a way to see if the open list contains a certain vertex. (this would be very
        ' difficult and expensive to do by analyzing the priority stack)
        Dim hash_table As New HashSet(Of Vertex)

        Public Function contains(ByRef vertex As Vertex) As Boolean
            Return hash_table.Contains(vertex)
        End Function

        Public Function any() As Boolean
            Return priority_stack.Any
        End Function

        Public Sub clear()
            hash_table.Clear()
            priority_stack.Clear()
        End Sub

        Public Sub add(ByRef source As Vertex)
            hash_table.Add(source) ' add vertex to HT

            ' add to the dictionary
            Dim f As Integer = source.f ' f-value is the key for the dictionary
            Dim stack As LinkedList(Of Vertex) = Nothing
            If Not priority_stack.TryGetValue(f, stack) Then ' retrieve the stack at key f. if the key/stack doesn't exist, create it
                stack = New LinkedList(Of Vertex) ' create a new stack
                priority_stack.Add(f, stack) ' and add it to the dictionary
            End If
            stack.AddFirst(source) ' push vertex on stack
        End Sub

        Public Function pop_lowest_f() As Vertex
            ' returns the vertex with the lowest f-value
            ' also removes it 
            ' returns Nothing if empty
            Dim result As Vertex = Nothing
            Dim stack As LinkedList(Of Vertex)

            If priority_stack.Any Then ' if not empty
                stack = priority_stack(priority_stack.Keys.Min) ' find the lowest key in the dictionary and accesses that stack. This is a surprisingly quick method
                ' pop the first item
                result = stack.First.Value ' set result to the first item
                stack.RemoveFirst() ' remove first item
                If Not stack.Any Then ' if we just removed the only item in the stack, it is now empty and we can get rid of it
                    priority_stack.Remove(result.f)
                End If
                hash_table.Remove(result) ' remove from the HT
            End If

            Return result
        End Function

        Public Sub change_f(ByRef vertex As Vertex, ByVal old_f As Integer)
            ' the priority stack is keyed by f-value, but the pathfinding sometimes changes a vertex's f-value. The priority stack must
            ' be updated accordingly
            Dim stack As LinkedList(Of Vertex)

            ' remove the old entry
            stack = priority_stack(old_f) ' grab the stack 
            stack.Remove(vertex) ' search through and remove the vertex from the stack
            If Not stack.Any Then ' if just removed the only item from the stack, we can eliminate the stack as well
                priority_stack.Remove(old_f)
            End If

            Dim f As Integer = vertex.f ' the vertex's new f value
            stack = Nothing
            If Not priority_stack.TryGetValue(f, stack) Then ' retrieve the stack at key f. If the key/stack doesn't exist, create it
                stack = New LinkedList(Of Vertex) ' create new stack
                priority_stack.Add(f, stack) ' add it to dictionary
            End If
            stack.AddFirst(vertex) ' push the vertex onto its new home
        End Sub

    End Class

    Public Class A_star
        ' the pathfinding class
        ' uses the algorithm known as A *

        Dim width As Integer
        Dim height As Integer
        Dim max_width As Integer = 0
        Dim max_height As Integer = 0
        Dim adj_list(,) As Vertex
        Dim start As Vertex
        Dim goal As Vertex
        Dim open_list As New Open_list
        Dim closed_list As New HashSet(Of Vertex)
        Dim path As LinkedList(Of PointI)
        Dim use_d_star As Boolean ' if set to true, sets all the heuristics to 0 (performs D* instead of A*)
        Dim status As pf_status

        Sub New(Optional ByVal set_max_width As Integer = DEFAULT_WIDTH, Optional ByVal set_max_height As Integer = DEFAULT_HEIGHT, Optional ByVal use_d_star As Boolean = False)
            ' default constructor
            ' can include dimensions or it will use default
            ' can tell it to run the pathfinding from both ends (not yet programmed)
            Me.use_d_star = use_d_star
            resize(set_max_width, set_max_height) ' define the adjacency list
        End Sub

        Sub New(ByRef map(,) As Tile, ByRef map_size As PointI, Optional ByVal set_max_width As Integer = DEFAULT_WIDTH, Optional ByVal set_max_height As Integer = DEFAULT_HEIGHT)
            ' this constructor allows a tilemap to be loaded during instantiation
            ' can specify dimensions or it will use default values

            ' check dimensions
            If set_max_width < map_size.x Or set_max_height < map_size.y Then ' if one of the dimensions is too small
                If set_max_width < map_size.x Then
                    set_max_width = map_size.x
                End If
                If set_max_height < map_size.y Then
                    set_max_height = map_size.y
                End If
            End If
            resize(set_max_width, set_max_height) ' define the adjacency list
            set_map(map, map_size) ' set the map
        End Sub

        Public Sub resize(ByVal new_max_width As Integer, ByVal new_max_height As Integer)
            ' redefines the adjacency list to match the given dimensions
            max_width = new_max_width
            max_height = new_max_height
            ReDim adj_list(max_width, max_height)
            For x = 0 To max_width
                For y = 0 To max_height
                    adj_list(x, y) = New Vertex
                Next y
            Next x
            status = pf_status.no_map
        End Sub

        Public Sub set_map(ByVal map(,) As Tile, ByRef map_size As PointI)
            'check dimensions
            If max_width < map_size.x Or max_height < map_size.y Then ' if one of the dimensions is too small

                If max_width < map_size.x Then
                    max_width = map_size.x
                Else
                    max_width = Me.max_width
                End If
                If max_height < map_size.y Then
                    max_height = map_size.y
                Else
                    max_height = Me.max_height
                End If
                resize(max_width, max_height) ' resize the graph so it will fit
            End If

            'prepare the graph
            width = map_size.x
            height = map_size.y
            set_adj_list(map)
            build_edge_lists()
            If use_d_star Then
                set_h()
            End If
            status = pf_status.no_goal
        End Sub

        Public Function get_status() As pf_status
            Return status
        End Function

        Public Sub find_path()
            ' if a map, start, and goal are set, this sub can be run
            ' will run until a path is found or it has determined that no path is possible
            If status = pf_status.pending Then
                status = pf_status.searching
            End If
            While status = pf_status.searching
                find_path_step()
            End While
        End Sub

        Public Sub find_path_step()
            ' the main pathfinding function
            ' uses A *
            '
            ' can only be run if a map, start, and goal are set
            ' status is set to searching while it is searching
            ' if successful, runs save_path() and sets status to path_found. path can be retrieved by running get_path()
            ' if unsuccessful, sets status to no_path

            If status = pf_status.pending Then
                status = pf_status.searching
            End If
            If Not status = pf_status.searching Then
                Return
            End If

            Dim current As Vertex
            current = open_list.pop_lowest_f() ' take the vertex with the lowest f-value in the open list
            If current Is goal Then ' if it is the goal, we are successful
                status = pf_status.path_found
                save_path()
            ElseIf current Is Nothing Then ' if it is nothing, the open list is empty and no path is possible
                status = pf_status.no_path
            Else
                closed_list.Add(current) 'otherwise...
                For Each edge In current.edge_list ' examine each of this vertex's neighbors
                    Dim vertex As Vertex = edge.vertex
                    If closed_list.Contains(vertex) Then ' if this vertex is in the closed list, no action is required
                        'do nothing
                    ElseIf open_list.contains(vertex) Then ' if this vertex is in the open list...
                        Dim new_g As Integer
                        new_g = current.g + edge.cost ' check to see if we have found a better path (lower cost) to it
                        If new_g < vertex.g Then ' if we have found a cheaper path...
                            Dim old_f As Integer = vertex.f ' hang onto the original f value for a the change_f sub
                            vertex.g = new_g ' set the new g (cost)
                            vertex.parent = current ' change the parent to the current vertex
                            vertex.f = new_g + vertex.h ' set the new f
                            open_list.change_f(vertex, old_f) ' inform the open list that the f has changed (and the table needs a re-sort)
                        End If
                    Else ' if the vertex is not in the open list or the closed list...
                        vertex.parent = current ' set the parent
                        vertex.g = edge.cost + current.g ' calculate the g and f values
                        vertex.f = vertex.g + vertex.h
                        open_list.add(vertex) ' add it to the open list
                    End If
                Next
            End If

        End Sub



        Private Sub set_adj_list(ByRef map(,) As Tile)
            ' sets the graph's vertices. Each tile in the tile map is represented by a vertex
            Dim tile_cast As Tile ' for God knows what reason, this won't work without this temporary variable
            For x As Integer = 0 To width
                For y As Integer = 0 To height
                    tile_cast = map(x, y)
                    'Passed x/y to set_all
                    adj_list(x, y).set_all(tile_cast, x, y)
                Next y
            Next x
        End Sub

        Private Sub build_edge_lists()
            ' takes every vertex in the graph and builds an edge list for it
            Dim vertex As Vertex
            For x As Integer = 0 To width
                For y As Integer = 0 To height
                    vertex = adj_list(x, y)
                    vertex.edge_list.Clear()
                    Dim left As Edge = Nothing
                    Dim right As Edge = Nothing
                    Dim upper As Edge = Nothing
                    Dim lower As Edge = Nothing
                    Dim upper_left As Edge
                    Dim upper_right As Edge
                    Dim lower_left As Edge
                    Dim lower_right As Edge
                    ' set the left edge
                    If x > 0 Then
                        left = New Edge
                        left.vertex = adj_list(x - 1, y)
                        If left.vertex.walkable Then
                            left.cost = 10
                            vertex.edge_list.Add(left) ' add to edge list
                        End If
                    End If
                    ' set the right edge
                    If x < width Then
                        right = New Edge
                        right.vertex = adj_list(x + 1, y)
                        If right.vertex.walkable Then
                            right.cost = 10
                            vertex.edge_list.Add(right)
                        End If
                    End If
                    ' set the upper edge
                    If y > 0 Then
                        upper = New Edge
                        upper.vertex = adj_list(x, y - 1)
                        If upper.vertex.walkable Then
                            upper.cost = 10
                            vertex.edge_list.Add(upper)
                        End If
                    End If
                    ' set the lower edge
                    If y < height Then
                        lower = New Edge
                        lower.vertex = adj_list(x, y + 1)
                        If lower.vertex.walkable Then
                            lower.cost = 10
                            vertex.edge_list.Add(lower)
                        End If
                    End If
                    ' set the upper left edge
                    If x > 0 And y > 0 Then
                        upper_left = New Edge
                        upper_left.vertex = adj_list(x - 1, y - 1)
                        If upper_left.vertex.walkable And upper.vertex.walkable And left.vertex.walkable Then
                            upper_left.cost = 14
                            vertex.edge_list.Add(upper_left)
                        End If
                    End If
                    ' set the upper right edge
                    If x < width And y > 0 Then
                        upper_right = New Edge
                        upper_right.vertex = adj_list(x + 1, y - 1)
                        If upper_right.vertex.walkable And upper.vertex.walkable And right.vertex.walkable Then
                            upper_right.cost = 14
                            vertex.edge_list.Add(upper_right)
                        End If
                    End If
                    ' set the lower left edge
                    If x > 0 And y < height Then
                        lower_left = New Edge
                        lower_left.vertex = adj_list(x - 1, y + 1)
                        If lower_left.vertex.walkable And lower.vertex.walkable And left.vertex.walkable Then
                            lower_left.cost = 14
                            vertex.edge_list.Add(lower_left)
                        End If
                    End If
                    ' set the lower right edge
                    If x < width And y < height Then
                        lower_right = New Edge
                        lower_right.vertex = adj_list(x + 1, y + 1)
                        If lower_right.vertex.walkable And lower.vertex.walkable And right.vertex.walkable Then
                            lower_right.cost = 14
                            vertex.edge_list.Add(lower_right)
                        End If
                    End If
                Next y
            Next x
        End Sub

        Public Sub set_start_end(ByRef set_start As PointI, ByRef set_goal As PointI)
            ' set the start and the goal

            If status = pf_status.no_map Then ' if the map hasn't been loaded, we can't set a start or goal
                Return
            End If

            goal = adj_list(set_goal.x, set_goal.y)
            If Not use_d_star Then
                set_h() ' now that we know the goal, we can set all the vertice's h-values
            End If

            start = adj_list(set_start.x, set_start.y)
            start.g = 0
            start.f = start.h
            open_list.clear()
            open_list.add(start)
            closed_list.Clear()
            path = Nothing
            status = pf_status.pending
        End Sub

        Private Sub set_h()
            ' calculates and sets the vertex.h (heuristic) for all vertices in the adjacency list
            ' h-value is the shortest possible distance from this vertex to the goal if no obstacles are in the way
            If Not use_d_star Then
                For x As Integer = 0 To width
                    For y As Integer = 0 To height
                        Dim x_dist As Integer = Math.Abs(x - goal.x)
                        Dim y_dist As Integer = Math.Abs(y - goal.y)
                        Dim diag_dist As Integer = Math.Min(x_dist, y_dist) ' diagonal distance
                        Dim straight_dist As Integer = Math.Max(x_dist, y_dist) - diag_dist ' straight distance
                        Dim result As Integer = 14 * diag_dist + 10 * straight_dist
                        adj_list(x, y).h = result ' set the value
                    Next y
                Next x
            Else
                For x As Integer = 0 To width
                    For y As Integer = 0 To height
                        adj_list(x, y).h = 0
                    Next y
                Next x
            End If
        End Sub

        Private Sub save_path()
            ' realizes the path by tracing parents backwards from the goal to the start
            ' saves it as a linked list of points
            path = New LinkedList(Of PointI)
            Dim current As Vertex = goal ' start at the goal
            Do Until current Is Nothing
                path.AddFirst(New PointI(current.x, current.y)) ' add to the front so it comes out in order
                current = current.parent
            Loop
        End Sub

        Public Function get_path() As LinkedList(Of PointI)
            ' returns the path if one has been found
            ' the path is a linked list of points
            ' otherwise returns nothing
            If status = pf_status.path_found Then
                Return path
            Else
                Return Nothing
            End If
        End Function

    End Class

End Module
