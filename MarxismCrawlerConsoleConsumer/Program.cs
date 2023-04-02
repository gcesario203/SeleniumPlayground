using Adapters.MarxismOrgCrawler.Enums;
using Adapters.MarxismOrgCrawler.Services;
using Adapters.MarxismOrgCrawler.Utils;


for (var x = 1; x <= Constants.NUMBER_OF_DRIVERS; x++)
{
    var teste = new MarxismOrgCrawlerService(new MarxismOrgSeleniumCrawler(Constants.LINK_TO_CRAWL, Constants.DRIVER_PATH, x));

    Thread thread = new Thread(() => teste.Run());
    thread.Start();
}