using Lib.Abstractions;
using Lib.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Lib.Implementations
{
    public class Eleicoes2022Crawler : SeleniumCrawler
    {
        private Total _apuracaoTotal;
        public Eleicoes2022Crawler(string baseUrl, string driverPath) : base(baseUrl, driverPath)
        {
        }

        public override void Crawl()
        {
            try
            {
                Driver.Manage().Window.Maximize();

                Driver.Navigate();
                Thread.Sleep(2000);

                GetAllStates();
            }
            catch (System.Exception ex)
            {
                var cu = ex;
                Driver.Dispose();
            }
            finally
            {
                Driver.Dispose();
            }
        }

        protected override void SetDriver()
        {
            Driver = new EdgeDriver(DriverPath);

            Driver.Url = BaseUrl;

            _apuracaoTotal = new Total() { Name = "Apuração de votos segundo turno" };
        }

        public void GetAllStates()
        {
            OpenChangeLocationModal();

            Thread.Sleep(1000);
            // var cities = Driver.FindElement(By.Id("mat-autocomplete-0")).FindElements(By.XPath(".//*")).Where(x => x.TagName == "mat-option").Select(x => x.Text).Where(x => x != "Brasil - BR").Select(x => new Cidade(){ Name = x }).ToList();

            _apuracaoTotal.Cidades = GetOpenedDropdownElements().FirstOrDefault().FindElements(By.XPath(".//*")).Where(x => x.TagName == "mat-option").Select(x => new Cidade { Name = x.Text }).ToList();

            Thread.Sleep(1000);

            foreach (var cidade in _apuracaoTotal.Cidades)
            {
                GetAllMuniciplesByCity(cidade);

                GetModalCombos().FirstOrDefault().Click();
            }
        }

        private void GetAllMuniciplesByCity(Cidade cidade)
        {
            var elementToClick = GetOpenedDropdownElements().FirstOrDefault().FindElements(By.XPath(".//*")).Where(x => x.TagName == "mat-option").FirstOrDefault(x => x.Text == cidade.Name);

            if (elementToClick == null)
                return;

            elementToClick.Click();

            Thread.Sleep(1000);

            var municipiosToClick = GetOpenedDropdownElements().FirstOrDefault().FindElements(By.XPath(".//*")).Where(x => x.TagName == "mat-option");

            cidade.Municipios = municipiosToClick.Select(x => new Municipio { Name = x.Text }).ToList();

            foreach (var municipio in cidade.Municipios)
                GetAllZones(municipio, municipiosToClick);

            GetModalCombos().FirstOrDefault().Click();
        }

        private void GetAllZones(Municipio municipio, IEnumerable<IWebElement> municipiosToClick)
        {
            var elementToClick = municipiosToClick.FirstOrDefault(x => x.Text == municipio.Name);

            if (elementToClick == null)
                return;

            elementToClick.Click();

            Thread.Sleep(1000);

            var confirmButton = Driver.FindElements(By.TagName("ion-button")).FirstOrDefault(x => x.Text == "Confirmar");

            if (confirmButton == null)
                return;

            confirmButton.Click();

            Thread.Sleep(1000);

            ZoneAndSectionsCombos().FirstOrDefault().Click();

            Thread.Sleep(1000);

            var zonas = Driver.FindElements(By.TagName("mat-option")).Select(x => x.Text.Split(" ")).Where(x => x.Count() > 1).Select(x => new Zona() { Name = String.Join(" ", x) }).ToList();

            municipio.Zonas = zonas;
            // GetModalCombos().FirstOrDefault().Click();

            foreach(var zona in municipio.Zonas)
            {
                var index = municipio.Zonas.IndexOf(zona);

                if(index != 0)
                    ZoneAndSectionsCombos().FirstOrDefault().Click();

                Driver.FindElements(By.TagName("mat-option")).FirstOrDefault(x => x.Text == zona.Name).Click();

                ZoneAndSectionsCombos().LastOrDefault().Click();

                var secoes = Driver.FindElements(By.TagName("mat-option")).Select(x => x.Text.Split(" ")).Where(x => x.Count() > 1).Select(x => new Secao() { Name = String.Join(" ", x) }).ToList();

                zona.Secoes = secoes;

                foreach(var secao in zona.Secoes)
                {
                    Driver.FindElements(By.TagName("mat-option")).FirstOrDefault(x => x.Text == secao.Name).Click();

                    Driver.FindElements(By.TagName("button")).FirstOrDefault(x => x.Text == "Pesquisar").Click();

                    Thread.Sleep(1000);
                    
                    var elements = Driver.FindElements(By.TagName("div"))
                                         .FirstOrDefault(x => x.GetAttribute("class") == "ng-star-inserted")
                                         .FindElement(By.TagName("div"))
                                         .FindElement(By.TagName("div"))
                                         .FindElements(By.TagName("p"))
                                         .Select(x => x.Text)
                                         .ToList();
                    Thread.Sleep(1000);
                }
            }

            OpenChangeLocationModal();

            Thread.Sleep(2000);
        }

        private List<IWebElement> ZoneAndSectionsCombos()
        => Driver.FindElements(By.TagName("mat-form-field")).ToList();

        private List<IWebElement> GetOpenedDropdownElements()
        {
            var returnList = Driver.FindElements(By.TagName("div")).Where(x => x.GetAttribute("class").Contains("mat-autocomplete")).ToList();

            if (returnList.Count > 2)
                returnList.RemoveAt(0);

            return returnList;
        }

        private List<IWebElement> GetModalCombos()
        {
            var returnList = Driver.FindElements(By.TagName("div")).Where(x => x.GetAttribute("class").Contains("local-campo")).ToList();

            if (returnList.Count > 2)
                returnList.RemoveAt(0);

            return returnList;
        }

        private void OpenChangeLocationModal()
        {
            var selector = Driver.FindElements(By.TagName("div")).FirstOrDefault(x => x.GetAttribute("title") == "Alterar localidade");

            if (selector == null)
                return;


            selector.Click();
        }
    }
}