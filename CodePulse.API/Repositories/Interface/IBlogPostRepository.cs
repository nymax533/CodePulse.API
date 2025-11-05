namespace CodePulse.API.Repositories.Interface
{
    public interface IBlogPostRepository <T> where T : class
    {
        Task<T> CreateAsync(T category);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsyncById(Guid id);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(Guid id);

        Task<T> GetByUrlHandleAsync(string urlHandle);

    }
}
