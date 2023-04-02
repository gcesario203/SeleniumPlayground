using Adapters.MarxismOrgCrawler.Enums;
using Adapters.MarxismOrgCrawler.Services;
using Adapters.MarxismOrgCrawler.Utils;


for (var x = 1; x <= Constants.NUMBER_OF_DRIVERS; x++)
{
    var teste = new MarxismOrgCrawlerService(new MarxismOrgSeleniumCrawler(Constants.LINK_TO_CRAWL, Constants.DRIVER_PATH, x, Constants.SELENIUM_DRIVER_TYPE));

    Thread thread = new Thread(() => teste.Run());
    thread.Start();
}