using CatOs.Core.DbModels.SKU;
using CatOs.Core.DTOs.API;
using CatOs.Core.DTOs.SKU;
using CatOs.Core.Interface.SKU;
using CatOs.Infrastructure.AppContextDb;
using Microsoft.EntityFrameworkCore;
namespace CatOs.Infrastructure.Implements.SKU
{
    internal class Sku : ISku
    {
        private readonly AppDbContext _context;
        public Sku(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ReturnResult<string>> AddSku(AddSku addSku)
        {
            try
            {
                if (addSku is null)
                    return new ReturnResult<string> { Data = null, Error = "Invalid SKU data.", IsSuccess = false, StatusCode = 400 };

                var existSku = await _context.Skus.AsNoTracking().FirstOrDefaultAsync(s => s.SkuCode == addSku.SkuCode);
                if (existSku is not null)
                    return new ReturnResult<string> { Data = null, Error = "SKU with the same code already exists.", IsSuccess = false, StatusCode = 409 };

                var newSku = new SkuDb()
                {
                    Name = addSku.Name,
                    Description = addSku.Description,
                    SkuCode = addSku.SkuCode,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Skus.AddAsync(newSku);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { Data = $"SKU {newSku.SkuCode} added successfully.", IsSuccess = true, StatusCode = 201 };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { Data = null, Error = ex.Message, IsSuccess = false, StatusCode = 500 };
            }
        }

        public async Task<ReturnResult<object>> ListSkus(Seach seach)
        {
            try
            {
                var query = _context.Skus.AsNoTracking().Include(e => e.SkuComponents).AsQueryable();
                if (!string.IsNullOrEmpty(seach.Ref))
                    query = query.Where(e => e.Name.ToLower().Contains(seach.Ref.ToLower()) || e.SkuCode.ToLower().Contains(seach.Ref.ToLower()));

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

        public async Task<ReturnResult<string>> UpdateSku(UpdateSku updateSku)
        {
            try
            {
                if (updateSku is null)
                    return new ReturnResult<string> { Data = null, Error = "Invalid SKU data.", IsSuccess = false, StatusCode = 400 };

                var existingSku = await _context.Skus.FirstOrDefaultAsync(s => s.Uuid == updateSku.Uuid);
                if (existingSku is null)
                    return new ReturnResult<string> { Data = null, Error = "SKU not found.", IsSuccess = false, StatusCode = 404 };

                if (!string.Equals(existingSku.SkuCode, updateSku.SkuCode, StringComparison.OrdinalIgnoreCase))
                {
                    var skuWithSameCode = await _context.Skus.AsNoTracking().FirstOrDefaultAsync(s => s.SkuCode == updateSku.SkuCode);
                    if (skuWithSameCode is not null)
                        return new ReturnResult<string> { Data = null, Error = "Another SKU with the same code already exists.", IsSuccess = false, StatusCode = 409 };

                    existingSku.SkuCode = updateSku.SkuCode;
                }
                if (!string.IsNullOrEmpty(updateSku.SkuCode))
                {
                    var skuWithSameCode = await _context.Skus.AsNoTracking().FirstOrDefaultAsync(s => s.SkuCode == updateSku.SkuCode);
                    if (skuWithSameCode is not null)
                        return new ReturnResult<string> { Data = null, Error = "Another SKU with the same code already exists.", IsSuccess = false, StatusCode = 409 };

                    existingSku.SkuCode = updateSku.SkuCode;
                }

                if (!string.IsNullOrEmpty(updateSku.Name))
                    existingSku.Name = updateSku.Name;

                if (!string.IsNullOrEmpty(updateSku.Description))
                    existingSku.Description = updateSku.Description;

                existingSku.UpdatedAt = DateTime.UtcNow;

                _context.Skus.Update(existingSku);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { Data = $"SKU {existingSku.SkuCode} updated successfully.", IsSuccess = true, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { Data = null, Error = ex.Message, IsSuccess = false, StatusCode = 500 };
            }
        }

        public async Task<ReturnResult<string>> DeleteSku(Guid skuUuid)
        {
            try
            {
                var existSku = await _context.Skus.FirstOrDefaultAsync(s => s.Uuid == skuUuid);
                if (existSku is null)
                    return new ReturnResult<string> { Data = null, Error = "SKU not found.", IsSuccess = false, StatusCode = 404 };

                _context.Skus.Remove(existSku);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { Data = $"SKU {existSku.SkuCode} deleted successfully.", IsSuccess = true, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { Data = null, Error = ex.Message, IsSuccess = false, StatusCode = 500 };
            }
        }

        public async Task<ReturnResult<string>> AddComponents(AddComponent addComponent)
        {
            try
            {
                if (addComponent is null)
                    return new ReturnResult<string> { Data = null, Error = "Invalid component data.", IsSuccess = false, StatusCode = 400 };

                var existSku = await _context.Skus.AsNoTracking().FirstOrDefaultAsync(s => s.Uuid == addComponent.SkuId);
                if (existSku is null)
                    return new ReturnResult<string> { Data = null, Error = "SKU not found.", IsSuccess = false, StatusCode = 404 };

                var existComponent = await _context.Components.AsNoTracking().FirstOrDefaultAsync(c => c.Uuid == addComponent.ComponentId);
                if (existComponent is null)
                    return new ReturnResult<string> { Data = null, Error = "Component not found.", IsSuccess = false, StatusCode = 404 };

                var skuComponent = new SkuComponents()
                {
                    SkuUuid = existSku.Uuid,
                    ComponentUuid = existComponent.Uuid,
                    Quantity = addComponent.Quantity,
                };

                await _context.SkuComponents.AddAsync(skuComponent);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { Data = $"Component added to SKU {existSku.SkuCode} successfully.", IsSuccess = true, StatusCode = 201 };

            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { Data = null, Error = ex.Message, IsSuccess = false, StatusCode = 500 };
            }
        }

        public async Task<ReturnResult<string>> UpdateComponents(UpdateComponent updateComponent)
        {
            try
            {
                if (updateComponent is null)
                    return new ReturnResult<string> { Data = null, Error = "Invalid component data.", IsSuccess = false, StatusCode = 400 };

                var existSkuComponent = await _context.SkuComponents.FirstOrDefaultAsync(sc => sc.Uuid == updateComponent.ComponentId);
                if (existSkuComponent is null)
                    return new ReturnResult<string> { Data = null, Error = "SKU Component not found.", IsSuccess = false, StatusCode = 404 };

                existSkuComponent.Quantity = updateComponent.Quantity;

                _context.SkuComponents.Update(existSkuComponent);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { Data = $"SKU Component updated successfully.", IsSuccess = true, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { Data = null, Error = ex.Message, IsSuccess = false, StatusCode = 500 };
            }
        }

        public async Task<ReturnResult<string>> RemoveComponents(Guid skuComponentUuid)
        {
            try
            {

                var existSkuComponent = await _context.SkuComponents.FirstOrDefaultAsync(sc => sc.Uuid == skuComponentUuid);
                if (existSkuComponent is null)
                    return new ReturnResult<string> { Data = null, Error = "SKU Component not found.", IsSuccess = false, StatusCode = 404 };

                _context.SkuComponents.Remove(existSkuComponent);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { Data = $"SKU Component removed successfully.", IsSuccess = true, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { Data = null, Error = ex.Message, IsSuccess = false, StatusCode = 500 };
            }
        }
    }
}
