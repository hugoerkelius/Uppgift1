namespace Apps;

public enum TradeStatus //Förbestämda värden för vad en trade kan vara
{
  Pending,
  Accepted,
  Denied,
}

public class Trade
{
  private Trader fromTrader;
  private Trader toTrader;
  private ItemInfo offered;
  private ItemInfo requested;
  private TradeStatus status;

  public Trade(Trader fromT, Trader toT, Items reqItem, Items offerItem)
  {
    fromTrader = fromT;
    toTrader = toT;
    offered = new ItemInfo(offerItem.ItemName, offerItem.Amount, offerItem.Description);
    requested = new ItemInfo(reqItem.ItemName, reqItem.Amount, reqItem.Description);
    status = TradeStatus.Pending;
  }

  public Trade(Trader fromT, Trader toT, ItemInfo offerItem, ItemInfo reqItem, TradeStatus stat)
  {
    fromTrader = fromT;
    toTrader = toT;
    offered = offerItem;
    requested = reqItem;
    status = stat;
  }

  public void SetStatus(TradeStatus newStatus) => status = newStatus;

  public Trader GetFromTrader() => fromTrader;
  public Trader GiveToTrader() => toTrader;
  public TradeStatus GetStatus() => status;
  public ItemInfo GetOffered() => offered;
  public ItemInfo GetRequested() => requested;

  public string Info()
  {
    return $"From: {fromTrader}, To: {toTrader}, Status: {status}\nOffered: {offered.Show()}\nRequested: {requested.Show()}";
  }

  public void Accept() //Metod för att acceptera trade
  {
    if (status != TradeStatus.Pending) return; //Kontrollerar att status fortfarande är pending
    status = TradeStatus.Accepted;//ändrar pending till accepeted
    var fromInv = fromTrader.Inventory;//"Flyttar" item från olika användares inventorys
    var toInv = toTrader.Inventory;
    var fromItem = fromInv.Find(i => i.ItemName == offered.GetName());
    var toItem = toInv.Find(i => i.ItemName == requested.GetName());

    if (fromItem != null)//Urskilljer vem som får vad och vad det är som byts
    {
      fromInv.Remove(fromItem);
      toInv.Add(fromItem);
    }
    if (toItem != null)
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
  public class ItemInfo
  {
    private string name;
    private int amount;
    private string description;

    public ItemInfo(string n, int a, string d)
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

