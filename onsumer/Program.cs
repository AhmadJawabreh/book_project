using Consumer.General;
using Consumer.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace Consumer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ServiceProvider services = Helper.RegisterAllServices();

            IMessageHandler messageHandler = services.GetService<IMessageHandler>();

            messageHandler.CreateConnection();

            messageHandler.Listen();
        }
    }
}
