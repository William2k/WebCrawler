using HtmlAgilityPack;

namespace WebCrawler.Core;

public static class HttpHelpers
{
    public static IEnumerable<string> GetLinksFromUrl(string url)
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
}
