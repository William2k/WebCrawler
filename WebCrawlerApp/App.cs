using WebCrawler.Core.Interfaces;
using WebCrawler.Core.Models;

namespace WebCrawlerApp;

internal static class App
{
    private static ICrawlerService _crawlerService;

    internal static async Task Start(ICrawlerService crawlerService)
    {
        _crawlerService = crawlerService;

        await StartService();
    }

    private static async Task StartService()
    {
        Console.WriteLine("CrawlerService started");

        var settings = new CrawlerSettings
        {
            MaxDepth = GetMaxDepthSetting(),
            OnlySameOrigin = GetOnlySameOriginSetting()
        };
        Console.WriteLine();

        while (true)
        {
            await CrawlUrl(settings);
            Console.WriteLine();
        }
    }

    private static bool GetOnlySameOriginSetting()
    {
        Console.WriteLine("Enter Crawling Same Origin Only (Y/N) [Default is N]");

        var yn = Console.ReadLine()?.ToUpper()?.FirstOrDefault();

        var onlySameOrigin = yn == 'Y';
        
        if (onlySameOrigin)
            Console.WriteLine("OnlySameOrigin is Enabled");
        else
            Console.WriteLine("OnlySameOrigin is Disabled");

        return onlySameOrigin;
    }

    private static int GetMaxDepthSetting()
    {
        Console.WriteLine("Enter Crawling Max Depth [Default is 1]");

        if (!int.TryParse(Console.ReadLine(), out int depth))
            depth = 1;

        Console.WriteLine($"Max Depth set to {depth}");

        return depth;
    }

    private static async Task CrawlUrl(CrawlerSettings settings)
    {
        Console.WriteLine("Enter url");

        settings.Url = Console.ReadLine();

        if (string.IsNullOrEmpty(settings.Url))
            return;

        Console.WriteLine("Crawling in progress...");

        var result = await _crawlerService.Index(settings);

        Console.WriteLine($"{result.VisitedCount} sites visited and {result.FailedCount} failed");
    }
}
