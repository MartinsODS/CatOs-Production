using CatOs.Core.DbModels.SKU;
using CatOs.Core.DbModels.Stock;
using CatOs.Core.DbModels.Ticket;
using CatOs.Core.DbModels.UFP;
using Microsoft.EntityFrameworkCore;

namespace CatOs.Infrastructure.AppContextDb
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Components> Components { get; set; }
        public DbSet<SkuComponents> SkuComponents { get; set; }
        public DbSet<SkuDb> Skus { get; set; }
        public DbSet<TicketItensDb> TicketItens { get; set; }
        public DbSet<TicketDb> Tickets { get; set; }
        public DbSet<UfpDb> Ufps { get; set; }
        public DbSet<LinkUfpSku> LinkUfpSkus { get; set; }
    }
}
