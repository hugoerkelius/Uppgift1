using Apps;

List<Iuser> users = new List<Iuser>();
users.Add(new Trader("Hugo", "123"));

Iuser? active_user = null;

bool running = true;

while (running)
{
  Console.Clear();

  if(active_user == null)
  {
    Console.WriteLine("Welcome to the trading post\nPress ENTER to continue...");
    Console.ReadLine();
    Console.Clear();

    Console.WriteLine("Username: ");
    string username = Console.ReadLine();
    Console.Clear();

    Console.WriteLine("Password: ");
    string password = Console.ReadLine();
    Console.Clear();

    foreach(Iuser user in users)
    {
      if(users.TryLoggin(username, password))
      {
        active_user = user;
        break;
      }  
    }
  }
  else
  {
    if(active_user is Trader t)
    {
      Console.WriteLine("Welcome trader " + t.Username);
      Console.WriteLine("What would you like to do?\n1. List all items\n2. Add items you want to trade\n4. Logout");

      string input = Console.ReadLine();

      switch(input)
      {
        case "4":
        Console.Clear();
        break;
      }
    }
  }
}
