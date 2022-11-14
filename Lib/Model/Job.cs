using Lib.Abstractions;

namespace Lib.Model
{
    public class Job : BaseEntity
    {
        public string Title { get; set; }

        public string PublicationYear { get; set; }

        public string AuthorId { get; set; }

        public IDictionary<string?, IEnumerable<string?>> Content { get; set;}
    }
}