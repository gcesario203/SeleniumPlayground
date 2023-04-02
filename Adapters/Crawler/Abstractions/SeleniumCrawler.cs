using Application.Crawler.Abstractions;
using OpenQA.Selenium;

namespace Adapters.Crawler.Abstractions
{
    public abstract class SeleniumCrawler : BaseCrawlerDriver
    {
        public IWebDriver Driver { get; protected set; }
        public SeleniumCrawler(string baseUrl, string driverPath, int instanceNumber) : base(baseUrl, driverPath, instanceNumber)
        {
            SetDriver();
        }

        protected abstract void SetDriver();

        public override abstract void Crawl();

        public override void Dispose()
        {
            Driver.Dispose();
        }
    }
}