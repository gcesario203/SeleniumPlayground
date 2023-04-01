namespace Lib.Shared.Contracts
{
    public interface IBaseEntity : IEqualityComparer<IBaseEntity>
    {
        string GetId();

        DateTime GetCreationDate();

        DateTime GetUpdateDate();
    }
}