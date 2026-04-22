using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Entity.DTO
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public T Data { get; set; }
        public string? ErrorDetails { get; set; }

        public ApiResponse(int statusCode, string message, T data = default, string errorDetails = null)
        {
            StatusCode = statusCode;
            Message = message;
            Timestamp = DateTime.UtcNow;
            Data = data;
            ErrorDetails = errorDetails;
        }

      
        public static ApiResponse<T> Success(T data, string message = "Success")
        {
            return new ApiResponse<T>(200, message, data);
        }

        
        public static ApiResponse<T> Error(string message, int statusCode = 400, string errorDetails = null)
        {
            return new ApiResponse<T>(statusCode, message, default, errorDetails);
        }
    }
}
