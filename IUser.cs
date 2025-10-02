namespace Apps;

interface IUser
{
  public bool TryLogin(string username, string password);
}