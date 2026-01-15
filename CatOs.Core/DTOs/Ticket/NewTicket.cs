using CatOs.Core.Enums.Ticket;

namespace CatOs.Core.DTOs.Ticket
{
    public class NewTicket
    {
        public Guid Uuid { get; set; }
        public string Requester { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime DeliveryTime { get; set; }
        public EGarages Garage { get; set; } = EGarages.Oeste;
        public ERequestTypes Type { get; set; } = ERequestTypes.Production;
        public EPriority Priority { get; set; } = EPriority.Stock;
        public ETicketStatus Status { get; set; } = ETicketStatus.Open;
    }
}
