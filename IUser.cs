namespace Apps;

interface IUser
{
  public bool TryLoggin(string username, string password);
}