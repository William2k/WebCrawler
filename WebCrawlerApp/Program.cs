using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Core;
using WebCrawler.Core.Interfaces;
using WebCrawlerApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        //setup our DI
        var serviceProvider = SetupDI();

        var crawlerService = serviceProvider.GetService<ICrawlerService>();

        if (crawlerService == null)
            throw new ArgumentNullException(nameof(crawlerService));

        await App.Start(crawlerService);
    }

    private static ServiceProvider SetupDI() => new ServiceCollection()
            .AddSingleton<ICrawlerService, AltCrawlerService>()
            .BuildServiceProvider();
}