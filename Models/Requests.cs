using Microsoft.Identity.Client;
using System.Diagnostics.Eventing.Reader;

namespace COeX_India1._2.Models
{
    public class Requests
    {
        public int Id { get; set; }
        public int SidingId { get; set; }
        public double FrieghtAmount { get; set; }
        public int RakesRequired { get; set; }
        public DateTime RequiredOn { get; set; }
        public DateTime PlacedOn { get; set; }
        public int InsertedBy { get; set; }
        public EStatus Status { get; set; }
        public string Remarks { get; set; }

        public enum EStatus
        {
            pending=0, 
            accepted=1,
            rejected=2,
            completed=3,
        }
    }

    public class AddRequestModel
    {
        public int? Id { get; set; }
        public double FrieghtAmount { get; set; }
        public int RakesRequired { get; set; }
        public DateTime RequiredOn { get; set; }
        public string Remarks { get; set; }
    }

    public class UpdateRequestModel
    {
        public int? Id { get; set; }
        public double FrieghtAmount { get; set; }
        public int RakesRequired { get; set; }
        public DateTime RequiredOn { get; set; }
        public string Remarks { get; set; }
        public string Reason { get; set; }
    }
}
