using CatOs.Core.DbModels.Stock;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatOs.Core.DbModels.SKU
{
    public class SkuComponents
    {
        [Key]
        public Guid Uuid { get; set; }
        public Guid SkuUuid { get; set; }
        public Guid ComponentUuid { get; set; }
        public double Quantity { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(SkuUuid))]
        public SkuDb? Sku { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(ComponentUuid))]
        public Components? Components { get; set; }
    }
}
