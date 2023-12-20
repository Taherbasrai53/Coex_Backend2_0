namespace COeX_India1._2.Models
{
    public class Siding
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Division { get; set; }
        public string Station { get; set; }
        public string State { get; set; }
        public int NumMines { get; set; }
        public double Inventory { get; set; }
        public string Zone { get; set; }
        public int AvailableRakes { get; set; }
    }

    public class UpdateInventoryModel
    {
        public double Inventory { get; set; }
    }
}
