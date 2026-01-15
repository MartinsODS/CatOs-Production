using CatOs.Core.DbModels.UFP;
using CatOs.Core.DTOs.API;
using CatOs.Core.DTOs.UFP;
using CatOs.Core.Interface.UFP;
using CatOs.Infrastructure.AppContextDb;
using Microsoft.EntityFrameworkCore;

namespace CatOs.Infrastructure.Implements.UFP
{
    internal class Ufp : IUfp
    {
        private readonly AppDbContext _context;
        public Ufp(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ReturnResult<object>> ListUfp(Seach seach)
        {
            try
            {
                var query = _context.Ufps.AsNoTracking().Include(e => e.SKUs).AsQueryable();
                if (!string.IsNullOrEmpty(seach.Ref))
                    query = query.Where(e => e.Name.ToLower().Contains(seach.Ref.ToLower()) || e.Status.ToString().Contains(seach.Ref));

                var totalRecords = await query.CountAsync();
                int pageSize = seach.PageSize > 0 ? seach.PageSize : 10;
                int currentPage = seach.Page > 0 ? seach.Page : 1;
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                var data = await query.OrderBy(e => e.CreatedAt).Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

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

        public async Task<ReturnResult<string>> CreaterUfp(NewUfp newUfp)
        {
            try
            {
                if (newUfp is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "Invalid UFP data." };

                var existUfp = _context.Ufps.FirstOrDefault(u => u.Name == newUfp.Name);
                if (existUfp != null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "UFP with the same name already exists." };

                var ufpEntity = new UfpDb()
                {
                    Name = newUfp.Name,
                    Description = newUfp.Description,
                    CreatedAt = DateTime.UtcNow,
                };
                _context.Ufps.Add(ufpEntity);

                await _context.SaveChangesAsync();

                return new ReturnResult<string> { IsSuccess = true, StatusCode = 201, Data = "UFP created successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<ReturnResult<string>> UpdateUfp(UpdateUfp updateUfp)
        {
            try
            {
                if (updateUfp is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "Invalid UFP data." };

                var existUfp = await _context.Ufps.FirstOrDefaultAsync(u => u.Uuid == updateUfp.UfpId);
                if (existUfp is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "UFP not found." };

                if (!string.IsNullOrEmpty(updateUfp.Name))
                    existUfp.Name = updateUfp.Name;

                if (!string.IsNullOrEmpty(updateUfp.Description))
                    existUfp.Description = updateUfp.Description;

                _context.Ufps.Update(existUfp);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { IsSuccess = true, StatusCode = 200, Data = "UFP updated successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<ReturnResult<string>> DeleteUfp(Guid ufpUuid)
        {
            try
            {
                var existUfp = await _context.Ufps.FirstOrDefaultAsync(u => u.Uuid == ufpUuid);
                if (existUfp is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "UFP not found." };

                _context.Ufps.Remove(existUfp);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { IsSuccess = true, StatusCode = 200, Data = "UFP deleted successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<ReturnResult<string>> AssignUfpToSku(AssignUfpToSku assignUfpToSku)
        {
            try
            {
                if (assignUfpToSku is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "Invalid assignment data." };

                var existUfp = _context.Ufps.AsNoTracking().FirstOrDefault(u => u.Uuid == assignUfpToSku.UfpUuid);
                if (existUfp is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "UFP not found." };

                var existSku = await _context.Skus.AsNoTracking().FirstOrDefaultAsync(s => s.Uuid == assignUfpToSku.SkuUuid);
                if (existSku is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "SKU not found." };

                var newLink = new LinkUfpSku()
                {
                    UfpUuid = existUfp.Uuid,
                    SkuUuid = existSku.Uuid,
                    Quantity = assignUfpToSku.Quantity
                };

                _context.LinkUfpSkus.Add(newLink);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { IsSuccess = true, StatusCode = 201, Data = "UFP assigned to SKU successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<ReturnResult<string>> RemoveUfpFromSku(Guid skuUfpUuid)
        {
            try
            {
                var link = await _context.LinkUfpSkus.FirstOrDefaultAsync(l => l.Uuid == skuUfpUuid);
                if (link is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "Link between UFP and SKU not found." };

                _context.LinkUfpSkus.Remove(link);
                await _context.SaveChangesAsync();
                return new ReturnResult<string> { IsSuccess = true, StatusCode = 200, Data = "UFP removed from SKU successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<ReturnResult<string>> UpdateAssingUfpToSku(UpdateAssingUfpToSku updateAssingUfpToSku)
        {
            try
            {
                if (updateAssingUfpToSku is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "Invalid update data." };

                var existLink = await _context.LinkUfpSkus.FirstOrDefaultAsync(l => l.Uuid == updateAssingUfpToSku.SkuUfpUuid);
                if (existLink is null)
                    return new ReturnResult<string> { IsSuccess = false, Error = "Link between UFP and SKU not found." };

                if (updateAssingUfpToSku.NewUfpUuid != Guid.Empty)
                {
                    var newUfp = await _context.Ufps.FirstOrDefaultAsync(u => u.Uuid == updateAssingUfpToSku.NewUfpUuid);
                    if (newUfp is null)
                        return new ReturnResult<string> { IsSuccess = false, Error = "New UFP not found." };
                    existLink.UfpUuid = newUfp.Uuid;
                }

                existLink.Quantity = updateAssingUfpToSku.Quantity;

                _context.LinkUfpSkus.Update(existLink);
                await _context.SaveChangesAsync();

                return new ReturnResult<string> { IsSuccess = true, StatusCode = 200, Data = "UFP assignment to SKU updated successfully." };
            }
            catch (Exception ex)
            {
                return new ReturnResult<string> { IsSuccess = false, Error = ex.Message };
            }
        }
    }
}
