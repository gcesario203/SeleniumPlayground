using Lib.Contracts;
using Lib.Services;

namespace Eleicoes.Services
{
    public class Eleicoes2022CrawlerService: ICrawlerService
    {
        private readonly ICrawlerDriver _crawlerDriver;

        public Eleicoes2022CrawlerService(ICrawlerDriver crawlerDriver)
        {
            _crawlerDriver = crawlerDriver;
        }

        public void Run()
        {
            _crawlerDriver.Crawl();
        }
    }
}