using Lib.Shared.Abstractions;

namespace MrCrawler.Application.Crawlers.DataObjects
{
    public class WorkLink : BaseEntity
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public WorkLink(string title, string link) : base()
        {
            Title = title;

            Link = link;
        }
    }
}