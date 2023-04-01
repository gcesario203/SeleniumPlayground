namespace MrCrawler.Application.Crawlers.DataObjects
{
    public class AuthorDataObject
    {
        public string Name { get; set; }

        public string LifePeriod { get; set; }

        public string Biography { get; set; }

        public IEnumerable<string> WorkLinks { get; set; }

        public AuthorDataObject(string name, string lifePeriod, string biography, IEnumerable<string> workLinks)
        {
            Name = name;
            LifePeriod = lifePeriod;
            Biography = biography;
            WorkLinks = workLinks;   
        }
    }
}