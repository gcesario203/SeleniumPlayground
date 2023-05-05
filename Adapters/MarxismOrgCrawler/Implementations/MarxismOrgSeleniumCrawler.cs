using System.Text;
using System.Text.RegularExpressions;
using Adapters.Crawler.Abstractions;
using Adapters.MarxismOrgCrawler.DataObjects;
using Adapters.MarxismOrgCrawler.Utils;
using OpenQA.Selenium;

namespace Adapters.MarxismOrgCrawler.Enums
{
    public class MarxismOrgSeleniumCrawler : SeleniumCrawler
    {
        private readonly int AuthorPerDriver = Constants.AUTHORS_PER_DRIVER;
        public MarxismOrgSeleniumCrawler(string baseUrl, string driverPath, int instanceNumber, Type seleniumDriverType) : base(baseUrl, driverPath, instanceNumber, seleniumDriverType)
        {
        }

        public override void Crawl()
        {
            var authors = GetAuthorsAndTheirJobs();

            var authorWorks = GetAllAuthorsWorks(authors);

            Dispose();
        }

        private IEnumerable<WorkContent> GetAllAuthorsWorks(IEnumerable<AuthorDataObject> authors)
        {
            var authorContents = new List<WorkContent>();

            foreach (var author in authors)
            {
                var parsedAuthorContents = GetAuthorWork(author);

                if (parsedAuthorContents?.Count() == 0)
                    continue;

                authorContents.AddRange(parsedAuthorContents);
            };

            return authorContents;
        }

        private IEnumerable<AuthorDataObject> GetAuthorsAndTheirJobs()
        {
            /// Navega pra home do site
            Driver.Navigate();

            var authorsSelect = GetAuthorsSelectOnFirstPage();

            // Pega todos os autores do select da esquerda
            var authors = authorsSelect.Text.Split("\r\n").ToList();

            // Remove o placeholder do select
            // authors.RemoveAt(0);

            if (InstanceNumber > 0)
                authors = authors.Skip((InstanceNumber - 1) * AuthorPerDriver).Take(AuthorPerDriver).ToList();

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
            if (author?.WorkLinks?.Count() == 0)
                return null;

            var returnList = new List<WorkContent>();

            foreach (var joblink in author.WorkLinks)
            {
                try
                {
                    var workContentToBeAdded = GetFileContentByJobLink(joblink, author);

                    if (workContentToBeAdded == null)
                        continue;

                    returnList.AddRange(workContentToBeAdded);
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine($"ERRO: Erro ao buscar obra a {joblink.Title} do autor {author.Name}: {ex.Message}");
                    return null;
                }
            }

            return returnList;
        }

        private IEnumerable<WorkContent?> GetFileContentByJobLink(WorkLink jobLink, AuthorDataObject author, string parent = null)
        {
            if (string.IsNullOrEmpty(jobLink.Link) || !jobLink.Link.Split('/').Contains("portugues"))
                return null;

            /// SIMPLE HTML
            if (jobLink.Link.Contains("htm") && !jobLink.Link.Contains("index.htm"))
            {
                return PrepareSimpleHtmlWorkContent(jobLink, author, parent);
            }

            // CHAINED HTML
            if (jobLink.Link.Contains("index.htm"))
                return PrepareChainedHtmlWorkContent(jobLink, author, parent);

            // FILE CONTENT
            return PrepareFileWorkContent(jobLink, author, parent);
        }

        private IEnumerable<WorkContent?> PrepareSimpleHtmlWorkContent(WorkLink jobLink, AuthorDataObject author, string parent = null)
        {
            try
            {
                Driver.Url = jobLink.Link;

                Driver.Navigate();

                var bodyXpath = Driver.FindElements(By.TagName("body")).FirstOrDefault();

                var children = GetMarxismoOrgContentBetweenHrTags(bodyXpath.FindElements(By.XPath(".//*")).ToList());

                if (children?.Count() == 0)
                    return null;

                var rawHtmlContent = string.Join("<br/>", children.Select(x => $"<{x.TagName}>{x.Text}<{x.TagName}/>")
                                                                  .Where(x => !x.Contains("Início da página"))
                                                );

                return new List<WorkContent?>() { new WorkContent(jobLink.GetId(), author.GetId(), jobLink.Title, rawHtmlContent, Enums.WorkType.HTML_SIMPLE_CONTENT, parent) };
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

            if (Regex.IsMatch(webElement.TagName, "([h1]{1}[1-9]{1})"))
                return true;

            if (webElement.TagName == "p" && webElement.FindElements(By.XPath(".//*")).Select(x => x.TagName).Contains("strong"))
                return true;

            return false;
        }

        private IEnumerable<WorkContent?> PrepareChainedHtmlWorkContent(WorkLink jobLink, AuthorDataObject author, string parent = null)
        {
            try
            {
                Driver.Url = jobLink.Link;

                Driver.Navigate();

                var bodyXpath = Driver.FindElements(By.TagName("body")).FirstOrDefault();

                var children = GetMarxismoOrgContentBetweenHrTags(bodyXpath.FindElements(By.XPath(".//*")).ToList());

                var jobLinksArticle = children.Where(x => x.TagName == "a" && x.Text != "Início da página")
                                       .Select(x => new WorkLink(x.Text, x.GetAttribute("href")))
                                       .ToList();

                var indexWorkContent = new WorkContent(jobLink.GetId(), author.GetId(), jobLink.Title, string.Empty, WorkType.HTML_CHAINED_CONTENT, parent);

                var returnList = new List<WorkContent?>() { indexWorkContent };

                foreach (var jobLinkArticle in jobLinksArticle)
                {
                    var childArticles = GetFileContentByJobLink(jobLinkArticle, author, indexWorkContent.GetId());

                    if (childArticles?.Count() == 0)
                        continue;

                    returnList.AddRange(childArticles);
                }

                return returnList;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"ERRO: Erro ao buscar obra a {jobLink.Title} do autor {author.Name}: {ex.Message}");
                return null;
            }
        }

        private IEnumerable<IWebElement?> GetMarxismoOrgContentBetweenHrTags(List<IWebElement?> webElements)
        {
            if (webElements?.Count() == 0)
                return null;

            var lastHr = webElements.LastOrDefault(x => x.TagName == "hr");

            var firstHr = webElements.LastOrDefault(x => x.TagName == "hr" && x.Location != lastHr.Location);

            return webElements.Skip(webElements.IndexOf(firstHr)).Take(webElements.IndexOf(lastHr) - webElements.IndexOf(firstHr));
        }

        private IEnumerable<WorkContent?> PrepareFileWorkContent(WorkLink jobLink, AuthorDataObject author, string parent = null)
        {
            var downloadedFile = new HttpClient().GetStringAsync(jobLink.Link).GetAwaiter().GetResult();

            var fileWorkContent = new WorkContent(jobLink.GetId(), author.GetId(), jobLink.Title, Encoding.ASCII.GetBytes(downloadedFile), WorkType.FILE_CONTENT, parent);
            return new List<WorkContent?> { fileWorkContent };
        }
    }
}