using HtmlAgilityPack;
using System;

namespace WebCrawler.Core;

public static class HttpHelpers
{
    public static IEnumerable<string> GetLinksFromUrl(string url, bool onlySameOrigin)
    {
        var uri = new Uri(url);
        var doc = new HtmlWeb().Load(url);

        var linkedPages = doc.DocumentNode.SelectNodes("//a[@href]")
            .Select(a => a.GetAttributeValue("href", null))
            .Where(u => !string.IsNullOrEmpty(u)).ToList();

        var baseUri = uri.GetLeftPart(UriPartial.Authority);

        for (int i = 0; i < linkedPages.Count; i++)
        {
            var current = linkedPages[i];

            if (current == null || !current.StartsWith("/")) continue;

            linkedPages[i] = baseUri + current;
        }

        linkedPages.RemoveAll(x =>
        {
            try
            {
                var current = new Uri(x); // Validates uri format

                return onlySameOrigin && current.GetLeftPart(UriPartial.Authority) != baseUri;
            }
            catch (UriFormatException)
            {
                return true;
            }
        });

        return linkedPages;
    }
}
