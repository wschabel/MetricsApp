using MetricsApp.Middleware;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseExceptionHandler("/error");

app.UseMiddleware<RequestTimingMiddleware>();

app.MapGet("/", () => "Hello World!");

app.MapGet("/metrics", () => RequestTimingMiddleware.GetRequests());

app.Map("/error", (HttpContext ctx) =>
Results.Problem("Something went Wrong."));

app.MapGet("/test-error", () =>
{
    throw new Exception("This is a test exception.");
});

app.Run();
