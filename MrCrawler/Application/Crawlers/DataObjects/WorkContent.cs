using Lib.Shared.Abstractions;

namespace MrCrawler.Application.Crawlers.DataObjects
{
    public class WorkContent : BaseEntity
    {
        public string AuthorId { get; set; }

        public string Title { get; set; }

        public Dictionary<string, List<string?>> Content { get; set; }

        public string FileContent { get; set; }

        public Enums.WorkType Type { get; set; }

        public WorkContent(string id, string authorId, string title, Dictionary<string, List<string?>> content, Enums.WorkType type)
        {
            AuthorId = authorId;
            Title = title;
            Content = content;
            Id = id;
            CreatedDate = DateTime.Now;
            Type = type;
        }

        public WorkContent(string id, string authorId, string title, string fileContent, Enums.WorkType type)
        {
            AuthorId = authorId;
            Title = title;
            FileContent = fileContent;
            Id = id;
            CreatedDate = DateTime.Now;
            Type = type;
        }
    }
}