using CatOs.Core.DbModels.Ticket;
using CatOs.Core.DTOs.API;
using CatOs.Core.DTOs.Ticket;
using CatOs.Core.Enums.Ticket;
using CatOs.Core.Interface.Ticket;
using CatOs.Infrastructure.AppContextDb;
using Microsoft.EntityFrameworkCore;

namespace CatOs.Infrastructure.Implements.Ticket
{
    internal class Ticket : ITicket
    {
        private readonly AppDbContext _context;
        public Ticket(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ReturnResult<object>> ListTickets(Seach seach)
        {
            try
            {
                var query = _context.Tickets.AsNoTracking().Include(e => e.TicketItens).AsQueryable();
                if (!string.IsNullOrEmpty(seach.Ref))
                    query = query.Where(e => e.TicketNumber.ToString().Contains(seach.Ref) || e.Status.ToString() == seach.Ref);

                var totalRecords = await query.CountAsync();
                int pageSize = seach.PageSize > 0 ? seach.PageSize : 10;
                int currentPage = seach.Page > 0 ? seach.Page : 1;
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                var data = await query.OrderBy(e => e.DeliveryTime).Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

                return new ReturnResult<object>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Data = new
                    {
                        Prints = data,
                        Pagination = new
                        {
                            CurrentPage = currentPage,
                            TotalPages = totalPages,
                            TotalRecords = totalRecords,
                            PageSize = pageSize
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                return new ReturnResult<object> { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<ReturnResult<string>> CloseTicket(Guid ticketUuid)
        {
            try
            {
                var existingTicket = await _context.Tickets.FirstOrDefaultAsync(t => t.Uuid == ticketUuid);
                if (existingTicket is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "Ticket not found." };

                existingTicket.Status = ETicketStatus.Closed;
                _context.Tickets.Update(existingTicket);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { IsSuccess = true, StatusCode = 200, Data = "Ticket closed successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { IsSuccess = false, Error = ex.Message };
            }
        }
        public async Task<ReturnResult<string>> NewTicket(NewTicket newTicket)
        {
            try
            {
                if (newTicket is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "Invalid ticket data." };

                var existingTicket = await _context.Tickets.AnyAsync(t => t.Uuid == newTicket.Uuid);
                if (existingTicket)
                    return new ReturnResult<string> { IsSuccess = false, Error = "Ticket with the same UUID already exists." };

                var ticketEntity = new TicketDb
                {
                    Uuid = newTicket.Uuid,
                    Requester = newTicket.Requester,
                    RequestDate = newTicket.RequestDate,
                    DeliveryTime = newTicket.DeliveryTime,
                    Garage = newTicket.Garage,
                    Type = newTicket.Type,
                    Priority = newTicket.Priority,
                    Status = newTicket.Status
                };

                await _context.Tickets.AddAsync(ticketEntity);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { IsSuccess = true, StatusCode = 201, Data = "Ticket created successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<ReturnResult<string>> AddItem(AddItemTicket addItemTicket)
        {
            try
            {
                if (addItemTicket is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "Invalid item data." };

                var existingTicket = await _context.Tickets.FirstOrDefaultAsync(t => t.Uuid == addItemTicket.TicketId);
                if (existingTicket is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "Ticket not found." };

                var existSku = await _context.Skus.FirstOrDefaultAsync(s => s.Uuid == addItemTicket.SkuId);
                if (existSku is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "SKU not found." };

                var ticketItemEntity = new TicketItensDb
                {
                    Uuid = Guid.NewGuid(),
                    SkuUuid = existSku.Uuid,
                    Quantity = addItemTicket.Quantity,
                    TicketUuid = addItemTicket.TicketId,
                    Status = ETicketStatus.OnHold
                };

                await _context.TicketItens.AddAsync(ticketItemEntity);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { IsSuccess = true, StatusCode = 201, Data = "Item added to ticket successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<ReturnResult<string>> UpdateItemStatus(UpdateItemStatus updateItemStatus)
        {
            try
            {
                if (updateItemStatus is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "Invalid item status data." };

                var existingItem = await _context.TicketItens.FirstOrDefaultAsync(i => i.Uuid == updateItemStatus.ItemId);
                if (existingItem is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "Ticket item not found." };

                existingItem.Status = updateItemStatus.Status;
                
                _context.TicketItens.Update(existingItem);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { IsSuccess = true, StatusCode = 200, Data = "Item status updated successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { IsSuccess = false, Error = ex.Message };
            }
        }
    }
}
