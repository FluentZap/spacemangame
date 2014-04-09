Public Class PlanetExchange
    Public ExchangeInventory As New Dictionary(Of Item_Enum, Integer)()
    Public HeldMoney As New Dictionary(Of Integer, Integer)() 'Money keld for buildings
    Public SellLists As New Dictionary(Of Item_Enum, Dictionary(Of Integer, Integer))() 'Keeps track of what items are listed by what buildings(for payout)

    Public SellAdvance As New Dictionary(Of Item_Enum, Integer)() 'Keeps track of what items are listed by what buildings(for payout)

    Sub List_Item(ByVal Item As Item_Enum, ByVal BuildingID As Integer)
        If ExchangeInventory.ContainsKey(Item) Then
            ExchangeInventory(Item) += 1
        Else
            ExchangeInventory.Add(Item, 1)
        End If
        If Not SellLists.ContainsKey(Item) Then SellLists.Add(Item, New Dictionary(Of Integer, Integer))
        If Not SellLists(Item).ContainsKey(BuildingID) Then SellLists(Item).Add(BuildingID, 0)

        SellLists(Item)(BuildingID) += 1

    End Sub


    Function Buy_Item(ByVal Item As Item_Enum) As Boolean
        If ExchangeInventory.ContainsKey(Item) AndAlso SellLists.ContainsKey(Item) AndAlso ExchangeInventory(Item) > 0 Then
            ExchangeInventory(Item) -= 1
        Else
            Return False
        End If

        If Not SellAdvance.ContainsKey(Item) Then SellAdvance.Add(Item, 0)

        Dim advance As Integer = 0
        Dim sellId As Integer

        For Each Sell In SellLists(Item)
            If SellAdvance(Item) = advance Then sellId = Sell.Key : Exit For
            advance += 1
        Next

        SellLists(Item)(sellId) -= 1
        If HeldMoney.ContainsKey(sellId) Then
            HeldMoney(sellId) += 10
        Else
            HeldMoney.Add(sellId, 10)
        End If


        If SellLists(Item)(sellId) <= 0 Then SellLists(Item).Remove(sellId)

        SellAdvance(Item) += 1
        If SellAdvance(Item) > SellLists(Item).Count - 1 Then SellAdvance(Item) = 0

        Return True
    End Function


    Function Pickup_Money(ByVal BuildingID As Integer) As Boolean

        If HeldMoney.ContainsKey(BuildingID) Then
            HeldMoney(BuildingID) -= 1
            If HeldMoney(BuildingID) <= 0 Then HeldMoney.Remove(BuildingID)            
            Return True
        Else
            Return False
        End If

    End Function




    Function Check_Exchange(ByVal Item As Item_Enum) As Integer
        If ExchangeInventory.ContainsKey(Item) Then
            Return ExchangeInventory(Item)
        Else
            Return 0
        End If
    End Function

End Class
