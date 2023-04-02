using Application.Shared.Abstractions;

namespace Adapters.MarxismOrgCrawler.DataObjects
{
    public class AuthorDataObject : BaseEntity
    {
        public string Name { get; private set; }

        public string LifePeriod { get; private set; }

        public string Biography { get; private set; }

        public IEnumerable<WorkLink> WorkLinks { get; private set; }

        public AuthorDataObject(string name, string lifePeriod, string biography, IEnumerable<WorkLink> workLinks) : base()
        {
            Name = name;
            LifePeriod = lifePeriod;
            Biography = biography;
            WorkLinks = workLinks;   
        }
    }
}