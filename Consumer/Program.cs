using Consumer.Handler;
using Contract.RabbitMQ;
using System.Threading.Tasks;
using Unity;

namespace Consumer
{
    internal class Program
    {
        static async Task Main(string[] args)

        {

            Message message;
            Reader reader = new Reader();
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IMessageHandler, MessageHandler>();
            MessageHandler mssageHandler = container.Resolve<MessageHandler>();



            while (true)
            {
                message = reader.GetMessage();
                await mssageHandler.Handle(message);
            }
        }
    }
}
