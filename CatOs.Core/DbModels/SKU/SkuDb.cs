using CatOs.Core.Enums.UFP;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatOs.Core.DbModels.SKU
{
    public class SkuDb
    {
        [Key]
        public Guid Uuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SkuCode { get; set; } = string.Empty;
        public EStatusUfp Status { get; set; } = EStatusUfp.Aporoved;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrdenTable { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }


        public ICollection<SkuComponents> SkuComponents { get; set; } = new List<SkuComponents>();
    }
}
