namespace Apps;

public class Trader : IUser //Skapar en klass för traders med namn osv och en egen lista för items
{
  public string Name;
  public string Username;
  public string Password;
  public List<Items> Inventory;

  public Trader(string username, string name, string password)
  {
    Name = name;
    Username = username;
    Password = password;
    Inventory = new List<Items>();
  }

  public bool TryLogin(string username, string password) //Inputvärden från användare ska matcha registrerade användarnamn och lösenord
  {
    return username == Username && password == Password;
  }

  public void AddItem(string itemName, int amount, string description)//Skapar metod för att lägga till items i användarens inventory
  {
    var existing = Inventory.Find(i => i.ItemName == itemName && i.Description == description);
    {
      Inventory.Add(new Items(itemName, amount, description));
    }
  }
  public void ListItems()//Skapar en metod för att lista alla items i inventory
  {
    if (Inventory.Count == 0)//Finns det inget innehåll visas det
    {
      Console.WriteLine("No items in the inventory");
    }
    else
    {
      foreach (var item in Inventory)
      {
        Console.WriteLine(item.Info());//visar vad varje item har för innehåll
      }
    }
  }
}