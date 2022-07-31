using System;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace RetryAndCircuitBreaker
{
    internal class Program
    {
        private static AsyncRetryPolicy _retryPolicy;
        private static AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        static void Main(string[] args)
        {
          //  Console.WriteLine("Retry Policy!");
            // TestRetryServiceApi();
            Console.ReadLine();
            TestRetryServiceApi();
             Console.WriteLine("Retry Policy!");
            Console.ReadLine();
            Console.WriteLine("Circuit Breaker!");
            Console.ReadLine();
            TestCircuitServiceApi(1);
            TestCircuitServiceApi(2);          
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            TestCircuitServiceApi(3);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(11));
            TestCircuitServiceApi(4);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(6));
            TestCircuitServiceApi(5);
            TestCircuitServiceApi(6);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
            TestCircuitServiceApi(7);
            TestCircuitServiceApi(8);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            TestCircuitServiceApi(9);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(11));
            TestCircuitServiceApi(10);
            //_retryPolicy = Policy.Handle<Exception>()
            //    .WaitAndRetryAsync(3, retryAttempt =>
            //    {
            //        var timeToWait = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
            //        Console.WriteLine($"Waiting {timeToWait.TotalSeconds} seconds");
            //        return timeToWait;
            //    }
            //    );

            //   _circuitBreakerPolicy = Policy.Handle<Exception>()
            //.CircuitBreakerAsync(3, TimeSpan.FromSeconds(25),
            //(ex, t) =>
            //{
            //    Console.WriteLine("Circuit broken!");
            //},
            //() =>
            //{
            //    Console.WriteLine("Circuit Reset!");
            //},
            //() =>
            //{
            //    Console.WriteLine("Circuit Half - Open!");
            //});
            //   //t();

            //   GetGoodbyeMessage22();
            //   GetGoodbyeMessage22();
            //   GetGoodbyeMessage22();
            //   GetGoodbyeMessage2();
            //   System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
            //   GetGoodbyeMessage2();
            //   System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
            //   GetGoodbyeMessage2();
            //   System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
            //   GetGoodbyeMessage2();
            //GetGoodbyeMessage();
            //GetGoodbyeMessage();
            //Console.WriteLine("first two exception and wait for 5 secs");
            //System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
            //Console.WriteLine("Executing circuit within 5 second so it will be open");
            //GetGoodbyeMessage2();
            //System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            //Console.WriteLine("Executing circuit within 8 second so it will be half-open and reseted");
            //GetGoodbyeMessage2();

            //Console.WriteLine("Executing circuit will be closed");
            ////  System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            //GetGoodbyeMessage2();
            Console.WriteLine("Retry Finished");
            Console.ReadLine();
        }

        static async void t()
        {
            await _retryPolicy.ExecuteAsync<string>(async () => await GetGoodbyeMessage1());
        }


        static private async Task<string> TestCircuitServiceApi(int i)
        {
            HttpClient _client = new HttpClient();
            Console.WriteLine("MessageRepository TestServiceApi running");

            _client.BaseAddress = new Uri("https://localhost:7245/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await _client.GetAsync($"/TestCircuit?i={i}");
            if (response.IsSuccessStatusCode)
            {
                return "B";
            }
            return "A";
        }

        static private async Task<string> TestRetryServiceApi()
        {
            HttpClient _client = new HttpClient();
            Console.WriteLine("MessageRepository TestServiceApi running");

            _client.BaseAddress = new Uri("https://localhost:7245/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await _client.GetAsync("/TestRetry");
            if (response.IsSuccessStatusCode)
            {
                return "B";
            }
            return "A";
        }


        static public async Task<string> GetGoodbyeMessage22()
        {
            try
            {
                Console.WriteLine($"Circuit State: {_circuitBreakerPolicy.CircuitState}");
                return await _circuitBreakerPolicy.ExecuteAsync<string>(async () =>
                {
                    return await GetGoodbyeMessage1();
                });
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        static public async Task<string> GetGoodbyeMessage2()
        {
            try
            {
                Console.WriteLine($"Circuit State: {_circuitBreakerPolicy.CircuitState}");
                return await _circuitBreakerPolicy.ExecuteAsync<string>(async () =>
                {
                    return await GetGoodbyeMessage21();
                });
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        static public async Task<string> GetGoodbyeMessage1()
        {
            Console.WriteLine("MessageRepository GetGoodbyeMessage running");
            throw new NotImplementedException();
            return "b";
        }
        static public async Task<string> GetGoodbyeMessage21()
        {
            Console.WriteLine("MessageRepository GetGoodbyeMessage21 running");
            //   throw new NotImplementedException();
            return "b";
        }
    }
}
