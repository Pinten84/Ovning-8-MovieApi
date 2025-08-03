using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class ProblemDetailsMiddleware
{
    private readonly RequestDelegate _next;

    public ProblemDetailsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (GenreNotFoundException ex)
        {
            await WriteProblem(context, 404, "Not Found", ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteProblem(context, 401, "Unauthorized", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteProblem(context, 400, "Bad Request", ex.Message);
        }
        catch (NotSupportedException ex)
        {
            await WriteProblem(context, 406, "Not Acceptable", ex.Message);
        }
        catch (Exception ex)
        {
            await WriteProblem(context, 500, "Internal Server Error", ex.Message);
        }
    }

    private static async Task WriteProblem(HttpContext context, int status, string title, string detail)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";
        var problem = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = status
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}