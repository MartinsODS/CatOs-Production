namespace CatOs.Core.DTOs.UFP
{
    public class UpdateAssingUfpToSku
    {
        public Guid SkuUfpUuid { get; set; }
        public Guid NewUfpUuid { get; set; } = Guid.Empty;
        public int Quantity { get; set; }
    }
}
