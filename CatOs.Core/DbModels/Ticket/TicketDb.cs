using CatOs.Core.Enums.Ticket;
using System.ComponentModel.DataAnnotations;

namespace CatOs.Core.DbModels.Ticket
{
    public class TicketDb
    {
        [Key]
        public Guid Uuid { get; set; }
        public int TicketNumber { get; set; }
        public string Requester { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime DeliveryTime { get; set; }
        public EGarages Garage { get; set; } = EGarages.Oeste;
        public ERequestTypes Type { get; set; } = ERequestTypes.Production;
        public EPriority Priority { get; set; } = EPriority.Stock;
        public ETicketStatus Status { get; set; } = ETicketStatus.Open;


        public ICollection<TicketItensDb> TicketItens { get; set; } = new List<TicketItensDb>();
    }
}
