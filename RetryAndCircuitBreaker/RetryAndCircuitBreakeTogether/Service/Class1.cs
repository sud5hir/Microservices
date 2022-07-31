namespace Service
{
    public class SomeService : ISomeService
    {
        private readonly HttpClient _httpClient;

        public SomeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> DoSomething(CancellationToken cancellationToken)
        {
            string result = string.Empty;
            try
            {
                 await _httpClient.GetAsync("notfounduri", cancellationToken);
                //await Task.Delay(1);
                //throw new NullReferenceException();
                result = "Ok";
            }
            catch (Polly.CircuitBreaker.BrokenCircuitException)
            {
                result = "Service is unavailable. please try again";
            }
            return result;
        }
    }
}