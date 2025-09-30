namespace Apps;

class Trader : IUser
{
  public string Name;
  public string u_password;

  public Trader(string n, string p)
  {
    Name = n;
    u_password = p;
  }

  public bool TryLoggin(string username, string password)
  {
    return username == Name && password == u_password;
  }
}