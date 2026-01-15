namespace CatOs.Core.DTOs.Ticket
{
    public class AddItemTicket
    {
        public Guid TicketId { get; set; }
        public Guid SkuId { get; set; }
        public int Quantity { get; set; }
    }
}
