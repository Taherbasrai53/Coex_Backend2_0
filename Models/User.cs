namespace COeX_India1._2.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int? SidingId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public EUserType UserType { get; set; } = EUserType.Admin;

        public enum EUserType
        {
            SidingUser=0,
            Admin=1,
        } 
    }
}
