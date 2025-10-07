namespace Apps;

public enum TradeStatus //Förbestämda värden för vad en trade kan vara
{
  Pending,
  Accepted,
  Denied,
}

public class Trade //Skapar en klass för trades
{
  private Trader fromTrader; //Vilka användare som är involverade
  private Trader toTrader; 
  private string ownItemName; //Det egna items innehåll
  private int ownItemAmount;
  private string ownItemDescription;
  private string reqItemName; //Det efterfrågade items innehåll
  private int reqItemAmount;
  private string reqItemDescription;
  private TradeStatus status; //Status på trade

  public Trade(Trader from, Trader to, Items own, Items req)//Användardata och items hämtas när användaren skapar en trade i programmet och sätts alltid till pending
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
  //Användardata och items hämtas från trades.csv för att skapa en trade när programmet startar
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
  public string OfferedDesc() => ownItemDescription;
  public string ReqName() => reqItemName;
  public int ReqAmount() => reqItemAmount;
  public string ReqDesc() => reqItemDescription;


  public void Accept() //Metod för att acceptera trade
  {
    if (status != TradeStatus.Pending) return; //Kontrollerar att status fortfarande är pending
    status = TradeStatus.Accepted;//ändrar pending till accepeted
    var fromInv = fromTrader.Inventory;//"Flyttar" item från olika användares inventorys
    var toInv = toTrader.Inventory;
    var fromItem = fromInv.Find(i => i.ItemName == ownItemName);
    var toItem = toInv.Find(i => i.ItemName == reqItemName);

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
