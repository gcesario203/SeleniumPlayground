using Lib.Abstractions;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Lib.Implementations
{
    public class MarxismOrgCrawler : SeleniumCrawler
    {
        public MarxismOrgCrawler(string baseUrl, string driverPath) : base(baseUrl, driverPath)
        {
        }

        public override void Crawl()
        {
            Driver.Navigate();

            var firstPageTable = Driver.FindElements(By.TagName("table"));

            var authors = firstPageTable.FirstOrDefault().FindElements(By.TagName("select")).FirstOrDefault().Text.Split("\r\n").ToList();

            firstPageTable.FirstOrDefault().FindElements(By.TagName("select")).FirstOrDefault().Click();

            authors.RemoveAt(0);

            firstPageTable.FirstOrDefault().FindElements(By.TagName("select"))
                                    .FirstOrDefault()
                                    .FindElements(By.TagName("option"))
                                    .FirstOrDefault(x => x.Text == authors[0])
                                    .Click();

            var lifeYears = Driver.FindElement(By.CssSelector("div.link")).Text;
            var biography = String.Join("\n", Driver.FindElements(By.CssSelector("p.texto-sem-espaco")).Select(x => x.Text));

            var jobLinks = Driver.FindElement(By.CssSelector("table.tabela-obras")).FindElements(By.TagName("a")).Select(x => x.GetAttribute("href")).ToList();

            foreach (var joblink in jobLinks)
            {
                if (joblink.Contains("htm"))
                {
                    Driver.Url = joblink;

                    Driver.Navigate();

                    var bodyXpath = Driver.FindElements(By.TagName("body")).FirstOrDefault();

                    var children = bodyXpath.FindElements(By.XPath(".//*")).ToList();

                    var teste = new Dictionary<string?, List<string?>>();

                    foreach (var articleElement in children)
                    {
                        var aaa = articleElement.TagName;
                        if (articleElement.TagName != "h4" && articleElement.TagName != "p")
                            continue;

                        if (articleElement.TagName == "h4" && teste.Keys.Contains(articleElement.Text))
                            continue;
                        else if (articleElement.TagName == "h4")
                            teste.Add(articleElement.Text, new List<string?>());

                        if (articleElement.TagName == "p" && teste.Keys.Count > 0)
                            teste[teste.Keys.LastOrDefault()].Add(articleElement.Text);
                    }

                    Driver.Navigate().Back();
                }
            }

            Driver.Navigate().Back();

            Dispose();
        }

        protected override void SetDriver()
        {
            Driver = new EdgeDriver(DriverPath);

            Driver.Url = BaseUrl;
        }
    }
}