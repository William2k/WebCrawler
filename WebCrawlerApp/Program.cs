// See https://aka.ms/new-console-template for more information
using WebCrawler.Core;

var service = new CrawlerService();

Console.WriteLine("CrawlerService started");

Console.WriteLine("Enter Crawling Depth (Default is 1)");

if(!int.TryParse(Console.ReadLine(), out int depth))
    depth = 1;

Console.WriteLine($"Depth set to {depth}");

Console.WriteLine();

while (true)
{
    await Start();
    Console.WriteLine();
}

async Task Start()
{
    Console.WriteLine("Enter url");

    var url = Console.ReadLine();

    if (string.IsNullOrEmpty(url))
        return;

    Console.WriteLine("Crawling in progress...");

    var result = await service.Index(url, depth);

    Console.WriteLine($"{result.VisitedCount} sites visited and {result.FailedCount} failed");
}
