using OpenQA.Selenium;

namespace Lib.Crawler.Abstractions
{
    public abstract class SeleniumCrawler : BaseCrawlerDriver
    {
        public IWebDriver Driver { get; protected set; }
        public SeleniumCrawler(string baseUrl, string driverPath) : base(baseUrl, driverPath)
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