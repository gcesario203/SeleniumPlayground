using Application.Crawler.Abstractions;
using OpenQA.Selenium;

namespace Adapters.Crawler.Abstractions
{
    public abstract class SeleniumCrawler : BaseCrawlerDriver
    {
        private readonly Type SeleniumDriverType;
        public IWebDriver Driver { get; protected set; }
        public SeleniumCrawler(string baseUrl, string driverPath, int instanceNumber, Type seleniumDriverType, int instanceQuantity) : base(baseUrl, driverPath, instanceNumber,instanceQuantity)
        {
            SeleniumDriverType = seleniumDriverType;
            SetDriver();
        }

        protected virtual void SetDriver()
        {
            if(SeleniumDriverType == null)
                throw new Exception("ERRO: Tipo de driver não declarado");

            Driver = Activator.CreateInstance(SeleniumDriverType, DriverPath) as IWebDriver;

            Driver.Url = BaseUrl;
        }

        public override abstract void Crawl();

        public override void Dispose()
        {
            Driver.Dispose();
        }
    }
}