using CatOs.Core.Enums.Stock;

namespace CatOs.Core.DTOs.Stock
{
    public class UpdateItemStock
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public EUnitOfMeasure UnitOfMeasure { get; set; } = EUnitOfMeasure.unit;
        public double QuantityInStock { get; set; }
        public string UnitPrice { get; set; } = string.Empty;
    }
}
