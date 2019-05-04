using Gong.Common.Contract;
using Gong.Common.Core.CQRS.Dispatcher;
using Gong.Common.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Gong.Common.Tests 
{
    public class BaseTest
    {
        protected IServiceProvider ServiceProvider;
        protected Dispatcher Dispatcher { get; private set; }
        public BaseTest()
        {
            var Configuration = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json")
                   .Build();
            var services = new ServiceCollection();
            services.AddScoped(typeof(IRepository<>), typeof(GenericEFRepository<>));
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<Dispatcher>();

            //Func<IServiceProvider, ITokenHttpClient> httpClientBuiler = new Func<IServiceProvider, ITokenHttpClient>((px) =>
            //{
            //    string baseUrl = Configuration.GetValue<string>("apiMgmtUrl");                
            //    TokenHttpClient client = new TokenHttpClient();
            //    client.BaseAddress = new Uri(baseUrl);
            //    return client;
            //});
            //services.AddScoped(httpClientBuiler);
            //services.AddScoped<ISample01Resource, Sample01Resource>();


            ServiceProvider = services.BuildServiceProvider();

            Init();
        }
        protected T GetService<T>()
        {
            return ServiceProvider.GetService<T>();

        }
        private void Init()
        {
            Dispatcher = ServiceProvider.GetService<Dispatcher>();
        }

    }
}
