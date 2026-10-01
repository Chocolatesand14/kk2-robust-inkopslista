# Robust Inköpslista - Felhantering och felsökning

I programmet fanns det 6 fel som kunde göra att programmet kraschade, gav fel resultat eller dolde att något hade gått fel. Jag testade programmet och det kraschade direkt. 
Jag gick därför igenom koden för att hitta och rätta felen. Under felsökningen hittade jag även ytterligare fel.

## Fel 1 - Felaktig inmatning av menyval

Första felet som jag hittade var `int.Parse()` när användaren skulle skriva ett menyval.

Om användaren skrev bokstäver istället för ett nummer 
kraschade programmet.

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
        Console.WriteLine("Felaktig inmatning. Skriv in ett nummer.");
        continue;
    }
```
    
## Fel 2 - Tom rad i filen

Andra felet hittade jag genom att köra programmet.
Felmeddelandet visade att felet fanns i `ShoppingList.cs rad 90`.

När programmet läste in inköpslistan kunde det krascha med `IndexOutOfRangeException`.
Det berodde på att programmet försökte läsa en tom rad och sedan komma åt `parts[1]`, som inte fanns.

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
```

**Efter:**

```csharp
if (!int.TryParse(Console.ReadLine(), out int price))
{
    Console.WriteLine("Felaktig inmatning. Skriv ett nummer.");
    continue;
}
```

## Fel 4 - Ta bort en vara som inte finns

I det här felet skulle jag prova att ta bort en vara som inte fanns i menyn.

Felet fanns i `ShoppingList.cs rad 20`.


**Lösning:**
Jag lade till en kontroll så att programmet inte försöker ta bort en vara som inte finns. Istället visas ett felmeddelande.


**Före:**

```csharp
public void RemoveAt(int number)
{
    items.RemoveAt(number - 1);
}
```

**Efter:**

```csharp
public void RemoveAt(int number)
{
    if (number < 1 || number > items.Count)
    {
        Console.WriteLine("Det finns ingen vara med det numret.");
        return;
    }

    items.RemoveAt(number - 1);
}
```

## Fel 5 - items.txt saknas

Jag döpte om `items.txt` till `items-old.txt` och körde programmet igen.

Då fick jag upp ett nytt fel i `ShoppingList.cs rad 90`.


**Lösning:**
Jag fick tänka till lite mer här eftersom programmet behövde kunna hantera att filen saknades. Jag kom på att vi hade gått igenom `try-catch`, som används för att fånga exceptions. I det här fallet använde jag `FileNotFoundException`. Om filen inte finns visas nu ett felmeddelande och programmet fortsätter med en tom lista utan att krascha.


**Före:**

```csharp
string text = File.ReadAllText(path);
```

**Efter:**

```csharp
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
```

## Fel 6 - Felaktig totalsumma

Jag har lagt till varor, men den räknar inte med alla varor. Just nu räknar den inte med den som är överst i listan.


**Lösning:**
Jag ändrade startvärdet i `for`-loopen från `1` till `0`. Listan börjar på index `0`, så nu räknas även den första varans pris med i totalsumman.


**Före:**

```csharp
for (int i = 1; i < items.Count; i++)
```

**Efter:**

```csharp
for (int i = 0; i < items.Count; i++)
```

## Fel 7 - Felaktig inmatning vid borttagning

Jag hittade även ett fel när jag skulle ta bort en vara från listan.

Om jag skrev en bokstav istället för ett nummer kraschade programmet eftersom `int.Parse()` användes.


**Lösning:**
Jag bytte från `int.Parse()` till `int.TryParse()`. Om användaren skriver något annat än ett nummer visas ett felmeddelande och programmet fortsätter istället för att krascha.


**Före:**

```csharp
int number = int.Parse(Console.ReadLine());
list.RemoveAt(number);
```

**Efter:**

```csharp
if (!int.TryParse(Console.ReadLine(), out int number))
{
    Console.WriteLine("Felaktig inmatning. Skriv in ett nummer.");
    continue;
}

list.RemoveAt(number);
```

## Fel 8 - Tom catch vid sparning

Jag hittade ett fel i `ShoppingList.cs` i metoden `Save()`.

Det fanns en tom catch, vilket gjorde att programmet inte visade något felmeddelande om sparningen misslyckades. Programmet skrev även ut att listan var sparad fast sparningen kunde ha misslyckats.


**Lösning:**
Jag ändrade `catch` så att den fångar `IOException` och visar ett felmeddelande om sparningen misslyckas.Jag flyttade också `"Listan är sparad."` till `try`, så att meddelandet bara visas om sparningen lyckas.


**Före:**

```csharp
try
{
    File.WriteAllText(path, string.Join("\r\n", lines) + "\r\n");
}
catch
{
}

Console.WriteLine("Listan är sparad.");
```

**Efter:**

```csharp
try
{
    File.WriteAllText(path, string.Join("\r\n", lines) + "\r\n");
    Console.WriteLine("Listan är sparad.");
}
catch (IOException)
{
    Console.WriteLine("Det gick inte att spara listan.");
}
```