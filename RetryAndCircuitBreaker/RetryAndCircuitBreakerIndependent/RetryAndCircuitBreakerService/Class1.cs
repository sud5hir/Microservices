using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RetryAndCircuitBreakerRepo;
using System;
using System.Threading.Tasks;
namespace RetryAndCircuitBreakerService
{
    //public class Class1 : Interface1
    //{
    //    private readonly HttpClient _httpClient;

    //    public Class1(HttpClient httpClient)
    //    {
    //        _httpClient = httpClient;
    //    }

    //    public async Task<string> DoSomething(int i)
    //    {
    //        string result = string.Empty;
    //        try
    //        {
    //            if (i <= 2)
    //            {
    //                await _httpClient.GetAsync("notfound");
    //            }
    //            result = "Ok";
    //        }
    //        catch (Polly.CircuitBreaker.BrokenCircuitException)
    //        {
    //            result = "Service is unavailable. please try again";
    //        }

    //        return result;
    //    }
    //}
    public interface IMessageService
    {
        Task<string> GetHelloMessage();
        Task<string> GetGoodbyeMessage(int i);
    }

    public class MessageService : IMessageService
    {
        private IMessageRepository _messageRepository;
        private AsyncRetryPolicy _retryPolicy;
        private AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt =>
                {
                    var timeToWait = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                    Console.WriteLine($"Waiting {timeToWait.TotalSeconds} seconds");
                    return timeToWait;
                }
                );

            _circuitBreakerPolicy = Policy.Handle<Exception>()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(10),
                (ex, t) =>
                {
                    Console.WriteLine("Circuit broken!");
                },
                () =>
                {
                    Console.WriteLine("Circuit Reset!");
                },
                () =>
                {
                    Console.WriteLine("Circuit Half-Open!");
                });
        }

        public async Task<string> GetHelloMessage()
        {
            try
            {
                return await _retryPolicy.ExecuteAsync<string>(async () => await _messageRepository.GetHelloMessage());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> GetGoodbyeMessage(int i)
        {
            try
            {
                Console.WriteLine($"GetGoodbyeMessage {i}");
                //if (i <= 2 || i == 7 || i == 8)
                //{
                //    throw new Exception("Exception in MessageRepository");
                //}


                // return "ok";
                Console.WriteLine($"Circuit State: {_circuitBreakerPolicy.CircuitState}");
                return await _circuitBreakerPolicy.ExecuteAsync<string>(async () =>
                {
                    return await _messageRepository.GetGoodbyeMessage(i);
                });
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}