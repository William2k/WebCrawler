using WebCrawler.Core.Interfaces;

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

        var depth = GetDepth();

        Console.WriteLine();

        while (true)
        {
            await CrawlUrl(depth);
            Console.WriteLine();
        }
    }

    private static int GetDepth()
    {
        Console.WriteLine("Enter Crawling Depth (Default is 1)");

        if (!int.TryParse(Console.ReadLine(), out int depth))
            depth = 1;

        Console.WriteLine($"Depth set to {depth}");

        return depth;
    }

    private static async Task CrawlUrl(int depth)
    {
        Console.WriteLine("Enter url");

        var url = Console.ReadLine();

        if (string.IsNullOrEmpty(url))
            return;

        Console.WriteLine("Crawling in progress...");

        var result = await _crawlerService.Index(url, depth);

        Console.WriteLine($"{result.VisitedCount} sites visited and {result.FailedCount} failed");
    }
}
