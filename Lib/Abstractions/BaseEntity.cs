using System.Diagnostics.CodeAnalysis;
using Lib.Contracts;

namespace Lib.Abstractions
{
    public abstract class BaseEntity : IBaseEntity
    {
        public string Id {get; private set;}
        public DateTime CreatedDate {get;private set;}

        public DateTime UpdatedDate { get; private set; }

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
    }
}