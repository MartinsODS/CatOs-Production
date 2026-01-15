namespace CatOs.Core.DTOs.SKU
{
    public class AddComponent
    {
        public Guid SkuId { get; set; }
        public Guid ComponentId { get; set; }
        public double Quantity { get; set; }
    }
}
