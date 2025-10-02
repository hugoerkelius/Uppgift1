using System.Runtime.CompilerServices;

namespace Apps;

public enum Trading
{
  Pending,
  Accepted,
  Denied,
}

public class Trade
{
  private Trader fromTrader;
  private Trader toTrader;
  private Item ownItem;
  private Item requestedItem;
  private TradeStatus status;

  public Trade(Trader from, Trader to, Items own, Items req)
  {
    fromTrader = from;
    toTrader = to;
    ownItem = own;
    requestedItem = req;
    status = TradeStatus.Pending;
  }
}