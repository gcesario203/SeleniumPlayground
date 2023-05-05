using System.Diagnostics.CodeAnalysis;
using Application.Shared.Contracts;

namespace Application.Shared.Abstractions
{
    public abstract class BaseEntity : IBaseEntity
    {
        protected string Id;
        protected DateTime CreatedDate;

        protected DateTime UpdatedDate;

        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();

            CreatedDate = DateTime.Now;
        }

        public bool Equals(IBaseEntity? x, IBaseEntity? y)
            => x?.GetId() == y?.GetId();


        public int GetHashCode([DisallowNull] IBaseEntity obj)
            => obj?.GetId()?.GetHashCode() ?? 0;

        public string GetId()
            => Id;

        public DateTime GetCreationDate()
            => CreatedDate;

        public DateTime GetUpdateDate()
            => UpdatedDate;
    }
}