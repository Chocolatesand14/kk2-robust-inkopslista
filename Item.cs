// One item on the shopping list.
class Item
{
    public string Name { get; set; }
    public int Price { get; set; }

    public Item(string name, int price)
    {
     // Kontrollerar att namnet inte är tomt eller bara whitespace. 
     // Om det är det kastas ett undantag.
     if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Namnet får inte vara tomt.");
        }

     // Kontrollerar att priset inte är negativt.
     // Om det är det kastas ett undantag.
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(price),
                "Priset får inte vara negativt."
        );
    }

        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Name} - {Price} kr";
    }
}

