using Apps;

List<IUser> users = new List<IUser>();
users.Add(new Trader("Hugo", "123"));

Dictonary<string, List<Item>> trader_item = new Dictonary<string, List<Item>>();

IUser? active_user = null;

bool running = true;

while (running)
{
  Console.Clear();

  if(active_user == null)
  {
    Console.WriteLine("Welcome to the trading post\n1. Loggin\n2. Create account");
    string input = Console.ReadLine();

    switch (input)
    {
      case "1":

      Console.Clear();

      Console.WriteLine("Name: ");
      string username = Console.ReadLine();
      Console.Clear();

      Console.WriteLine("Password: ");
      string password = Console.ReadLine();
      Console.Clear();

      bool exists = false;

      foreach(IUser user in users)
      {
        if(user.TryLoggin(username, password))
        {
          active_user = user;
          exists = true;
          break;
        }  
      }

      if (!exists)
      {
        Console.WriteLine("Wrong username or password\nPress ENTER to continue...");
        Console.ReadLine();
        break;
      }

      if(active_user is Trader t)
      {
        Console.WriteLine("Welcome trader " + t.Name);
        Console.WriteLine("What would you like to do?\n1. List all items\n2. Add items you want to trade\n4. Logout");

        string userInput = Console.ReadLine();

        switch(userInput)
        {
          case "1":
          Console.Clear();
          break;

          case "4":
          Console.Clear();
          active_user = null;
          break;
        }
      }
      break;

      case "2":
        Console.WriteLine("Create a new trader\nEnter a username: ");
        string newUsername = Console.ReadLine();
        Console.WriteLine("Enter a password: ");
        string newPassword = Console.ReadLine();

        Trader newTrader = new Trader(newUsername, newPassword);

        users.Add(newTrader);
        Console.WriteLine("New trader created\nPress ENTER to continue...");
        Console.ReadLine();
        break;
    }
  }
}