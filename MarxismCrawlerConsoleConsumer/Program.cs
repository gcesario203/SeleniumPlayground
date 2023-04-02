using Adapters.MarxismOrgCrawler.Enums;
using Adapters.MarxismOrgCrawler.Services;
using Adapters.MarxismOrgCrawler.Utils;

var teste = new MarxismOrgCrawlerService(new MarxismOrgSeleniumCrawler(Constants.LINK_TO_CRAWL, Constants.DRIVER_PATH));


teste.Run();