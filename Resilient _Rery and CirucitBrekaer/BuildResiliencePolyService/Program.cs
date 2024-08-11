using Polly;
using Polly.Registry;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddSingleton<WeatherService>();


//Add resilience pipeline
builder.Services.AddResiliencePipeline("default", x =>
{
    x.AddRetry(new Polly.Retry.RetryStrategyOptions
    {
        ShouldHandle = new PredicateBuilder().Handle<Exception>(),
        Delay = TimeSpan.FromSeconds(2),
        MaxRetryAttempts = 2,
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true
    })
    .AddCircuitBreaker(new Polly.CircuitBreaker.CircuitBreakerStrategyOptions
    { 
      FailureRatio = 0.5,
    SamplingDuration = TimeSpan.FromSeconds(5),
    MinimumThroughput = 8,
    BreakDuration = TimeSpan.FromSeconds(20),
    ShouldHandle = new PredicateBuilder().Handle<Exception>()
    }) ;
    //.AddTimeout(TimeSpan.FromSeconds(30));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/weatherService/weather", async (WeatherService weatherService) =>
{
    var result = await weatherService.GetWeatherAsync();
    return result;
})
    .WithName("GetWeather")
    .WithOpenApi();

app.Run();


public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly ResiliencePipelineProvider<string> _resiliencePipelineProvider;
    public WeatherService(HttpClient httpClient,
                         ResiliencePipelineProvider<string> resiliencePipelineProvider)
    {
        _httpClient = httpClient;
        _resiliencePipelineProvider = resiliencePipelineProvider;

    }
    public async Task<string> GetWeatherAsync()
    {
        var pipeline = _resiliencePipelineProvider.GetPipeline("default");
        var response = await pipeline
            .ExecuteAsync( async ct=> await _httpClient.GetAsync($"https://localhost:7187/weatherforecast",ct));

        return await response.Content.ReadAsStringAsync();
    }

}


