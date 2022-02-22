using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;

namespace NewsPlus.Infrastructure
{
    public interface IConfigRepository
    {
        Task<IEnumerable<Config>> GetAll();
        Task<Pagination<Config>> GetAllPaging(string? filter, int pageIndex, int pageSize);
        Task<Config> GetById(Guid? id);
        Task<RepositoryResponse> Create(CreateConfigViewModel model);
        Task<RepositoryResponse> Update(Guid id, UpdateConfigViewModel model);
        Task<int> Delete(Guid id);
    }
}
