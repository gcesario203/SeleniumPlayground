using Lib.Shared.Abstractions;

namespace MrCrawler.Application.Crawlers.DataObjects
{
    public class AuthorDataObject : BaseEntity
    {
        public string Name { get; set; }

        public string LifePeriod { get; set; }

        public string Biography { get; set; }

        public IEnumerable<WorkLink> WorkLinks { get; set; }

        public AuthorDataObject(string name, string lifePeriod, string biography, IEnumerable<WorkLink> workLinks) : base()
        {
            Name = name;
            LifePeriod = lifePeriod;
            Biography = biography;
            WorkLinks = workLinks;   
        }
    }
}