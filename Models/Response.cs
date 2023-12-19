namespace COeX_India1._2.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string Msg { get; set; }
        public Response(bool Success, string Msg)
        {
            this.Success = Success;
            this.Msg = Msg;
        }
    }
}
