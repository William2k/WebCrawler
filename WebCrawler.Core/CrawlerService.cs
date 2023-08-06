using HtmlAgilityPack;
using System.Collections.Concurrent;
using WebCrawler.Core.Interfaces;
using WebCrawler.Core.Models;

namespace WebCrawler.Core;

public class CrawlerService : ICrawlerService
{
    private ConcurrentDictionary<string, byte> _visited = new();
    private int _failed = 0;

    public async Task<CrawlerResult> Index(string url, int depth)
    {
        await Process(url, 0, depth);

        return new CrawlerResult
        {
            VisitedCount = _visited.Count,
            FailedCount = _failed
        };
    }

    private async Task Process(string url, int currentDepth, int maxDepth)
    {
        url = url.ToLowerInvariant().Trim();

        if (_visited.ContainsKey(url) || ++currentDepth > maxDepth)
            return;

        var links = GetLinks(url);
        _visited.TryAdd(url, 0);

        await Task.Run(() => Parallel.ForEach(links, async link => await Process(link, currentDepth, maxDepth)));
    }

    private IEnumerable<string> GetLinks(string url)
    {
        try
        {
            var doc = new HtmlWeb().Load(url);

            var linkedPages = doc.DocumentNode.SelectNodes("//a[@href]")
                .Select(a => a.GetAttributeValue("href", null))
                .Where(u => !string.IsNullOrEmpty(u)).ToList();

            var uri = new Uri(url);
            var baseUri = uri.GetLeftPart(UriPartial.Authority);

            for (int i = 0; i < linkedPages.Count; i++)
            {
                var current = linkedPages[i];

                if (current == null || !current.StartsWith("/")) continue;

                linkedPages[i] = baseUri + current;
            }

            return linkedPages;
        }
        catch (Exception)
        {
            _failed++;

            return Array.Empty<string>();
        }
    }
}