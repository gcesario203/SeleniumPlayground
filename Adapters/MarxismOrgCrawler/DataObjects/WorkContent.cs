using Application.Shared.Abstractions;

namespace Adapters.MarxismOrgCrawler.DataObjects
{
    public class WorkContent : BaseEntity
    {
        public string AuthorId { get; private set; }

        public string Title { get; private set; }

        public string Parent { get; private set; }

        public string Content { get; private set; }

        public byte[] FileContent { get; private set; }

        public Enums.WorkType Type { get; private set; }

        public WorkContent(string id, string authorId, string title, string content, Enums.WorkType type, string parent = null)
        {
            AuthorId = authorId;
            Title = title;
            Content = content;
            Id = id;
            CreatedDate = DateTime.Now;
            Type = type;
            Parent = parent;
        }

        public WorkContent(string id, string authorId, string title, byte[] fileContent, Enums.WorkType type, string parent = null)
        {
            AuthorId = authorId;
            Title = title;
            FileContent = fileContent;
            Id = id;
            CreatedDate = DateTime.Now;
            Type = type;
            Parent = parent;
        }
    }
}