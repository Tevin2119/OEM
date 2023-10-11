using OEM_RPS.Shared.Enums;

namespace OEM_RPS.Shared.DTO
{
    public class ApiResponse<T>
    {
        public StatusCodeEnum StatusCode { get; set; }
         public string? Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(StatusCodeEnum statusCode, string? message, T? data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T>(StatusCodeEnum.Success, "Success", data);
        }

        public static ApiResponse<T> NotFound(string message = "Not Found")
        {
            return new ApiResponse<T>(StatusCodeEnum.NotFound, message, default);
        }

        public static ApiResponse<T> BadRequest(string message = "Bad Request")
        {
            return new ApiResponse<T>(StatusCodeEnum.BadRequest, message, default);
        }

        // You can add more methods for other status codes as needed.
    }

}

