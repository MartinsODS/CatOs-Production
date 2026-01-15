namespace CatOs.Core.DTOs.API
{
    public class Seach
    {
        public string Ref { get; set; } = string.Empty;
        public int PageSize { get; set; } = 10;
        public int Page { get; set; } = 1;
    }
}
