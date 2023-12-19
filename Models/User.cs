using Microsoft.IdentityModel.Tokens;

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
    public class PostResponse
    {
        public int NewId { get; set; }
        public bool Success { get; set; }
        public string Msg { get; set; }
        public PostResponse(int NewId, bool Success, string Msg)
        {
            this.NewId = NewId;
            this.Success = Success;
            this.Msg = Msg;
        }
    }
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string token { get; set; }
        public LoginResponse(bool Success, string token)
        {
            this.Success = Success;
            this.token = token;
        }
    }
    public class LoginModel
    {
        public int? SidingId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ELoggingAs LogginAs { get; set; }= ELoggingAs.Admin;

        public enum ELoggingAs
        {
            SidingUser=0, 
            Admin=1
        }
    }

}
