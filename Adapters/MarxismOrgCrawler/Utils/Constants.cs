using OpenQA.Selenium.Edge;

namespace Adapters.MarxismOrgCrawler.Utils
{
    public static class Constants
    {
        public readonly static string DRIVER_PATH = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application";

        public readonly static string LINK_TO_CRAWL = "https://www.marxists.org/portugues/index.htm";

        public readonly static int NUMBER_OF_DRIVERS = 4;

        public readonly static Type SELENIUM_DRIVER_TYPE = typeof(EdgeDriver);
    }
}