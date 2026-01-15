using CatOs.Core.DTOs.API;
using CatOs.Core.DTOs.Ticket;
using CatOs.Core.Interface.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace CatOs.ApiHost.Controllers.Ticket
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicket _ticket;
        public TicketController(ITicket ticket)
        {
            _ticket = ticket;
        }

        [HttpGet]
        public async Task<IActionResult> ListTickets([FromQuery] Seach seach)
        {
            var result = await _ticket.ListTickets(seach);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost]
        public async Task<IActionResult> NewTicket([FromBody] NewTicket newTicket)
        {
            var result = await _ticket.NewTicket(newTicket);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("CloseTicket/{ticketUuid}")]
        public async Task<IActionResult> CloseTicket(Guid ticketUuid)
        {
            var result = await _ticket.CloseTicket(ticketUuid);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItem([FromBody] AddItemTicket addItemTicket)
        {
            var result = await _ticket.AddItem(addItemTicket);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("UpdateItemStatus")]
        public async Task<IActionResult> UpdateItemStatus([FromBody] UpdateItemStatus updateItemStatus)
        {
            var result = await _ticket.UpdateItemStatus(updateItemStatus);
            return StatusCode(result.StatusCode, result);
        }
    }
}
