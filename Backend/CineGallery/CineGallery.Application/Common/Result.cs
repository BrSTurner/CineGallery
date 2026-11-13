namespace CineGallery.Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string[]? Errors { get; set; } = [];

        private Result(T? data, string[]? errors)
        {
            IsSuccess = errors is null;
            Data = data;
            Errors = errors;
        }

        public static Result<T> Success(T data)
            => new(data, null);

        public static Result<T> Failure(string[] errors)
            => new(default, errors);
    }
}
