
using Eleicoes.Constants;
using Eleicoes.Services;
using Lib.Implementations;

var teste = new Eleicoes2022CrawlerService(new Eleicoes2022Crawler(ConfigConstants.CRAWL_CITY_URL, ConfigConstants.DRIVER_PATH));


teste.Run();