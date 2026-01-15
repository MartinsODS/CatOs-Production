namespace CatOs.Core.DTOs.SKU
{
    public class UpdateSku
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SkuCode { get; set; } = string.Empty;
    }
}
