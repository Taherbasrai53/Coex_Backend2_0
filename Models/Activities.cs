namespace COeX_India1._2.Models
{
    public class Activities
    {
        public int Id { get; set; }
        public int SidingId { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string ColorCode { get; set; }
        public bool Acknowledged { get; set; }
        public DateTime InsertedAt { get; set; }
    }
}
