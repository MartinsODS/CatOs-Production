namespace CatOs.Core.DTOs.UFP
{
    public class UpdateUfp
    {
        public Guid UfpId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
