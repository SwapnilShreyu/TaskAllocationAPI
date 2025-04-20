namespace EnhancementAPI.Models
{
    public class Login
    { 
     public string userName { get; set; }
    public string Password { get; set; }
    public string IpAddress { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime LogoutTime { get; set; }
    public char LoginSucceeded { get; set; }
}

}