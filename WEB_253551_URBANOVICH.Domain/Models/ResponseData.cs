namespace WEB_253551_URBANOVICH.Domain.Models;

public class ResponseData<T>
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public T? Data { get; set; }

    public static ResponseData<T> Success(T? data) =>
        new() { IsSuccess = true, Data = data };

    public static ResponseData<T> Error(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}
