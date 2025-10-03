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
  private Items ownItem;
  private Items requestedItem;
  private TradeStatus status;

  public Trade(Trader from, Trader to, Items own, Items req)
  {
    fromTrader = from;
    toTrader = to;
    ownItem = own;
    requestedItem = req;
    status = TradeStatus.Pending;
  }

  public Trader GetFromTrader() { return fromTrader; }  
  public Trader GiveTrader() { return toTrader; }
  public Items SendOwnItem() { return ownItem; }
  public Items GetReqItem() { return requestedItem; }

  public TradeStatus GetStatus() { return status; }


  public void Accept()
  {
    if (status == TradeStatus.Pending)
    {
      status = TradeStatus.Accepted;

      toTrader.Inventory.Remove(requestedItem);
      fromTrader.Inventory.Add(requestedItem);

      fromTrader.Inventory.Remove(ownItem);
      toTrader.Inventory.Add(ownItem);
    }
  }
  public void Deny()
  {
    if(status == TradeStatus.Pending)
    {
      status = TradeStatus.Denied;
    }
  }
}