namespace Apps;//Interface för inloggnign av olika användare, om man skulle vilja ha en admin utöver vanlig trader valde jag denna

interface IUser
{
  public bool TryLogin(string username, string password);
}