namespace BTBaseServices.Models
{
    public class ApiResult
    {
        public int code;
        public string msg;
        public object content;
        public ErrorResult error;
    }

    public class ErrorResult
    {
        public int code;
        public string msg;
    }
}