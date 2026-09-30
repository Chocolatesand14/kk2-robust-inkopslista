# Robust Inköpslista - Felhantering och felsökning

I programmet fanns det 6 fel som kunde göra att programmet kraschade, gav fel resultat eller dolde att något hade gått fel. Jag testade programmet och det kraschade direkt. Nu ska jag kolla efter felen och rätta dem.

## Fel 1 - Felaktig inmatning

Första fel som jag hittade var `int.Parse()` när användaren skulle skriva menyval, pris och nummer på en vara. 

Om användare skulle skriva bokstäver istället för nummer, så kraschade programmet. 

**Lösning:**
Jag bytte från `int.Parse()` till `int.TryParse()`

**Före:**

int choice = int.Parse(Console.ReadLine());

**Efter:**

 if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Felaktig inmatning. Skriv ett nummer.");
        continue;
    }