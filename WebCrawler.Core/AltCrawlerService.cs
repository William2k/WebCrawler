using System.Collections.Concurrent;
using WebCrawler.Core.Interfaces;
using WebCrawler.Core.Models;

namespace WebCrawler.Core;

public class AltCrawlerService : ICrawlerService
{
    private ConcurrentDictionary<string, byte> _visited = new();
    private ConcurrentDictionary<Guid, CrawlerResult> _results = new();

    public async Task<CrawlerResult> Index(string url, int depth)
    {
        var id = Guid.NewGuid();
        var result = new CrawlerResult
        {
            VisitedCount = 0,
            FailedCount = 0
        };

       _results.TryAdd(id, result);

        await StartCrawling(id, url, depth);

        return result;
    }

    private async Task StartCrawling(Guid id, string url, int depth)
    {
        var currentDepth = 0;
        var sites = new List<string>
        {
            url
        };

        while (++currentDepth <= depth && sites.Any())
        {
            var linksFound = new ConcurrentBag<string>();

            var actions = sites.Select(s => new Action(() => Process(id, s, ref linksFound)));

            await Task.Run(() => Parallel.Invoke(actions.ToArray()));

            sites = linksFound.ToList();
        }
    }


    private void Process(Guid id, string url, ref ConcurrentBag<string> linksFound)
    {
        url = url.ToLowerInvariant().Trim();

        if (_visited.ContainsKey(url))
            return;

        try
        {
            var links = HttpHelpers.GetLinksFromUrl(url);
            _visited.TryAdd(url, 0);

            if(_results.TryGetValue(id, out CrawlerResult result))
                result.VisitedCount++;

            foreach ( var link in links)
            {
                linksFound.Add(link);
            }
        }
        catch (Exception)
        {
            if (_results.TryGetValue(id, out CrawlerResult result))
                result.FailedCount++;
        }
    }
}
