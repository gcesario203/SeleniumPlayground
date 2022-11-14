using Lib.Abstractions;

namespace Lib.Model
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }

        public string BirthYear { get; set; }

        public string DeathYear { get; set; }

        public string Biography { get; set; }

        public IEnumerable<string> JobIds { get; set; }
    }
}