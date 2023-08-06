namespace WebCrawler.Core.Models;

public class CrawlerSettings
{
    public string Url { get; set; }

    public int MaxDepth { get; set; }

    public bool OnlySameOrigin { get; set; }        
}
