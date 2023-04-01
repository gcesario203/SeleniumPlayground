using System.Diagnostics.CodeAnalysis;
using Lib.Shared.Contracts;

namespace Lib.Shared.Abstractions
{
    public abstract class BaseEntity : IBaseEntity
    {
        private string Id;
        private DateTime CreatedDate;

        private DateTime UpdatedDate;

        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();

            CreatedDate = DateTime.Now;
        }

        public bool Equals(IBaseEntity? x, IBaseEntity? y)
        {
            return GetIdByReflection(x) == GetIdByReflection(y);
        }

        public int GetHashCode([DisallowNull] IBaseEntity obj)
        {
            return GetIdByReflection(obj)?.GetHashCode() ?? 0;
        }

        private string? GetIdByReflection(IBaseEntity? entity)
        {
            return entity?.GetType().GetProperty("Id")?.GetValue(entity)?.ToString() ?? null;
        }

        public string GetId()
            => Id;

        public DateTime GetCreationDate()
            => CreatedDate;
            
        public DateTime GetUpdateDate()
            => UpdatedDate;
    }
}