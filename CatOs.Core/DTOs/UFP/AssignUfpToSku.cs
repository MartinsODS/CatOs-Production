namespace CatOs.Core.DTOs.UFP
{
    public class AssignUfpToSku
    {
        public Guid UfpUuid { get; set; }
        public Guid SkuUuid { get; set; }
        public int Quantity { get; set; }
    }
}
