namespace CivicCare.Api.Dtos
{
    public class CommonResponseDto<T>
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public T Data { get; set; }
        public string Error { get; set; }
    }
}