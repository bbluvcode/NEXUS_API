namespace NEXUS_API.Helpers
{
    public class ApiResponse
    {
        public int Status { get; }
        public string Message { get; }
        public object? Data { get; }
        public ApiResponse(int status, string message, object? data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
