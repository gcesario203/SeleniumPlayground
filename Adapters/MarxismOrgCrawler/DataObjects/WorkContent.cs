using Application.Shared.Abstractions;

namespace Adapters.MarxismOrgCrawler.DataObjects
{
    public class WorkContent : BaseEntity
    {
        public string AuthorId { get; private set; }

        public string Title { get; private set; }

        public WorkContent? Parent { get; private set; }

        public Dictionary<string, List<string?>> Content { get; private set; }

        public string FileContent { get; private set; }

        public Enums.WorkType Type { get; private set; }

        public WorkContent(string id, string authorId, string title, Dictionary<string, List<string?>> content, Enums.WorkType type, WorkContent parent = null)
        {
            AuthorId = authorId;
            Title = title;
            Content = content;
            Id = id;
            CreatedDate = DateTime.Now;
            Type = type;
        }

        public WorkContent(string id, string authorId, string title, string fileContent, Enums.WorkType type, WorkContent parent = null)
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