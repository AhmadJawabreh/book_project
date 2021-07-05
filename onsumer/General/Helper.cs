using BusinessLogic;
using BusinessLogic.Manngers;
using Consumer.Handler;
using Consumer.Services;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Producer;
using Repoistories;
using System.Net.Http;

namespace Consumer.General
{
    internal class Helper
    {
        public static ServiceProvider RegisterAllServices()
        {
            return new ServiceCollection()
             .AddScoped<HttpClient, HttpClient>()
             .AddScoped<IUnitOfWork, UnitOfWork>()
             .AddTransient<IHarvesterManager, HarvesterManager>()
             .AddScoped<IProduce, Produce>()
             .AddScoped<IMessageHandler, MessageHandler>()
             .AddScoped<IAuthorService, AuthorService>()
             .AddScoped<IAuthorManager, AuthorManager>()
             .AddScoped<IPublisherService, PublisherService>()
             .AddScoped<IPublisherManager, PublisherManager>()
             .AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer("Server=localhost;Database=db_book;Trusted_Connection=True;"))
             .AddSingleton<ApplicationDbContext, ApplicationDbContext>()
             .BuildServiceProvider();
        }
    }
}
