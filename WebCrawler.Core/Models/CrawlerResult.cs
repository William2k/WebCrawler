using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Core.Models;

public record CrawlerResult
{
    public int VisitedCount { get; set; }
    public int FailedCount { get; set; }
}
