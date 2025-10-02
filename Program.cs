using Apps;

List<IUser> users = new List<IUser>();

if(File.Exists("traders.csv"))
{
  string[] lines = File.ReadAllLines("traders.csv");
  foreach (string line in lines)
  {
    string[] traderData = line.Split(',');
    if(traderData.Length == 3)
    {
      string username = traderData[0];
      string name = traderData[1];
      string password = traderData[2];
      users.Add(new Trader(username, name, password));
    }
  }
}

if(File.Exists("items.csv"))
{
  string[] lines = File.ReadAllLines("items.csv");
  foreach (string line in lines)
  {
    string[] itemdata = line.Split(',');
    if(itemdata.Length >= 4)
    {
      string username = itemdata[0];
      string itemName = itemdata[1];
      int amount = int.Parse (itemdata[2]);
      string description = itemdata[3];

      var trader = users.Find(u => u is Trader t && t.Username == username) as Trader;
      if(trader != null)
      {
        trader.AddItem(itemName, amount, description);
      }
    }
  }
}

users.Add(new Trader("Hugo","hugo1", "123"));

IUser? active_user = null;

bool running = true;

while (running)
{
  // Console.Clear();

  if(active_user == null)
  {
    Console.WriteLine("Welcome to the trading post\n1. Loggin\n2. Create account");
    string input = Console.ReadLine();

    switch (input)
    {
      case "1":

      Console.Clear();

      Console.WriteLine("Username: ");
      string username = Console.ReadLine();
      Console.Clear();

      Console.WriteLine("Password: ");
      string password = Console.ReadLine();
      Console.Clear();

      bool exists = false;

      foreach(IUser user in users)
      {
        if(user is Trader t && t.TryLogin(username, password))
        {
            active_user = t;
            exists = true;
            break;
        }    
      }

      if(!exists)
      {
        Console.WriteLine("Wrong username or password\nPress ENTER to continue...");
        Console.ReadLine();
        break;
      }
        while (exists)
        {
          if (active_user is Trader trader)
          {
            Console.WriteLine("Welcome trader " + trader.Name);
            Console.WriteLine("What would you like to do?\n1. List own items\n2. Add items to your inventory\n3. List all traders items\n4. Trade\n5. Logout");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
              case "1":
                Console.Clear();
                trader.ListItems();
                Console.WriteLine("Press ENTER to continue");

                Console.ReadLine();
                break;

              case "2":
                Console.Clear();
                Console.WriteLine("Enter name of the item: ");
                string itemName = Console.ReadLine();

                Console.WriteLine("Enter amount of items: ");
                int amount;
                while (!int.TryParse(Console.ReadLine(), out amount) || amount <= 0)
                {
                  Console.WriteLine("Invalid amount. Enter a valid number");
                }

                Console.WriteLine("Enter a description: ");
                string description = Console.ReadLine();

                trader.AddItem(itemName, amount, description);

                File.AppendAllLines("items.csv", new string[] { $"{trader.Username},{itemName},{amount},{description}" });

                Console.WriteLine("Item added to inventory\nPress ENTER to continue");
                Console.ReadLine();
                break;

              case "5":
                Console.Clear();
                active_user = null;
                exists = false;
                break;

              default:
                Console.WriteLine("Invalid input. Press ENTER to continue");
                Console.ReadLine();
                break;
            }
          }
        }          
      break;

      case "2":
        Console.Clear();
        Console.WriteLine("Create a new trader");
        string newUsername = "";
        bool usernameExist = false;

        while (true)
        {
          Console.WriteLine("Enter a username");
          newUsername = Console.ReadLine();

          usernameExist = false;

          foreach(IUser user in users) 
          {
            if(user is Trader t && t.Username.ToLower() == newUsername.ToLower())
            {
              usernameExist = true;
              break;
            }
          }  
        
        
        if(usernameExist)
          {
            Console.Clear();
            Console.WriteLine("Username already exists\nPress ENTER to continue");
            Console.ReadLine();
            Console.Clear();
            
          }
        else
          {
          Console.Clear();
          break;
          }

        }

        Console.Clear();
        Console.WriteLine("Enter your name: ");
        string newName = Console.ReadLine();
        Console.Clear();
        
        string newPassword = "";

        while (true)
        {
          Console.WriteLine("Enter a password with atleast 8 characters");
          newPassword = Console.ReadLine();

          if(newPassword.Length >= 8)
          {
            break;
          }
          Console.WriteLine("Password must contain atleast 8 characters\nPress enter to continue");
          Console.ReadLine();
          Console.Clear();
        }
        

        Trader newTrader = new Trader(newUsername, newName, newPassword);

        users.Add(newTrader);

        List<string> newLine = new List<string> { $"{newTrader.Username},{newTrader.Name},{newTrader.Password}" };
        File.AppendAllLines("traders.csv", newLine);

        Console.WriteLine("New trader created\nPress ENTER to continue...");
        Console.ReadLine();
        break;
    }
  }
}