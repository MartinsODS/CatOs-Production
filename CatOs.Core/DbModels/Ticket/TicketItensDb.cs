using CatOs.Core.DbModels.SKU;
using CatOs.Core.Enums.Ticket;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatOs.Core.DbModels.Ticket
{
    public class TicketItensDb
    {
        [Key]
        public Guid Uuid { get; set; }
        public string SkuName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public Guid TicketUuid { get; set; }
        public Guid SkuUuid { get; set; }
        public ETicketStatus Status { get; set; } = ETicketStatus.OnHold;


        [ForeignKey(nameof(SkuUuid))]
        public SkuDb? Sku { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(TicketUuid))]
        public TicketDb? Ticket { get; set; } 
    }
}
