using Lib.Contracts;
using Lib.Crawler.Contracts;

namespace MrCrawler.Application.Crawlers.Services
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
            try
            {
                _crawlerDriver.Crawl();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                _crawlerDriver.Dispose();
            }
        }
    }
}