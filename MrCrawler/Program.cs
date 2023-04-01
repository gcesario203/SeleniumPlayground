
using MrCrawler.Application.Crawlers.Implementations;
using MrCrawler.Application.Crawlers.Services;
using MrCrawler.Application.Crawlers.Utils;

var teste = new MarxismOrgCrawlerService(new MarxismOrgSeleniumCrawler(Constants.LINK_TO_CRAWL, Constants.DRIVER_PATH));


teste.Run();