namespace UserCRUD.DTOs
{
    public class ResponseDTO
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string Status { get; set; }
        public object Data { get; set; }

        public ResponseDTO() { }

        public ResponseDTO(string message, bool isSuccess, string status, object data)
        {
            Message = message;
            IsSuccess = isSuccess;
            Status = status;
            Data = data;
        }

        public void IsSucess(string message, string status, object data)
        {
            Message = message;
            IsSuccess = true;
            Status = status;
            Data = data;
        }
        public void IsFailure(string message, string status, object data)
        {
            Message = message;
            IsSuccess = false;
            Status = status;
            Data = data;
        }
    }
}
