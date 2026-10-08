// Holds the items and takes care of loading and saving them.
using System.Data.Common;

class ShoppingList
{
    private List<Item> items = new List<Item>();
    private string path;
    private decimal budget; // Nytt fält för budget

    public ShoppingList(string path, decimal budget)
    {
        this.path = path;
        this.budget = budget; // Sätter budget när listan skapas
    }

    public void Add(Item item)
    {
        // Kontrollera om budgettaket överskrids
        if (Total() + item.Price > budget)
        {   // Extra: Visar ett fel om budgeten överskrids
            throw new BudgetExceededException("Du har överskridit budgeten.");
        }
        items.Add(item); // Varan läggs till om budgeten inte överskrids
    }

    public List<Item> GetItems()
    {
        return items;
    }

    // Removes the item the user sees as number 1, 2, 3 ...
    public void RemoveAt(int number)
    {
        if (number < 1 || number > items.Count)
        {
            Console.WriteLine("Det finns ingen vara med det numret.");
            return;
        }
        items.RemoveAt(number - 1);
    }

    // Adds up the price of every item on the list.
    public decimal Total()
    {
        decimal sum = 0;

        for (int i = 0; i < items.Count; i++)
        {
            sum += items[i].Price;
        }

        return sum;
    }

    // Looks up an item by its name. Returns null if there is no such item.
    public Item Find(string name)
    {
        foreach (Item item in items)
        {
            if (item.Name == name)
            {
                return item;
            }
        }

        return null;
    }

    public void Print()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {items[i]}");
        }

        Console.WriteLine($"Totalt: {Total()} kr");
    }


    // Writes one item per line, as "price;name".
    public void Save()
    {
        try
        {
            // Extra: Using gör att filen stängs automatiskt när programmet har skrivit klart.
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Extra: Sparar budgettaket på första raden i filen.
                writer.WriteLine($"Budget;{budget}");

                foreach (Item item in items)
                {
                    writer.WriteLine($"{item.Price};{item.Name}");
                }
            }
            Console.WriteLine("Listan är sparad.");
        }
        catch (IOException)
        {
            Console.WriteLine("Det gick inte att spara listan.");
        }
    }

    // Reads the file back into the list.
    public void Load()
    {
        string text;
        try
        {
            text = File.ReadAllText(path);

            string[] lines = text.Split('\n');

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] parts = line.Split(';');
                if (parts.Length != 2)
                {
                    Console.WriteLine("Felaktig rad i filen.");
                    continue;
                }

                if (parts[0].Trim() == "Budget")
                {
                    if (decimal.TryParse(parts[1], out decimal b) && b >= 0)
                    {
                        budget = b; // Sätter budget från fil
                    }
                    continue; // Går vidare till nästa rad (inte en vara)  
                }
                if (!int.TryParse(parts[0], out int price) || price < 0)
                {
                    Console.WriteLine("Felaktigt pris i filen.");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(parts[1]))
                {
                    Console.WriteLine("Varans namn får inte vara tomt.");
                    continue;
                }

                items.Add(new Item(parts[1].Trim(), price));
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Filen hittades inte. En tom lista används.");
        }
        catch (IOException)
        {
            Console.WriteLine("Det gick inte att läsa filen.");
        }
    }
}

