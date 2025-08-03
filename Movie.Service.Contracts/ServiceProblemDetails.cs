namespace Movie.Service.Contracts
{
    public class ServiceProblemDetails
    {
        public string? Title { get; set; }
        public string? Detail { get; set; }
        public int? Status { get; set; }

        public static ServiceProblemDetails BadRequest(string detail) => new() { Title = "Bad Request", Detail = detail, Status = 400 };
        public static ServiceProblemDetails Unauthorized(string detail) => new() { Title = "Unauthorized", Detail = detail, Status = 401 };
        public static ServiceProblemDetails Forbidden(string detail) => new() { Title = "Forbidden", Detail = detail, Status = 403 };
        public static ServiceProblemDetails NotFound(string detail) => new() { Title = "Not Found", Detail = detail, Status = 404 };
        public static ServiceProblemDetails NotAcceptable(string detail) => new() { Title = "Not Acceptable", Detail = detail, Status = 406 };
        public static ServiceProblemDetails Conflict(string detail) => new() { Title = "Conflict", Detail = detail, Status = 409 };
        public static ServiceProblemDetails InternalError(string detail) => new() { Title = "Internal Server Error", Detail = detail, Status = 500 };
    }
}