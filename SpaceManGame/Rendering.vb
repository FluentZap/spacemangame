Module Rendering

    Public shipexternal_redraw As Boolean

    Sub Draw_Ship_Tile(ByVal TileSet As Integer, ByVal Tile As Integer, ByVal Position As PointD, ByVal Color As Color)
        'd3d_sprite.Draw2D(tile_texture(TileSet), New Rectangle(Tile * 32, 0, 32, 32), New SizeF(atSize(), atSize()), New PointF(0, 0), 0, New PointF(Position.x, Position.y), Color)
        If TileSet < tile_type_enum.Device_Base OrElse TileSet = tile_type_enum.Restricted Then
            d3d_sprite.Draw(tile_texture(TileSet), New Rectangle(Tile * 32, 0, 32, 32), New Vector3(0, 0, 0), New Vector3(Position.intX, Position.intY, 0), Color.ToArgb)
        End If
    End Sub

    Sub Draw_Planet_Tile(ByVal TileSet As Integer, ByVal Tile As Integer, ByVal Position As PointD, ByVal Color As Color)
        'd3d_sprite.Draw2D(tile_texture(TileSet), New Rectangle(Tile * 32, 0, 32, 32), New SizeF(atSize(), atSize()), New PointF(0, 0), 0, New PointF(Position.x, Position.y), Color)
        If TileSet < tile_type_enum.Device_Base OrElse TileSet = tile_type_enum.Restricted Then
            d3d_sprite.Draw(planet_tile_texture(TileSet), New Rectangle(Tile * 32, 0, 32, 32), New Vector3(0, 0, 0), New Vector3(Position.sngX, Position.sngY, 0), Color)
        End If
    End Sub

    Sub Draw_Device_Tile(ByVal TileSet As device_tile_type_enum, ByVal Tile As Integer, ByVal Tile_Animation As Integer, ByVal Position As PointD, ByVal Rotation As rotate_enum, ByVal flip As flip_enum, ByVal scale As Single, ByVal Color As Color)
        If Tile >= 0 Then
            Dim matstore As Matrix = d3d_sprite.Transform
            Dim scalex, scaley As Single
            'd3d_sprite.Transform = Matrix.AffineTransformation2D(scale, New Vector2(16 * scale, 16 * scale), Geometry.DegreeToRadian(Rotation), New Vector2(Position.x * scale, Position.y * scale))
            'd3d_sprite.Transform = Matrix.AffineTransformation2D(scale, New Vector2(16 * scale, 16 * scale), Geometry.DegreeToRadian(Rotation), New Vector2(Position.x * scale, Position.y * scale))
            'If Rotation = rotate_enum.Ninty OrElse Rotation = rotate_enum.OneEighty Then Position.x += Convert.ToInt32(32 * scale)
            'If Rotation = rotate_enum.OneEighty OrElse Rotation = rotate_enum.TwoSeventy Then Position.y += Convert.ToInt32(32 * scale)
            'd3d_sprite.Transform = Matrix.Scaling(scalex, scaley, 0) * Matrix.RotationZ(Geometry.DegreeToRadian(Rotation)) * Matrix.Translation(Position.x, Position.y, 0)
            scalex = 1 : scaley = 1
            If flip = flip_enum.Flip_X OrElse flip = flip_enum.Both Then scalex = -1 : Position.x += 32
            If flip = flip_enum.Flip_Y OrElse flip = flip_enum.Both Then scaley = -1 : Position.y += 32


            Dim mat As Matrix = Matrix.Identity
            'mat = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scalex, scaley), New Vector2(16 * scalex, 16 * scaley), Geometry.DegreeToRadian(Rotation), New Vector2(CSng(Position.x * scale), CSng(Position.y * scale)))
            mat = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scalex, scaley), New Vector2(16 * scalex, 16 * scaley), Geometry.DegreeToRadian(Rotation), New Vector2(CSng(Position.x), CSng(Position.y)))


            'mat.RotateZ(Geometry.DegreeToRadian(Rotation))
            'mat.Scale(scale, scale, 1)
            'mat.Translate(Position.x, Position.y, 0)

            d3d_sprite.Transform = mat * matstore

            d3d_sprite.Draw(device_tile_texture(TileSet), New Rectangle(Tile * 32, Tile_Animation * 32, 32, 32), New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color)
            d3d_sprite.Transform = matstore
        End If
    End Sub


    Sub Draw_Character(ByVal TileSet As character_sprite_set_enum, ByVal Tile As character_sprite_enum, ByVal Position As PointI, ByVal Color As Color)
        d3d_sprite.Draw(character_texture(TileSet), New Rectangle(Tile * 32, 0, 32, 32), New Vector3(0, 0, 0), New Vector3(Position.x, Position.y, 0), Color)
    End Sub

    Sub Draw_Crew(ByVal TileSet As character_sprite_set_enum, ByVal Tile As character_sprite_enum, ByVal Position As PointD, ByVal Color As Color)
        d3d_sprite.Draw(character_texture(TileSet), New Rectangle(Tile * 32, 0, 32, 32), New Vector3(0, 0, 0), New Vector3(Position.sngX, Position.sngY, 0), Color)
    End Sub


    Sub Draw_Officer(ByVal Texture As Texture, ByVal Tile As character_sprite_enum, ByVal Position As PointD, ByVal Color As Color)
        d3d_sprite.Draw(Texture, New Rectangle(0, 0, 32, 32), New Vector3(0, 0, 0), New Vector3(Position.sngX, Position.sngY, 0), Color)
    End Sub




    Sub Draw_Button(ByVal TileSet As button_texture_enum, ByVal Tile As Integer, ByVal Position As Rectangle, ByVal Color As Color)
        'd3d_sprite.Draw2D(button_texture(TileSet), New Rectangle(Tile * 32, 0, 32, 32), New SizeF(atSize(), atSize()), New PointF(0, 0), 0, New PointF(Position.X, Position.Y), Color)
        d3d_sprite.Draw(button_texture(TileSet), New Rectangle(Tile * 32, 0, 32, 32), New Vector3(0, 0, 0), New Vector3(Position.X, Position.Y, 0), Color)
    End Sub

    Sub draw_sprite(ByVal sprite As Texture, ByVal position As Point, ByVal color As Color)
        d3d_sprite.Draw2D(sprite, New PointF(0, 0), 0, New PointF(position.X, position.Y), color)
    End Sub

    Sub draw_sprite(ByVal sprite As Texture, ByVal rect As Rectangle, ByVal size As SizeF, ByVal position As Point, ByVal color As Color)
        d3d_sprite.Draw2D(sprite, rect, size, New PointF(0, 0), 0, New PointF(position.X, position.Y), color)
    End Sub

    Sub draw_text(ByVal text As String, ByVal position As Point, ByVal color As Color, ByVal font_device As Direct3D.Font)
        font_device.DrawText(d3d_sprite, text, position.X, position.Y, color)
    End Sub

    Sub draw_text(ByVal text As String, ByVal position As Rectangle, ByVal format As DrawTextFormat, ByVal color As Color, ByVal font_device As Direct3D.Font)
        font_device.DrawText(d3d_sprite, text, position, format, color)
    End Sub

#Region "Personal View"

    Sub render_personal(ByVal player As Integer)


        'Dim view_location_personal As PointD = lerpPointD(MainModule.view_location_personal, view_location_personal_Last, Logic_Time - render_start \ logic_rate)
        'Rview_location_personal = view_location_personal
        Dim atsize As Integer = Convert.ToInt32(32 * personal_zoom)
        Dim scale As Single = CSng(personal_zoom)

        Dim pos As PointD
        d3d_device.BeginScene()
        d3d_device.Clear(ClearFlags.Target, Color.Blue, 1, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.Linear)
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)
        d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2(0, 0), 0, New Vector2(0, 0))

        'Render on ship
        If Officer_List(player).region = Officer_location_enum.Ship Then
            Dim ship As Ship = Ship_List(Officer_List(player).Location_ID)
            render_personal_ship(player)

            For Each officer In ship.Officer_List
                pos.x = officer.Value.GetLocation.x - view_location_personal.x
                pos.y = officer.Value.GetLocation.y - view_location_personal.y
                'Draw_Crew(character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, pos, Color.White)
                Draw_Officer(Get_Officer_Texture(officer.Key), character_sprite_enum.Head, pos, Color.White)
            Next

            For Each crew In ship.Crew_list
                pos.x = crew.Value.location.x - view_location_personal.x
                pos.y = crew.Value.location.y - view_location_personal.y                
                Draw_Crew(character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, pos, Color.White)
            Next
        End If

        'Render on planet
        If Officer_List(player).region = Officer_location_enum.Planet Then
            Dim planet As Planet = Planet_List(Officer_List(player).Location_ID)
            'For a = 0 To 100
            render_personal_planet(player)
            'Next a
            d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2(0, 0), 0, New Vector2(0, 0))
            For Each officer In planet.officer_list
                pos.x = officer.Value.GetLocationD.x - view_location_personal.x
                pos.y = officer.Value.GetLocationD.y - view_location_personal.y
                Draw_Officer(Get_Officer_Texture(officer.Key), character_sprite_enum.Head, pos, Color.White)
            Next

            For Each crew In planet.crew_list
                pos.x = crew.Value.location.x - view_location_personal.x
                pos.y = crew.Value.location.y - view_location_personal.y
                Draw_Crew(character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, pos, Color.White)
            Next
        End If

        d3d_sprite.Transform = Matrix.Identity
        draw_text("FPS " + FPS.ToString, New Rectangle(0, 0, 100, 20), CType(DrawTextFormat.Center + DrawTextFormat.VerticalCenter, DrawTextFormat), Color.White, d3d_font(d3d_font_enum.SB_small))

        d3d_sprite.End()
        d3d_device.EndScene()
        Try
            'd3d_device.GetSwapChain(0).Present(Present.DoNotWait)
            d3d_device.Present()
        Catch e As DeviceLostException
            DeviceLost = True
        End Try
        If DeviceLost = True Then AttemptRecovery()
    End Sub

    Sub render_personal_ship(ByVal player As Integer)
        Dim atsize As Integer = CInt(32 * personal_zoom)
        Dim scale As Single = CSng(Math.Round(personal_zoom, 2))
        Dim ship As Ship = Ship_List(Officer_List(player).Location_ID)
        Dim TileMap(,) As Ship_tile
        TileMap = ship.tile_map
        Dim pos As PointD
        For x = (CInt(view_location_personal.x * personal_zoom) \ atsize) - 1 To (CInt(view_location_personal.x * personal_zoom) \ atsize) + (screen_size.x \ atsize) + 1
            For y = (CInt(view_location_personal.y * personal_zoom) \ atsize) - 1 To (CInt(view_location_personal.y * personal_zoom) \ atsize) + (screen_size.y \ atsize) + 1
                pos.x = (x * 32) - view_location_personal.x
                pos.y = (y * 32) - view_location_personal.y
                If x >= 0 AndAlso x <= ship.shipsize.x AndAlso y >= 0 AndAlso y <= ship.shipsize.y Then
                    If TileMap(x, y).type < tile_type_enum.Restricted Then
                        Draw_Ship_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, Color.FromArgb(255, 255, 255, 255))
                        If TileMap(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, ship.device_list(TileMap(x, y).device_tile.device_ID).Sprite_Animation_Key, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                    End If
                End If
            Next
        Next
    End Sub

    Sub render_personal_planet(ByVal player As Integer)
        Dim atsize As Integer = Convert.ToInt32(32 * personal_zoom)
        Dim scale As Single = CSng(personal_zoom)
        Dim planet As Planet = Planet_List(Officer_List(player).Location_ID)
        Dim TileMap(,) As Planet_tile
        TileMap = planet.tile_map
        Dim pos As PointD
        Dim viewRect As Rectangle = New Rectangle(CInt(view_location_personal.x * personal_zoom) \ atsize, CInt(view_location_personal.y * personal_zoom) \ atsize, (screen_size.x \ atsize) + 1, (screen_size.y \ atsize) + 1)
        'For x = (CInt(view_location_personal.intX * personal_zoom) \ atsize) - 1 To (CInt(view_location_personal.x * personal_zoom) \ atsize) + (screen_size.x \ atsize) + 1
        'For y = (CInt(view_location_personal.intY * personal_zoom) \ atsize) - 1 To (CInt(view_location_personal.y * personal_zoom) \ atsize) + (screen_size.y \ atsize) + 1
        For x = viewRect.X To viewRect.Right
            For y = viewRect.Y To viewRect.Bottom

                pos.x = (x * 32) - view_location_personal.x
                pos.y = (y * 32) - view_location_personal.y
                If x >= 0 AndAlso x <= planet.size.x AndAlso y >= 0 AndAlso y <= planet.size.y Then
                    If TileMap(x, y).type < planet_tile_type_enum.empty Then
                        Draw_Planet_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, Color.FromArgb(255, 255, 255, 255))
                        'If TileMap(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                    End If
                End If
            Next
        Next


        For Each landed_ship In planet.landed_ships
            Dim ship As Ship = Ship_List(landed_ship.Key)

            If landed_ship.Value.x + ship.shipsize.x > viewRect.X AndAlso landed_ship.Value.x < viewRect.Right Then
                If landed_ship.Value.y + ship.shipsize.y > viewRect.Y AndAlso landed_ship.Value.y < viewRect.Bottom Then
                    Dim ship_tile_Map(,) As Ship_tile
                    ship_tile_Map = ship.tile_map

                    For x = (CInt(view_location_personal.x * personal_zoom) \ atsize) - 1 To (CInt(view_location_personal.x * personal_zoom) \ atsize) + (screen_size.x \ atsize) + 1
                        For y = (CInt(view_location_personal.y * personal_zoom) \ atsize) - 1 To (CInt(view_location_personal.y * personal_zoom) \ atsize) + (screen_size.y \ atsize) + 1
                            pos.x = (x * 32) - view_location_personal.x
                            pos.y = (y * 32) - view_location_personal.y
                            If x >= 0 AndAlso x <= ship.shipsize.x AndAlso y >= 0 AndAlso y <= ship.shipsize.y Then
                                If ship_tile_Map(x, y).type < tile_type_enum.Restricted Then
                                    Draw_Ship_Tile(ship_tile_Map(x, y).type, ship_tile_Map(x, y).sprite, pos, Color.FromArgb(100, 100, 100, 100))
                                    If ship_tile_Map(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(ship_tile_Map(x, y).device_tile.type, ship_tile_Map(x, y).device_tile.sprite, ship_tile_Map(x, y).device_tile.spriteAni, pos, ship_tile_Map(x, y).device_tile.rotate, ship_tile_Map(x, y).device_tile.flip, scale, Color.FromArgb(100, 100, 100, 100))
                                End If
                            End If
                        Next
                    Next

                End If
            End If

        Next

    End Sub


    Sub render_personal_planetS(ByVal player As Integer)
        Dim atsize As Integer = Convert.ToInt32(32 * personal_zoom)
        Dim scale As Single = CSng(personal_zoom)
        Dim planet As Planet = Planet_List(Officer_List(player).Location_ID)
        Dim TileMap(,) As Planet_tile
        TileMap = planet.tile_map
        Dim pos As PointD
        Dim viewRect As Rectangle = New Rectangle(CInt(view_location_personal.x * personal_zoom) \ atsize, CInt(view_location_personal.y * personal_zoom) \ atsize, (screen_size.x \ atsize) + 1, (screen_size.y \ atsize) + 1)
        'For x = (CInt(view_location_personal.intX * personal_zoom) \ atsize) - 1 To (CInt(view_location_personal.x * personal_zoom) \ atsize) + (screen_size.x \ atsize) + 1
        'For y = (CInt(view_location_personal.intY * personal_zoom) \ atsize) - 1 To (CInt(view_location_personal.y * personal_zoom) \ atsize) + (screen_size.y \ atsize) + 1
        d3d_sprite.Transform = Matrix.Identity
        For x = viewRect.X To viewRect.Right
            For y = viewRect.Y To viewRect.Bottom

                pos.x = (x * 32 * scale) - view_location_personal.x * scale
                pos.y = (y * 32 * scale) - view_location_personal.y * scale
                If x >= 0 AndAlso x <= planet.size.x AndAlso y >= 0 AndAlso y <= planet.size.y Then
                    If TileMap(x, y).type < tile_type_enum.Restricted Then
                        Draw_Planet_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, Color.FromArgb(255, 255, 255, 255))
                        'If TileMap(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                    End If
                End If
            Next
        Next


        For Each landed_ship In planet.landed_ships
            Dim ship As Ship = Ship_List(landed_ship.Key)

            If landed_ship.Value.x + ship.shipsize.x > viewRect.X AndAlso landed_ship.Value.x < viewRect.Right Then
                If landed_ship.Value.y + ship.shipsize.y > viewRect.Y AndAlso landed_ship.Value.y < viewRect.Bottom Then
                    Dim ship_tile_Map(,) As Ship_tile
                    ship_tile_Map = ship.tile_map

                    For x = (CInt(view_location_personal.x * personal_zoom) \ atsize) - 1 To (CInt(view_location_personal.x * personal_zoom) \ atsize) + (screen_size.x \ atsize) + 1
                        For y = (CInt(view_location_personal.y * personal_zoom) \ atsize) - 1 To (CInt(view_location_personal.y * personal_zoom) \ atsize) + (screen_size.y \ atsize) + 1
                            pos.x = (x * 32 * scale) - view_location_personal.x * scale
                            pos.y = (y * 32 * scale) - view_location_personal.y * scale
                            If x >= 0 AndAlso x <= ship.shipsize.x AndAlso y >= 0 AndAlso y <= ship.shipsize.y Then
                                If ship_tile_Map(x, y).type < tile_type_enum.Restricted Then
                                    Draw_Ship_Tile(ship_tile_Map(x, y).type, ship_tile_Map(x, y).sprite, pos, Color.FromArgb(255, 255, 255, 255))
                                    If ship_tile_Map(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(ship_tile_Map(x, y).device_tile.type, ship_tile_Map(x, y).device_tile.sprite, ship_tile_Map(x, y).device_tile.spriteAni, pos, ship_tile_Map(x, y).device_tile.rotate, ship_tile_Map(x, y).device_tile.flip, scale, Color.FromArgb(255, 255, 255, 255))
                                End If
                            End If
                        Next
                    Next

                End If
            End If

        Next

    End Sub

#End Region

    Sub render_personal_level()

        Dim pos As PointD
        Dim scale As Integer        
        d3d_device.BeginScene()        
        d3d_device.Clear(ClearFlags.Target, Color.Black, 1, 0)        
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)        
        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)
        d3d_sprite.Transform = Matrix.Identity

        pos.x = 440
        pos.y = 120
        Dim advance As Integer = 0
        Dim backcolor As Color = Color.White
        For Each item In Player_Data.Officer_List
            If advance >= PLV__Officer_Scroll Then
                draw_text(Officer_List(item).name, New Rectangle(pos.intX + 72, pos.intY, 100, 100), DrawTextFormat.Left, Color.White, d3d_font(d3d_font_enum.SB_small))
                draw_text("Lvl " + Officer_List(item).GetCurrentClass.Level.ToString, New Rectangle(pos.intX + 72, pos.intY + 12, 100, 100), DrawTextFormat.Left, Color.SkyBlue, d3d_font(d3d_font_enum.SB_small))
                draw_text("Exp " + Officer_List(item).GetCurrentClass.Experance.ToString + "/100", New Rectangle(pos.intX + 72, pos.intY + 24, 100, 100), DrawTextFormat.Left, Color.Green, d3d_font(d3d_font_enum.SB_small))
                draw_text(Officer_List(item).Current_Class.ToString, New Rectangle(pos.intX + 72, pos.intY + 36, 100, 100), DrawTextFormat.Left, Color.Orange, d3d_font(d3d_font_enum.SB_small))

                If PLV__Selected_Officer = item Then backcolor = Color.LightGreen Else backcolor = Color.White
                d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(3, 3), New Vector2(0, 0), 0, New Vector2(0, 0))
                d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Officer_Background), Vector3.Empty, New Vector3(CSng(pos.x / 3), CSng(pos.y / 3), 0), backcolor.ToArgb)
                'Draw_Crew(character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, New PointD(pos.x / 3 - 4, pos.y / 3 + 1), Color.White)

                Draw_Officer(Get_Officer_Texture(item), character_sprite_enum.Head, New PointD(pos.x / 3 - 4, pos.y / 3 + 1), Color.White)

                d3d_sprite.Transform = Matrix.Identity

                'Draw_Officer(Get_Officer_Texture(item), character_sprite_enum.Head, New PointD(0, 0), Color.White)

                pos.y += 115
                If advance - PLV__Officer_Scroll >= 4 Then Exit For
            End If
            advance += 1
        Next


        pos.x = 780
        pos.y = 604

        advance = 0
        backcolor = Color.White
        Dim Class_Color As Color
        Dim Class_Color_2 As Color
        For Each item In Officer_List(PLV__Selected_Officer).Officer_Classes
            If Not item.ClassID = Class_List_Enum.Engineer AndAlso Not item.ClassID = Class_List_Enum.Security AndAlso Not item.ClassID = Class_List_Enum.Scientist AndAlso Not item.ClassID = Class_List_Enum.Aviator Then
                If advance >= PLV__Class_Scroll Then

                    If item.Base_Class > Class_List_Enum.Empty Then
                        If item.Base_Class = Class_List_Enum.Engineer Then Class_Color = Color.Orange
                        If item.Base_Class = Class_List_Enum.Security Then Class_Color = Color.Red
                        If item.Base_Class = Class_List_Enum.Scientist Then Class_Color = Color.LightBlue
                        If item.Base_Class = Class_List_Enum.Aviator Then Class_Color = Color.Green

                        If item.Base_Class_2 > Class_List_Enum.Empty Then

                            If item.Base_Class_2 = Class_List_Enum.Engineer Then Class_Color_2 = Color.Orange
                            If item.Base_Class_2 = Class_List_Enum.Security Then Class_Color_2 = Color.Red
                            If item.Base_Class_2 = Class_List_Enum.Scientist Then Class_Color_2 = Color.LightBlue
                            If item.Base_Class_2 = Class_List_Enum.Aviator Then Class_Color_2 = Color.Green

                            d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Class_List_HalfFrame1), Vector3.Empty, New Vector3(pos.sngX - 9, pos.sngY - 24, 0), Class_Color.ToArgb)
                            d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Class_List_HalfFrame2), Vector3.Empty, New Vector3(pos.sngX - 9, pos.sngY - 24, 0), Class_Color_2.ToArgb)
                        Else
                            d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Class_List_Frame), Vector3.Empty, New Vector3(pos.sngX - 9, pos.sngY - 24, 0), Class_Color.ToArgb)
                        End If
                    Else
                        d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Class_List_Frame), Vector3.Empty, New Vector3(pos.sngX - 9, pos.sngY - 24, 0), Color.Gray.ToArgb)
                    End If




                    draw_text(item.ClassID.ToString, New Rectangle(pos.intX, pos.intY - 18, 72, 100), DrawTextFormat.Left, Color.Black, d3d_font(d3d_font_enum.SB_small))
                    draw_text("Lvl " + item.Level.ToString, New Rectangle(pos.intX, pos.intY + 108, 100, 100), DrawTextFormat.Left, Color.Black, d3d_font(d3d_font_enum.SB_small))
                    If item.Skill_Points > 0 Then d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Level_Up), Vector3.Empty, New Vector3(pos.sngX + 50, pos.sngY + 109, 0), Color.White.ToArgb)
                    d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Experience), Vector3.Empty, New Vector3(pos.sngX, pos.sngY + 124, 0), Color.White.ToArgb)


                    scale = CInt(72 * item.Experance \ 100)
                    d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Experience), New Rectangle(0, 0, scale, 8), New Vector3(0, 0, 0), New Vector3(pos.sngX, pos.sngY + 124, 0), Color.Cyan.ToArgb)

                    If PLV__Selected_Class = item.ClassID Then backcolor = Color.LightGreen Else backcolor = Color.White
                    d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(3, 3), New Vector2(0, 0), 0, New Vector2(0, 0))
                    d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Officer_Background), Vector3.Empty, New Vector3(CSng(pos.x / 3), CSng(pos.y / 3), 0), backcolor.ToArgb)

                    d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Class_List), New Rectangle(item.ClassID * 32, 0, 32, 32), New Vector3(0, 0, 0), New Vector3(CSng(pos.x / 3 - 4), CSng(pos.y / 3 + 1), 0), Color.White.ToArgb)

                    d3d_sprite.Transform = Matrix.Identity

                    'Draw_Officer(Get_Officer_Texture(item), character_sprite_enum.Head, New PointD(0, 0), Color.White)

                    pos.x += 92
                    If advance - PLV__Class_Scroll >= 4 Then Exit For
                End If
                advance += 1
            End If
        Next

        pos.x = 640
        pos.y = 604
        'Draw Base Classes
        If PLV__Selected_Class = Class_List_Enum.Engineer Then d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Base_Class_Frame), Vector3.Empty, New Vector3(pos.sngX - 2, pos.sngY - 2, 0), Color.White.ToArgb)
        d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Base_Class), Vector3.Empty, New Vector3(pos.sngX, pos.sngY, 0), Color.Gray.ToArgb)
        scale = CInt(96 * Officer_List(PLV__Selected_Officer).GetClass(Class_List_Enum.Engineer).Experance \ 100)
        d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Base_Class), New Rectangle(0, 0, scale, 16), New Vector3(0, 0, 0), New Vector3(pos.sngX, pos.sngY, 0), Color.Orange.ToArgb)
        If Officer_List(PLV__Selected_Officer).GetClass(Class_List_Enum.Engineer).Skill_Points > 0 Then d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Level_Up), Vector3.Empty, New Vector3(pos.sngX - 54, pos.sngY, 0), Color.White.ToArgb)
        draw_text("Lv " + Officer_List(PLV__Selected_Officer).GetClass(Class_List_Enum.Engineer).Level.ToString, New Rectangle(pos.intX - 32, pos.intY, 96, 16), DrawTextFormat.Left, Color.White, d3d_font(d3d_font_enum.SB_small))
        draw_text("Engineer", New Rectangle(pos.intX, pos.intY, 96, 16), DrawTextFormat.Center, Color.Black, d3d_font(d3d_font_enum.SB_small))
        pos.y += 32

        If PLV__Selected_Class = Class_List_Enum.Security Then d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Base_Class_Frame), Vector3.Empty, New Vector3(pos.sngX - 2, pos.sngY - 2, 0), Color.White.ToArgb)
        d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Base_Class), Vector3.Empty, New Vector3(pos.sngX, pos.sngY, 0), Color.Gray.ToArgb)
        scale = CInt(96 * Officer_List(PLV__Selected_Officer).GetClass(Class_List_Enum.Security).Experance \ 100)
        d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Base_Class), New Rectangle(0, 0, scale, 16), New Vector3(0, 0, 0), New Vector3(pos.sngX, pos.sngY, 0), Color.Red.ToArgb)
        If Officer_List(PLV__Selected_Officer).GetClass(Class_List_Enum.Security).Skill_Points > 0 Then d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Level_Up), Vector3.Empty, New Vector3(pos.sngX - 54, pos.sngY, 0), Color.White.ToArgb)
        draw_text("Lv " + Officer_List(PLV__Selected_Officer).GetClass(Class_List_Enum.Security).Level.ToString, New Rectangle(pos.intX - 32, pos.intY, 96, 16), DrawTextFormat.Left, Color.White, d3d_font(d3d_font_enum.SB_small))
        draw_text("Security", New Rectangle(pos.intX, pos.intY, 96, 16), DrawTextFormat.Center, Color.Black, d3d_font(d3d_font_enum.SB_small))
        pos.y += 32

        If PLV__Selected_Class = Class_List_Enum.Scientist Then d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Base_Class_Frame), Vector3.Empty, New Vector3(pos.sngX - 2, pos.sngY - 2, 0), Color.White.ToArgb)
        d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Base_Class), Vector3.Empty, New Vector3(pos.sngX, pos.sngY, 0), Color.Gray.ToArgb)
        scale = CInt(96 * Officer_List(PLV__Selected_Officer).GetClass(Class_List_Enum.Scientist).Experance \ 100)
        d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Base_Class), New Rectangle(0, 0, scale, 16), New Vector3(0, 0, 0), New Vector3(pos.sngX, pos.sngY, 0), Color.LightBlue.ToArgb)
        If Officer_List(PLV__Selected_Officer).GetClass(Class_List_Enum.Scientist).Skill_Points > 0 Then d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Level_Up), Vector3.Empty, New Vector3(pos.sngX - 54, pos.sngY, 0), Color.White.ToArgb)
        draw_text("Lv " + Officer_List(PLV__Selected_Officer).GetClass(Class_List_Enum.Scientist).Level.ToString, New Rectangle(pos.intX - 32, pos.intY, 96, 16), DrawTextFormat.Left, Color.White, d3d_font(d3d_font_enum.SB_small))
        draw_text("Scientist", New Rectangle(pos.intX, pos.intY, 96, 16), DrawTextFormat.Center, Color.Black, d3d_font(d3d_font_enum.SB_small))
        pos.y += 32

        If PLV__Selected_Class = Class_List_Enum.Aviator Then d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Base_Class_Frame), Vector3.Empty, New Vector3(pos.sngX - 2, pos.sngY - 2, 0), Color.White.ToArgb)
        d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Base_Class), Vector3.Empty, New Vector3(pos.sngX, pos.sngY, 0), Color.Gray.ToArgb)
        scale = CInt(96 * Officer_List(PLV__Selected_Officer).GetClass(Class_List_Enum.Aviator).Experance \ 100)
        d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Base_Class), New Rectangle(0, 0, scale, 16), New Vector3(0, 0, 0), New Vector3(pos.sngX, pos.sngY, 0), Color.Cyan.ToArgb)
        If Officer_List(PLV__Selected_Officer).GetClass(Class_List_Enum.Aviator).Skill_Points > 0 Then d3d_sprite.Draw(button_texture(button_texture_enum.PLV__Level_Up), Vector3.Empty, New Vector3(pos.sngX - 54, pos.sngY, 0), Color.White.ToArgb)
        draw_text("Lv " + Officer_List(PLV__Selected_Officer).GetClass(Class_List_Enum.Aviator).Level.ToString, New Rectangle(pos.intX - 32, pos.intY, 96, 16), DrawTextFormat.Left, Color.White, d3d_font(d3d_font_enum.SB_small))
        draw_text("Aviator", New Rectangle(pos.intX, pos.intY, 96, 16), DrawTextFormat.Center, Color.Black, d3d_font(d3d_font_enum.SB_small))



        For Each item In Menu_Items_Personal_Level
            If item.Value.enabled = True AndAlso Not item.Value.tile_Set = button_texture_enum.Blank Then
                d3d_sprite.Draw(button_texture(item.Value.tile_Set), Vector3.Empty, New Vector3(item.Value.bounds.X, item.Value.bounds.Y, 0), item.Value.get_color.ToArgb)
                If Not item.Value.text = "" Then draw_text(item.Value.text, item.Value.bounds, item.Value.align, item.Value.fontcolor, d3d_font(item.Value.font))
            End If
        Next



        d3d_sprite.End()
        d3d_device.EndScene()
        Try
            d3d_device.Present()
        Catch e As DeviceLostException
            DeviceLost = True
        End Try
        If DeviceLost = True Then AttemptRecovery()



    End Sub





    Sub render_Test()
        Weapon_Control_zoom = 1.0F
        Dim scale As Single = CSng(Weapon_Control_zoom)
        Dim pos As PointD
        d3d_device.BeginScene()
        d3d_device.Clear(ClearFlags.Target, Color.Black, 1, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)
        d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2(0, 0), 0, New Vector2(0, 0))






        Dim atsize As Integer = CInt(32 * Weapon_Control_zoom)
        Dim ship As Ship = Ship_List(Officer_List(current_player).Location_ID)
        Dim TileMap(,) As Ship_tile
        TileMap = ship.tile_map
        For x = (CInt(view_location_weapon_control.x * Weapon_Control_zoom) \ atsize) - 1 To (CInt(view_location_weapon_control.x * Weapon_Control_zoom) \ atsize) + (screen_size.x \ atsize) + 1
            For y = (CInt(view_location_weapon_control.y * Weapon_Control_zoom) \ atsize) - 1 To (CInt(view_location_weapon_control.y * Weapon_Control_zoom) \ atsize) + (screen_size.y \ atsize) + 1
                pos.x = (x * 32) - view_location_weapon_control.x
                pos.y = (y * 32) - view_location_weapon_control.y
                If x >= 0 AndAlso x <= ship.shipsize.x AndAlso y >= 0 AndAlso y <= ship.shipsize.y Then
                    If TileMap(x, y).type < tile_type_enum.Restricted Then
                        Draw_Ship_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, Color.FromArgb(255, 255, 255, 255))
                        If TileMap(x, y).device_tile IsNot Nothing AndAlso Not TileMap(x, y).device_tile.type = device_tile_type_enum.Empty Then
                            If ship.device_list(TileMap(x, y).device_tile.device_ID).type = device_type_enum.weapon Then
                                Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, ship.device_list(TileMap(x, y).device_tile.device_ID).Sprite_Animation_Key, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                                If Weapon_Control__Selected_Group > -1 AndAlso ship.Weapon_control_groups(Weapon_Control__Selected_Group).Connected_Weapons.Contains(TileMap(x, y).device_tile.device_ID) Then
                                    Draw_Ship_Tile(tile_type_enum.Hull_1, 4, pos, Color.FromArgb(100, 0, 0, 255))
                                Else
                                    Draw_Ship_Tile(tile_type_enum.Hull_1, 4, pos, Color.FromArgb(100, 0, 255, 0))
                                End If
                            Else
                                Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, ship.device_list(TileMap(x, y).device_tile.device_ID).Sprite_Animation_Key, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                            End If
                        End If
                    End If
                End If
            Next
        Next

        d3d_sprite.Transform = Matrix.Identity

        For Each item In External_Menu_Items_Weapon_Control
            If item.Value.enabled = True Then
                d3d_sprite.Draw(button_texture(item.Value.tile_Set), Vector3.Empty, New Vector3(item.Value.bounds.X, item.Value.bounds.Y, 0), item.Value.get_color.ToArgb)
                If Not item.Value.text = "" Then draw_text(item.Value.text, item.Value.bounds, item.Value.align, item.Value.fontcolor, d3d_font(item.Value.font))
            End If
        Next



        d3d_sprite.End()
        d3d_device.EndScene()
        Try
            d3d_device.Present()
        Catch e As DeviceLostException
            DeviceLost = True
        End Try
        If DeviceLost = True Then AttemptRecovery()


    End Sub

    Sub render_ship_internal(ByRef ship As Ship)
        Dim atsize As Integer = Convert.ToInt32(32 * internal_zoom)

        Dim ship_size = ship.GetShipSize()
        Dim TileMap(ship_size.x, ship_size.y) As Ship_tile
        Dim pos As PointD
        Dim sprite_set As character_sprite_set_enum
        Dim sprite As character_sprite_enum

        'current_player.GetLocation()

        TileMap = ship.tile_map
        d3d_device.BeginScene()
        d3d_device.Clear(ClearFlags.Target, Color.Black, 0, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)
        Dim scale As Decimal = Convert.ToDecimal(internal_zoom)
        d3d_sprite.Transform = Matrix.Identity * Matrix.Scaling(scale, scale, 0)


        For x = (view_location_internal.x * Convert.ToInt32(internal_zoom) \ atsize) - 1 To (view_location_internal.x * Convert.ToInt32(internal_zoom) \ atsize) + (screen_size.x \ atsize) + 1
            For y = (view_location_internal.y * Convert.ToInt32(internal_zoom) \ atsize) - 1 To (view_location_internal.y * Convert.ToInt32(internal_zoom) \ atsize) + (screen_size.y \ atsize) + 1
                pos.x = ((x * 32) - view_location_internal.x)
                pos.y = ((y * 32) - view_location_internal.y)
                If x >= 0 AndAlso x <= ship_size.x AndAlso y >= 0 AndAlso y <= ship_size.y Then
                    If TileMap(x, y).type < tile_type_enum.Device_Base Then
                        Select Case TileMap(x, y).viewed
                            Case tile_view_level.Unexplored
                                'Draw_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, Color.FromArgb(255, 0, 0, 0))
                                'If TileMap(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.FromArgb(255, 0, 0, 0))
                            Case tile_view_level.Explored
                                'Draw_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, Color.FromArgb(255, 100, 100, 100))
                                'If TileMap(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.FromArgb(255, 100, 100, 100))
                            Case tile_view_level.ViewedDark
                                'Draw_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, Color.FromArgb(255, 200, 200, 200))
                                'If TileMap(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.FromArgb(255, 200, 200, 200))
                            Case tile_view_level.ViewedDim
                                'Draw_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, Color.FromArgb(255, 225, 225, 225))
                                'If TileMap(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.FromArgb(255, 225, 225, 225))
                            Case tile_view_level.Viewed
                                'Draw_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, Color.FromArgb(255, 255, 255, 255))
                                'If TileMap(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                        End Select
                    End If
                End If
            Next
        Next


        'draw officers
        Dim officer_temp(ship.GetOfficer.Keys.Count - 1) As Integer
        ship.GetOfficer.Keys.CopyTo(officer_temp, 0)

        For x = 0 To officer_temp.Length - 1
            pos.x = Convert.ToInt32(ship.GetOfficer.Item(officer_temp(x)).GetLocation.x - view_location_internal.x)
            pos.y = Convert.ToInt32(ship.GetOfficer.Item(officer_temp(x)).GetLocation.y - view_location_internal.y)
            Draw_Crew(character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, pos, Color.White)
        Next


        'draw crew
        Dim crew_temp(ship.Crew_list.Count - 1) As Integer
        ship.Crew_list.Keys.CopyTo(crew_temp, 0)

        For x = 0 To crew_temp.Length - 1
            pos.x = Convert.ToInt32(ship.Crew_list.Item(crew_temp(x)).location.x - view_location_internal.x)
            pos.y = Convert.ToInt32(ship.Crew_list.Item(crew_temp(x)).location.y - view_location_internal.y)
            sprite = ship.Crew_list.Item(crew_temp(x)).Sprite
            sprite_set = ship.Crew_list.Item(crew_temp(x)).SpriteSet

            Draw_Crew(sprite_set, sprite, pos, Color.White)

        Next
        'd3d_sprite.Draw(tile_texture(9), Rectangle.Empty, New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color.White)


        d3d_sprite.End()


        'draw_text(Ships(0).pipeline_list(0).Supply.ToString, New Point(2, 0), Color.Green, d3d_font(d3d_font_enum.Big_Button))

        Dim sphere As Mesh = Mesh.Sphere(d3d_device, 100, 8, 8)
        sphere.DrawSubset(0)

        d3d_device.EndScene()


        Try
            d3d_device.Present()
        Catch e As DeviceLostException
            DeviceLost = True
        End Try
        If DeviceLost = True Then AttemptRecovery()

    End Sub

    Sub render_planetoid(ByVal planet As Planet)
        Dim atsize As Integer = Convert.ToInt32(32 * screen_zoom)

        Dim planet_size = planet.size
        Dim TileMap(planet_size.x, planet_size.y) As Planet_tile
        Dim pos As PointD

        '        current_player.GetLocation()

        TileMap = planet.tile_map
        d3d_device.BeginScene()
        d3d_device.Clear(ClearFlags.Target, Color.Black, 0, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)
        Dim aa As Decimal = Convert.ToDecimal(planet_zoom)
        d3d_sprite.Transform = Matrix.Identity * Matrix.Scaling(aa, aa, 0)


        For x = (view_location_planet.x * Convert.ToInt32(planet_zoom) \ atsize) - 1 To (view_location_planet.x * Convert.ToInt32(planet_zoom) \ atsize) + (screen_size.x \ atsize) + 1
            For y = (view_location_planet.y * Convert.ToInt32(planet_zoom) \ atsize) - 1 To (view_location_planet.y * Convert.ToInt32(planet_zoom) \ atsize) + (screen_size.y \ atsize) + 1
                pos.x = ((x * 32) - view_location_planet.x)
                pos.y = ((y * 32) - view_location_planet.y)
                If x >= 0 AndAlso x <= planet_size.x AndAlso y >= 0 AndAlso y <= planet_size.y Then
                    If TileMap(x, y).type < tile_type_enum.Device_Base Then
                        'Draw_Ship_Tile(tile_type_enum.Armor_1, TileMap(x, y).sprite, pos, Color.FromArgb(255, 0, 0, 0), scale)

                    End If
                End If
            Next
        Next


        'draw officers
        Dim officer_temp(planet.GetOfficer.Keys.Count - 1) As Integer
        planet.GetOfficer.Keys.CopyTo(officer_temp, 0)

        For x = 0 To officer_temp.Length - 1

            pos.x = Convert.ToInt32(planet.GetOfficer.Item(officer_temp(x)).GetLocation.x - view_location_planet.x)
            pos.y = Convert.ToInt32(planet.GetOfficer.Item(officer_temp(x)).GetLocation.y - view_location_planet.y)

            'Draw_Crew(character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, pos, Color.White)
        Next


        d3d_sprite.End()

        d3d_device.EndScene()


        Try
            d3d_device.Present()
        Catch e As DeviceLostException
            DeviceLost = True
        End Try
        If DeviceLost = True Then AttemptRecovery()


    End Sub

    Sub Render_Ship_Texture(ByVal Ship As Ship, ByVal Ship_Texture As Texture, ByVal MipLevels As Integer)
        Dim atsize As Integer = Convert.ToInt32(32 * external_zoom)
        Dim BB As Surface
        BB = d3d_device.GetRenderTarget(0)

        Dim scale As Single = 0.125

        For Level = 0 To MipLevels
            d3d_device.SetRenderTarget(0, Ship_Texture.GetSurfaceLevel(Level))
            d3d_device.Clear(ClearFlags.Target, Color.Transparent, 1, 0)
            'd3d_device.Clear(ClearFlags.Target, Color.Blue, 1, 0)
            d3d_sprite.Begin(SpriteFlags.AlphaBlend)
            d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)
            d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)

            If Level = 0 Then d3d_sprite.Transform = Matrix.Scaling(scale, scale, 1)
            Dim s As Single = Convert.ToSingle(scale / (2 ^ Level))
            If Level > 0 Then d3d_sprite.Transform = Matrix.Scaling(s, s, 1)


            Dim pos As PointI
            For x = 0 To Ship.shipsize.x
                For y = 0 To Ship.shipsize.y
                    If x >= 0 AndAlso x <= Ship.shipsize.x AndAlso y >= 0 AndAlso y <= Ship.shipsize.y Then
                        If Ship.tile_map(x, y).type < tile_type_enum.Device_Base Then
                            pos.x = x * 32
                            pos.y = y * 32
                            d3d_sprite.Draw(tile_texture(Ship.tile_map(x, y).type), New Rectangle(Ship.tile_map(x, y).sprite * 32, 0, 32, 32), New Vector3(0, 0, 0), New Vector3(pos.x, pos.y, 0), Color.White)                            
                            'If TileMap(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, Ship.device_list(TileMap(x, y).device_tile.device_ID).Sprite_Animation_Key, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                            If Ship.tile_map(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(Ship.tile_map(x, y).device_tile.type, Ship.tile_map(x, y).device_tile.sprite, Ship.device_list(Ship.tile_map(x, y).device_tile.device_ID).Sprite_Animation_Key, pos.ToPointD, Ship.tile_map(x, y).device_tile.rotate, Ship.tile_map(x, y).device_tile.flip, scale, Color.White)
                            'If Ship.tile_map(x, y).sprite = room_sprite_enum.blank Then
                            'If Ship.tile_map(x, y).device_tile IsNot Nothing AndAlso Not Ship.tile_map(x, y).device_tile.type = device_tile_type_enum.Empty Then Draw_Device_Tile(Ship.tile_map(x, y).device_tile.type, Ship.tile_map(x, y).device_tile.sprite, pos, Ship.tile_map(x, y).device_tile.rotate, Ship.tile_map(x, y).device_tile.flip, 1, Color.White)
                            'End If
                        End If
                    End If
                Next
            Next
            d3d_sprite.End()
        Next
        d3d_device.SetRenderTarget(0, BB)

    End Sub


    Sub Render_Planet_Texture(ByVal planet As Planet, ByVal Planet_Texture() As Texture, ByVal MipLevels As Integer)
        Dim BB As Surface = d3d_device.GetRenderTarget(0)
        Dim Chunksize As PointI = New PointI((planet.size.x \ 4), (planet.size.y \ 4))
        Dim scale As Single = 32
        For tiley = 0 To 3
            For tilex = 0 To 3

                For Level = 0 To MipLevels
                    d3d_device.SetRenderTarget(0, Planet_Texture(tiley * 4 + tilex).GetSurfaceLevel(Level))
                    d3d_device.Clear(ClearFlags.Target, Color.Black, 1, 1)

                    d3d_sprite.Begin(SpriteFlags.AlphaBlend)
                    d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)
                    d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)
                    Dim s As Single = Convert.ToSingle(0.125 / (2 ^ Level))
                    If Level = 0 Then s = 0.125
                    d3d_sprite.Transform = Matrix.AffineTransformation2D(s, Vector2.Empty, 0, Vector2.Empty)

                    Dim pos As PointI
                    For x = 0 To Chunksize.x
                        For y = 0 To Chunksize.y
                            If x >= 0 AndAlso x <= planet.size.x AndAlso y >= 0 AndAlso y <= planet.size.y Then
                                If planet.tile_map(x, y).type < planet_tile_type_enum.empty Then
                                    pos.x = x * 32
                                    pos.y = y * 32
                                    Dim x2 As Integer = x + Chunksize.x * tilex
                                    Dim y2 As Integer = y + Chunksize.y * tiley
                                    'Draw_Planet_Tile(planet.tile_map(x2, y2).type, planet.tile_map(x2, y2).sprite, pos.ToPointD, Color.FromArgb(255, 255, 255, 255))

                                    d3d_sprite.Draw(planet_tile_texture(planet.tile_map(x2, y2).type), New Rectangle(planet.tile_map(x2, y2).sprite * 32, 0, 32, 32), New Vector3(0, 0, 0), New Vector3(pos.x, pos.y, 0), Color.White)
                                    'd3d_sprite.Draw2D(planet_tile_texture(planet.tile_map(x2, y2).type), New Rectangle(planet.tile_map(x2, y2).sprite * 32, 0, 32, 32), New SizeF(4, 4), pos.ToPointF, Color.White)
                                End If
                            End If
                        Next
                    Next
                    d3d_sprite.End()
                Next


            Next
        Next

        d3d_device.SetRenderTarget(0, BB)

    End Sub

    Sub Render_MiniMap_Texture(ByVal Ship As Ship, ByVal Minimap_Texture As Texture)
        Dim BB As Surface
        BB = d3d_device.GetRenderTarget(0)

        Dim scale As Single = 0.125
        d3d_device.SetRenderTarget(0, Minimap_Texture.GetSurfaceLevel(0))
        'd3d_device.Clear(ClearFlags.Target, Color.Transparent, 1, 0)
        d3d_device.Clear(ClearFlags.Target, Color.Blue, 1, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)

        d3d_sprite.Transform = Matrix.Scaling(scale, scale, 1)

        Dim pos As PointD
        For x = 0 To Ship.shipsize.x
            For y = 0 To Ship.shipsize.y
                If x >= 0 AndAlso x <= Ship.shipsize.x AndAlso y >= 0 AndAlso y <= Ship.shipsize.y Then
                    If Ship.tile_map(x, y).type < tile_type_enum.empty Then
                        pos.x = x * 32
                        pos.y = y * 32
                        Select Case Ship.tile_map(x, y).type
                            Case Is < tile_type_enum.Device_Base
                                d3d_sprite.Draw(tile_texture(Ship.tile_map(x, y).type), New Rectangle(Ship.tile_map(x, y).sprite * 32, 0, 32, 32), New Vector3(0, 0, 0), New Vector3(pos.sngX, pos.sngY, 0), Color.White)
                                If Ship.tile_map(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(Ship.tile_map(x, y).device_tile.type, Ship.tile_map(x, y).device_tile.sprite, Ship.tile_map(x, y).device_tile.spriteAni, pos, Ship.tile_map(x, y).device_tile.rotate, Ship.tile_map(x, y).device_tile.flip, Convert.ToDecimal(scale), Color.White)
                            Case Is = tile_type_enum.Device_Base
                                If Ship.tile_map(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(Ship.tile_map(x, y).device_tile.type, Ship.tile_map(x, y).device_tile.sprite, Ship.tile_map(x, y).device_tile.spriteAni, pos, Ship.tile_map(x, y).device_tile.rotate, Ship.tile_map(x, y).device_tile.flip, Convert.ToDecimal(scale), Color.White)
                        End Select
                    End If
                End If
            Next
        Next
        d3d_sprite.End()
        d3d_device.SetRenderTarget(0, BB)

    End Sub

    Sub Draw_device_diagram(ByVal Device As tech_list_enum, ByVal Position As PointI)
        Dim size As PointI = New PointI(Device_tech_list(Device).cmap(0).Length - 1, Device_tech_list(Device).cmap.Length - 1)
        Dim tile As Integer
        Dim pad As Integer = 32
        d3d_sprite.Transform = Matrix.Identity * Matrix.Scaling(0.5, 0.5, 0)
        Position.x = Position.x * 2
        Position.y = Position.y * 2
        For y = 0 To size.y
            For x = 0 To size.x
                Dim cmap As Byte = Device_tech_list(Device).cmap(y)(x)
                Dim Tileset As Integer = Tech_list(Tech_list(Device).Device_room_type).tile_type
                If Tileset = Tech_list(tech_list_enum.All_rooms).tile_type Then Tileset = Tech_list(tech_list_enum.Corridor).tile_type

                If cmap = 1 OrElse cmap = 4 Then
                    Draw_Ship_Tile(Tileset, room_sprite_enum.Floor, New PointD(Position.x + x * pad, Position.y + y * pad), Color.White)
                End If

                If cmap = 2 OrElse cmap = 5 Then
                    Draw_Ship_Tile(Tileset, room_sprite_enum.WallB, New PointD(Position.x + x * pad, Position.y + y * pad), Color.White)
                End If

                If cmap = 3 OrElse cmap = 6 Then
                    Draw_Ship_Tile(tile_type_enum.Hull_1, 2, New PointD(Position.x + x * pad, Position.y + y * pad), Color.White)
                End If


                If cmap = 7 Then
                    Draw_Ship_Tile(tile_type_enum.Hull_1, 1, New PointD(Position.x + x * pad, Position.y + y * pad), Color.White)
                End If

                If cmap > 0 AndAlso cmap < 4 Then
                    Draw_Device_Tile(Device_tech_list(Device).tile_type, tile, 0, New PointD(Position.x + x * pad, Position.y + y * pad), 0, flip_enum.None, 0.5, Color.FromArgb(255, 255, 255, 255))
                    tile += 1
                End If


                If cmap = 1 OrElse cmap = 4 OrElse cmap = 7 Then
                    Draw_Ship_Tile(tile_type_enum.Hull_1, 5, New PointD(Position.x + x * pad, Position.y + y * pad), Color.FromArgb(200, 255, 255, 255))
                End If

                If cmap = 2 OrElse cmap = 5 Then
                    Draw_Ship_Tile(tile_type_enum.Hull_1, 6, New PointD(Position.x + x * pad, Position.y + y * pad), Color.FromArgb(200, 255, 255, 255))
                End If

                If cmap = 3 OrElse cmap = 6 Then
                    Draw_Ship_Tile(tile_type_enum.Hull_1, 7, New PointD(Position.x + x * pad, Position.y + y * pad), Color.FromArgb(200, 255, 255, 255))
                End If

            Next
        Next

        d3d_sprite.Transform = Matrix.Identity

    End Sub

    Sub Render_Officer_Texture(ByVal OfficerID As Integer, ByVal Off_tex As Texture)
        d3d_sprite.End()
        Dim O As Officer = Officer_List(OfficerID)
        Dim BB As Surface
        Dim MatStore As Matrix
        BB = d3d_device.GetRenderTarget(0)

        d3d_device.SetRenderTarget(0, Off_tex.GetSurfaceLevel(0))        
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)
        MatStore = d3d_sprite.Transform
        d3d_sprite.Transform = Matrix.Identity
        d3d_device.Clear(ClearFlags.Target, Color.Transparent, 1, 0)
        d3d_sprite.Transform = Matrix.Identity
        d3d_sprite.Draw(character_texture(O.sprite.Head_SpriteSet), New Rectangle(O.sprite.Head_Sprite * 32, 0, 32, 32), New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color.White)
        d3d_sprite.Draw(character_texture(O.sprite.Torso_SpriteSet), New Rectangle(O.sprite.Torso_Sprite * 32, 32, 32, 32), New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color.White)
        d3d_sprite.Draw(character_texture(O.sprite.Left_Arm_SpriteSet), New Rectangle(O.sprite.Left_Arm_Sprite * 32, 64, 32, 32), New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color.White)
        d3d_sprite.Draw(character_texture(O.sprite.Right_Arm_SpriteSet), New Rectangle(O.sprite.Right_Arm_Sprite * 32, 96, 32, 32), New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color.White)
        d3d_sprite.Draw(character_texture(O.sprite.Left_Leg_SpriteSet), New Rectangle(O.sprite.Left_Leg_Sprite * 32, 128, 32, 32), New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color.White)
        d3d_sprite.Draw(character_texture(O.sprite.Right_Leg_SpriteSet), New Rectangle(O.sprite.Right_Leg_Sprite * 32, 160, 32, 32), New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color.White)
        d3d_sprite.End()

        d3d_sprite.Transform = MatStore
        d3d_device.SetRenderTarget(0, BB)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
    End Sub


#Region "Ship_External"


    Sub Render_Ship_External_close(ByVal Ship As Ship)
        Dim scale As Single = external_zoom
        Dim pos As PointD
        Dim atsize As Integer = CInt(32 * external_zoom)
        Dim TileMap(,) As Ship_tile
        TileMap = Ship.tile_map
        For x = 0 To Ship.shipsize.x
            For y = 0 To Ship.shipsize.y
                pos.x = CInt(x * 32)
                pos.y = CInt(y * 32)
                If x >= 0 AndAlso x <= Ship.shipsize.x AndAlso y >= 0 AndAlso y <= Ship.shipsize.y Then
                    If TileMap(x, y).type < tile_type_enum.Restricted Then
                        Draw_Ship_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, TileMap(x, y).color)
                        If TileMap(x, y).device_tile IsNot Nothing Then Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, Ship.device_list(TileMap(x, y).device_tile.device_ID).Sprite_Animation_Key, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                    End If
                End If
            Next
        Next

        For Each officer In Ship.Officer_List
            pos.x = officer.Value.GetLocation.x
            pos.y = officer.Value.GetLocation.y
            'Draw_Crew(character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, pos, Color.White)
            Draw_Officer(Get_Officer_Texture(officer.Key), character_sprite_enum.Head, pos, Color.White)
        Next

        For Each crew In Ship.Crew_list
            pos.x = crew.Value.location.x
            pos.y = crew.Value.location.y
            Draw_Crew(character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, pos, Color.White)
        Next

    End Sub

    Sub render_ship_external_new(ByVal Ship As Ship)        
        'Dim pos As PointD
        d3d_device.BeginScene()

        External_redraw(Ship)

        d3d_device.Clear(ClearFlags.Target, Color.Black, 1, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        Dim scale As Single = external_zoom
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.Point)
        d3d_device.SetSamplerState(0, SamplerStageStates.MipFilter, TextureFilter.Linear)
        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)

        view_location_external.x = Ship.location.x - ((screen_size.x / 2) - (Ship.center_point.x * 32 + 16) * scale) / scale
        view_location_external.y = Ship.location.y - ((screen_size.y / 2) - (Ship.center_point.y * 32 + 16) * scale) / scale

        For Each Draw_Ship In Ship_List.Values
            If external_zoom > 0.25 Then
                d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2((Draw_Ship.center_point.x * 32 + 16) * scale, (Draw_Ship.center_point.y * 32 + 16) * scale), CSng(Draw_Ship.rotation), New Vector2(CInt((Draw_Ship.location.x - view_location_external.x) * scale), CInt((Draw_Ship.location.y - view_location_external.y) * scale)))
                Render_Ship_External_close(Draw_Ship)
                d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2(0, 0), 0, New Vector2(0, 0))
            Else
                d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale * 8, scale * 8), New Vector2((Draw_Ship.center_point.x * 32 + 16) * scale, (Draw_Ship.center_point.y * 32 + 16) * scale), CSng(Draw_Ship.rotation), New Vector2(CInt((Draw_Ship.location.x - view_location_external.x) * scale), CInt((Draw_Ship.location.y - view_location_external.y) * scale)))
                d3d_sprite.Draw(Ship_Texture, Rectangle.Empty, New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color.White)
                d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2(0, 0), 0, New Vector2(0, 0))
            End If
        Next


        Dim x1 As Single
        Dim y1 As Single
        For Each pro In u.Projectiles
            x1 = CSng(pro.Location.x - view_location_external.x - 16)
            y1 = CSng(pro.Location.y - view_location_external.y - 16)
            d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2(16 * scale, 16 * scale), CSng(pro.Rotation), New Vector2(x1 * scale, y1 * scale))
            d3d_sprite.Draw(projectile_tile_texture(Projectile_Tile_Type_Enum.Energy1), Rectangle.Empty, Vector3.Empty, New Vector3(0, 0, 0), Color.White)
        Next


        d3d_sprite.Transform = Matrix.Identity
        'Render UI
        Draw_pipeline(Ship)
        render_ship_external_UI()

        d3d_sprite.Draw(Ship_Map_Texture, Rectangle.Empty, New Vector3(0, 0, 0), New Vector3(screen_size.x - (Ship.shipsize.x + 1) * 4 - 20, 200, 0), Color.White)

        d3d_sprite.End()
        d3d_device.EndScene()

        Try
            d3d_device.Present()
        Catch e As DeviceLostException
            DeviceLost = True
        End Try
        If DeviceLost = True Then AttemptRecovery()

    End Sub





    Sub External_redraw(ByVal ship As Ship)
        If shipexternal_redraw = False Then
            Ship_Texture = New Texture(d3d_device, (ship.shipsize.x + 1) * 4, (ship.shipsize.y + 1) * 4, 0, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default)
            Render_Ship_Texture(ship, Ship_Texture, 3)
            shipexternal_redraw = True

            d3d_sprite.Transform = Matrix.Identity
            Ship_Map_Texture = New Texture(d3d_device, (ship.shipsize.x + 1) * 4, (ship.shipsize.y + 1) * 4, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default)
            render_ship_map(ship, Ship_Map_Texture, 4)
        End If


        If Not Near_planet = Loaded_planet Then
            Render_Planet_Texture(Planet_List(Near_planet), external_planet_texture, 8)
            Loaded_planet = Near_planet
        End If

    End Sub


    Sub render_ship_external(ByRef ship As Ship)                
        Dim atsize As Integer = Convert.ToInt32(32 * external_zoom)
        Dim pos As PointD
        d3d_device.BeginScene()


        'Do external redraws
        External_redraw(ship)



        d3d_device.Clear(ClearFlags.Target, Color.Black, 1, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        Dim scale As Single = external_zoom
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.Point)
        d3d_device.SetSamplerState(0, SamplerStageStates.MipFilter, TextureFilter.Point)
        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)

        'd3d_device.RenderState.AlphaBlendEnable = True
        'd3d_device.RenderState.SourceBlend = Blend.BothSourceAlpha
        'd3d_device.RenderState.DestinationBlend = Blend.InvSourceAlpha
        'd3d_device.SetTextureStageState(0,TextureStageStates.AlphaArgument1 ,

        view_location_external.x = ship.location.x - ((screen_size.x / 2) - (ship.center_point.x * 4 + 2) * scale) / scale
        view_location_external.y = ship.location.y - ((screen_size.y / 2) - (ship.center_point.y * 4 + 2) * scale) / scale


        d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2(0, 0), 0, New Vector2(0, 0))
        'Draw planet
        If Loaded_planet > -1 Then
            Dim planet = Planet_List(Loaded_planet)
            Dim planetpos As PointD
            Dim drawpos As PointD
            If Planet_List(Loaded_planet).orbits_planet = True Then
                'moons
                planetpos.x = u.stars(u.planets(planet.orbit_point).orbit_point).location.x + u.planets(planet.orbit_point).orbit_distance * Math.Cos((u.planets(planet.orbit_point).theta * planet_theta_offset) * 0.017453292519943295)
                planetpos.y = u.stars(u.planets(planet.orbit_point).orbit_point).location.y + u.planets(planet.orbit_point).orbit_distance * Math.Sin((u.planets(planet.orbit_point).theta * planet_theta_offset) * 0.017453292519943295)

                pos.x = planetpos.x + planet.orbit_distance * Math.Cos((planet.theta * planet_theta_offset) * 0.017453292519943295)
                pos.y = planetpos.y + planet.orbit_distance * Math.Sin((planet.theta * planet_theta_offset) * 0.017453292519943295)

                pos.x = pos.x / 1000 + view_location_external.x
                pos.y = pos.y / 1000 + view_location_external.y
            Else
                'Planets
                pos.x = u.stars(planet.orbit_point).location.x + planet.orbit_distance * Math.Cos((planet.theta * planet_theta_offset) * 0.017453292519943295)
                pos.y = u.stars(planet.orbit_point).location.y + planet.orbit_distance * Math.Sin((planet.theta * planet_theta_offset) * 0.017453292519943295)

                pos.x = pos.x / 1000 - view_location_external.x
                pos.y = pos.y / 1000 - view_location_external.y
            End If

            For y = 0 To 3
                For x = 0 To 3
                    drawpos.x = pos.x + (planet.size.x * x)
                    drawpos.y = pos.y + (planet.size.y * y)
                    'd3d_sprite.Draw(external_planet_texture(y * 4 + x), Rectangle.Empty, New Vector3(0, 0, 0), New Vector3(drawpos.sngX, drawpos.sngY, 0), Color.White)
                    'd3d_sprite.Draw(external_planet_texture(y * 4 + x), Rectangle.Empty, New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color.White)
                Next
            Next
            'd3d_sprite.Draw(external_planet_texture(0), Rectangle.Empty, New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color.White)
        End If

        'Draw ship
        'd3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2((ship.center_point.x * 4 + 2) * scale, (ship.center_point.y * 4 + 2) * scale), CSng(ship.rotation), New Vector2(CSng(ship.location.x * scale - view_location_external.x * scale), CSng(ship.location.y * scale - view_location_external.y * scale)))
        'd3d_sprite.Draw(Ship_Texture, Rectangle.Empty, New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color.White)
        'd3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2(0, 0), 0, New Vector2(0, 0))

        'Draw 2nd ship
        Dim ship2 As Ship = Ship_List(1)
        d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2((ship2.center_point.x * 4 + 2) * scale, (ship2.center_point.y * 4 + 2) * scale), CSng(ship2.rotation), New Vector2(CSng(ship2.location.x * scale - view_location_external.x * scale), CSng(ship2.location.y * scale - view_location_external.y * scale)))
        d3d_sprite.Draw(Ship_Texture, Rectangle.Empty, New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color.White)
        d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2(0, 0), 0, New Vector2(0, 0))



        'd3d_sprite.Draw(external_planet_texture(0), Rectangle.Empty, New Vector3(0, 0, 0), New Vector3(0, 0, 0), Color.White)


        Dim x1 As Single
        Dim y1 As Single

        For Each Star In u.stars
            x1 = CSng(Star.Value.location.x / 1000 + view_location_external.x)
            y1 = CSng(Star.Value.location.y / 1000 + view_location_external.y)
            d3d_sprite.Draw(icon_texture(0), Rectangle.Empty, Vector3.Empty, New Vector3(x1, y1, 0), Color.White)
        Next


        For Each pro In u.Projectiles
            x1 = CSng(pro.Location.x - view_location_external.x - 16)
            y1 = CSng(pro.Location.y - view_location_external.y - 16)
            d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2(16 * scale, 16 * scale), CSng(pro.Rotation), New Vector2(x1 * scale, y1 * scale))
            d3d_sprite.Draw(projectile_tile_texture(Projectile_Tile_Type_Enum.Energy1), Rectangle.Empty, Vector3.Empty, New Vector3(0, 0, 0), Color.White)
        Next

        d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2(0, 0), 0, New Vector2(0, 0))

        d3d_sprite.Transform = Matrix.Identity


        Draw_pipeline(ship)
        render_ship_external_UI()

        'd3d_sprite.Draw(button_texture(button_texture_enum.ship_build__menu_button), Vector3.Empty, New Vector3(0, 256, 0), Color.White.ToArgb)


        d3d_sprite.Draw(Ship_Map_Texture, Rectangle.Empty, New Vector3(0, 0, 0), New Vector3(screen_size.x - (ship.shipsize.x + 1) * 4 - 20, 200, 0), Color.White)




        'draw_text("Rotation: " + (Ship_List(current_selected_ship_view).rotation).ToString, New Point(0, 32), Color.White, d3d_font(d3d_font_enum.SB_small))
        'Dim rot As Double = Ship_List(current_selected_ship_view).target_rotation - Ship_List(current_selected_ship_view).rotation
        'draw_text(rot.ToString, New Point(0, 64), Color.Blue, d3d_font(d3d_font_enum.SB_small))
        'draw_text("Target Rotation: " + Ship_List(current_selected_ship_view).target_rotation.ToString, New Point(0, 96), Color.White, d3d_font(d3d_font_enum.SB_small))
        'draw_text(view_location_external.x.ToString, New Point(0, 256), Color.White, d3d_font(d3d_font_enum.SB_small))
        'draw_text((Ships(current_selected_ship_view).target_rotation * 180 / PI).ToString, New Point(0, 32), Color.White, d3d_font(d3d_font_enum.SB_small))


        'Dim rot As Double = 0
        'rot = Ships(current_selected_ship_view).rotation * (180 / PI) - Ships(current_selected_ship_view).target_rotation * (180 / PI)
        'draw_text(rot.ToString, New Point(0, 64), Color.White, d3d_font(d3d_font_enum.SB_small))
        'rot = Ships(current_selected_ship_view).rotation * (180 / PI)
        'draw_text(rot.ToString, New Point(0, 96), Color.White, d3d_font(d3d_font_enum.SB_small))

        d3d_sprite.End()
        d3d_device.EndScene()

        Try
            d3d_device.Present()
        Catch e As DeviceLostException
            DeviceLost = True
        End Try
        If DeviceLost = True Then AttemptRecovery()
    End Sub



    Sub render_ship_external_UI()        

        For Each item In External_Menu_Items
            If item.Value.enabled = True Then
                d3d_sprite.Draw(button_texture(item.Value.tile_Set), Vector3.Empty, New Vector3(item.Value.bounds.X, item.Value.bounds.Y, 0), item.Value.get_color.ToArgb)
                If Not item.Value.text = "" Then draw_text(item.Value.text, item.Value.bounds, item.Value.align, item.Value.fontcolor, d3d_font(item.Value.font))
            End If
        Next

    End Sub

    Sub Draw_pipeline(ByVal ship As Ship)
        Dim pos As PointI
        Dim Supply As Integer
        Dim Drain As Integer
        Dim scale As Double
        pos.x = 0
        pos.y = 0
        For Each pipe In ship.pipeline_list

            d3d_sprite.Draw(button_texture(button_texture_enum.ship_external__Pipeline_Display), Rectangle.Empty, New Vector3(0, 0, 0), New Vector3(pos.x, pos.y, 0), Set_Brighness(pipe.Value.Color, 0.6))

            scale = 160 / pipe.Value.Supply_Limit

            If pipe.Value.Supply >= 0 Then Supply = Convert.ToInt32(scale * pipe.Value.Supply)
            If pipe.Value.Supply_Drain >= 0 Then Drain = Convert.ToInt32(scale * pipe.Value.Supply_Drain)
            If pipe.Value.Supply_Drain >= pipe.Value.Supply_Limit Then Drain = Convert.ToInt32(scale * pipe.Value.Supply_Limit)
            d3d_sprite.Draw(button_texture(button_texture_enum.ship_external__Pipeline_Display), New Rectangle(0, 0, Supply, 8), New Vector3(0, 0, 0), New Vector3(pos.x, pos.y, 0), Set_Brighness(pipe.Value.Color, 1))
            d3d_sprite.Draw(button_texture(button_texture_enum.ship_external__Pipeline_Display), New Rectangle(0, 0, Drain, 8), New Vector3(0, 0, 0), New Vector3(pos.x, pos.y + 8, 0), Set_Brighness(pipe.Value.Color, 0.8))


            draw_text(pipe.Value.Name, New Rectangle(pos.x, pos.y, 160, 16), DrawTextFormat.Center, Color.White, d3d_font(d3d_font_enum.SB_small))
            pos.y += 20
        Next
    End Sub

    Sub render_ship_map(ByVal ship As Ship, ByVal Ship_Texture As Texture, ByVal scale As Integer)
        'Dim scale As Integer = 4

        Dim BB As Surface
        BB = d3d_device.GetRenderTarget(0)

        d3d_device.SetRenderTarget(0, Ship_Texture.GetSurfaceLevel(0))
        d3d_device.Clear(ClearFlags.Target, Color.Transparent, 1, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)
        Dim col As Color
        Dim pos As PointI
        For x = 0 To ship.shipsize.x
            For y = 0 To ship.shipsize.y
                If x >= 0 AndAlso x <= ship.shipsize.x AndAlso y >= 0 AndAlso y <= ship.shipsize.y Then
                    If ship.tile_map(x, y).type < tile_type_enum.empty Then
                        pos.x = x * scale
                        pos.y = y * scale

                        If ship.room_list(ship.tile_map(x, y).roomID).type = tech_list_enum.Engineering Then col = Color.Orange
                        If ship.room_list(ship.tile_map(x, y).roomID).type = tech_list_enum.Bridge Then col = Color.Red
                        If ship.room_list(ship.tile_map(x, y).roomID).type = tech_list_enum.Corridor Then col = Color.Gray
                        If ship.room_list(ship.tile_map(x, y).roomID).type = tech_list_enum.Armor Then col = Color.LightSeaGreen
                        If ship.room_list(ship.tile_map(x, y).roomID).type = tech_list_enum.Engineering Then col = Color.Orange

                        If Not ship.tile_map(x, y).type = tile_type_enum.Restricted Then

                            If ship.tile_map(x, y).sprite = room_sprite_enum.Floor OrElse ship.tile_map(x, y).roomID = 1 Then
                                d3d_sprite.Draw(tile_texture(tile_type_enum.Hull_1), New Rectangle(128, 0, scale, scale), New Vector3(0, 0, 0), New Vector3(pos.x, pos.y, 0), Set_Brighness(col, 0.9))
                            Else
                                d3d_sprite.Draw(tile_texture(tile_type_enum.Hull_1), New Rectangle(128, 0, scale, scale), New Vector3(0, 0, 0), New Vector3(pos.x, pos.y, 0), Set_Brighness(col, 0.7))
                            End If
                            If ship.tile_map(x, y).device_tile IsNot Nothing AndAlso Not ship.tile_map(x, y).device_tile.type = device_tile_type_enum.Empty Then d3d_sprite.Draw(tile_texture(tile_type_enum.Hull_1), New Rectangle(128, 0, scale, scale), New Vector3(0, 0, 0), New Vector3(pos.x, pos.y, 0), Set_Brighness(col, 1))

                        End If
                    End If
                End If
            Next
        Next
        d3d_sprite.End()
        d3d_device.SetRenderTarget(0, BB)


    End Sub

#End Region



#Region "Quick Map"

    Sub Render_External_Quick()

    End Sub


    Sub Render_Personal_Quick()

    End Sub





#End Region

    Sub render_ship_other()
        d3d_device.BeginScene()
        d3d_device.Clear(ClearFlags.Target, Color.Black, 0, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)

        d3d_sprite.End()

        d3d_device.EndScene()


        Try
            d3d_device.Present()
        Catch e As DeviceLostException
            DeviceLost = True
        End Try
        If DeviceLost = True Then AttemptRecovery()


        'Dim RenderTarget As Surface
        'RenderTarget = d3d_device.GetRenderTarget(0)
        'd3d_device.SetRenderTarget(1, offscreentexture.GetSurfaceLevel(0))

        'd3d_sprite.Begin(SpriteFlags.AlphaBlend)
        'd3d_device.SetRenderTarget(0, RenderTarget)
        'd3d_sprite.Draw2D(offscreentexture, New PointF(512, 384), 0, New PointF(0, 0), Color.White)
        'd3d_sprite.End()

    End Sub

    Sub render_main_menu(ByRef menu_item() As Menu_button)



        d3d_device.BeginScene()
        d3d_device.Clear(ClearFlags.Target, Color.Black, 0, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        'draw_text("00000000", New Rectangle(0, 0, 100, 100), CType(DrawTextFormat.Center + DrawTextFormat.VerticalCenter, DrawTextFormat), Color.Green, d3d_font(d3d_font_enum.Big_Button))

        For Each item In menu_item
            'd3d_sprite.Draw2D(button_texture(item.tile_Set), New PointF(0, 0), 0, item.bounds.Location, item.color)
            d3d_sprite.Draw(button_texture(item.tile_Set), Vector3.Empty, New Vector3(item.bounds.X, item.bounds.Y, 0), item.get_color.ToArgb)
            If Not item.text = "" Then draw_text(item.text, item.bounds, item.align, item.get_font_color, d3d_font(item.font))
        Next

        d3d_sprite.End()

        d3d_device.EndScene()

        Try
            d3d_device.Present()
        Catch e As DeviceLostException
            DeviceLost = True
        End Try
        If DeviceLost = True Then AttemptRecovery()
    End Sub


    Sub render_new_ship_options(ByRef menu_item() As Menu_button)



        d3d_device.BeginScene()
        d3d_device.Clear(ClearFlags.Target, Color.Black, 0, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        'd3d_device.TextureState(0).ColorOperation = TextureOperation.AddSigned
        'd3d_device.TextureState(0).ColorArgument1 = TextureArgument.TextureColor
        'd3d_device.TextureState(0).ColorArgument2 = TextureArgument.Complement
        d3d_sprite.Transform = Matrix.Identity


        For Each item In menu_item
            d3d_sprite.Draw(button_texture(item.tile_Set), Vector3.Empty, New Vector3(item.bounds.X, item.bounds.Y, 0), item.get_color.ToArgb)
            If Not item.text = "" Then draw_text(item.text, item.bounds, item.align, item.fontcolor, d3d_font(item.font))
        Next



        d3d_sprite.End()

        '        For Each item In menu_item
        'd3d_sprite.Draw2D(button_texture(item.tile_Set), New PointF(0, 0), 0, item.bounds.Location, item.color)
        'If Not item.text = "" Then draw_text(item.text, item.bounds, DrawTextFormat.Center, Color.Violet, d3d_font)
        'Next

        d3d_device.EndScene()
        Try
            d3d_device.Present()
        Catch e As DeviceLostException
            DeviceLost = True
        End Try
        If DeviceLost = True Then AttemptRecovery()
    End Sub

    Sub render_starmap()
        If DeviceLost = True Then AttemptRecovery()
        d3d_device.BeginScene()
        d3d_device.Clear(ClearFlags.Target, Color.Black, 0, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)
        d3d_sprite.Transform = Matrix.Identity * Matrix.Scaling(1, 1, 1)
        'sector = 1024000 pixels

        view_location_star_map.x = u.stars(star_map_sector).location.x \ Convert.ToInt32(60000 - star_map_zoom) - Convert.ToInt32(screen_size.x \ 2)
        view_location_star_map.y = u.stars(star_map_sector).location.y \ Convert.ToInt32(60000 - star_map_zoom) - Convert.ToInt32(screen_size.y \ 2)

        Dim x2, x1 As Integer
        Dim y2, y1 As Integer

        For Each Star In u.stars
            x1 = Star.Value.location.x \ Convert.ToInt32(60000 - star_map_zoom) - view_location_star_map.x
            y1 = Star.Value.location.y \ Convert.ToInt32(60000 - star_map_zoom) - view_location_star_map.y
            d3d_sprite.Draw(icon_texture(0), Rectangle.Empty, Vector3.Empty, New Vector3(x1, y1, 0), Color.White)
        Next


        For Each planet In u.planets
            If planet.Value.orbits_planet = True Then
                x2 = Convert.ToInt32(u.stars(u.planets(planet.Value.orbit_point).orbit_point).location.x + u.planets(planet.Value.orbit_point).orbit_distance * Math.Cos((u.planets(planet.Value.orbit_point).theta * planet_theta_offset) * 0.017453292519943295))
                y2 = Convert.ToInt32(u.stars(u.planets(planet.Value.orbit_point).orbit_point).location.y + u.planets(planet.Value.orbit_point).orbit_distance * Math.Sin((u.planets(planet.Value.orbit_point).theta * planet_theta_offset) * 0.017453292519943295))

                x2 = Convert.ToInt32(x2 + planet.Value.orbit_distance * Math.Cos((planet.Value.theta * planet_theta_offset) * 0.017453292519943295))
                y2 = Convert.ToInt32(y2 + planet.Value.orbit_distance * Math.Sin((planet.Value.theta * planet_theta_offset) * 0.017453292519943295))

                x1 = x2 \ Convert.ToInt32(60000 - star_map_zoom) - view_location_star_map.x
                y1 = y2 \ Convert.ToInt32(60000 - star_map_zoom) - view_location_star_map.y

                d3d_sprite.Draw(icon_texture(1), Rectangle.Empty, Vector3.Empty, New Vector3(x1, y1, 0), Color.White)

            Else
                x2 = Convert.ToInt32(u.stars(planet.Value.orbit_point).location.x + planet.Value.orbit_distance * Math.Cos((planet.Value.theta * planet_theta_offset) * 0.017453292519943295))
                y2 = Convert.ToInt32(u.stars(planet.Value.orbit_point).location.y + planet.Value.orbit_distance * Math.Sin((planet.Value.theta * planet_theta_offset) * 0.017453292519943295))

                x1 = x2 \ Convert.ToInt32(60000 - star_map_zoom) - view_location_star_map.x
                y1 = y2 \ Convert.ToInt32(60000 - star_map_zoom) - view_location_star_map.y

                d3d_sprite.Draw(icon_texture(2), Rectangle.Empty, Vector3.Empty, New Vector3(x1, y1, 0), Color.White)

            End If



        Next


        draw_text(u.planets.Count.ToString, New Point(2, 0), Color.Green, d3d_font(d3d_font_enum.Big_Button))
        'draw_text(star_map_sector.ToString, New Point(2, 0), Color.Green, d3d_font)
        'draw_text(star_map_sector.ToString, New Point(2, 0), Color.Green, d3d_font)

        d3d_sprite.End()




        Dim ver(16) As CustomVertex.TransformedColored
        For Each Nebula In u.nebula
            Dim p = 0
            For Each Point In Nebula.Value.points
                Dim x As Integer = Convert.ToInt32(Point.x / (60000 - star_map_zoom) - view_location_star_map.x)
                Dim y As Integer = Convert.ToInt32(Point.y / (60000 - star_map_zoom) - view_location_star_map.y)
                ver(p) = New CustomVertex.TransformedColored(x, y, 0, 0, Color.Blue.ToArgb)
                If p = 0 Then
                    ver(16) = New CustomVertex.TransformedColored(x, y, 0, 0, Color.Blue.ToArgb)
                End If

                p += 1
            Next

            Nebula_VB.SetData(ver, 0, LockFlags.None)
            d3d_device.VertexFormat = CustomVertex.TransformedColored.Format
            d3d_device.SetStreamSource(0, Nebula_VB, 0)
            d3d_device.DrawPrimitives(PrimitiveType.LineStrip, 0, 16)
        Next

        d3d_device.EndScene()


        Try
            d3d_device.Present()
        Catch e As DeviceLostException
            DeviceLost = True
        End Try
        If DeviceLost = True Then AttemptRecovery()
    End Sub

    Sub render_Weapon_Control()
        Weapon_Control_zoom = 1.0F
        Dim scale As Single = CSng(Weapon_Control_zoom)
        Dim pos As PointD
        d3d_device.BeginScene()
        d3d_device.Clear(ClearFlags.Target, Color.Black, 1, 0)
        d3d_sprite.Begin(SpriteFlags.AlphaBlend)
        d3d_device.SetSamplerState(0, SamplerStageStates.MinFilter, TextureFilter.None)
        d3d_device.SetSamplerState(0, SamplerStageStates.MagFilter, TextureFilter.None)
        d3d_sprite.Transform = Matrix.Transformation2D(New Vector2(0, 0), 0, New Vector2(scale, scale), New Vector2(0, 0), 0, New Vector2(0, 0))






        Dim atsize As Integer = CInt(32 * Weapon_Control_zoom)
        Dim ship As Ship = Ship_List(Officer_List(current_player).Location_ID)
        Dim TileMap(,) As Ship_tile
        TileMap = ship.tile_map
        For x = (CInt(view_location_weapon_control.x * Weapon_Control_zoom) \ atsize) - 1 To (CInt(view_location_weapon_control.x * Weapon_Control_zoom) \ atsize) + (screen_size.x \ atsize) + 1
            For y = (CInt(view_location_weapon_control.y * Weapon_Control_zoom) \ atsize) - 1 To (CInt(view_location_weapon_control.y * Weapon_Control_zoom) \ atsize) + (screen_size.y \ atsize) + 1
                pos.x = (x * 32) - view_location_weapon_control.x
                pos.y = (y * 32) - view_location_weapon_control.y
                If x >= 0 AndAlso x <= ship.shipsize.x AndAlso y >= 0 AndAlso y <= ship.shipsize.y Then
                    If TileMap(x, y).type < tile_type_enum.Restricted Then
                        Draw_Ship_Tile(TileMap(x, y).type, TileMap(x, y).sprite, pos, Color.FromArgb(255, 255, 255, 255))
                        If TileMap(x, y).device_tile IsNot Nothing AndAlso Not TileMap(x, y).device_tile.type = device_tile_type_enum.Empty Then
                            If ship.device_list(TileMap(x, y).device_tile.device_ID).type = device_type_enum.weapon Then
                                Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, ship.device_list(TileMap(x, y).device_tile.device_ID).Sprite_Animation_Key, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                                If Weapon_Control__Selected_Group > -1 AndAlso ship.Weapon_control_groups(Weapon_Control__Selected_Group).Connected_Weapons.Contains(TileMap(x, y).device_tile.device_ID) Then
                                    Draw_Ship_Tile(tile_type_enum.Hull_1, 4, pos, Color.FromArgb(100, 0, 0, 255))
                                Else
                                    Draw_Ship_Tile(tile_type_enum.Hull_1, 4, pos, Color.FromArgb(100, 0, 255, 0))
                                End If
                            Else
                                Draw_Device_Tile(TileMap(x, y).device_tile.type, TileMap(x, y).device_tile.sprite, ship.device_list(TileMap(x, y).device_tile.device_ID).Sprite_Animation_Key, pos, TileMap(x, y).device_tile.rotate, TileMap(x, y).device_tile.flip, scale, Color.White)
                            End If
                        End If
                    End If
                End If
            Next
        Next

        d3d_sprite.Transform = Matrix.Identity

        For Each item In External_Menu_Items_Weapon_Control
            If item.Value.enabled = True Then
                d3d_sprite.Draw(button_texture(item.Value.tile_Set), Vector3.Empty, New Vector3(item.Value.bounds.X, item.Value.bounds.Y, 0), item.Value.get_color.ToArgb)
                If Not item.Value.text = "" Then draw_text(item.Value.text, item.Value.bounds, item.Value.align, item.Value.fontcolor, d3d_font(item.Value.font))
            End If
        Next



        d3d_sprite.End()
        d3d_device.EndScene()
        Try
            d3d_device.Present()
        Catch e As DeviceLostException
            DeviceLost = True
        End Try
        If DeviceLost = True Then AttemptRecovery()



    End Sub

    Sub AttemptRecovery()
        Do Until DeviceLost = False
            Try
                d3d_device.TestCooperativeLevel()
            Catch e As DeviceLostException
                Application.DoEvents()
            Catch e As DeviceNotResetException
                Try
                    Dim D3D_PP As New PresentParameters
                    D3D_PP.BackBufferFormat = Format.A8R8G8B8
                    D3D_PP.BackBufferWidth = screen_size.x
                    D3D_PP.BackBufferHeight = screen_size.y
                    D3D_PP.Windowed = windowed
                    D3D_PP.SwapEffect = SwapEffect.Discard
                    d3d_sprite.OnLostDevice()
                    d3d_device.Reset(D3D_PP)
                    d3d_sprite.OnResetDevice()
                    DeviceLost = False
                Catch bug As DeviceLostException
                End Try
            End Try
        Loop
    End Sub


End Module
