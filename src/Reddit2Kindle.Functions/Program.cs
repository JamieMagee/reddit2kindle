using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reddit2Kindle.Functions.Contracts.Options;
using Reddit2Kindle.Functions.Services;
using StrongGrid;
using static Reddit2Kindle.Functions.Constants;

namespace Reddit2Kindle.Functions
{
    public class Program
    {
        public static void Main()
        {
            new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", true, false)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, false)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((context, serviceCollection) =>
                {
                    serviceCollection.AddHttpClient()
                        .AddSingleton<Reddit2KindleService>()
                        .AddSingleton<RedditService>()
                        .AddSingleton<RazorService>()
                        .AddSingleton<SendGridService>()
                        .AddSingleton<ReadabilityService>()
                        .AddSingleton<WebhookParser>()
                        .AddFluentEmail(Reddit2KindleEmailAddress)
                        .AddSendGridSender(context.Configuration.GetSection("SendGrid")["ApiKey"])
                        .Services
                        .AddOptions<RedditOptions>()
                        .BindConfiguration("Reddit");
                })
                .Build()
                .Run();
        }
    }
}
