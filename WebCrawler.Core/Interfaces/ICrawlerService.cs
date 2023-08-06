using WebCrawler.Core.Models;

namespace WebCrawler.Core.Interfaces
{
    public interface ICrawlerService
    {
        Task<CrawlerResult> Index(CrawlerSettings settings);
    }
}