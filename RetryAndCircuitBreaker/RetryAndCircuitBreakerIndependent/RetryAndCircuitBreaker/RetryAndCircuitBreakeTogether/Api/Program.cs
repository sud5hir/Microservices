using Polly;
using Polly.Extensions.Http;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISomeService, SomeService>();

builder.Services.AddHttpClient("errorApi", c => { c.BaseAddress = new Uri("http://localhost:5000"); })
.AddPolicyHandler(GetCircuitBreakerPolicy())
    .AddPolicyHandler(GetRetryPolicy());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
                   .HandleTransientHttpError()
                    .WaitAndRetryAsync(2, retryAttempt =>
                    {
                        var timeToWait = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                        Console.WriteLine($"Waiting {timeToWait.TotalSeconds} seconds");
                        return timeToWait;
                    }
                );
}
static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
                   .HandleTransientHttpError()
                     .CircuitBreakerAsync(1, TimeSpan.FromSeconds(10),
                (ex, t) =>
                {
                    Console.WriteLine("Circuit broken!");
                },
                () =>
                {
                    Console.WriteLine("Circuit Reset!");
                });
}