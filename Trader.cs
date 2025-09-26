namespace Apps;

class Trader : Iuser
{
  public string Name;
  string u_password;

  public Trader(string name, string Password)
  {
    Name = name;
    u_password = Password;
  }

  public bool TryLoggin(string username, string Password)
  {
    return username == Name && Password == u_password;
  }

  
}