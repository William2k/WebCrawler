using System.Collections.Concurrent;
using WebCrawler.Core.Interfaces;
using WebCrawler.Core.Models;

namespace WebCrawler.Core;

public class CrawlerService : ICrawlerService
{
    private ConcurrentDictionary<string, byte> _visited = new();
    private ConcurrentDictionary<Guid, CrawlerResult> _results = new();

    public async Task<CrawlerResult> Index(CrawlerSettings settings)
    {
        var id = Guid.NewGuid();
        var result = new CrawlerResult
        {
            VisitedCount = 0,
            FailedCount = 0
        };

        _results.TryAdd(id, result);

        await Process(id, settings.Url, 0, settings);

        return result;
    }

    private async Task Process(Guid id, string url, int currentDepth, CrawlerSettings settings)
    {
        url = url.ToLowerInvariant().Trim();

        if (_visited.ContainsKey(url) || ++currentDepth > settings.MaxDepth)
            return;

        try
        {
            var links = HttpHelpers.GetLinksFromUrl(url, settings.OnlySameOrigin);
            _visited.TryAdd(url, 0);

            if (_results.TryGetValue(id, out CrawlerResult result))
                result.VisitedCount++;

            await Task.Run(() => Parallel.ForEach(links, async link => await Process(id, link, currentDepth, settings)));
        }
        catch (Exception)
        {
            if (_results.TryGetValue(id, out CrawlerResult result))
                result.FailedCount++;
        }
    }
}