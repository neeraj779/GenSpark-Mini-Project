namespace StudentManagementAPI.Models
{
    public class ErrorModel
    {
        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }

        public ErrorModel()
        {
        }

        public ErrorModel(int statusCode, string message)
        {
            ErrorCode = statusCode;
            ErrorMessage = message;
        }

    }
}