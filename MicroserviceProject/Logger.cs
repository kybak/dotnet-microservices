namespace MicroserviceProject
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Before handling the request
            Console.WriteLine("Hello from LoggingMiddleware!");

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

}
