using CatOs.Core.Enums.Stock;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatOs.Core.DbModels.Stock
{
    public class Components
    {
        [Key]
        public Guid Uuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public EUnitOfMeasure UnitOfMeasure { get; set; } = EUnitOfMeasure.unit;
        public double QuantityInStock { get; set; }
        public string UnitPrice { get; set; } = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrdenTable { get; set; }
    }
}
