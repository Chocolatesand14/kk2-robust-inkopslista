# Robust Inköpslista - Felhantering och felsökning

I programmet fanns det 6 fel som kunde göra att programmet kraschade, gav fel resultat eller dolde att något hade gått fel. Jag testade programmet och det kraschade direkt. Nu ska jag kolla efter felen och rätta dem.

## Fel 1 - Felaktig inmatning av menyval

Första fel som jag hittade var `int.Parse()` när användaren skulle skriva menyval, pris och nummer på en vara. 

Om användare skulle skriva bokstäver istället för nummer, så kraschade programmet. 

**Lösning:**
Jag bytte från `int.Parse()` till `int.TryParse()`. Om användaren skriver något annat än ett nummer visas nu ett felmeddelande och programmet fortsätter istället för att krascha.

**Före:**

```csharp
int choice = int.Parse(Console.ReadLine());
```

**Efter:**

```csharp
 if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Felaktig inmatning. Skriv ett nummer.");
        continue;
    }
```
    

## Fel 2 - Tom rad i filen

Andra felet hittade jag genom att köra programmet.
Då dök det upp att det var något med fel `ShoppingList.cs rad 90`.

När programmet läste in inköpslistan kunde det krascha med `IndexOutOfRangeException`.
Vilket berodde på att programmet försökte läsa en tom rad, sedan försökte den komma åt `[1]`, som inte fanns.

**Lösning:**
Jag lade till `string.IsNullOrWhiteSpace(line)`, för att kontrollera om raden är tom. Om den skulle vara tom används continue för att hoppa över raden.

Nu ska inte programmet krascha om det finns en tom rad i filen.

**Före:**

```csharp
foreach (string line in lines)
{
    string[] parts = line.Split(';');
    items.Add(new Item(parts[1], int.Parse(parts[0])));
}
```

**Efter:**

```csharp
foreach (string line in lines)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        continue;
    }

    string[] parts = line.Split(';');
    items.Add(new Item(parts[1], int.Parse(parts[0])));
}
```

## Fel 3 - Felaktig inmatning av pris

Tredje felet dök upp när jag skrev in en bokstav istället för nummer när programmet frågade om ett pris.

Felet fanns i `Program.cs rad 27`.


**Lösning:**
Jag bytte från `int.Parse()` till `int.TryParse()`. Nu visas ett felmeddelande om användaren skriver något annat än ett nummer.


**Före:**

```csharp
int price = int.Parse(Console.ReadLine());
}
```

**Efter:**

```csharp
if (!int.TryParse(Console.ReadLine(), out int price))
{
    Console.WriteLine("Felaktig inmatning. Skriv ett nummer.");
    continue;
}
```