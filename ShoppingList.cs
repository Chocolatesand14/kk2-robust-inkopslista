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

    public bool Add(Item item)
    {
        if (Total() + item.Price > budget)
        {
            return false; // Skulle returnera false om budgeten överskrids
        }
        items.Add(item);
        return true; // Varan läggs till om budgeten inte överskrids
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

                // Extra: Budgettaket på första raden i filen
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
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Filen hittades inte. En tom lista används.");
            return;
        }

        string[] lines = text.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] parts = line.Split(';');

            if (parts[0].Trim() == "Budget")
            {
                if (decimal.TryParse(parts[1], out decimal b))
                {
                    budget = b; // Sätter budget från fil
                }
                continue; // Går vidare till nästa rad (inte en vara)  
            }

            items.Add(new Item(parts[1].Trim(), int.Parse(parts[0])));
        }
    }
}
