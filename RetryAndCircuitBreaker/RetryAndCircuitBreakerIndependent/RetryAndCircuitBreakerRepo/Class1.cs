
using Google.Protobuf.Reflection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System;
using System.Threading.Tasks;

namespace RetryAndCircuitBreakerRepo
{
    public interface IMessageRepository
    {
        Task<string> GetHelloMessage();
        Task<string> GetGoodbyeMessage(int i);
    }

    public class MessageRepository : IMessageRepository
    {
        private MessageOptions _messageOptions;

        public MessageRepository(IOptions<MessageOptions> messageOptions)
        {
            _messageOptions = messageOptions.Value;
        }

        public async Task<string> GetHelloMessage()
        {
            Console.WriteLine("MessageRepository GetHelloMessage running");
            ThrowRandomException();
            return ".HelloMessage";
        }

        public async Task<string> GetGoodbyeMessage(int i)
        {
            Console.WriteLine("MessageRepository GetGoodbyeMessage running");
            if (i <= 2 || i == 7 || i == 8)
            {
                throw new Exception("Exception in MessageRepository");
            }
           // ThrowRandomException();
            return ".GoodbyeMessage";
        }

        private void ThrowRandomException()
        {
            var diceRoll = new Random().Next(0, 10);

            if (diceRoll <=2 || diceRoll <= 7)
            {
                Console.WriteLine("ERROR! Throwing Exception");
                throw new Exception("Exception in MessageRepository");
            }
        }
    }

}