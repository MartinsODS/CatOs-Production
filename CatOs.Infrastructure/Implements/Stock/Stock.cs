using CatOs.Core.DbModels.Stock;
using CatOs.Core.DTOs.API;
using CatOs.Core.DTOs.Stock;
using CatOs.Core.Interface.Stock;
using CatOs.Infrastructure.AppContextDb;
using Microsoft.EntityFrameworkCore;

namespace CatOs.Infrastructure.Implements.Stock
{
    internal class Stock : IStock
    {
        private readonly AppDbContext _context;

        public Stock(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReturnResult<string>> AddItemStock(AddItemStock addItemStock)
        {
            try
            {
                if (addItemStock is null)
                    return new ReturnResult<string> { IsSuccess = false, StatusCode = 400, Error = "Invalid item stock data." };

                var existItem = await _context.Components.AsNoTracking().FirstOrDefaultAsync(c => c.Name == addItemStock.Name && c.Code == addItemStock.Code);
                if (existItem is not null)
                    return new ReturnResult<string> { IsSuccess = false, StatusCode = 400, Error = "Item stock already exists." };

                Components newComponets = new()
                {
                    Name = addItemStock.Name,
                    Description = addItemStock.Description,
                    Code = addItemStock.Code,
                    UnitOfMeasure = addItemStock.UnitOfMeasure,
                    QuantityInStock = addItemStock.QuantityInStock,
                    UnitPrice = addItemStock.UnitPrice
                };

                await _context.Components.AddAsync(newComponets);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { IsSuccess = true, StatusCode = 200, Data = "Item stock added successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<ReturnResult<string>> DeleteItemStock(Guid uuid)
        {
            try
            {
                var existItem = await _context.Components.FirstOrDefaultAsync(c => c.Uuid == uuid);
                if (existItem is null)
                    return new ReturnResult<string> { IsSuccess = false, StatusCode = 400, Error = "Item stock not found." };

                _context.Components.Remove(existItem);
                _context.SaveChanges();

                return new ReturnResult<string> { IsSuccess = true, StatusCode = 200, Data = "Item stock deleted successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<ReturnResult<object>> ListStock(Seach seach)
        {
            try
            {
                var query = _context.Components.AsNoTracking().AsQueryable();
                if (!string.IsNullOrEmpty(seach.Ref))
                    query = query.Where(e => e.Name.ToLower().Contains(seach.Ref.ToLower()) || e.Code.ToLower().Contains(seach.Ref.ToLower()));

                var totalRecords = await query.CountAsync();
                int pageSize = seach.PageSize > 0 ? seach.PageSize : 10;
                int currentPage = seach.Page > 0 ? seach.Page : 1;
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                var data = await query.OrderBy(e => e.OrdenTable).Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

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

        public async Task<ReturnResult<string>> UpdateItemStock(UpdateItemStock updateItemStock)
        {
            try
            {
                if (updateItemStock is null)
                    return new ReturnResult<string> { IsSuccess = false, StatusCode = 400, Error = "Invalid item stock data." };

                var existItem = await _context.Components.FirstOrDefaultAsync(c => c.Uuid == updateItemStock.Uuid);
                if (existItem is null)
                    return new ReturnResult<string> { IsSuccess = false, StatusCode = 400, Error = "Item stock not found." };

                if (existItem.Name != updateItemStock.Name)
                {
                    var existName = await _context.Components.AnyAsync(c => c.Name == updateItemStock.Name);
                    if (existName)
                        return new ReturnResult<string> { IsSuccess = false, StatusCode = 490, Error = "Item stock name already exists." };

                    existItem.Name = updateItemStock.Name;
                }
                if (existItem.Code != updateItemStock.Code)
                {
                    var existCode = await _context.Components.AnyAsync(c => c.Code == updateItemStock.Code);
                    if (existCode)
                        return new ReturnResult<string> { IsSuccess = false, StatusCode = 409, Error = "Item stock code already exists." };
                    existItem.Code = updateItemStock.Code;
                }

                if (!string.IsNullOrEmpty(updateItemStock.Description))
                    existItem.Description = updateItemStock.Description;

                if (updateItemStock.UnitOfMeasure != existItem.UnitOfMeasure)
                    existItem.UnitOfMeasure = updateItemStock.UnitOfMeasure;

                if (updateItemStock.QuantityInStock != existItem.QuantityInStock)
                    existItem.QuantityInStock = updateItemStock.QuantityInStock;

                if (!string.IsNullOrEmpty(updateItemStock.UnitPrice))
                    existItem.UnitPrice = updateItemStock.UnitPrice;

                _context.Components.Update(existItem);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { IsSuccess = true, StatusCode = 200, Data = "Item stock updated successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { IsSuccess = false, Error = ex.Message };
            }
        }
    }
}
