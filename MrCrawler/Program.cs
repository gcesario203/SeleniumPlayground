
using Lib.Implementations;
using MrCrawler.Services;

var teste = new MarxismOrgCrawlerService(new MarxismOrgCrawler("https://www.marxists.org/portugues/index.htm", "C:\\Program Files (x86)\\Microsoft\\Edge\\Application"));


teste.Run();