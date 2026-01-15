namespace CatOs.Core.DTOs.API
{
    public class ReturnResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 500;
        public T? Data { get; set; }
    }
}
