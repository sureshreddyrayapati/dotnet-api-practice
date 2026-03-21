namespace EF_core_practice_API.Model
{
    public class Users
    {
        public int id { get; set; }
        public string username { get; set; } = "";
        public string passwordHash { get; set; } = "";
        public string Role { get; set; } = "";

    }
}
