namespace MyBudgetManagement.Application.Wrappers;
public class ApiResponse<T>
{
    public ApiResponse()
    {
            
    }

    //success response
    public ApiResponse(T data, string message = null)
    {
        Succeeded = true;
        Message = message;
        Data = data;
    }

    //failed response
    public ApiResponse(string message)
    {
        Succeeded = true;
        Message = message;
    }

    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
    public T Data { get; set; }
}