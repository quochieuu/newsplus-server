using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;

namespace NewsPlus.Infrastructure
{
    public interface INewsRepository
    {
        Task<IEnumerable<News>> GetAll();
        Task<Pagination<News>> GetAllPaging(string? filter, int pageIndex, int pageSize);
        Task<News> GetById(Guid? id);
        Task<News> GetBySlug(string? slug);
        Task<RepositoryResponse> Create(CreateNewsViewModel model);
        Task<RepositoryResponse> CreateImage(CreateNewsImageViewModel model, Guid newsId);
        Task<RepositoryResponse> Update(Guid id, UpdateNewsViewModel model);
        Task<int> Delete(Guid id);
    }
}
