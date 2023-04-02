using Adapters.Crawler.Abstractions;
using Adapters.MarxismOrgCrawler.DataObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Adapters.MarxismOrgCrawler.Enums
{
    public class MarxismOrgSeleniumCrawler : SeleniumCrawler
    {
        public MarxismOrgSeleniumCrawler(string baseUrl, string driverPath) : base(baseUrl, driverPath)
        {
        }

        public override void Crawl()
        {
            var authors = GetAuthorsAndTheirJobs();

            var authorContents = new List<WorkContent>();

            foreach (var author in authors)
            {
                var parsedAuthorContents = GetAuthorWork(author);

                if (parsedAuthorContents?.Count() == 0)
                    continue;

                authorContents.AddRange(parsedAuthorContents);
            };

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

                if (authorList.Count > 0)
                    break;

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
                var jobLinks = Driver.FindElement(By.CssSelector("table.tabela-obras"))
                                     .FindElements(By.TagName("a"))
                                     .Select(x => new WorkLink(x.Text, x.GetAttribute("href")))
                                     .ToList();

                return new AuthorDataObject(authorName, lifeYears, biography, jobLinks);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"ERRO: Erro ao buscar o autor {authorName}: {ex.Message}");

                return null;
            }
        }

        private IEnumerable<WorkContent> GetAuthorWork(AuthorDataObject author)
        {
            if (author?.WorkLinks.Count() == 0)
                return null;

            var returnList = new List<WorkContent>();

            foreach (var joblink in author.WorkLinks)
            {
                try
                {
                    var workContentToBeAdded = GetFileContentByJobLink(joblink, author);

                    if(workContentToBeAdded == null)
                        continue;

                    returnList.Add(workContentToBeAdded);
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine($"ERRO: Erro ao buscar obra a {joblink.Title} do autor {author.Name}: {ex.Message}");
                    return null;
                }
            }

            return returnList;
        }

        private WorkContent? GetFileContentByJobLink(WorkLink jobLink, AuthorDataObject author)
        {
            /// SIMPLE HTML
            if (jobLink.Link.Contains("htm") && !jobLink.Link.Contains("index.htm"))
            {
                return PrepareSimpleHtmlWorkContent(jobLink, author);
            }

            // CHAINED HTML
            if (jobLink.Link.Contains("index.htm"))
            {
                return null;
            }

            // FILE CONTENT
            return null;
        }

        private WorkContent? PrepareSimpleHtmlWorkContent(WorkLink jobLink, AuthorDataObject author)
        {
            try
            {
                Driver.Url = jobLink.Link;

                Driver.Navigate();

                var bodyXpath = Driver.FindElements(By.TagName("body")).FirstOrDefault();

                var children = bodyXpath.FindElements(By.XPath(".//*")).ToList();

                var article = new Dictionary<string?, List<string?>>();

                var lastHr = children.LastOrDefault(x => x.TagName == "hr");

                var firstHr = children.LastOrDefault(x => x.TagName == "hr" && x.Location != lastHr.Location);

                foreach (var childElement in children.Skip(children.IndexOf(firstHr)).Take(children.IndexOf(lastHr) - children.IndexOf(firstHr)))
                {
                    var isTitle = CheckIfElementIsATitle(childElement);

                    if (!isTitle && childElement.TagName != "p")
                        continue;

                    if (isTitle)
                    {
                        article.Add(childElement.Text, new List<string?>());

                        continue;
                    }

                    if (childElement.TagName == "p" && childElement.Text != "Início da página")
                    {
                        if (article.Keys.Count == 0)
                            article.Add("NoTitle", new List<string?>());

                        article[article.Keys.LastOrDefault()].Add(childElement.Text);
                    }
                }

                return new WorkContent(jobLink.GetId(), author.GetId(), jobLink.Title, article, Enums.WorkType.HTML_SIMPLE_CONTENT);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"ERRO: Erro ao buscar obra a {jobLink.Title} do autor {author.Name}: {ex.Message}");
                return null;
            }
        }

        private bool CheckIfElementIsATitle(IWebElement? webElement)
        {
            if (webElement == null)
                return false;

            if (webElement.TagName == "h4")
                return true;

            if (webElement.TagName == "p" && webElement.FindElements(By.XPath(".//*")).Select(x => x.TagName).Contains("strong"))
                return true;

            return false;
        }
    }
}