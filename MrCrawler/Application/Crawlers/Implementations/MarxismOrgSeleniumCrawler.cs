using Lib.Crawler.Abstractions;
using MrCrawler.Application.Crawlers.DataObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace MrCrawler.Application.Crawlers.Implementations
{
    public class MarxismOrgSeleniumCrawler : SeleniumCrawler
    {
        public MarxismOrgSeleniumCrawler(string baseUrl, string driverPath) : base(baseUrl, driverPath)
        {
        }

        public override void Crawl()
        {
            GetAuthorsAndTheirJobs();

            // foreach (var joblink in jobLinks)
            // {

            //     if (!joblink.Contains("htm"))
            //         continue;

            //     Driver.Url = joblink;

            //     Driver.Navigate();

            //     var bodyXpath = Driver.FindElements(By.TagName("body")).FirstOrDefault();

            //     var children = bodyXpath.FindElements(By.XPath(".//*")).ToList();

            //     var teste = new Dictionary<string?, List<string?>>();

            //     foreach (var articleElement in children)
            //     {
            //         var aaa = articleElement.TagName;
            //         if (articleElement.TagName != "h4" && articleElement.TagName != "p")
            //             continue;

            //         if (articleElement.TagName == "h4" && teste.Keys.Contains(articleElement.Text))
            //             continue;
            //         else if (articleElement.TagName == "h4")
            //             teste.Add(articleElement.Text, new List<string?>());

            //         if (articleElement.TagName == "p" && teste.Keys.Count > 0)
            //             teste[teste.Keys.LastOrDefault()].Add(articleElement.Text);
            //     }

            //     Driver.Navigate().Back();
            // }

            Driver.Navigate().Back();

            Dispose();
        }

        protected override void SetDriver()
        {
            Driver = new EdgeDriver(DriverPath);

            Driver.Url = BaseUrl;
        }

        private IEnumerable<AuthorDataObject> GetAuthorsAndTheirJobs()
        {
            /// Navega pra home do site
            Driver.Navigate();

            var authorsSelect = GetAuthorsSelectOnFirstPage();

            // Pega todos os autores do select da esquerda
            var authors = authorsSelect.Text.Split("\r\n").ToList();

            // sei la pq removi, provavelmente ta pegando algo em branco ou um valor default
            authors.RemoveAt(0);

            /// Clica no select de autores
            authorsSelect.Click();

            var authorList = new List<AuthorDataObject>();

            foreach (var author in authors)
            {
                /// Faz a tratativa na pagina atual para buscar os dados do autor
                var authorObj = GetAuthorData(author);

                // caso estoure alguma exception, retornamos para a primeira pagina e continuamos iterando a lista de autores
                if (authorObj == null)
                {
                    Driver.Navigate().Back();

                    GetAuthorsSelectOnFirstPage(true);

                    continue;
                }

                authorList.Add(authorObj);

                Driver.Navigate().Back();

                GetAuthorsSelectOnFirstPage(true);
            }

            return authorList;
        }

        private IWebElement? GetAuthorsSelectOnFirstPage(bool shouldClick = false)
        {
            // Pega a table que tem os selects
            var firstPageTable = Driver.FindElements(By.TagName("table"));

            /// Clica no select de autores
            var authorsSelect = firstPageTable.FirstOrDefault().FindElements(By.TagName("select")).FirstOrDefault();

            if (shouldClick)
                authorsSelect.Click();

            return authorsSelect;
        }

        private AuthorDataObject? GetAuthorData(string authorName)
        {
            try
            {
                var iterationAuthorSelect = GetAuthorsSelectOnFirstPage();

                /// Clica no autor da iteração atual
                iterationAuthorSelect.FindElements(By.TagName("option"))
                     .FirstOrDefault(x => x.Text == authorName)
                     .Click();

                // Pega o periodo de vida do maldito
                var lifeYears = Driver.FindElement(By.CssSelector("div.link")).Text;

                // Pega toda a biografia
                var biography = String.Join("\n", Driver.FindElements(By.CssSelector("p.texto-sem-espaco")).Select(x => x.Text));

                /// Pego todos os links das obras do autor
                var jobLinks = Driver.FindElement(By.CssSelector("table.tabela-obras")).FindElements(By.TagName("a")).Select(x => x.GetAttribute("href")).ToList();

                return new AuthorDataObject(authorName, lifeYears, biography, jobLinks);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"ERRO: Erro ao buscar o autor {authorName}: {ex.Message}");

                return null;
            }
        }
    }
}