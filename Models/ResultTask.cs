namespace Models
{
    public class ResultTask<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public static ResultTask<T> Success(T data)
        {
            return new ResultTask<T>
            {
                IsSuccess = true,
                Data = data,
                ErrorMessage = null
            };
        }

        public static ResultTask<T> Failure(string errorMessage)
        {
            return new ResultTask<T>
            {
                IsSuccess = false,
                Data = default(T),
                ErrorMessage = errorMessage
            };
        }
    }
}
