using System.Diagnostics;
using MetricsApp.Models;

namespace MetricsApp.Middleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly List<RequestMetric> _requests = new();
        private static readonly object _lock = new();

        public RequestTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {

                await _next(context);
            }
            finally
            {

                stopwatch.Stop();

                lock (_lock)
                {

                    _requests.Add(new RequestMetric(
                        context.Request.Path.ToString(),
                        stopwatch.ElapsedMilliseconds
                        ));

                    if (_requests.Count > 20)
                    {
                        _requests.RemoveAt(0);
                    }
                }

                Console.WriteLine(
                    $"{context.Request.Path} took {stopwatch.ElapsedMilliseconds} ms");
            }
        }

        public static List<RequestMetric> GetRequests()
        {
            lock(_lock)
            {
                return new List<RequestMetric>(_requests);
            }
        }
    }
}
