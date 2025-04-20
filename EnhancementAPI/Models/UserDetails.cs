namespace EnhancementAPI.Models
{
    public class UserDetails
    {
        public Guid ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
