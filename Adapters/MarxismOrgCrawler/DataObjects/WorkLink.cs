using Application.Shared.Abstractions;

namespace Adapters.MarxismOrgCrawler.DataObjects
{
    public class WorkLink : BaseEntity
    {
        public string Title { get; private set; }

        public string Link { get; private set; }

        public WorkLink(string title, string link) : base()
        {
            Title = title;

            Link = link;
        }
    }
}