using Apps;

class Program
{
    static void Main()
    {
        List<IUser> users = new List<IUser>();  //Skapar en lista för mina användare
        List<Trade> trades = new List<Trade>(); //Skapar en lista för mina trades

        if (File.Exists("traders.csv")) // vid start frågar om filen traders.csv existerar och om den innehåller användarnamn, namn och lösenord skapas en användare
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

        if (File.Exists("items.csv"))   //vid start frågar om filen items.scv existerar och om den innehåller rätt mängd data sparas den till respektive användare
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

        if (File.Exists("trades.csv"))  //Vid start av program laddas filen trades.csv och laddar data gällande genomförda och nekade trades, från och till andra användare.
        {
            foreach (var line in File.ReadAllLines("trades.csv"))
            {
                string[] tradeData = line.Split(',');
                if (tradeData.Length == 9)
                {
                  string fromUser = tradeData[0];
                  string toUser = tradeData[1];
                  string ownName = tradeData[2];    //own = eget item du tradat
                  int ownAmount = int.Parse(tradeData[3]);
                  string ownDesc = tradeData[4];
                  string reqName = tradeData[5];    //req = requested = item du vill trade
                  int reqAmount = int.Parse(tradeData[6]);
                  string reqDesc = tradeData[7];
                  TradeStatus status = Enum.Parse<TradeStatus>(tradeData[8]); //förbestämda tre värden, Pending, Accepted och Denied hämtas här kopplat till din trade.

                    var fromTrader = users.Find(u => u is Trader t && t.Username == fromUser) as Trader;
                    var toTrader = users.Find(u => u is Trader t && t.Username == toUser) as Trader;

                    if (fromTrader != null && toTrader != null)
                    {
                        trades.Add(new Trade(fromTrader, toTrader, ownName, ownAmount, ownDesc, reqName, reqAmount, reqDesc, status));
                    }
                }
            }
        }

        void UpdateItems(List<IUser> users) //Uppdaterar Items.csv, används när man lägger till nya items och uppdaterar när man loggar ut om eventuella ändringar har skett.
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

        IUser? active_user = null; //Vid start av programmet är ingen användare inloggad

        bool running = true;

        while (running)
        {
            if (active_user == null) //Ingen användare inloggad = Inlogg meny
            {
                Console.WriteLine("Welcome to the trading post\n1. Loggin\n2. Create account");
                string input = Console.ReadLine();

                switch (input) //Väljer antingen login eller create account
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

                        foreach (IUser user in users) //Frågor om det finns en användare registrerad i listan
                        {
                            if (user is Trader t && t.TryLogin(username, password))
                            {
                                active_user = t;
                                exists = true;
                                break;
                            }
                        }

                        if (!exists)//Det finns inget konto med dessa användarnamn eller lösenord
                        {
                            Console.WriteLine("Wrong username or password\nPress ENTER to continue...");
                            Console.ReadLine();
                            break;
                        }
                        while (exists)//Det finns ett konto med dessa uppgifter i listan av users
                        {
                            if (active_user is Trader trader)//Den aktiva användaren är en av klassen trader
                            {
                                Console.WriteLine("Welcome trader " + trader.Name);
                                Console.WriteLine("What would you like to do?\n1. List own items\n2. Add items to your inventory\n3. Trade\n4. List avalible trades\n5. List completed trades\n6. Logout");

                                string userInput = Console.ReadLine();

                                switch (userInput)//Användaren väljer i menyn vad som ska göras
                                {
                                    case "1"://Kallar på listan av alla items som användaren har
                                        Console.Clear();
                                        trader.ListItems();
                                        Console.WriteLine("Press ENTER to continue");
                                        Console.ReadLine();
                                        Console.Clear();
                                        break;

                                    case "2"://Lägger till ett item med ett namn, mängd och en beskrivning av item
                                        Console.Clear();
                                        Console.WriteLine("Enter name of the item: ");
                                        string itemName = Console.ReadLine();

                                        Console.WriteLine("Enter amount of items: ");
                                        int amount;
                                        while (!int.TryParse(Console.ReadLine(), out amount) || amount <= 0)//Måste vara mer än ett item
                                        {
                                            Console.WriteLine("Invalid amount. Enter a valid number");
                                        }

                                        Console.WriteLine("Enter a description: ");
                                        string description = Console.ReadLine();

                                        trader.AddItem(itemName, amount, description);//Klassen trader använder metoden AddItem för att lägga till värden/argument 

                                        UpdateItems(users);//Anropar metoden UpdateItems med argument users

                                        Console.WriteLine("Item added to inventory\nPress ENTER to continue");
                                        Console.ReadLine();
                                        Console.Clear();
                                        break;

                                    case "3"://Visar tillgängliga traders och vad de har i inventory
                                        Console.Clear();
                                        Console.WriteLine("Avalible traders and their items: ");
                                        foreach (var u in users) //Loopar igenom alla traders och listar alla traders förutom den som är inloggad
                                        {
                                            if (u is Trader otherTrader && otherTrader != trader)
                                            {
                                                Console.WriteLine($"\nTrader: {otherTrader.Username} ({otherTrader.Name})");
                                                otherTrader.ListItems();
                                            }
                                        }
                                        Console.WriteLine("\nEnter username of who you want to trade with: ");
                                        string TargetTrader = Console.ReadLine();//Användaren uppger ett användarnamn 
                                        Console.Clear();

                                        var tradePartner = users.Find(u => u is Trader t && t.Username == TargetTrader) as Trader; //Går igenom listan av traders för att se att användarnamn ovan finns i listan av traders

                                        if (tradePartner == null || tradePartner == trader) //Om användarnamnet finns eller du uppger eget användarnamn får du felmeddelande
                                        {
                                            Console.WriteLine("Invalid trader inputed\nPress ENTER to continue");
                                            Console.ReadLine();
                                            Console.Clear();
                                            break;
                                        }

                                        Console.WriteLine("Your items: ");//Listar egna items
                                        trader.ListItems();
                                        Console.WriteLine("Enter name of the item you want to trade away: ");
                                        string tradeItem = Console.ReadLine();
                                        Console.Clear();

                                        var offerItem = trader.Inventory.Find(i => i.ItemName == tradeItem);//Kontrollerar att det items du vill trade finns i listan av items
                                        if (offerItem == null)
                                        {
                                            Console.WriteLine("Invalid item\nPress ENTER to continue");
                                            Console.ReadLine();
                                            Console.Clear();
                                            break;
                                        }

                                        Console.WriteLine($"{tradePartner.Name} items: ");//Listar motparts items
                                        tradePartner.ListItems();
                                        Console.WriteLine("Enter item you want to get from trade: ");
                                        string requestName = Console.ReadLine(); //Uppger det item du vill trade till ditt eget inventory
                                        Console.Clear();

                                        var reqItem = tradePartner.Inventory.Find(i => i.ItemName == requestName);//Kontrollerar att det item du vill trade existerar i deras lista
                                        if (reqItem == null)
                                        {
                                            Console.WriteLine("Invalid item\nPress ENTER to continue");
                                            Console.ReadLine();
                                            Console.Clear();
                                            break;
                                        }

                                        Trade newTrade = new Trade(trader, tradePartner, offerItem, reqItem);//Skapar ett nytt objekt av klassen Trade med namn NewTrade av ovan införd data som sedan lägger till det i listan trades
                                        trades.Add(newTrade);

                                        Console.WriteLine("Trade offer created\nPress ENTER to continue");
                                        Console.ReadLine();
                                        Console.Clear();
                                        break;

                                    case "4":
                                        Console.Clear();
                                        var inc = trades.FindAll(t => t.GiveTrader() == trader && t.GetStatus() == TradeStatus.Pending);//Frågar om det existerar några inkommande trades till användaren
                                        if (inc.Count == 0)//Finns det inga inkommande trades meddelas det
                                        {
                                            Console.WriteLine("No avalible trades");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Avalible trades: ");
                                            for (int i = 0; i < inc.Count; i++)//Existerar det trades kommer dessa listas här från 1 uppåt
                                            {
                                                var tradeReq = inc[i];//Array för inc
                                                Console.WriteLine($"{i + 1}. {tradeReq.GetFromTrader().Username} offers {tradeReq.OfferedInfo()} for your {tradeReq.ReqInfo()}");
                                                //Här listas innehållet av en trade, vem som har skickat det och vad som önskas tradeas.
                                            }

                                            Console.WriteLine("Enter index of the trade you want to handle");
                                            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= inc.Count)//Användare skriver in det indexnr på vilken trade man vill hantera
                                                Console.Clear();
                                            {
                                                var chosen = inc[choice - 1];

                                                Console.WriteLine("Type Accept to accept trade or deny to deny trade");
                                                string respons = Console.ReadLine();

                                                if (respons.ToLower() == "accept")
                                                {
                                                    chosen.Accept();
                                                    UpdateItems(users);//Uppdaterar listan av items så korrekt användare har item
                                                    Console.WriteLine("Trade accepted");
                                                }
                                                else
                                                {
                                                    chosen.Deny();
                                                    Console.WriteLine("Trade denied");
                                                }

                                                File.WriteAllLines("trades.csv", trades.Select(t => $"{t.GetFromTrader().Username},{t.GiveTrader().Username},{t.OfferedName()},{t.OfferedAmount()},{t.OfferedDesc()},{t.ReqName()},{t.ReqAmount()},{t.ReqDesc()},{t.GetStatus()}"));
                                            }   //Sparar vem som har tradeat vad i en csv fil där namn på mottagande och givande trader finns med och vad det är för namn, amount och desc på item man får och byter bort
                                        }
                                        Console.WriteLine("Press ENTER to continue");
                                        Console.ReadLine();
                                        break;

                                    case "5":
                                        Console.Clear();
                                        Console.WriteLine("Trade history: ");
                                        var history = trades.FindAll(t => (t.GetFromTrader() == trader || t.GiveTrader() == trader) && t.GetStatus() != TradeStatus.Pending);
                                        //Letar efter genomförda trades
                                        if (history.Count == 0)//finns inga genomförda trades
                                        {
                                            Console.WriteLine("No trade has been done");
                                        }
                                        else
                                        {
                                            foreach (var trade in history)//Hämtar värden på respektive trade som har genomförts. Status på genomförd trade (Accepted eller denied), användarnamn på de som genomfört trade och vad det är för items som har tradeats från vem
                                            {
                                                string status = trade.GetStatus().ToString();
                                                string partner = trade.GetFromTrader() == trader ? trade.GiveTrader().Username : trade.GetFromTrader().Username;
                                                string order = trade.GetFromTrader() == trader ? "You offered" : "You recivied";

                                                Console.WriteLine($"[{status}] {order} {partner}: \nTheir item: {trade.ReqInfo()} \nYour item: {trade.OfferedInfo()}\n----");
                                            }
                                        }
                                        Console.WriteLine("Press ENTER to continue");
                                        Console.ReadLine();
                                        Console.Clear();
                                        break;

                                    case "6": //Loggar ut och uppdaterar listan av items
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

                    case "2"://Funktion för att skapa nya traders
                        Console.Clear();
                        Console.WriteLine("Create a new trader");
                        string newUsername = "";
                        bool usernameExist = false;//Används för att se om det redan existerar ett konto med det användarnamnet

                        while (true) //Bryts inte förens unikt användarnamn har skrivits in
                        {
                            Console.WriteLine("Enter a username");
                            newUsername = Console.ReadLine();

                            usernameExist = false;//Bryter loopen

                            foreach (IUser user in users)//Kontrollerar alla users användarnamn för att se om det redan finns
                            {
                                if (user is Trader t && t.Username.ToLower() == newUsername.ToLower())
                                {
                                    usernameExist = true;
                                    break;
                                }
                            }
                            if (usernameExist)//Stannar kvar i loopen då det inte är ett unikt anvnamn
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
                            Console.WriteLine("Enter a password with atleast 8 characters"); //Skapar ett password med ett max anatal karktärer
                            newPassword = Console.ReadLine();

                            if (newPassword.Length >= 8)
                            {
                                break;
                            }
                            Console.WriteLine("Password must contain atleast 8 characters\nPress enter to continue");
                            Console.ReadLine();
                            Console.Clear();
                        }

                        //Sparar trader i en en csv fil
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