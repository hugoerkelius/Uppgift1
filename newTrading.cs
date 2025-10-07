namespace Apps;

public enum TradeStatus //Förbestämda värden för vad en trade kan vara
{
  Pending,
  Accepted,
  Denied,
}

public class Trade //Skapar klassen för trades som innehåller värden för vem som är involverade i traden och vilka items samt status på traden
{
  private Trader fromTrader;
  private Trader toTrader;
  private ItemInfo offered;
  private ItemInfo requested;
  private TradeStatus status;//Nyttjar ENUM ovan för att definera status på trade

  public Trade(Trader fromT, Trader toT, Items reqItem, Items offerItem)
  {
    fromTrader = fromT;
    toTrader = toT;
    offered = new ItemInfo(offerItem.ItemName, offerItem.Amount, offerItem.Description);//Definera vad offered item(det användare skickar) ska innehålla för data,
    requested = new ItemInfo(reqItem.ItemName, reqItem.Amount, reqItem.Description);//Definerar vad requested item(vad användaren vill ha) ska innehålla för data
    status = TradeStatus.Pending;//Sätter status på trade till pending tills den ändras i accepet eller deny
  }

  public Trade(Trader fromT, Trader toT, ItemInfo offerItem, ItemInfo reqItem, TradeStatus stat)
  {
    fromTrader = fromT;
    toTrader = toT;
    offered = offerItem;
    requested = reqItem;
    status = stat;
  }

  public Trader GetFromTrader() => fromTrader;//Returnar vem som har skickat/skapat traden
  public Trader GiveToTrader() => toTrader;//Returnar vem som motar traden
  public TradeStatus GetStatus() => status;//Ger status på trade
  public ItemInfo GetOffered() => offered;//Return på eget item som ska tradeas med innehållet från ItemInfo
  public ItemInfo GetRequested() => requested;// -||- Men på det item man vill ha 

  public void Accept() //Metod för att acceptera trade
  {
    if (status != TradeStatus.Pending) return; //Kontrollerar att status fortfarande är pending
    status = TradeStatus.Accepted;//ändrar pending till accepeted
    var fromInv = fromTrader.Inventory;//"Flyttar" item från olika användares inventorys
    var toInv = toTrader.Inventory;
    var fromItem = fromInv.Find(i => i.ItemName == offered.GetName());
    var toItem = toInv.Find(i => i.ItemName == requested.GetName());

    if (fromItem != null)//Måste innehållet ett värde
    {
      fromInv.Remove(fromItem);//Tar bort valt item från den som skickas inventory
      toInv.Add(fromItem);//Lägger till valt item som blir requestat
    }
    if (toItem != null)//Måste innehålla ett värde
    {
      toInv.Remove(toItem);
      fromInv.Add(toItem);
    }
  }
  public void Deny()//Ändrar från pending till denied om den är pending
  {
    if (status == TradeStatus.Pending)
    {
      status = TradeStatus.Denied;
    }
  }
}
  public class ItemInfo//Klass för att hämta infon på respektive items
  {
    private string name;
    private int amount;
    private string description;

    public ItemInfo(string n, int a, string d)//När jag kallar på ItemInfo ska den ge denna data
    {
      name = n;
      amount = a;
      description = d;
    }
    public string GetName() => name;
    public int GetAmount() => amount;
    public string GetDescription() => description;

    public string Show() => $"{name} | {amount} | {description}";
  }

