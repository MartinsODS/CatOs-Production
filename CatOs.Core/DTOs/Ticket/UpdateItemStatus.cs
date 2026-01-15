using CatOs.Core.Enums.Ticket;

namespace CatOs.Core.DTOs.Ticket
{
    public class UpdateItemStatus
    {
        public Guid ItemId { get; set; }
        public ETicketStatus Status { get; set; } = ETicketStatus.OnHold;
    }
}
