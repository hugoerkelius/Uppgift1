namespace Apps;

class Items

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

    public string Info()
    {
        return ItemName + " | " + Amount + " | " + Description;
    }
}