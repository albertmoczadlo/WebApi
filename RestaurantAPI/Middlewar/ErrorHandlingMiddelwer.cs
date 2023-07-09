
namespace RestaurantAPI.Middlewar
{
    public class ErrorHandlingMiddelwer : IMiddleware
    {

        private readonly ILogger<ErrorHandlingMiddelwer> _logger;

        public ErrorHandlingMiddelwer(ILogger<ErrorHandlingMiddelwer> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
