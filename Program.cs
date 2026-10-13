var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
var service = Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "sample-dotnet";
var version = Environment.GetEnvironmentVariable("APP_VERSION")
    ?? typeof(Program).Assembly.GetName().Version?.ToString()
    ?? "0.1.0";

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
var app = builder.Build();

app.MapGet("/healthz", () => Results.Json(new { status = "ok" }));
app.MapGet("/version", () => Results.Json(new { version, service }));

app.Run();

public partial class Program { }
