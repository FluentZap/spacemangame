Module MainModule


#Region "DirectX"
    Public d3d_device As Device
    Public d3d_chain As SwapChain
    Public d3d_sprite As Sprite
    Public d3d_font() As Direct3D.Font
    'Public offscreentexture As Texture

    Public Ship_Texture As Texture
    Public Minimap_Texture As Texture
    Public Ship_Map_Texture As Texture    
    'Public Ship_Surface As Surface


    Public screen_size As PointI = New PointI(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)

    Public screen_zoom As Single = 2
    'Public atSize As Integer = 64

    Public Lum As Single


    Public windowed As Boolean = True
    Public monitor As Integer = 0
    Public DeviceLost As Boolean
    Public AnimationTick As UInteger

    Public Nebula_VB As VertexBuffer    
    Public Nebula_VB_FVF As CustomVertex

    Public Planet_VB As VertexBuffer
    Public Orbit_VB As VertexBuffer
    Public Orbit2_VB As VertexBuffer

    Structure Nebula_VB_struct
        Dim pos As Vector3
        Dim color As Color
    End Structure


#End Region
#Region "Resources"
    Public mouse_texture As Texture
    Public button_texture(100) As Texture
    Public tile_texture(100) As Texture
    Public planet_tile_texture(100) As Texture
    Public projectile_tile_texture(100) As Texture

    Public effect_texture(100) As Texture


    Public device_tile_texture(100) As Texture
    Public character_texture(100) As Texture
    Public panel_texture(100) As Texture
    Public Ship_Decal_Texture(100) As Texture


    Public icon_texture(100) As Texture
    Public test_texture As Texture

    Public Cursor1 As Surface = Nothing
#End Region
    Public Const TILE_SIZE As Integer = 32
    Public Const DIAGONAL_SPEED_MODIFIER As Double = 0.70710678118654757


    Public Const PI As Double = 3.14159265358979

    Public Const ACCEPTABLE_MARGIN As Double = 0

    Declare Function QueryPerformanceCounter Lib "Kernel32" (ByRef X As Long) As Short
    Declare Function QueryPerformanceFrequency Lib "Kernel32" (ByRef X As Long) As Short

#Region "Region Structures"

    'Structure Region_Type
    'Dim Ships As Dictionary(Of Integer, HashSet(Of Integer))
    'Dim Planets As HashSet(Of Planet_Id_Type)
    'End Structure

#End Region

    Enum button_style
        DisplayOnly
        Clickable
        Hilightable
        Both
    End Enum

    Class Menu_button
        Public enabled As Boolean
        Public tile As Integer
        Public tile_Set As button_texture_enum
        Public bounds As Rectangle
        Public color As Color
        Public Adj_color As Color
        Public align As DrawTextFormat

        Public style As button_style
        Public text As String
        Public font As d3d_font_enum
        Public fontcolor As Color
        Public Adj_font_color As Color

        Sub New(ByVal tile_Set As button_texture_enum, ByRef bounds As Rectangle, ByVal Style As button_style, ByVal color As Color, ByVal text As String, ByVal fontcolor As Color, ByVal font As d3d_font_enum, Optional ByVal Enabled As Boolean = True, Optional ByVal Align As Integer = CType(DrawTextFormat.VerticalCenter + DrawTextFormat.Center + DrawTextFormat.WordBreak, DrawTextFormat))
            Me.color = color
            Me.Adj_color = color
            Me.style = Style
            Me.tile_Set = tile_Set
            Me.tile = tile
            Me.bounds = bounds
            Me.text = text
            Me.fontcolor = fontcolor
            Me.Adj_font_color = fontcolor
            Me.font = font
            Me.enabled = Enabled
            Me.align = CType(Align, DrawTextFormat)
            Me.reset()
        End Sub

        Sub New(ByVal tile_Set As button_texture_enum, ByRef bounds As Rectangle, ByVal Style As button_style, Optional ByVal Enabled As Boolean = True)
            Me.style = Style
            Me.tile_Set = tile_Set
            Me.bounds = bounds
            Me.color = Drawing.Color.White
            Me.Adj_color = Drawing.Color.White
            Me.enabled = Enabled
            Me.reset()
        End Sub

        Sub New(ByVal tile_Set As button_texture_enum, ByRef bounds As Rectangle, ByVal Style As button_style, ByVal color As Color, Optional ByVal Enabled As Boolean = True)
            Me.style = Style
            Me.tile_Set = tile_Set
            Me.bounds = bounds
            Me.color = color
            Me.Adj_color = Drawing.Color.White
            Me.enabled = Enabled
            Me.reset()
        End Sub


        Public Sub press()
            If style = button_style.Clickable OrElse style = button_style.Both Then
                Me.Adj_color = Set_Brighness(Me.color, 1)
                Me.Adj_font_color = Set_Brighness(Me.fontcolor, 1)
            End If
        End Sub

        Public Sub click()
            If style = button_style.Clickable OrElse style = button_style.Both Then
                Me.Adj_color = Set_Brighness(Me.color, 0.9)
                Me.Adj_font_color = Set_Brighness(Me.fontcolor, 0.9)
            End If
        End Sub

        Public Sub hilight()
            If style = button_style.Hilightable OrElse style = button_style.Both Then
                Me.Adj_color = Set_Brighness(Me.color, 0.85)
                Me.Adj_font_color = Set_Brighness(Me.fontcolor, 0.85)
            End If
        End Sub

        Sub reset()
            If style = button_style.Clickable Then Me.Adj_color = Set_Brighness(Me.color, 0.75) : Me.Adj_font_color = Set_Brighness(Me.fontcolor, 0.75)
            If style = button_style.Both Then Me.Adj_color = Set_Brighness(Me.color, 0.75) : Me.Adj_font_color = Set_Brighness(Me.fontcolor, 0.75)
            If style = button_style.Hilightable Then Me.Adj_color = Set_Brighness(Me.color, 0.75) : Me.Adj_font_color = Set_Brighness(Me.fontcolor, 0.75)
            If style = button_style.DisplayOnly Then Me.Adj_color = Me.color : Me.Adj_font_color = Me.fontcolor
        End Sub


        Sub clear_color()
            Adj_color = color
        End Sub


        Function get_color() As Color            
            Return Adj_color
        End Function

        Function get_font_color() As Color
            Return Adj_font_color
        End Function

    End Class

#Region "Game Varables"

    'Temp varables
    Public FPS As Double
    Public Distance_from As Double

    Public Logic_Time As Double
    Public render_start, render_end, time_current, cps, refresh_rate As Long
    Public logic_start, logic_end, logic_rate As Long
    Public Logic_Ratio As Double
    Public Logic_Duration As Long

    Public render_time As Long
    Public render_duration As Long

    Public Logic_Per_Frame As Long


    'Ship Decals
    Public Ship_Decals As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String)

    Public terminate As Boolean = False
    Public Screen_Lock As Boolean = True
    Public Random_Seed As Double
    Public Player_Tech As HashSet(Of tech_list_enum) = New HashSet(Of tech_list_enum)


    Public Tech_list As Dictionary(Of tech_list_enum, Tech_type) = New Dictionary(Of tech_list_enum, Tech_type)
    Public Device_tech_list As Dictionary(Of tech_list_enum, device_data) = New Dictionary(Of tech_list_enum, device_data)
    Public Weapon_tech_list As Dictionary(Of Ship_weapon_enum, Ship_Weapon) = New Dictionary(Of Ship_weapon_enum, Ship_Weapon)    


    Public current_region As PointI
    Public current_view As current_view_enum


    Public view_location_internal As PointI
    Public view_location_external As PointD
    Public view_location_sb As PointI
    Public view_location_planet As PointI
    Public view_location_star_map As PointI

    'Public Rview_location_personal As PointD
    Public view_location_personal As PointD

    Public view_location_weapon_control As PointD

    Public view_location_personal_Last As PointD


    'Public view_personal_menu_items As Dictionary(Of Internal_menu_items_Enum, Menu_button) = New Dictionary(Of Internal_menu_items_Enum, Menu_button)
    'Public view_external_menu_items As Dictionary(Of External_menu_items_Enum, Menu_button) = New Dictionary(Of External_menu_items_Enum, Menu_button)

    'Menu Items
    Public External_Menu_Items As Dictionary(Of Integer, Menu_button) = New Dictionary(Of Integer, Menu_button)
    Public External_Menu_Items_Weapon_Control As Dictionary(Of Integer, Menu_button) = New Dictionary(Of Integer, Menu_button)
    Public External_Menu_Items_Weapon_Control_Buttons As Dictionary(Of Integer, Menu_button) = New Dictionary(Of Integer, Menu_button)

    Public Menu_Items_Personal_Level As Dictionary(Of Integer, Menu_button) = New Dictionary(Of Integer, Menu_button)

    Public internal_zoom As Double
    Public external_zoom As Single
    Public personal_zoom As Single

    Public Weapon_Control_zoom As Single = 1.0

    Public sb_zoom As Double
    Public sb_map_zoom As Double
    Public planet_zoom As Double

    Public star_map_zoom As Double
    Public star_map_sector As Integer
    Public planet_theta_offset As Double

    Public planet_cloud_theta As Double

    Public current_player As Integer
    Public current_selected_ship_view As Integer
    Public current_selected_planet_view As Integer


    Public Ship_List As New Dictionary(Of Integer, Ship)(500)
    Public Planet_List As New Dictionary(Of Integer, Planet)(150)
    Public Crew_List As Dictionary(Of Integer, Crew) = New Dictionary(Of Integer, Crew)
    Public Officer_List As Dictionary(Of Integer, Officer) = New Dictionary(Of Integer, Officer)

    Public u As Universe.Universe = New Universe.Universe
    Public Drag As Double = 0.02
    Public Near_planet As Integer
    Public Loaded_planet As Integer


    Public external_planet_texture(15) As Texture

    Public Loaded_Officer_Textures As Dictionary(Of Integer, Texture) = New Dictionary(Of Integer, Texture)


    'External View
    Public External_Menu_Open As Boolean    
    Public External__Reload_Menu As Boolean = True


    Dim pressedkeys As HashSet(Of Keys) = Nothing
    Dim mouse_info As MainForm.mouse_info_type = Nothing

    'Weapon_Control
    Public Weapon_Control__Reload_Menu As Boolean = True
    Public Weapon_Control__Selected_Group As Integer = -1


    'Personal Level up
    Public PLV__Selected_Officer As Integer
    Public PLV__Selected_Officer_Old As Integer
    Public PLV__Selected_Class As Class_List_Enum
    Public PLV__Selected_Class_Old As Class_List_Enum
    Public PLV__Officer_Scroll As Integer
    Public PLV__Class_Scroll As Integer    

    'Personal view
    Public Mouse_Target As Integer


    'Dim region(100, 100) As Region_Type
    'Dim ship_view_Region As Point
    'Dim ship_view_location As Point

    Structure game_parameters
        Dim new_game As Boolean
    End Structure


    Public Player_Data As New Player_Data_Type




    Class Player_Data_Type
        Public Officer_List As HashSet(Of Integer) = New HashSet(Of Integer)

    End Class

#End Region

    Sub Set_defaults()
        Random_Seed = DateAndTime.Timer
        Randomize(Random_Seed)
        sb_zoom = 1
        internal_zoom = 2
        external_zoom = 2
        personal_zoom = 2

        Near_planet = -1
        Loaded_planet = -1

        Player_Data.Officer_List.Add(0)

        Load_tech_list()
        set_device_map()
        Set_Weapon_Tech()
        Load_Menu_Items()

        QueryPerformanceFrequency(cps)        
        'refresh_rate = CLng(Math.Round(cps / (d3d_device.GetSwapChain(0).DisplayMode.RefreshRate)))


        refresh_rate = CLng(Math.Round(cps / 60))

        Logic_Ratio = refresh_rate / (cps / 120)

    End Sub


    Structure point_sprite_vertex_format
        Public Pos As Vector3
        Public Color As Integer

        Public Sub New(ByVal _Pos As Vector3, ByVal _Color As Integer)
            Pos = _Pos
            Color = _Color
        End Sub
    End Structure

    Public star_map_vertex As VertexBuffer

    Function Set_Brighness(ByVal color As Color, ByVal Percent As Single) As Color
        Dim R, G, B As Single
        R = If(((color.R * Percent) > 255), 255, (color.R * Percent))
        G = If(((color.G * Percent) > 255), 255, (color.G * Percent))
        B = If(((color.B * Percent) > 255), 255, (color.B * Percent))

        Return color.FromArgb(CInt(R), CInt(G), CInt(B))
    End Function

    Sub init_DX()
        'Load Main Window

        MainForm.Width = screen_size.x
        MainForm.Height = screen_size.y
        'MainForm.TopMost = True
        'MainForm.FormBorderStyle = FormBorderStyle.FixedSingle

        MainForm.Show()
        MainForm.Left = 0
        MainForm.Top = 0

        'Create D3D Device Paramaters
        Dim D3D_PP As New PresentParameters
        D3D_PP.BackBufferFormat = Format.X8R8G8B8
        D3D_PP.BackBufferWidth = screen_size.x
        D3D_PP.BackBufferHeight = screen_size.y
        D3D_PP.Windowed = windowed
        D3D_PP.SwapEffect = SwapEffect.Discard
        D3D_PP.BackBufferCount = 1
        D3D_PP.PresentationInterval = PresentInterval.Immediate
        'Create D3D Device
        d3d_device = New Device(monitor, DeviceType.Hardware, MainForm.Handle, CreateFlags.MixedVertexProcessing, D3D_PP)
        'd3d_chain = New SwapChain(d3d_device, D3D_PP)
        'Create Sprite Object
        d3d_sprite = New Sprite(d3d_device)

        'd3d_device.GetSwapChain(0).DisplayMode


        Dim fontcollection As New Drawing.Text.PrivateFontCollection
        'Dim fontstream As IO.Stream
        'fontstream=Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(
        'Dim bytestream(CType(fontstream.Length, Integer)) As Byte




        'Dim pointer As IntPtr = Runtime.InteropServices.Marshal.AllocCoTaskMem(My.Resources.vanberger_stencil.Length)
        'Runtime.InteropServices.Marshal.Copy(My.Resources.vanberger_stencil, 0, pointer, My.Resources.vanberger_stencil.Length)

        'Dim font(My.Resources.CubicRefit.Length) As Byte
        'Array.Copy(My.Resources.CubicRefit, font, My.Resources.CubicRefit.Length)        

        'Dim pointer As IntPtr = Runtime.InteropServices.Marshal.AllocCoTaskMem(My.Resources.CubicRefit.Length)
        'Runtime.InteropServices.Marshal.Copy(My.Resources.CubicRefit, 0, pointer, My.Resources.CubicRefit.Length)

        'Dim text As String = Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString()

        'fontcollection.AddMemoryFont(pointer, My.Resources.CubicRefit.Length)



        'Runtime.InteropServices.Marshal.FreeCoTaskMem(pointer)

        'Dim myfont As new Drawing.Font( = New Drawing.Font(fontcollection.Families(0), 20, FontStyle.Regular)

        'Dim fontfam As FontFamily


        'fontcollection.AddFontFile("data/fonts/vanberger_stencil.ttf")
        'Dim myfont As New Drawing.Font(fontcollection.Families(0), 18, FontStyle.Bold)
        Dim myfont As New Drawing.Font("Ariel", 18, FontStyle.Regular)



        ReDim d3d_font(1)
        d3d_font(d3d_font_enum.Big_Button) = New Microsoft.DirectX.Direct3D.Font(d3d_device, myfont)

        myfont = New Drawing.Font("Ariel", 10, FontStyle.Regular)
        d3d_font(d3d_font_enum.SB_small) = New Microsoft.DirectX.Direct3D.Font(d3d_device, myfont)
        Nebula_VB = New VertexBuffer(GetType(CustomVertex.TransformedColored), 17, d3d_device, 0, CustomVertex.TransformedColored.Format, Pool.Managed)

        Planet_VB = New VertexBuffer(GetType(CustomVertex.PositionColored), 65, d3d_device, 0, CustomVertex.PositionColored.Format, Pool.Managed)
        Orbit_VB = New VertexBuffer(GetType(CustomVertex.PositionColored), 65, d3d_device, 0, CustomVertex.PositionColored.Format, Pool.Managed)
        Orbit2_VB = New VertexBuffer(GetType(CustomVertex.PositionColored), 65, d3d_device, 0, CustomVertex.PositionColored.Format, Pool.Managed)

        Dim ver(64) As CustomVertex.PositionColored
        Dim x, y As Single
        Dim theta As Single = 0
        For p = 0 To 64
            y = CSng(Math.Cos(theta)) ' * external_zoom
            x = CSng(Math.Sin(theta)) ' * external_zoom
            ver(p) = New CustomVertex.PositionColored(x, y, 0, Color.Gold.ToArgb)
            theta += CSng(PI / 32)
        Next

        Planet_VB.SetData(ver, 0, LockFlags.None)

        For p = 0 To 64
            ver(p).Color = Color.Green.ToArgb
        Next
        Orbit_VB.SetData(ver, 0, LockFlags.None)

        For p = 0 To 64
            ver(p).Color = Color.Silver.ToArgb
        Next
        Orbit2_VB.SetData(ver, 0, LockFlags.None)



        'set planet external textures
        For a = 0 To 15
            external_planet_texture(a) = New Texture(d3d_device, 2048, 2048, 0, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default)
        Next
    End Sub

    Sub Clean_Up()
        'test_texture.Dispose()
        'Ship_Texture.Dispose()
        'Minimap_Texture.Dispose()
        If d3d_sprite IsNot Nothing Then d3d_sprite.Dispose()
        If d3d_device IsNot Nothing Then d3d_device.Dispose()

        Erase tile_texture
        'mouse_texture.Dispose()
        Erase button_texture
        Erase tile_texture
        Erase device_tile_texture
        Erase character_texture
        Erase panel_texture
        Erase icon_texture



    End Sub

    'program entry point
    Sub main()
        Try
            load_config_file()
        Catch ex As Exception
            MsgBox("Failed To Load Config File" + ex.Message, MsgBoxStyle.OkOnly, "Error")
            Clean_Up()
            End
        End Try

        Try
            init_DX()
        Catch ex As Exception
            MsgBox("Failed To Initilise D3D " + ex.Message, MsgBoxStyle.OkOnly, "Error")
            Clean_Up()
            End
        End Try

        Try
            init_resources()
        Catch ex As Exception
            MsgBox("Failed To Initilise resources" + ex.Message, MsgBoxStyle.OkOnly, "Error")
            Clean_Up()
            End
        End Try


        Set_defaults()


        Cursor.Clip = MainForm.RectangleToScreen(MainForm.ClientRectangle)
        'Cursor.Position = New Point(MainForm.Location.X + screen_size.x, screen_size.y \ 2)

        d3d_device.SetCursorProperties(0, 0, Cursor1)
        generate()

        Select Case main_menu()
            Case 0
                current_view = current_view_enum.personal
                main_loop()
            Case 1
                new_ship_build_menu()
            Case 2
                current_view = current_view_enum.star_map
                main_loop()
            Case 3
                current_view = current_view_enum.ship_external
                main_loop()
            Case 4
                current_view = current_view_enum.Weapon_control
                main_loop()
            Case 5
                current_view = current_view_enum.personal_level_screen
                main_loop()
            Case 6
                'save_tech_tree()
        End Select


        Clean_Up()

    End Sub

    Sub lock_screen()

        If Screen_Lock = True Then
            Cursor.Clip = Nothing
            Screen_Lock = False
        Else
            Cursor.Clip = MainForm.RectangleToScreen(MainForm.ClientRectangle)
            Screen_Lock = True
        End If
    End Sub

    Sub Load_Menu_Items()
        Dim pos As PointI
        'External Menu
        External_Menu_Items.Add(External_menu_items_Enum.Menu, New Menu_button(button_texture_enum.ship_build__menu_button, New Rectangle(0, screen_size.y - 32, 32, 32), button_style.Both))
        External_Menu_Items.Add(External_menu_items_Enum.Menu_alert_setup, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(0, screen_size.y - 64, 80, 32), button_style.Both, Color.White, "Alert Setup", Color.Black, d3d_font_enum.SB_small, False))
        External_Menu_Items.Add(External_menu_items_Enum.Menu_Fighter_Control, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(0, screen_size.y - 96, 80, 32), button_style.Both, Color.White, "Fighter Control", Color.Black, d3d_font_enum.SB_small, False))
        External_Menu_Items.Add(External_menu_items_Enum.Menu_special_abilities, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(0, screen_size.y - 128, 80, 32), button_style.Both, Color.White, "Manuvers", Color.Black, d3d_font_enum.SB_small, False))
        External_Menu_Items.Add(External_menu_items_Enum.Menu_squad_setup, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(0, screen_size.y - 160, 80, 32), button_style.Both, Color.White, "Squads", Color.Black, d3d_font_enum.SB_small, False))
        External_Menu_Items.Add(External_menu_items_Enum.Menu_star_map, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(0, screen_size.y - 192, 80, 32), button_style.Both, Color.White, "Star Map", Color.Black, d3d_font_enum.SB_small, False))
        External_Menu_Items.Add(External_menu_items_Enum.Menu_weapon_control, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(0, screen_size.y - 224, 80, 32), button_style.Both, Color.White, "Weapon Control", Color.Black, d3d_font_enum.SB_small, False))



        'Weapon Control
        External_Menu_Items_Weapon_Control.Add(External_Weapon_Control_Enums.Control_Group_Panel, New Menu_button(button_texture_enum.ship_build__device_panel, New Rectangle(0, 32, 0, 0), button_style.DisplayOnly, Color.Blue))

        External_Menu_Items_Weapon_Control.Add(External_Weapon_Control_Enums.New_Control_Group, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(0, 32, 80, 32), button_style.Both, Color.Green, "New", Color.Black, d3d_font_enum.SB_small))
        External_Menu_Items_Weapon_Control.Add(External_Weapon_Control_Enums.Save, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(0, screen_size.y - 32, 80, 32), button_style.Both, Color.Green, "Save", Color.Black, d3d_font_enum.SB_small))
        External_Menu_Items_Weapon_Control.Add(External_Weapon_Control_Enums.Cancel, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(80, screen_size.y - 32, 80, 32), button_style.Both, Color.Red, "Cancel", Color.Black, d3d_font_enum.SB_small))


        'Personal Level
        pos.y = 120 - 23
        pos.x = 440
        Menu_Items_Personal_Level.Add(Personal_level_enums.Officer_ScrollUp, New Menu_button(button_texture_enum.PLV__Officer_ScrollUp, New Rectangle(pos.x, pos.y, 72, 16), button_style.Both, Color.White)) : pos.y += 23
        Menu_Items_Personal_Level.Add(Personal_level_enums.Officer_1, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 72, 108), button_style.DisplayOnly, Color.White)) : pos.y += 115
        Menu_Items_Personal_Level.Add(Personal_level_enums.Officer_2, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 72, 108), button_style.DisplayOnly, Color.White)) : pos.y += 115
        Menu_Items_Personal_Level.Add(Personal_level_enums.Officer_3, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 72, 108), button_style.DisplayOnly, Color.White)) : pos.y += 115
        Menu_Items_Personal_Level.Add(Personal_level_enums.Officer_4, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 72, 108), button_style.DisplayOnly, Color.White)) : pos.y += 115
        Menu_Items_Personal_Level.Add(Personal_level_enums.Officer_5, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 72, 108), button_style.DisplayOnly, Color.White)) : pos.y += 115
        Menu_Items_Personal_Level.Add(Personal_level_enums.Officer_ScrollDown, New Menu_button(button_texture_enum.PLV__Officer_ScrollDown, New Rectangle(pos.x, pos.y, 72, 16), button_style.Both, Color.White))


        pos.x = 640
        pos.y = 604
        Menu_Items_Personal_Level.Add(Personal_level_enums.Class_Engineer, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 96, 16), button_style.DisplayOnly, Color.White)) : pos.y += 32
        Menu_Items_Personal_Level.Add(Personal_level_enums.Class_Security, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 96, 16), button_style.DisplayOnly, Color.White)) : pos.y += 32
        Menu_Items_Personal_Level.Add(Personal_level_enums.Class_Scientist, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 96, 16), button_style.DisplayOnly, Color.White)) : pos.y += 32
        Menu_Items_Personal_Level.Add(Personal_level_enums.Class_Aviator, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 96, 16), button_style.DisplayOnly, Color.White)) : pos.y += 32




        pos.y = 604
        pos.x = 780
        Menu_Items_Personal_Level.Add(Personal_level_enums.Class_Scroll_Left, New Menu_button(button_texture_enum.PLV__Officer_ScrollLeft, New Rectangle(pos.x - 30, pos.y, 16, 108), button_style.Both, Color.White))
        Menu_Items_Personal_Level.Add(Personal_level_enums.Class_1, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 72, 108), button_style.DisplayOnly, Color.White)) : pos.x += 92
        Menu_Items_Personal_Level.Add(Personal_level_enums.Class_2, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 72, 108), button_style.DisplayOnly, Color.White)) : pos.x += 92
        Menu_Items_Personal_Level.Add(Personal_level_enums.Class_3, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 72, 108), button_style.DisplayOnly, Color.White)) : pos.x += 92
        Menu_Items_Personal_Level.Add(Personal_level_enums.Class_4, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 72, 108), button_style.DisplayOnly, Color.White)) : pos.x += 92
        Menu_Items_Personal_Level.Add(Personal_level_enums.Class_5, New Menu_button(button_texture_enum.Blank, New Rectangle(pos.x, pos.y, 72, 108), button_style.DisplayOnly, Color.White)) : pos.x += 92
        Menu_Items_Personal_Level.Add(Personal_level_enums.Class_Scroll_Right, New Menu_button(button_texture_enum.PLV__Officer_ScrollRight, New Rectangle(pos.x, pos.y, 16, 108), button_style.Both, Color.White))

    End Sub


    Sub change_to_view(ByVal view As current_view_enum)
        Dim ship As Ship = Ship_List(Officer_List(current_player).Location_ID)

        'If view = current_view_enum.ship_build Then
        'Build_ship = New Ship(-1,New PointD (0,0),ship_type_enum.steel 
        'ElseIf view = current_view_enum.new_ship_build Then


        'End If


        If view = current_view_enum.Weapon_control Then
            
        End If

        current_view = view
    End Sub





    Sub Load_View(ByVal View As current_view_enum)
        '        If View = current_view_enum.personal_level_screen Then
        If PLV__Officer_Scroll = 0 Then




        End If





    End Sub


    Sub check_near_planet(ByVal ship As Integer)
        Dim pos, planetpos As PointI

        Dim shipcenter As PointD = Ship_List(ship).Get_Relative_Pos
        'Dim shiprectPlanet As New Rectangle(Ship_List(ship).location.intX - 8192, Ship_List(ship).location.intY - 8192, 16384, 16384)
        'Dim shiprectMoon As New Rectangle(Ship_List(ship).location.intX - 4096, Ship_List(ship).location.intY - 4096, 8192, 8192)

        Ship_List(ship).orbiting = -1
        For Each planet In u.planets

            If planet.Value.orbits_planet = True Then
                'moons
                planetpos.x = Convert.ToInt32(u.stars(u.planets(planet.Value.orbit_point).orbit_point).location.x + u.planets(planet.Value.orbit_point).orbit_distance * Math.Cos((u.planets(planet.Value.orbit_point).theta * planet_theta_offset) * 0.017453292519943295))
                planetpos.y = Convert.ToInt32(u.stars(u.planets(planet.Value.orbit_point).orbit_point).location.y + u.planets(planet.Value.orbit_point).orbit_distance * Math.Sin((u.planets(planet.Value.orbit_point).theta * planet_theta_offset) * 0.017453292519943295))

                pos.x = Convert.ToInt32(planetpos.x + planet.Value.orbit_distance * Math.Cos((planet.Value.theta * planet_theta_offset) * 0.017453292519943295))
                pos.y = Convert.ToInt32(planetpos.y + planet.Value.orbit_distance * Math.Sin((planet.Value.theta * planet_theta_offset) * 0.017453292519943295))

                Dim distance As Double = Math.Sqrt(((shipcenter.x - pos.x) ^ 2) + ((shipcenter.y - pos.y) ^ 2))
                If distance < planet.Value.size.x * 32 Then
                    Near_planet = planet.Key
                    Ship_List(ship).orbiting = planet.Key
                End If

            Else
                'Planets
                pos.x = Convert.ToInt32(u.stars(planet.Value.orbit_point).location.x + planet.Value.orbit_distance * Math.Cos((planet.Value.theta * planet_theta_offset) * 0.017453292519943295))
                pos.y = Convert.ToInt32(u.stars(planet.Value.orbit_point).location.y + planet.Value.orbit_distance * Math.Sin((planet.Value.theta * planet_theta_offset) * 0.017453292519943295))

                Dim distance As Double = Math.Sqrt(((shipcenter.x - pos.x) ^ 2) + ((shipcenter.y - pos.y) ^ 2))
                If distance < planet.Value.size.x * 32 Then
                    Near_planet = planet.Key
                    Ship_List(ship).orbiting = planet.Key
                End If                

            End If
        Next
    End Sub


    Function Get_Officer_Texture(ByVal OfficerID As Integer) As Texture
        Dim Officer_Texture As Texture
        If Loaded_Officer_Textures.ContainsKey(OfficerID) Then
            Officer_Texture = Loaded_Officer_Textures(OfficerID)
        Else
            'Tile Size
            Officer_Texture = New Texture(d3d_device, 640, 32, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default)
            Render_Officer_Texture(OfficerID, Officer_Texture)
            Loaded_Officer_Textures.Add(OfficerID, Officer_Texture)
            Officer_Texture = Loaded_Officer_Textures(OfficerID)
        End If
        Return Officer_Texture
    End Function


    Sub main_loop()
        Dim ship1 As Ship = load_ship_schematic()
        Dim pos = New PointD(ship1.center_point.x * 32, ship1.center_point.y * 32)
        ship1.location = New PointD(0, 0)

        ship1.Refresh()
        ship1.Faction = 0
        Ship_List.Add(0, ship1)


        Dim ship2 As Ship = load_ship_schematic()
        ship2.location = New PointD(0, -2000)
        ship2.Refresh()
        Ship_List.Add(1, ship2)



        Dim planet1 As Planet = New Planet(planet_type_enum.Forest, New PointI(512, 512), 0, 50000, False, 0.5)
        planet1.populate()
        planet1.landed_ships.Add(0, New PointI(0, 0))
        Planet_List = u.planets
        u.planets.Remove(0)
        u.planets.Add(0, planet1)

        Add_Officer(0, New Officer(0, "Captian", Officer_location_enum.Planet, 0, pos, 1, 0.2, New Officer.sprite_list(character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head)))

        Officer_List(0).Officer_Classes.Add(New Officer_Class(Class_List_Enum.Mage, 0, 1))
        Officer_List(0).Officer_Classes.Add(New Officer_Class(Class_List_Enum.SpellSword, 0, 1))
        Officer_List(0).Officer_Classes.Add(New Officer_Class(Class_List_Enum.Shadow, 0, 1))
        Officer_List(0).Officer_Classes.Add(New Officer_Class(Class_List_Enum.Thief, 0, 1))
        Officer_List(0).Officer_Classes.Add(New Officer_Class(Class_List_Enum.Inventor, 0, 1))
        Officer_List(0).Officer_Classes.Add(New Officer_Class(Class_List_Enum.PlaceHolder, 20, 10))
        Officer_List(0).Officer_Classes.Add(New Officer_Class(Class_List_Enum.Eye_Of_The_Placeholder, 56, 1))



        Officer_List(0).Current_Class = Class_List_Enum.Mage

        Add_Officer(1, New Officer(0, "Skippy", Officer_location_enum.Ship, 0, pos, 1, 0.2, New Officer.sprite_list(0, 0)))
        Officer_List(1).Current_Class = Class_List_Enum.Engineer
        Add_Officer(2, New Officer(0, "Perpa", Officer_location_enum.Ship, 0, pos, 1, 0.2, New Officer.sprite_list(0, 0)))
        Officer_List(2).Current_Class = Class_List_Enum.Mage
        Add_Officer(3, New Officer(0, "Surpa", Officer_location_enum.Ship, 0, pos, 1, 0.2, New Officer.sprite_list(0, 0)))
        Officer_List(3).Current_Class = Class_List_Enum.Aviator
        Add_Officer(4, New Officer(0, "Kerpa", Officer_location_enum.Ship, 0, pos, 1, 0.2, New Officer.sprite_list(0, 0)))
        Officer_List(4).Current_Class = Class_List_Enum.Scientist
        Add_Officer(5, New Officer(0, "Kilki", Officer_location_enum.Ship, 0, pos, 1, 0.2, New Officer.sprite_list(0, 0)))
        Officer_List(5).Current_Class = Class_List_Enum.Security
        Add_Officer(6, New Officer(0, "Stan", Officer_location_enum.Ship, 0, pos, 1, 0.2, New Officer.sprite_list(0, 0)))
        Officer_List(6).Current_Class = Class_List_Enum.Mage
        Add_Officer(7, New Officer(0, "Vextorz", Officer_location_enum.Ship, 0, pos, 1, 0.2, New Officer.sprite_list(0, 0)))
        Officer_List(7).Current_Class = Class_List_Enum.Aviator
        Player_Data.Officer_List.Add(1)
        Player_Data.Officer_List.Add(2)
        Player_Data.Officer_List.Add(3)
        Player_Data.Officer_List.Add(4)
        Player_Data.Officer_List.Add(5)
        Player_Data.Officer_List.Add(6)
        Player_Data.Officer_List.Add(7)


        Officer_List(0).sprite.Head_SpriteSet = character_sprite_set_enum.Human_Renagade_1
        Officer_List(0).sprite.Torso_SpriteSet = character_sprite_set_enum.Human_Renagade_1
        Officer_List(0).sprite.Left_Arm_SpriteSet = character_sprite_set_enum.Human_Renagade_1
        Officer_List(0).sprite.Right_Arm_SpriteSet = character_sprite_set_enum.Human_Renagade_1
        Officer_List(0).sprite.Left_Leg_SpriteSet = character_sprite_set_enum.Human_Renagade_1
        Officer_List(0).sprite.Right_Leg_SpriteSet = character_sprite_set_enum.Human_Renagade_1


        current_player = 0
        'Officer_List.Add (0,
        'Do initial planet rotation
        For A = 0 To 10000
            planet_theta_offset += 0.1
        Next

        'current_selected_planet_view = 1

        'Dim render_start, render_end, time_current, cps, rate As Long


        'Reload_Officer_Textures()

        Dim FPS_start, loops As Long        
        render_time = Current_Time()
        'Logic_Time = render_time
        Dim Rendered As Boolean = False


        Do Until terminate
            'Get keys/mouse
            MainForm.getUI(pressedkeys, mouse_info)
            Ship_List(1).angular_velocity = 0.001
            'Logic
            'QueryPerformanceCounter(time_current)
            If Logic_Time > Logic_Ratio Then
                Logic_Time -= 1

                Select Case current_view
                    Case Is = current_view_enum.ship_internal
                        test_ui()
                    Case Is = current_view_enum.personal
                        Personal_UI(1)
                        For Each ship In Ship_List.Values
                            ship.DoEvents()
                        Next
                        For Each Planet In Planet_List.Values
                            Planet.DoEvents()
                        Next
                        update_Planet_Movements()
                        check_near_planet(current_selected_ship_view)


                    Case Is = current_view_enum.ship_external

                        For Each ship In Ship_List.Values
                            ship.DoEvents()
                        Next
                        External_UI()
                        update_Planet_Movements()
                        Handle_Projectiles()
                        check_near_planet(current_selected_ship_view)
                        Near_planet = 0

                    Case Is = current_view_enum.planet
                        For Each ship In Ship_List.Values
                            ship.run_crew_scrips()
                            ship.Process_Devices()
                        Next
                        Planet_UI()
                    Case Is = current_view_enum.star_map
                        update_Planet_Movements()
                        Starmap_UI()
                    Case Is = current_view_enum.Weapon_control
                        Weapon_Control_UI()
                    Case Is = current_view_enum.personal_level_screen
                        Personal_Level_UI()
                End Select
                'System.Threading.Thread.Sleep(60)                
            End If
            Logic_Duration = Current_Time() - time_current


            'Render
            QueryPerformanceCounter(time_current)
            If render_end + refresh_rate <= time_current + render_duration + Logic_Duration Then
                If render_end + render_duration <= time_current Then
                    QueryPerformanceCounter(render_start)

                    Select Case current_view
                        Case Is = current_view_enum.ship_internal
                            test_ui()
                        Case Is = current_view_enum.personal
                            render_personal(current_player)
                        Case Is = current_view_enum.ship_external
                            render_ship_external_new(Ship_List.Item(current_selected_ship_view))
                        Case Is = current_view_enum.planet
                            render_planetoid(u.planets(current_selected_planet_view))
                        Case Is = current_view_enum.star_map
                            render_starmap()
                        Case Is = current_view_enum.Weapon_control
                            render_Weapon_Control()
                        Case Is = current_view_enum.personal_level_screen
                            render_personal_level()
                    End Select
                    'System.Threading.Thread.Sleep(60)                    
                    QueryPerformanceCounter(render_end)
                    render_duration = render_end - render_start
                    loops += 1
                    Logic_Time += Logic_Ratio
                End If
            End If
            QueryPerformanceCounter(time_current)
            If pressedkeys.Contains(Keys.Escape) Then terminate = True
            If pressedkeys.Contains(Keys.F10) Then pressedkeys.Remove(Keys.F10) : lock_screen()

            Application.DoEvents()

            'FPS
            QueryPerformanceCounter(time_current)
            If FPS_start + cps <= time_current Then
                FPS = loops
                QueryPerformanceCounter(FPS_start)
                loops = 0
                'If FPS > 60 Then refresh_rate -= CLng(60 / FPS) * cps
                'If FPS < 60 Then refresh_rate += CLng(60 / FPS) * cps
            End If

        Loop
    End Sub

    Sub Add_Officer(ByVal Id As Integer, ByVal Officer As Officer)
        Officer_List.Add(Id, Officer)

        Select Case Officer.region
            Case Is = Officer_location_enum.Ship
                Ship_List(Officer.Location_ID).Officer_List.Add(Id, Officer)
            Case Is = Officer_location_enum.Planet
                Planet_List(Officer.Location_ID).officer_list.Add(Id, Officer)
        End Select

    End Sub


    Sub Add_Crew(ByVal Id As Integer, ByVal Crew As Crew)
        Crew_List.Add(Id, Crew)

        Select Case Crew.region
            Case Is = Officer_location_enum.Ship
                Ship_List(Crew.Location_ID).Crew_list.Add(Id, Crew)
            Case Is = Officer_location_enum.Planet
                Planet_List(Crew.Location_ID).Crew_List.Add(Id, Crew)
        End Select

    End Sub

    Sub Handle_Projectiles()
        Dim remove_List As List(Of Projectile) = New List(Of Projectile)
        For Each item In u.Projectiles
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



            For Each ship In Ship_List
                If ship.Key > 0 Then
                    Dim BoxSize As Integer
                    BoxSize = CInt(Math.Sqrt(ship.Value.GetShipSize.x ^ 2 + ship.Value.GetShipSize.y ^ 2))

                    Dim Ship_Rect As New Rectangle(ship.Value.location.PointI.ToPoint, New Size(BoxSize * 32, BoxSize * 32))

                    If Ship_Rect.Contains(item.Location.PointI.ToPoint) = True Then
                        Dim Relative_Pos As PointI
                        Dim contact_point As PointI

                        Relative_Pos.x = CInt(ship.Value.Get_Relative_Pos.x - item.Location.x)
                        Relative_Pos.y = CInt(ship.Value.Get_Relative_Pos.y - item.Location.y)

                        Dim center_point As PointD = ship.Value.Get_Relative_Pos
                        Dim Cos As Double = Math.Cos(-ship.Value.rotation)
                        Dim Sin As Double = Math.Sin(-ship.Value.rotation)

                        contact_point.x = CInt(Cos * (item.Location.x - ship.Value.Get_Relative_Pos.x) - Sin * (item.Location.y - ship.Value.Get_Relative_Pos.y) + ship.Value.Get_Center_Point.x) \ 32
                        contact_point.y = CInt(Sin * (item.Location.x - ship.Value.Get_Relative_Pos.x) + Cos * (item.Location.y - ship.Value.Get_Relative_Pos.y) + ship.Value.Get_Center_Point.y) \ 32

                        If contact_point.x >= 0 AndAlso contact_point.x <= ship.Value.shipsize.x AndAlso contact_point.y >= 0 AndAlso contact_point.y <= ship.Value.shipsize.y Then
                            If ship.Value.tile_map(contact_point.x, contact_point.y).type < tile_type_enum.empty Then
                                Detonate_Projectile(ship.Value, item, contact_point)
                                remove_List.Add(item)
                            End If
                        End If
                    End If
                End If

            Next


        Next

        For Each item In remove_List
            u.Projectiles.Remove(item)
        Next

    End Sub


    Sub Detonate_Projectile(ByVal ship As Ship, ByVal Pro As Projectile, ByVal contact_point As PointI)

        If Not Pro.Damage > ship.tile_map(contact_point.x, contact_point.y).integrity Then
            ship.tile_map(contact_point.x, contact_point.y).integrity -= CByte(Pro.Damage)
        Else
            ship.tile_map(contact_point.x, contact_point.y).integrity = 0
        End If

        If ship.tile_map(contact_point.x, contact_point.y).integrity = 0 Then ship.tile_map(contact_point.x, contact_point.y).type = tile_type_enum.empty



    End Sub







    Sub Activate_Ability(ByVal player As Integer, ByVal Ability As Ability_List_Enum)
        Select Case Ability
            Case Is = Ability_List_Enum.Mage__Fireball
                Dim start_Point As PointD = Officer_List(player).GetLocationD
                Dim Vector_Velocity As PointD
                Dim Rotation As Double
                start_Point.x += 15 : start_Point.y += 15
                Select Case Officer_List(player).input_flages.Facing
                    Case Is = Move_Direction.Up
                        Vector_Velocity = New PointD(0, -12) : Rotation = 0
                    Case Is = Move_Direction.Down
                        Vector_Velocity = New PointD(0, 12) : Rotation = PI
                    Case Is = Move_Direction.Left
                        Vector_Velocity = New PointD(-12, 0) : Rotation = PI * 1.5
                    Case Is = Move_Direction.Right
                        Vector_Velocity = New PointD(12, 0) : Rotation = PI * 0.5
                End Select
                Select Case Officer_List(player).region
                    Case Is = Officer_location_enum.Planet
                        Planet_List(Officer_List(player).Location_ID).Projectiles.Add(New Projectile(start_Point, Vector_Velocity, Rotation, 500))
                    Case Is = Officer_location_enum.Ship
                        Ship_List(Officer_List(player).Location_ID).Projectiles.Add(New Projectile(start_Point, New PointD(12, 0), PI * 0.5, 500))
                    Case Is = Officer_location_enum.Enemy_Ship                        
                End Select


        End Select
    End Sub




#Region "UI"
    Sub Personal_UI(ByVal rate As Double)

        Dim amount As Double = 1
        If pressedkeys.Contains(Keys.ShiftKey) Then amount = 50
        If pressedkeys.Contains(Keys.Z) Then personal_zoom = personal_zoom + 1 : pressedkeys.Remove(Keys.Z)

        If pressedkeys.Contains(Keys.Space) Then
            'Dim pos As PointD = New PointD(Ship_List(0).center_point.x * 32, Ship_List(0).center_point.y * 32)
            Dim pos As New PointD(16 * 32, 16 * 32)
            Dim id As Integer
            For id = 0 To 500
                If Not Crew_List.ContainsKey(id) Then Exit For
            Next
            Add_Crew(id, New Crew(1, pos, 0, Officer_location_enum.Planet, 1, character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, New crew_resource_type(10, 10)))
            'Ship_List(0).Crew_list.Add(id, New Crew(0, pos, 1, character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, New crew_resource_type(10, 10)))
            'ship1.Crew_list.Add(0, New Crew(pos, 1, 100, character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, New crew_resource_type(10, 10)))
            pressedkeys.Remove(Keys.Space)
        End If


        If pressedkeys.Contains(Keys.ControlKey) Then
            Activate_Ability(current_player, Ability_List_Enum.Mage__Fireball)
            pressedkeys.Remove(Keys.ControlKey)
        End If


        If pressedkeys.Contains(Keys.G) Then
            If Officer_List(current_player).Health.Torso > 0 Then Officer_List(current_player).Health.Torso -= CByte(1)
            pressedkeys.Remove(Keys.G)
        End If


        If pressedkeys.Contains(Keys.X) Then personal_zoom = personal_zoom - 1 : pressedkeys.Remove(Keys.X)

        If mouse_info.wheel > 0 Then
            personal_zoom += CInt(Math.Ceiling((mouse_info.wheel / 240) * personal_zoom))
        ElseIf mouse_info.wheel < 0 Then
            personal_zoom += CInt(Math.Ceiling((mouse_info.wheel / 240) * personal_zoom))
        End If
        mouse_info.wheel = 0

        If personal_zoom < 1 Then personal_zoom = 1
        If personal_zoom > 20 Then personal_zoom = 20
        If pressedkeys.Contains(Keys.ShiftKey) Then personal_zoom = 0.125
        'If (pressedkeys.Contains(Keys.W) AndAlso pressedkeys.Contains(Keys.A)) OrElse (pressedkeys.Contains(Keys.W) AndAlso pressedkeys.Contains(Keys.D)) Then amount = Convert.ToSingle(amount * DIAGONAL_SPEED_MODIFIER)
        'If (pressedkeys.Contains(Keys.S) AndAlso pressedkeys.Contains(Keys.A)) OrElse (pressedkeys.Contains(Keys.S) AndAlso pressedkeys.Contains(Keys.D)) Then amount = Convert.ToSingle(amount * DIAGONAL_SPEED_MODIFIER)


        Dim Input_flage As Officer.officer_input_flages = Officer_List(current_player).input_flages
        Input_flage.walking = False
        Input_flage.MoveX = Move_Direction.None
        Input_flage.MoveY = Move_Direction.None
        If pressedkeys.Contains(Keys.W) Then Input_flage.MoveY = Move_Direction.Up : Input_flage.walking = True
        If pressedkeys.Contains(Keys.S) Then Input_flage.MoveY = Move_Direction.Down : Input_flage.walking = True

        If pressedkeys.Contains(Keys.A) Then Input_flage.MoveX = Move_Direction.Left : Input_flage.walking = True
        If pressedkeys.Contains(Keys.D) Then Input_flage.MoveX = Move_Direction.Right : Input_flage.walking = True

        If pressedkeys.Contains(Keys.A) AndAlso pressedkeys.Contains(Keys.D) Then Input_flage.MoveX = Move_Direction.None
        If pressedkeys.Contains(Keys.W) AndAlso pressedkeys.Contains(Keys.S) Then Input_flage.MoveY = Move_Direction.None


        If Input_flage.MoveX = Move_Direction.None AndAlso Input_flage.MoveY > Move_Direction.None Then Input_flage.Facing = Input_flage.MoveY
        If Input_flage.MoveY = Move_Direction.None AndAlso Input_flage.MoveX > Move_Direction.None Then Input_flage.Facing = Input_flage.MoveX



        If Officer_List(current_player).region = Officer_location_enum.Ship Then

            view_location_personal.x = Ship_List(current_selected_ship_view).GetOfficer.Item(0).GetLocationD.x + 16 - ((screen_size.x / 2) / personal_zoom)
            view_location_personal.y = Ship_List(current_selected_ship_view).GetOfficer.Item(0).GetLocationD.y + 16 - ((screen_size.y / 2) / personal_zoom)


            If view_location_personal.x < -(screen_size.x / 2) Then view_location_personal.x = -(screen_size.x \ 2)
            'If view_location_personal.x > Ship_List.Item(current_selected_ship_view).GetShipSize.x * 32 * personal_zoom - screen_size.x / 2 Then view_location_personal.x = CInt(Ship_List.Item(current_selected_ship_view).GetShipSize.x * 32 * personal_zoom) - screen_size.x / 2

            If view_location_personal.y < -(screen_size.y / 2) Then view_location_personal.y = -(screen_size.y \ 2)
            If view_location_personal.y > Ship_List.Item(current_selected_ship_view).GetShipSize.y * 32 * personal_zoom - screen_size.y / 2 Then view_location_personal.y = CInt(Ship_List.Item(current_selected_ship_view).GetShipSize.y * 32 * personal_zoom) - screen_size.y / 2
        End If

        If Officer_List(current_player).region = Officer_location_enum.Planet Then
            'have to be separate

            'If pressedkeys.Contains(Keys.W) Then Planet_List(Officer_List(current_player).Location_ID).MoveOfficer(0, New PointD(0, -amount * rate))
            'If pressedkeys.Contains(Keys.S) Then Planet_List(Officer_List(current_player).Location_ID).MoveOfficer(0, New PointD(0, amount * rate))
            'If pressedkeys.Contains(Keys.A) Then Planet_List(Officer_List(current_player).Location_ID).MoveOfficer(0, New PointD(-amount * rate, 0))
            'If pressedkeys.Contains(Keys.D) Then Planet_List(Officer_List(current_player).Location_ID).MoveOfficer(0, New PointD(amount * rate, 0))

            'Planet_List(Officer_List(current_player).Location_ID).MoveOfficer(0, New PointD(amount * rate, 0))
            'Planet_List(Officer_List(1).Location_ID).MoveOfficer(1, New PointD(amount, 0))

            'If pressedkeys.Contains(Keys.W) Then view_location_personal.y -= 1 'CInt(3 * rate)
            'If pressedkeys.Contains(Keys.S) Then view_location_personal.y += 1 'CInt(3 * rate)
            'If pressedkeys.Contains(Keys.A) Then view_location_personal.x -= 1 'CInt(3 * rate)
            'If pressedkeys.Contains(Keys.D) Then view_location_personal.x += 1 'CInt(3 * rate)

            view_location_personal_Last = view_location_personal

            view_location_personal.x = Officer_List(current_player).GetLocationD.x + 16 - CInt((screen_size.x / 2) / personal_zoom)
            view_location_personal.y = Officer_List(current_player).GetLocationD.y + 16 - CInt((screen_size.y / 2) / personal_zoom)
        End If

        If pressedkeys.Contains(Keys.Tab) Then current_view = current_view_enum.ship_external : pressedkeys.Remove(Keys.Tab)
        Mouse_Target = -1
        For Each Crew In Ship_List(current_selected_ship_view).Crew_list
            Dim adj_mouse As PointI = New PointI(mouse_info.position.x / personal_zoom + view_location_personal.x, mouse_info.position.y / personal_zoom + view_location_personal.y)
            If Crew.Value.find_rect.Contains(adj_mouse.ToPoint) Then Mouse_Target = Crew.Key
        Next

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
        Dim choice As Integer = -1
        Dim ship As Ship = Ship_List(current_selected_ship_view)

        If mouse_info.get_left_click(left_down_point, left_release_point) Then
            mouse_click = True
            For Each Button In External_Menu_Items
                If Button.Value.enabled = True Then
                    If Button.Value.bounds.Contains(left_down_point.x, left_down_point.y) And Button.Value.bounds.Contains(left_release_point.x, left_release_point.y) Then
                        choice = Button.Key
                    End If
                End If
            Next
        End If

        Button_Selection(External_Menu_Items, mouse_info)
        If mouse_click = True Then External_Menu_Open = False

        Select Case choice
            Case Is = External_menu_items_Enum.Menu
                If External_Menu_Open = True Then External_Menu_Open = False Else External_Menu_Open = True

            Case Is = External_menu_items_Enum.Menu_weapon_control
                change_to_view(current_view_enum.Weapon_control)



            Case 1000 To 1999
                For Each weapon In ship.Weapon_control_groups(choice - 1000).Connected_Weapons
                    ship.Fire_Weapon(weapon)
                Next



        End Select



        If pressedkeys.Contains(Keys.D1) Then
            For Each weapon In ship.Weapon_control_groups(0).Connected_Weapons
                ship.Fire_Weapon(weapon)
            Next
        End If



        Dim amount As Double = 1
        'Dim right_click_location As PointI
        Dim atsize As Integer = Convert.ToInt32(32 * internal_zoom)


        If pressedkeys.Contains(Keys.Z) Then external_zoom -= 0.1F : pressedkeys.Remove(Keys.Z)
        If pressedkeys.Contains(Keys.X) Then external_zoom += 0.1F : pressedkeys.Remove(Keys.X)

        If mouse_info.wheel > 0 Then
            'external_zoom_End += (mouse_info.wheel / 240) * external_zoom
            external_zoom += CSng(mouse_info.wheel / 240) * external_zoom
        ElseIf mouse_info.wheel < 0 Then
            'external_zoom_End += (mouse_info.wheel / 240) * external_zoom
            external_zoom += CSng(mouse_info.wheel / 240) * external_zoom
        End If
        mouse_info.wheel = 0
        Math.Round(external_zoom, 2)
        If external_zoom > 5 Then external_zoom = 5
        If external_zoom < 0.0000001 Then external_zoom = 0.0000001

        'Dim dest_point, dest_release_point As PointI

        If mouse_info.right_down = True Then
            Ship_List(current_selected_ship_view).NavControl = True
            Dim delta As PointD
            delta.x = screen_size.x / 2 - mouse_info.position.x
            delta.y = screen_size.y / 2 - mouse_info.position.y
            Dim rot As Double = Math.Atan2(delta.y, delta.x) - 1.57079633
            If rot < 0 Then rot += PI * 2
            'If rot < -PI Then rot += PI * 2
            'Ships(current_selected_ship_view).target_rotation = rot
            Ship_List(current_selected_ship_view).angular_velocity = 0
            Ship_List(current_selected_ship_view).rotation = 0
            rot = PI / 2
            Ship_List(current_selected_ship_view).SetFullTurn(rot)
            mouse_info.right_down = False
        End If


        If pressedkeys.Contains(Keys.T) Then
            'Ships(current_selected_ship_view).SetFullTurn(PI / 2)
            'Ship_List(current_selected_ship_view).angular_velocity = 0.1
            'Ship_List(current_selected_ship_view).NavControl = False
        End If



        'If pressedkeys.Contains(Keys.W) Then Ships(0).Apply_Force(10, New PointD(13, 18), Direction_Enum.Bottom)
        'If pressedkeys.Contains(Keys.S) Then Ships(0).Apply_Force(10, New PointD(15, 18), Direction_Enum.Top)
        'If pressedkeys.Contains(Keys.A) Then Ships(0).Apply_Force(0.01, New PointD(15, 18), Direction_Enum.Left)
        'If pressedkeys.Contains(Keys.D) Then Ships(0).Apply_Force(0.01, New PointD(15, 18), Direction_Enum.Right)

        Dim percent As Double = 0.01

        If Ship_List(current_selected_ship_view).NavControl = False Then

            If pressedkeys.Contains(Keys.ShiftKey) Then percent = -0.01

            If pressedkeys.Contains(Keys.Q) Then
                For Each Device In Ship_List(current_selected_ship_view).Engine_Coltrol_Group(Direction_Enum.Right)
                    'Device.Value.
                    'If Ship_List(current_selected_ship_view).device_list(Device.Key).type = device_type_enum.thruster Then
                    Ship_List(current_selected_ship_view).Set_Engine_Throttle(Device.Key, percent, True)
                    'End If
                Next
            End If

            If pressedkeys.Contains(Keys.E) Then
                For Each Device In Ship_List(current_selected_ship_view).Engine_Coltrol_Group(Direction_Enum.Left)
                    'If Ship_List(current_selected_ship_view).device_list(Device.Key).type = device_type_enum.thruster Then
                    Ship_List(current_selected_ship_view).Set_Engine_Throttle(Device.Key, percent, True)
                    'End If
                Next
            End If


            If pressedkeys.Contains(Keys.W) Then percent = 1 Else percent = 0
            For Each Device In Ship_List(current_selected_ship_view).Engine_Coltrol_Group(Direction_Enum.Bottom)
                Ship_List(current_selected_ship_view).Set_Engine_Throttle(Device.Key, percent)
            Next


            If pressedkeys.Contains(Keys.S) Then percent = 1 Else percent = 0
            For Each Device In Ship_List(current_selected_ship_view).Engine_Coltrol_Group(Direction_Enum.Top)
                Ship_List(current_selected_ship_view).Set_Engine_Throttle(Device.Key, percent)
            Next


            If pressedkeys.Contains(Keys.A) Then percent = 1 Else percent = 0
            For Each Device In Ship_List(current_selected_ship_view).Engine_Coltrol_Group(Direction_Enum.RotateL)
                If Ship_List(current_selected_ship_view).device_list(Device.Key).type = device_type_enum.thruster Then
                    Ship_List(current_selected_ship_view).Set_Engine_Throttle(Device.Key, percent)
                End If
            Next


            If pressedkeys.Contains(Keys.D) Then percent = 1 Else percent = 0
            For Each Device In Ship_List(current_selected_ship_view).Engine_Coltrol_Group(Direction_Enum.RotateR)
                If Ship_List(current_selected_ship_view).device_list(Device.Key).type = device_type_enum.thruster Then
                    Ship_List(current_selected_ship_view).Set_Engine_Throttle(Device.Key, percent)
                End If
            Next


            If pressedkeys.Contains(Keys.X) Then
                For Each group In Ship_List(current_selected_ship_view).Engine_Coltrol_Group
                    For Each Device In group.Value
                        Ship_List(current_selected_ship_view).Set_Engine_Throttle(Device.Key, 0)
                    Next
                Next
            End If

        End If

        'Ships(current_selected_ship_view).rotation = Ships(current_selected_ship_view).destanation_rotation

        If pressedkeys.Contains(Keys.Tab) Then current_view = current_view_enum.personal : pressedkeys.Remove(Keys.Tab)


        'If Ship_List(current_selected_ship_view).angular_velocity > 0.005 Then Ship_List(current_selected_ship_view).angular_velocity = 0.005
        'If Ship_List(current_selected_ship_view).angular_velocity < -0.005 Then Ship_List(current_selected_ship_view).angular_velocity = -0.005



        'open/close menus/buttons
        External_Menu_Items(External_menu_items_Enum.Menu_alert_setup).enabled = External_Menu_Open
        External_Menu_Items(External_menu_items_Enum.Menu_Fighter_Control).enabled = External_Menu_Open
        External_Menu_Items(External_menu_items_Enum.Menu_special_abilities).enabled = External_Menu_Open
        External_Menu_Items(External_menu_items_Enum.Menu_squad_setup).enabled = External_Menu_Open
        External_Menu_Items(External_menu_items_Enum.Menu_star_map).enabled = External_Menu_Open
        External_Menu_Items(External_menu_items_Enum.Menu_weapon_control).enabled = External_Menu_Open



        If External__Reload_Menu = True Then

            Dim keys(External_Menu_Items.Count) As Integer
            External_Menu_Items.Keys.CopyTo(keys, 0)
            For Each item In keys
                If item >= 1000 Then
                    External_Menu_Items.Remove(item)
                End If
            Next
            'Add menu items
            Dim x As Integer = screen_size.x \ 2 - 80
            Dim count As Integer = 1000

            For Each item In ship.Weapon_control_groups
                External_Menu_Items.Add(count, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(x, screen_size.y - 32, 80, 32), button_style.Both, Color.White, item.Value.Name, Color.Black, d3d_font_enum.SB_small))
                x += 80
                count += 1
            Next

            External__Reload_Menu = False
        End If




    End Sub

    Sub Weapon_Control_UI()

        Dim ship As Ship = Ship_List(Officer_List(current_player).Location_ID)
        Dim mouse_info As New MainForm.mouse_info_type
        Dim mouse_click As Boolean
        Dim left_down_point As PointI = Nothing
        Dim left_release_point As PointI = Nothing
        MainForm.getUI(pressedkeys, mouse_info)
        Dim choice As Integer = -1

        If mouse_info.get_left_click(left_down_point, left_release_point) Then
            mouse_click = True
            For Each Button In External_Menu_Items_Weapon_Control
                If Button.Value.enabled = True Then
                    If Button.Value.bounds.Contains(left_down_point.x, left_down_point.y) And Button.Value.bounds.Contains(left_release_point.x, left_release_point.y) Then
                        choice = Button.Key
                    End If
                End If
            Next
        End If

        Button_Selection(External_Menu_Items_Weapon_Control, mouse_info)

        Select Case choice
            Case Is = External_Weapon_Control_Enums.Save
                External__Reload_Menu = True
                current_view = current_view_enum.ship_external
            Case Is = External_Weapon_Control_Enums.Cancel
                External__Reload_Menu = True
                current_view = current_view_enum.ship_external
            Case Is = External_Weapon_Control_Enums.New_Control_Group
                Dim id As Integer
                For id = 0 To 255
                    If Not Ship_List(current_selected_ship_view).Weapon_control_groups.ContainsKey(id) Then Exit For
                Next
                Ship_List(current_selected_ship_view).Weapon_control_groups.Add(id, New Weapon_control_group("Group 1"))
                Weapon_Control__Reload_Menu = True

            Case 1000 To 1999
                Weapon_Control__Selected_Group = choice - 1000
            Case 2000 To 3000

                For Each item In ship.Weapon_control_groups
                    If Not item.Key = Weapon_Control__Selected_Group AndAlso item.Value.Connected_Weapons.Contains(choice - 2000) Then item.Value.Connected_Weapons.Remove(choice - 2000)
                Next
                If Not ship.Weapon_control_groups(Weapon_Control__Selected_Group).Connected_Weapons.Contains(choice - 2000) Then
                    ship.Weapon_control_groups(Weapon_Control__Selected_Group).Connected_Weapons.Add(choice - 2000)
                Else
                    ship.Weapon_control_groups(Weapon_Control__Selected_Group).Connected_Weapons.Remove(choice - 2000)
                End If


        End Select


        Dim selection As PointI

        selection.x = Convert.ToInt32(view_location_weapon_control.x + mouse_info.position.x / Weapon_Control_zoom) \ 32
        selection.y = Convert.ToInt32(view_location_weapon_control.y + mouse_info.position.y / Weapon_Control_zoom) \ 32

        'Add Weapon to selected group
        If selection.x >= 0 AndAlso selection.x <= ship.shipsize.x AndAlso selection.y >= 0 AndAlso selection.y <= ship.shipsize.y Then
            If mouse_click = True Then
                If ship.tile_map(selection.x, selection.y).device_tile IsNot Nothing AndAlso ship.device_list(ship.tile_map(selection.x, selection.y).device_tile.device_ID).type = device_type_enum.weapon Then
                    If Weapon_Control__Selected_Group > -1 Then
                        'Remove from other control groups
                        For Each item In ship.Weapon_control_groups
                            If Not item.Key = Weapon_Control__Selected_Group AndAlso item.Value.Connected_Weapons.Contains(ship.tile_map(selection.x, selection.y).device_tile.device_ID) Then item.Value.Connected_Weapons.Remove(ship.tile_map(selection.x, selection.y).device_tile.device_ID)
                        Next
                        If Not ship.Weapon_control_groups(Weapon_Control__Selected_Group).Connected_Weapons.Contains(ship.tile_map(selection.x, selection.y).device_tile.device_ID) Then
                            ship.Weapon_control_groups(Weapon_Control__Selected_Group).Connected_Weapons.Add(ship.tile_map(selection.x, selection.y).device_tile.device_ID)
                        Else
                            ship.Weapon_control_groups(Weapon_Control__Selected_Group).Connected_Weapons.Remove(ship.tile_map(selection.x, selection.y).device_tile.device_ID)
                        End If
                    End If
                End If
            End If
        End If





        view_location_weapon_control.x = (ship.shipsize.x * 32) / 2 - screen_size.x / 2
        view_location_weapon_control.y = (ship.shipsize.y * 32) / 2 - screen_size.y / 2



        If Weapon_Control__Reload_Menu = True Then

            'Reset view location
            view_location_weapon_control.x = (ship.shipsize.x * 32) / 2 - screen_size.x / 2
            view_location_weapon_control.y = (ship.shipsize.y * 32) / 2 - screen_size.y / 2

            'Remove menu items
            Dim keys(External_Menu_Items_Weapon_Control.Count) As Integer
            External_Menu_Items_Weapon_Control.Keys.CopyTo(keys, 0)
            For Each item In keys
                If item >= 1000 Then
                    External_Menu_Items_Weapon_Control.Remove(item)
                End If
            Next
            'Add menu items
            Dim y As Integer = 64
            Dim count As Integer = 1000
            For Each item In ship.Weapon_control_groups
                External_Menu_Items_Weapon_Control.Add(count, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(16, y, 80, 32), button_style.Both, Color.White, item.Value.Name, Color.Black, d3d_font_enum.SB_small))
                y += 32
                count += 1
            Next


            y = 64
            count = 2000
            For Each item In ship.device_list
                If item.Value.type = device_type_enum.weapon Then
                    External_Menu_Items_Weapon_Control.Add(count, New Menu_button(button_texture_enum.ship_build__room_button, New Rectangle(screen_size.x - 80, y, 80, 32), button_style.Both, Color.White, item.Value.tech_ID.ToString, Color.Black, d3d_font_enum.SB_small))
                    y += 32
                End If
                count += 1
            Next

            Weapon_Control__Reload_Menu = False
        End If




        If Weapon_Control__Selected_Group > -1 Then
            If External_Menu_Items_Weapon_Control.ContainsKey(Weapon_Control__Selected_Group + 1000) Then
                External_Menu_Items_Weapon_Control(Weapon_Control__Selected_Group + 1000).hilight()
            End If
        End If

        If Weapon_Control__Selected_Group > -1 Then
            For a = 2000 To 2999
                If External_Menu_Items_Weapon_Control.ContainsKey(a) Then
                    If ship.Weapon_control_groups(Weapon_Control__Selected_Group).Connected_Weapons.Contains(a - 2000) Then
                        External_Menu_Items_Weapon_Control(a).hilight()
                    End If
                End If
            Next
        End If

    End Sub

    Sub Personal_Level_UI()

        Dim mouse_info As New MainForm.mouse_info_type
        Dim mouse_click As Boolean
        Dim left_down_point As PointI = Nothing
        Dim left_release_point As PointI = Nothing
        MainForm.getUI(pressedkeys, mouse_info)
        Dim choice As Integer = -1

        If mouse_info.get_left_click(left_down_point, left_release_point) Then
            mouse_click = True
            For Each Button In Menu_Items_Personal_Level
                If Button.Value.enabled = True Then
                    If Button.Value.bounds.Contains(left_down_point.x, left_down_point.y) And Button.Value.bounds.Contains(left_release_point.x, left_release_point.y) Then
                        choice = Button.Key
                    End If
                End If
            Next
        End If

        Button_Selection(Menu_Items_Personal_Level, mouse_info)

        Select Case choice
            Case Is = Personal_level_enums.Officer_ScrollUp
                If PLV__Officer_Scroll > 0 Then PLV__Officer_Scroll -= 1
            Case Is = Personal_level_enums.Officer_ScrollDown
                If PLV__Officer_Scroll < Player_Data.Officer_List.Count - 5 Then PLV__Officer_Scroll += 1
            Case Is = Personal_level_enums.Officer_1
                PLV__Selected_Officer = PLV__Officer_Scroll
                PLV__Selected_Class = Officer_List(PLV__Selected_Officer).Current_Class
                Officer_List(PLV__Selected_Officer).Recalculate_Abilities_Buffs()
            Case Is = Personal_level_enums.Officer_2
                PLV__Selected_Officer = PLV__Officer_Scroll + 1
                PLV__Selected_Class = Officer_List(PLV__Selected_Officer).Current_Class
                Officer_List(PLV__Selected_Officer).Recalculate_Abilities_Buffs()
            Case Is = Personal_level_enums.Officer_3
                PLV__Selected_Officer = PLV__Officer_Scroll + 2
                PLV__Selected_Class = Officer_List(PLV__Selected_Officer).Current_Class
                Officer_List(PLV__Selected_Officer).Recalculate_Abilities_Buffs()
            Case Is = Personal_level_enums.Officer_4
                PLV__Selected_Officer = PLV__Officer_Scroll + 3
                PLV__Selected_Class = Officer_List(PLV__Selected_Officer).Current_Class
                Officer_List(PLV__Selected_Officer).Recalculate_Abilities_Buffs()
            Case Is = Personal_level_enums.Officer_5
                PLV__Selected_Officer = PLV__Officer_Scroll + 4
                PLV__Selected_Class = Officer_List(PLV__Selected_Officer).Current_Class
                Officer_List(PLV__Selected_Officer).Recalculate_Abilities_Buffs()


            Case Is = Personal_level_enums.Class_Scroll_Left
                If PLV__Class_Scroll > 0 Then PLV__Class_Scroll -= 1
            Case Is = Personal_level_enums.Class_Scroll_Right
                If PLV__Class_Scroll < (Officer_List(PLV__Selected_Officer).Officer_Classes.Count - 4) - 5 Then PLV__Class_Scroll += 1

            Case Is = Personal_level_enums.Class_1
                If Officer_List(PLV__Selected_Officer).Officer_Classes.Count - 4 > 0 + PLV__Class_Scroll Then PLV__Selected_Class = Officer_List(PLV__Selected_Officer).GetClassByNumber(0 + PLV__Class_Scroll).ClassID
            Case Is = Personal_level_enums.Class_2
                If Officer_List(PLV__Selected_Officer).Officer_Classes.Count - 4 > 1 + PLV__Class_Scroll Then PLV__Selected_Class = Officer_List(PLV__Selected_Officer).GetClassByNumber(1 + PLV__Class_Scroll).ClassID                
            Case Is = Personal_level_enums.Class_3
                If Officer_List(PLV__Selected_Officer).Officer_Classes.Count - 4 > 2 + PLV__Class_Scroll Then PLV__Selected_Class = Officer_List(PLV__Selected_Officer).GetClassByNumber(2 + PLV__Class_Scroll).ClassID            
            Case Is = Personal_level_enums.Class_4
                If Officer_List(PLV__Selected_Officer).Officer_Classes.Count - 4 > 3 + PLV__Class_Scroll Then PLV__Selected_Class = Officer_List(PLV__Selected_Officer).GetClassByNumber(3 + PLV__Class_Scroll).ClassID            
            Case Is = Personal_level_enums.Class_5
                If Officer_List(PLV__Selected_Officer).Officer_Classes.Count - 4 > 4 + PLV__Class_Scroll Then PLV__Selected_Class = Officer_List(PLV__Selected_Officer).GetClassByNumber(4 + PLV__Class_Scroll).ClassID                


            Case Is = Personal_level_enums.Class_Engineer
                PLV__Selected_Class = Class_List_Enum.Engineer                
            Case Is = Personal_level_enums.Class_Security
                PLV__Selected_Class = Class_List_Enum.Security                
            Case Is = Personal_level_enums.Class_Scientist
                PLV__Selected_Class = Class_List_Enum.Scientist                
            Case Is = Personal_level_enums.Class_Aviator
                PLV__Selected_Class = Class_List_Enum.Aviator

            Case 999 To 1999
                Dim skill As Skill_Item = New Class_Tech_Tree(PLV__Selected_Class).Skills(CType(choice - 1000, Skill_List_Enum))
                If Officer_List(PLV__Selected_Officer).Officer_Classes(PLV__Selected_Class).Skill_Points >= skill.Cost Then
                    If skill.Parent = Skill_List_Enum.None OrElse Officer_List(PLV__Selected_Officer).Skill_List.Contains(skill.Parent) Then
                        Officer_List(PLV__Selected_Officer).Skill_List.Add(CType(choice - 1000, Skill_List_Enum))
                        Officer_List(PLV__Selected_Officer).Officer_Classes(PLV__Selected_Class).Skill_Points -= skill.Cost
                    End If
                End If

        End Select

        PLV_Load_Tech_Tree(PLV__Selected_Class)
        PLV_Set_Skills()



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



    Sub PLV_Load_Tech_Tree(ByVal Officer_Class As Class_List_Enum)
        If PLV__Selected_Class <> PLV__Selected_Class_Old OrElse PLV__Selected_Officer <> PLV__Selected_Officer_Old Then
            Dim PLV__Tech_Tree As Class_Tech_Tree = New Class_Tech_Tree(PLV__Selected_Class)

            Dim remove_list As New HashSet(Of Integer)
            For Each item In Menu_Items_Personal_Level
                If item.Key >= 1000 Then remove_list.Add(item.Key)
            Next
            For Each item In remove_list
                Menu_Items_Personal_Level.Remove(item)
            Next
            For Each item In PLV__Tech_Tree.Skills
                Menu_Items_Personal_Level.Add(1000 + item.Key, New Menu_button(button_texture_enum.PLV__Skill_Point, New Rectangle(740 + item.Value.Position.x * 100, 50 + item.Value.Position.y * 96, 32, 32), button_style.Both))
                If item.Value.Parent > Skill_List_Enum.None Then
                    Menu_Items_Personal_Level.Add(2000 + item.Value.Parent, New Menu_button(button_texture_enum.PLV__Skill_Bar, New Rectangle(740 + item.Value.Position.x * 100 + 12, 50 + item.Value.Position.y * 96 - 64, 8, 64), button_style.DisplayOnly))
                End If
                Menu_Items_Personal_Level(1000 + item.Key).tile = item.Value.Sprite
                If Officer_List(PLV__Selected_Officer).Skill_List.Contains(item.Key) Then Menu_Items_Personal_Level(1000 + item.Key).color = Color.FromArgb(255, 100, 100, 255)
            Next

            PLV__Selected_Class_Old = PLV__Selected_Class
            PLV__Selected_Officer_Old = PLV__Selected_Officer
        End If
    End Sub


    Sub PLV_Set_Skills()

        'Dim Tree As Class_Tech_Tree = New Class_Tech_Tree(PLV__Selected_Class)

        For Each item In Officer_List(PLV__Selected_Officer).Skill_List
            If item > -1 AndAlso Menu_Items_Personal_Level.ContainsKey(1000 + item) Then
                Menu_Items_Personal_Level(1000 + item).Adj_color = Color.FromArgb(255, 100, 100, 255)

                If Menu_Items_Personal_Level.ContainsKey(2000 + item) Then Menu_Items_Personal_Level(2000 + item).Adj_color = Color.FromArgb(255, 100, 100, 255)
            End If
        Next


        
        'If Officer_List(PLV__Selected_Officer).Skill_List.Contains(skill.Parent) Then
        'End If
        'If Menu_Items_Personal_Level.ContainsKey(2000 + item) Then Menu_Items_Personal_Level(2000 + item).Adj_color = Color.FromArgb(255, 100, 100, 255)



    End Sub


#End Region

    Sub update_Ship_Movements()


    End Sub

    Sub update_Planet_Movements()

        planet_theta_offset += 0.05
        planet_cloud_theta += 0.0001
    End Sub









    Public Function Get_Planet_Location(ByVal PlanetID As Integer, Optional ByVal ThetaOffset As Double = 0) As PointD
        Dim planet = Planet_List(PlanetID)
        Dim planetpos As PointD
        Dim pos As PointD
        If Planet_List(PlanetID).orbits_planet = True Then
            'moons
            planetpos.x = u.stars(u.planets(planet.orbit_point).orbit_point).location.x + u.planets(planet.orbit_point).orbit_distance * Math.Cos((u.planets(planet.orbit_point).theta * (planet_theta_offset + ThetaOffset)) * 0.017453292519943295)
            planetpos.y = u.stars(u.planets(planet.orbit_point).orbit_point).location.y + u.planets(planet.orbit_point).orbit_distance * Math.Sin((u.planets(planet.orbit_point).theta * (planet_theta_offset + ThetaOffset)) * 0.017453292519943295)
            pos.x = planetpos.x + planet.orbit_distance * Math.Cos((planet.theta * (planet_theta_offset + ThetaOffset)) * 0.017453292519943295)
            pos.y = planetpos.y + planet.orbit_distance * Math.Sin((planet.theta * (planet_theta_offset + ThetaOffset)) * 0.017453292519943295)
        Else
            'Planets
            pos.x = u.stars(planet.orbit_point).location.x + planet.orbit_distance * Math.Cos((planet.theta * (planet_theta_offset + ThetaOffset)) * 0.017453292519943295)
            pos.y = u.stars(planet.orbit_point).location.y + planet.orbit_distance * Math.Sin((planet.theta * (planet_theta_offset + ThetaOffset)) * 0.017453292519943295)
        End If
        Return pos
    End Function

    


    'Function distance(ByVal point1 As PointD, ByVal point2 As PointD) As Double
    'Dim result As Double
    '    result = Math.Sqrt(Math.Pow(point1.x - point2.x, 2) + Math.Pow(point1.y - point2.y, 2))
    'End Function

    Sub calculate_button_selection(ByRef menu_item As Menu_button(), ByVal mouse_info As MainForm.mouse_info_type)
        For Each button In menu_item
            'selection
            If button.enabled = True Then
                If button.bounds.Contains(mouse_info.position.x, mouse_info.position.y) Then
                    button.reset()
                    If mouse_info.left_down Then
                        If button.bounds.Contains(mouse_info.left_down_point.x, mouse_info.left_down_point.y) Then
                            ' button down on the original button
                            button.click()
                        Else
                            'button down but not on the original button
                            button.click()
                        End If
                    Else
                        ' mouse over
                        button.hilight()
                    End If
                Else
                    ' normal; no scrollover
                    button.reset()
                End If
            End If
        Next
    End Sub

    Public Function lerpPointD(ByVal startPoint As PointD, ByVal endPoint As PointD, ByVal percent As Double) As PointD
        Dim returnpoint As PointD
        returnpoint.x = (endPoint.x + percent * (startPoint.x - endPoint.x))
        returnpoint.y = (endPoint.y + percent * (startPoint.y - endPoint.y))

        'If returnpoint.x > endPoint.x Then returnpoint.x = endPoint.x
        'If returnpoint.y > endPoint.y Then returnpoint.y = endPoint.y

        'If returnpoint.x < startPoint.x Then returnpoint.x = startPoint.x
        'If returnpoint.y < startPoint.y Then returnpoint.y = startPoint.y

        Return returnpoint
    End Function



    Function Current_Time() As Long
        Dim Time As Long
        QueryPerformanceCounter(Time)
        Return Time
    End Function



    Public Sub Button_Selection(ByVal Menu_Items As Dictionary(Of Integer, Menu_button), ByVal mouse_info As MainForm.mouse_info_type)

        'Menu Items

        For Each button In Menu_Items.Values
            'selection
            If button.enabled = True Then
                If button.bounds.Contains(mouse_info.position.x, mouse_info.position.y) Then
                    button.reset()
                    If mouse_info.left_down Then
                        If button.bounds.Contains(mouse_info.left_down_point.x, mouse_info.left_down_point.y) Then
                            ' button down on the original button
                            button.click()
                        Else
                            'button down but not on the original button
                            button.click()
                        End If
                    Else
                        ' mouse over
                        button.hilight()
                    End If
                Else
                    ' normal; no scrollover
                    button.reset()
                End If
            End If
        Next


    End Sub




End Module