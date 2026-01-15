using CatOs.Core.Enums.UFP;
using System.ComponentModel.DataAnnotations;

namespace CatOs.Core.DbModels.UFP
{
    public class UfpDb
    {
        [Key]
        public Guid Uuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public EStatusUfp Status { get; set; } = EStatusUfp.Aporoved;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<LinkUfpSku> SKUs { get; set; } = new List<LinkUfpSku>();
    }
}
