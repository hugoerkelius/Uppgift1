namespace Apps;

public class Trader : IUser
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

  public bool TryLogin(string username, string password)
  {
    return username == Username && password == Password;
  }

  public void AddItem(string itemName, int amount, string description)
  {
    Items inList = null;
    foreach(var i in Inventory)
    {
      if(i.ItemName == itemName)
      {
        inList = i;
        break;
      }
    }
    Inventory.Add(new Items(itemName, amount, description));
  }

  public void ListItems()
  {
    if(Inventory.Count == 0)
    {
      Console.WriteLine("No items in the inventory");
    }
    else
    {
      foreach (var item in Inventory)
      {
        Console.WriteLine(item.Info());
      }
    }
  }
}