using Apps;

internal class Program
{
    private static void Main(string[] args)
    {
        List<IUser> users = new List<IUser>();
        List<Trade> trades = new List<Trade>();

        if (File.Exists("traders.csv"))
        {
            string[] lines = File.ReadAllLines("traders.csv");
            foreach (string line in lines)
            {
                string[] traderData = line.Split(',');
                if (traderData.Length == 3)
                {
                    string username = traderData[0];
                    string name = traderData[1];
                    string password = traderData[2];
                    users.Add(new Trader(username, name, password));
                }
            }
        }

        if (File.Exists("items.csv"))
        {
            string[] lines = File.ReadAllLines("items.csv");
            foreach (string line in lines)
            {
                string[] itemdata = line.Split(',');
                if (itemdata.Length >= 4)
                {
                    string username = itemdata[0];
                    string itemName = itemdata[1];
                    int amount = int.Parse(itemdata[2]);
                    string description = itemdata[3];

                    var trader = users.Find(u => u is Trader t && t.Username == username) as Trader;
                    if (trader != null)
                    {
                        trader.AddItem(itemName, amount, description);
                    }
                }
            }
        }

        if (File.Exists("trades.csv"))
        {
            foreach (var line in File.ReadAllLines("trades.csv"))
            {
                string[] tradeData = line.Split(',');
                if (tradeData.Length == 9)
                {
                  string fromUser = tradeData[0];
                  string toUser = tradeData[1];
                  string ownName = tradeData[2];
                  int ownAmount = int.Parse(tradeData[3]);
                  string ownDesc = tradeData[4];
                  string reqName = tradeData[5];
                  int reqAmount = int.Parse(tradeData[6]);
                  string reqDesc = tradeData[7];
                  TradeStatus status = Enum.Parse<TradeStatus>(tradeData[8]);

                    var fromTrader = users.Find(u => u is Trader t && t.Username == fromUser) as Trader;
                    var toTrader = users.Find(u => u is Trader t && t.Username == toUser) as Trader;

                    if (fromTrader != null && toTrader != null)
                    {
                        trades.Add(new Trade(fromTrader, toTrader, ownName, ownAmount, ownDesc,
                                             reqName, reqAmount, reqDesc, status));
                    }
                }
            }
        }

        void UpdateItems(List<IUser> users)
        {
            List<string> lines = new List<string>();
            foreach (var u in users)
            {
                if (u is Trader t)
                {
                    foreach (var item in t.Inventory)
                    {
                        lines.Add($"{t.Username},{item.ItemName},{item.Amount},{item.Description}");
                    }
                }
            }
            File.WriteAllLines("items.csv", lines);
        }

        IUser? active_user = null;

        bool running = true;

        while (running)
        {
            // Console.Clear();

            if (active_user == null)
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

                        foreach (IUser user in users)
                        {
                            if (user is Trader t && t.TryLogin(username, password))
                            {
                                active_user = t;
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
                        while (exists)
                        {
                            if (active_user is Trader trader)
                            {
                                Console.WriteLine("Welcome trader " + trader.Name);
                                Console.WriteLine("What would you like to do?\n1. List own items\n2. Add items to your inventory\n3. Trade\n4. List avalible trades\n5. List completed trades\n6. Logout");

                                string userInput = Console.ReadLine();

                                switch (userInput)
                                {
                                    case "1":
                                        Console.Clear();
                                        trader.ListItems();
                                        Console.WriteLine("Press ENTER to continue");
                                        Console.ReadLine();
                                        Console.Clear();
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
                                        Console.Clear();
                                        break;

                                    case "3":
                                        Console.Clear();
                                        Console.WriteLine("Avalible traders and their items: ");
                                        foreach (var u in users)
                                        {
                                            if (u is Trader otherTrader && otherTrader != trader)
                                            {
                                                Console.WriteLine($"\nTrader: {otherTrader.Username} ({otherTrader.Name})");
                                                otherTrader.ListItems();
                                            }
                                        }
                                        Console.WriteLine("\nEnter username of who you want to trade with: ");
                                        string TargetTrader = Console.ReadLine();
                                        Console.Clear();

                                        var tradePartner = users.Find(u => u is Trader t && t.Username == TargetTrader) as Trader;

                                        if (tradePartner == null || tradePartner == trader)
                                        {
                                            Console.WriteLine("Invalid trader inputed\nPress ENTER to continue");
                                            Console.ReadLine();
                                            Console.Clear();
                                            break;
                                        }

                                        Console.WriteLine("Your items: ");
                                        trader.ListItems();
                                        Console.WriteLine("Enter name of the item you want to trade away: ");
                                        string tradeItem = Console.ReadLine();
                                        Console.Clear();

                                        var offerItem = trader.Inventory.Find(i => i.ItemName == tradeItem);
                                        if (offerItem == null)
                                        {
                                            Console.WriteLine("Invalid item\nPress ENTER to continue");
                                            Console.ReadLine();
                                            Console.Clear();
                                            break;
                                        }

                                        Console.WriteLine($"{tradePartner.Name} items: ");
                                        tradePartner.ListItems();
                                        Console.WriteLine("Enter item you want to get from trade: ");
                                        string requestName = Console.ReadLine();
                                        Console.Clear();

                                        var reqItem = tradePartner.Inventory.Find(i => i.ItemName == requestName);
                                        if (reqItem == null)
                                        {
                                            Console.WriteLine("Invalid item\nPress ENTER to continue");
                                            Console.ReadLine();
                                            Console.Clear();
                                            break;
                                        }

                                        Trade newTrade = new Trade(trader, tradePartner, offerItem, reqItem);
                                        trades.Add(newTrade);

                                        Console.WriteLine("Trade offer created\nPress ENTER to continue");
                                        Console.ReadLine();
                                        Console.Clear();
                                        break;

                                    case "4":
                                        Console.Clear();
                                        var inc = trades.FindAll(t => t.GiveTrader() == trader && t.GetStatus() == TradeStatus.Pending);
                                        if (inc.Count == 0)
                                        {
                                            Console.WriteLine("No avalible trades");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Avalible trades: ");
                                            for (int i = 0; i < inc.Count; i++)
                                            {
                                                var tradeReq = inc[i];
                                                Console.WriteLine($"{i + 1}. {tradeReq.GetFromTrader().Username} offers {tradeReq.OfferedInfo()} for your {tradeReq.ReqInfo()}");
                                            }

                                            Console.WriteLine("Enter index of the trade you want to handle");
                                            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= inc.Count)
                                                Console.Clear();
                                            {
                                                var chosen = inc[choice - 1];

                                                Console.WriteLine("Type Accept to accept trade or deny to deny trade");
                                                string respons = Console.ReadLine();

                                                if (respons.ToLower() == "accept")
                                                {
                                                    chosen.Accept();
                                                    UpdateItems(users);
                                                    Console.WriteLine("Trade accepted");
                                                }
                                                else
                                                {
                                                    chosen.Deny();
                                                    Console.WriteLine("Trade denied");
                                                }

                                                File.WriteAllLines("trades.csv", trades.Select(t => $"{t.GetFromTrader().Username},{t.GiveTrader().Username},{t.OfferedName()},{t.OfferedAmount()},{t.OfferedDescription()},{t.ReqName()},{t.ReqAmount()},{t.ReqDescription()},{t.GetStatus()}"));
                                            }
                                        }
                                        Console.WriteLine("Press ENTER to continue");
                                        Console.ReadLine();
                                        break;

                                    case "5":
                                        Console.Clear();
                                        Console.WriteLine("Trade history: ");
                                        var history = trades.FindAll(t => (t.GetFromTrader() == trader || t.GiveTrader() == trader) && t.GetStatus() != TradeStatus.Pending);

                                        if (history.Count == 0)
                                        {
                                            Console.WriteLine("No trade has been done");
                                        }
                                        else
                                        {
                                            foreach (var trade in history)
                                            {
                                                string status = trade.GetStatus().ToString();
                                                string partner = trade.GetFromTrader() == trader ? trade.GiveTrader().Username : trade.GetFromTrader().Username;
                                                string order = trade.GetFromTrader() == trader ? "You offered" : "You recivied";

                                                Console.WriteLine($"[{status}] {order} {partner}: \nTheir item: {trade.ReqInfo()} \nYour item: {trade.OfferedInfo()}");
                                            }
                                        }
                                        Console.WriteLine("Press ENTER to continue");
                                        Console.ReadLine();
                                        Console.Clear();
                                        break;

                                    case "6":
                                        Console.Clear();
                                        UpdateItems(users);
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

                            foreach (IUser user in users)
                            {
                                if (user is Trader t && t.Username.ToLower() == newUsername.ToLower())
                                {
                                    usernameExist = true;
                                    break;
                                }
                            }


                            if (usernameExist)
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

                            if (newPassword.Length >= 8)
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
    }
}