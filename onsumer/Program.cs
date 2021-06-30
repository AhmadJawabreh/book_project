using BusinessLogic;
using Consumer.Handler;
using Consumer.Services;
using Contract.RabbitMQ;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repoistories;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Consumer
{


    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            Message message;
            Reader reader = new Reader();

            var  services = new ServiceCollection()
                    .AddScoped<IUnitOfWork, UnitOfWork>()
                    .AddScoped<MessageHandler, MessageHandler>()
                    .AddScoped<HttpClient, HttpClient>()
                    .AddScoped<AuthorService, AuthorService>()
                    .AddScoped<AuthorManager, AuthorManager>()
                    .AddScoped<PublisherService, PublisherService>()
                    .AddScoped<PublisherManager, PublisherManager>()
                    .AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer("Server=localhost;Database=book_db;Trusted_Connection=True;"))
                    .AddSingleton<ApplicationDbContext, ApplicationDbContext>()
                    .BuildServiceProvider();

            MessageHandler _messageHandler = services.GetService<MessageHandler>();

            while (true)
            {
                message = reader.GetMessage();
                await _messageHandler.Handle(message);
                message = null;
            }
        }
    }
}
