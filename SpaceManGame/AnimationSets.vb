Module AnimationSets

    MustInherit Class Animation
        Public Sprite() As Integer
        Public interval As Integer
    End Class



    Class Generator
        Inherits Animation 
        Sub New()
            interval = 3
            ReDim Sprite(2)
            Sprite(0) = 0
            Sprite(1) = 1
        End Sub
    End Class


End Module