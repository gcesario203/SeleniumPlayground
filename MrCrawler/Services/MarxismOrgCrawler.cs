using Lib.Contracts;
using Lib.Services;

namespace MrCrawler.Services
{
    public class MarxismOrgCrawlerService : ICrawlerService
    {
        private readonly ICrawlerDriver _crawlerDriver;

        public MarxismOrgCrawlerService(ICrawlerDriver crawlerDriver)
        {
            _crawlerDriver = crawlerDriver;
        }
        public void Run()
        {
            _crawlerDriver.Crawl();
        }
    }
}