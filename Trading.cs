namespace Apps;

public enum TradeStatus
{
  Pending,
  Accepted,
  Denied,
}

public class Trade
{
  private Trader fromTrader;
  private Trader toTrader;
  private string ownItemName;
  private int ownItemAmount;
  private string ownItemDescription;
  private string reqItemName;
  private int reqItemAmount;
  private string reqItemDescription;
  private TradeStatus status;

  public Trade(Trader from, Trader to, Items own, Items req)
  {
    fromTrader = from;
    toTrader = to;
    ownItemName = own.ItemName;
    ownItemAmount = own.Amount;
    ownItemDescription = own.Description;
    reqItemName = req.ItemName;
    reqItemAmount = req.Amount;
    reqItemDescription = req.Description;
    status = TradeStatus.Pending;
  }

  public Trade(Trader from, Trader to, string ownName, int ownAmount, string ownDesc, string reqName, int reqAmount, string reqDesc, TradeStatus tradeStatus)
  {
    fromTrader = from;
    toTrader = to;
    ownItemName = ownName;
    ownItemAmount = ownAmount;
    ownItemDescription = ownDesc;
    reqItemName = reqName;
    reqItemDescription = reqDesc;
    status = tradeStatus;
  }

  public Trader GetFromTrader() { return fromTrader; }
  public Trader GiveTrader() { return toTrader; }
  public TradeStatus GetStatus() { return status; }
  public void SetStatus(TradeStatus newStatus) => status = newStatus;
  public string OfferedInfo() => $"{ownItemName} | {ownItemAmount} | {ownItemDescription}";
  public string ReqInfo() => $"{reqItemName} | {reqItemAmount} | {reqItemDescription}";
  public string OfferedName() => ownItemName;
  public int OfferedAmount() => ownItemAmount;
  public string OfferedDescription() => ownItemDescription;
  public string ReqName() => reqItemName;
  public int ReqAmount() => reqItemAmount;
  public string ReqDescription() => reqItemDescription;


  public void Accept()
  {
    if (status != TradeStatus.Pending) return;
    status = TradeStatus.Accepted;
    var fromInv = fromTrader.Inventory;
    var toInv = toTrader.Inventory;
    var fromItem = fromInv.Find(i => i.ItemName == ownItemName);
    var toItem = toInv.Find(i => i.ItemName == reqItemName);

    if (fromItem != null)
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
  public void Deny()
  {
    if (status == TradeStatus.Pending)
    {
      status = TradeStatus.Denied;
    }
  }
  // public void SetStatus(TradeStatus newStatus) => status = newStatus;
}