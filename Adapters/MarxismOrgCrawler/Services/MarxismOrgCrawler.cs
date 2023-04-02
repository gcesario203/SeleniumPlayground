using Application.Contracts;
using Application.Crawler.Contracts;

namespace Adapters.MarxismOrgCrawler.Services
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