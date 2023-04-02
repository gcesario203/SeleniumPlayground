
using Application.Crawler.Contracts;

namespace Application.Crawler.Abstractions
{
    public abstract class BaseCrawlerDriver : ICrawlerDriver
    {
        public int InstanceNumber { get; private set; }
        public string BaseUrl { get; private set; }

        public string DriverPath { get; private set; }
        
        public BaseCrawlerDriver(string baseUrl, string driverPath, int insanceNumber)
        {
            BaseUrl = baseUrl;
            DriverPath = driverPath;
            InstanceNumber = insanceNumber;
        }
        public abstract void Crawl();

        public abstract void Dispose();
    }
}