namespace Miski.Shared.DTOs.Base;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public object? Errors { get; set; }

    public static ApiResponse<T> SuccessResult(T data, string message = "Operación exitosa")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Errors = null
        };
    }

    public static ApiResponse<T> ErrorResult(string message, object? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default(T),
            Errors = errors
        };
    }

    public static ApiResponse<T> ValidationErrorResult(object errors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = "Errores de validación encontrados",
            Data = default(T),
            Errors = errors
        };
    }
}

// Para respuestas sin data específica
public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse SuccessResult(string message = "Operación exitosa")
    {
        return new ApiResponse
        {
            Success = true,
            Message = message,
            Data = null,
            Errors = null
        };
    }

    public new static ApiResponse ErrorResult(string message, object? errors = null)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            Data = null,
            Errors = errors
        };
    }
}