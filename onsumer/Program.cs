namespace Consumer
{
    using BusinessLogic;
    using Consumer.Handler;
    using Consumer.Services;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Repoistories;
    using System.Net.Http;

    internal class Program
    {
        public static void Main(string[] args)
        {
            ServiceProvider services = RegisterAllServices();
            MessageHandler _messageHandler = services.GetService<MessageHandler>();

            _messageHandler.CreateConnection();
            _messageHandler.Listen();
        }

        public static ServiceProvider RegisterAllServices()
        {

            return new ServiceCollection()
             .AddScoped<IUnitOfWork, UnitOfWork>()
             .AddScoped<MessageHandler, MessageHandler>()
             .AddScoped<HttpClient, HttpClient>()
             .AddScoped<AuthorService, AuthorService>()
             .AddScoped<AuthorManager, AuthorManager>()
             .AddScoped<PublisherService, PublisherService>()
             .AddScoped<PublisherManager, PublisherManager>()
             .AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer("Server=localhost;Database=db_book;Trusted_Connection=True;"))
             .AddSingleton<ApplicationDbContext, ApplicationDbContext>()
             .BuildServiceProvider();
        }
    }
}
