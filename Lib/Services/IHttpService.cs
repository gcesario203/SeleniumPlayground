namespace Lib.Services
{
    public interface IHttpService<T>
    {
        Task<IEnumerable<T>> FindAll();

        Task<T> FindById(long id);

        Task<T> Create(T model);

        Task<T> Update(T model);

        Task<bool> Delete(long id);
    }
}