using CatOs.Core.DTOs.API;
using CatOs.Core.DTOs.Ticket;

namespace CatOs.Core.Interface.Ticket
{
    public interface ITicket
    {
        public Task<ReturnResult<object>> ListTickets(Seach seach);
        public Task<ReturnResult<string>> NewTicket(NewTicket newTicket);
        public Task<ReturnResult<string>> CloseTicket(Guid ticketUuid);

        public Task<ReturnResult<string>> AddItem(AddItemTicket addItemTicket);
        public Task<ReturnResult<string>> UpdateItemStatus(UpdateItemStatus updateItemStatus);
    }
}
