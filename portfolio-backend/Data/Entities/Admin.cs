namespace portfolio_backend.Data.Entities
{
    public class Admin
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        public Admin(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
