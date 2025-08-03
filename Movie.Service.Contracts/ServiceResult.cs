namespace Movie.Service.Contracts
{
    public class ServiceResult<T>
    {
        public T? Value { get; set; }
        public ServiceProblemDetails? Problem { get; set; }
        public int StatusCode { get; set; }

        public static ServiceResult<T> Success(T value) => new() { Value = value, StatusCode = 200 };
        public static ServiceResult<T> BadRequest(string detail) => new() { Problem = ServiceProblemDetails.BadRequest(detail), StatusCode = 400 };
        public static ServiceResult<T> Unauthorized(string detail) => new() { Problem = ServiceProblemDetails.Unauthorized(detail), StatusCode = 401 };
        public static ServiceResult<T> Forbidden(string detail) => new() { Problem = ServiceProblemDetails.Forbidden(detail), StatusCode = 403 };
        public static ServiceResult<T> NotFound(string detail) => new() { Problem = ServiceProblemDetails.NotFound(detail), StatusCode = 404 };
        public static ServiceResult<T> NotAcceptable(string detail) => new() { Problem = ServiceProblemDetails.NotAcceptable(detail), StatusCode = 406 };
        public static ServiceResult<T> Conflict(string detail) => new() { Problem = ServiceProblemDetails.Conflict(detail), StatusCode = 409 };
        public static ServiceResult<T> InternalError(string detail) => new() { Problem = ServiceProblemDetails.InternalError(detail), StatusCode = 500 };
    }
}