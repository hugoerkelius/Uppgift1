namespace Apps;

public class Items //Skapar en klass Items och vad de ska innehålla

{
    public string ItemName;
    public int Amount;
    public string Description;

    public Items(string itemname, int amount, string description)
    {
        ItemName = itemname;
        Amount = amount;
        Description = description;
    }

    public string Info() //Metod för att snabbt hämta items innehåll
    {
        return ItemName + " | " + Amount + " | " + Description;
    }
}