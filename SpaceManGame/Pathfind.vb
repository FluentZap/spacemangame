Module Pathfind
    Class Node
        Public Pos As PointI
        Public G As Integer
        Public H As Integer
        Public Parent As Node = Nothing

        Sub New(ByVal Pos As PointI)
            Me.Pos = Pos
        End Sub

        Sub New(ByVal Pos As PointI, ByVal Parent As Node, ByVal G As Integer, ByVal H As Integer)
            Me.Pos = Pos
            Me.Parent = Parent
            Me.G = G
            Me.H = H
        End Sub


        Function F() As Integer
            Return G + H
        End Function

    End Class


    Class Priority_List_Class
        Public List As New Dictionary(Of Integer, List(Of PointI))()


        Sub New()

        End Sub

        Sub Add(ByVal Amount As Integer, ByVal Node As PointI)
            If Not List.ContainsKey(Amount) Then List.Add(Amount, New List(Of PointI))
            If Not List(Amount).Contains(Node) Then List(Amount).Add(Node)
        End Sub

        Sub Remove(ByVal Amount As Integer, ByVal Node As PointI)
            List(Amount).Remove(Node)
            If List(Amount).Count = 0 Then List.Remove(Amount)
        End Sub

        Function Count() As Integer
            Return (List.Count)
        End Function

        Function MinList() As Integer
            Return List.Keys.Min
        End Function

        Function FirstNode(ByVal MinList As Integer) As PointI
            Return List(MinList).First
        End Function


    End Class


    Class Pathfind

        Dim Tile_Map As Tile(,)
        Dim MapRect As Rectangle
        Public Path As New LinkedList(Of PointI)()        

        Sub New(ByVal Map(,) As Tile, ByVal Rect As Rectangle)
            Tile_Map = Map
            MapRect = Rect            
        End Sub

        Public Priority_List As New Priority_List_Class
        Public List As New Dictionary(Of PointI, Node)()
        Public CloseList As New HashSet(Of PointI)()

        Sub AddTo_OpenList(ByVal Node As Node)            
            List.Add(Node.Pos, Node)
            Priority_List.Add(Node.F, Node.Pos)
        End Sub


        Sub AddTo_ClosedList(ByVal Node As Node)            
            CloseList.Add(Node.Pos)
        End Sub

        Public Start_Point As PointI
        Public End_Point As PointI
        'Public FoundPath As New List(Of PointI)()
        Public Found_State As PFState


        Function Pathfind_Step() As PFState
            If Priority_List.Count = 0 Then Found_State = PFState.Imposable
            If Found_State <> PFState.NotFinished Then Return Found_State

            If Found_State = PFState.NotFinished Then
                Dim MinF As Integer = Priority_List.MinList
                Dim Node As Node = List(Priority_List.FirstNode(MinF))
                If Node.Pos = End_Point Then Found_State = PFState.Found
                Priority_List.Remove(MinF, Node.Pos)
                Calculate_Node(Node)
                CloseList.Add(Node.Pos)

                If Found_State = PFState.Found Then
                    Dim ListNode As Node = List(End_Point)
                    Do Until ListNode Is Nothing
                        Path.AddFirst(ListNode.Pos)
                        ListNode = ListNode.Parent
                    Loop                    
                End If
                Return PFState.NotFinished
            End If
        End Function

        Enum PFState As Byte
            NotFinished
            Found
            Imposable
        End Enum

        Sub Standard_Pathfind(ByVal Start_Point As PointI, ByVal End_Point As PointI)
            Clear_All()

            AddTo_OpenList(New Node(Start_Point, Nothing, 10, 0))
            Me.Start_Point = Start_Point
            Me.End_Point = End_Point            
            If Not (Tile_Map(End_Point.x, End_Point.y).walkable = walkable_type_enum.Door OrElse Tile_Map(End_Point.x, End_Point.y).walkable = walkable_type_enum.OpenDoor OrElse Tile_Map(End_Point.x, End_Point.y).walkable = walkable_type_enum.Walkable) Then Found_State = PFState.Imposable : Exit Sub
            Do
                Pathfind_Step()
            Loop Until Found_State <> PFState.NotFinished
            'Path.Clear()
            'Path.AddFirst(End_Point)
            'Found_State = PFState.Found
            'Return Path
        End Sub


        Sub Calculate_Node(ByVal Node As Node)
            Dim Pos As PointI = Node.Pos

            For y = Pos.y - 1 To Pos.y + 1
                For x = Pos.x - 1 To Pos.x + 1
                    If Not (x = Pos.x AndAlso y = Pos.y) AndAlso x >= MapRect.X AndAlso x <= MapRect.Right AndAlso y >= MapRect.Y AndAlso y <= MapRect.Bottom Then


                        If Tile_Map(x, y).walkable = walkable_type_enum.Door OrElse Tile_Map(x, y).walkable = walkable_type_enum.OpenDoor OrElse Tile_Map(x, y).walkable = walkable_type_enum.Walkable Then
                            If Not CloseList.Contains(New PointI(x, y)) Then
                                Dim G_Add As Integer = 10
                                Dim H As Integer
                                Dim SetNode As Boolean = True
                                Dim dx, dy As Integer

                                If x <> Pos.x AndAlso y <> Pos.y Then
                                    G_Add = 14
                                    If Tile_Map(Pos.x, y).walkable = walkable_type_enum.Impassable Then SetNode = False
                                    If Tile_Map(x, Pos.y).walkable = walkable_type_enum.Impassable Then SetNode = False
                                End If


                                If SetNode = True Then

                                    If List.ContainsKey(New PointI(x, y)) Then
                                        Dim Old_G As Integer = List(New PointI(x, y)).G
                                        Dim New_G As Integer = Node.G + G_Add
                                        If New_G < Old_G Then
                                            Priority_List.Remove(List(New PointI(x, y)).F, New PointI(x, y))
                                            List(New PointI(x, y)).G = Node.G + G_Add
                                            List(New PointI(x, y)).Parent = Node
                                            Priority_List.Add(List(New PointI(x, y)).F, New PointI(x, y))
                                        End If
                                    Else
                                        'H = Math.Abs(x - End_Point.x) * 10 + Math.Abs(y - End_Point.y) * 10
                                        dx = Math.Abs(x - End_Point.x)
                                        dy = Math.Abs(y - End_Point.y)
                                        H = 10 * (dx + dy)
                                        AddTo_OpenList(New Node(New PointI(x, y), Node, Node.G + G_Add, H))
                                    End If

                                End If
                            End If
                        End If




                    Else
                        AddTo_ClosedList(New Node(New PointI(x, y)))
                    End If


                Next
            Next
            AddTo_ClosedList(Node)
        End Sub




        Sub Clear_All()
            Found_State = PFState.NotFinished
            Path.Clear()
            Priority_List.List.Clear()
            List.Clear()
            CloseList.Clear()
        End Sub

    End Class




End Module
