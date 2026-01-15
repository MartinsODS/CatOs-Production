using CatOs.Core.DbModels.SKU;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatOs.Core.DbModels.UFP
{
    public class LinkUfpSku
    {
        [Key]
        public Guid Uuid { get; set; }
        public Guid UfpUuid { get; set; }
        public Guid SkuUuid { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(UfpUuid))]
        public UfpDb? Ufp { get; set; }

        [ForeignKey(nameof(SkuUuid))]
        public SkuDb? Sku { get; set; }
    }
}
