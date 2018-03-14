SELECT Pips.*, Item.Ratings FROM Pips inner join Item ON Item.ID = Pips.Item_ID Where Pips.PipsType = 'Medium'
