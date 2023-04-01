
using Lib.Crawler.Contracts;

namespace Lib.Crawler.Abstractions
{
    public abstract class BaseCrawlerDriver : ICrawlerDriver
    {
        public string BaseUrl { get; private set; }

        public string DriverPath { get; private set; }
        
        public BaseCrawlerDriver(string baseUrl, string driverPath)
        {
            BaseUrl = baseUrl;
            DriverPath = driverPath;
        }
        public abstract void Crawl();

        public abstract void Dispose();
    }
}