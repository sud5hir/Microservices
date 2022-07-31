using Polly;
using Polly.Extensions.Http;
using RetryAndCircuitBreakerRepo;
using RetryAndCircuitBreakerService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IMessageService, MessageService>();

builder.Services.AddSingleton<IMessageRepository, MessageRepository>();
//builder.Services.AddHttpClient<IMessageService, MessageService>(options =>
//{
//    options.BaseAddress = new Uri("https://stackoverflow.com");
//})
//.AddPolicyHandler(GetCircuitBreakerPolicy())
//.AddPolicyHandler(GetRetryPolicy());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/TestCircuit", (IMessageService service, int i) =>
 {
     Console.WriteLine($"TestRetryAndCircuit:{i}");
     var result = service.GetGoodbyeMessage(i);
     return Results.Ok(result);
 });
app.MapGet("/TestRetry", (IMessageService service) =>
{
    var result = service.GetHelloMessage();
    return Results.Ok(result);
});

app.Run();

//static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
//{
//    return HttpPolicyExtensions
//                   .HandleTransientHttpError()
//                   .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
//                   .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
//}
//static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
//{
//    return Policy
//.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
//                   .CircuitBreakerAsync(2, TimeSpan.FromSeconds(10), OnBreak, OnReset, OnHalfOpen);

//}

//static void OnHalfOpen()
//{
//    Console.WriteLine("Circuit in test mode, one request will be allowed.");
//}

//static void OnReset()
//{
//    Console.WriteLine("Circuit closed, requests flow normally.");
//}

//static void OnBreak(DelegateResult<HttpResponseMessage> result, TimeSpan ts)
//{
//    Console.WriteLine("Circuit cut, requests will not flow.");
//}