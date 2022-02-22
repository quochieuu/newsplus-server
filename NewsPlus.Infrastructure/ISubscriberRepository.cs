using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;

namespace NewsPlus.Infrastructure
{
    public interface ISubscriberRepository
    {
        Task<IEnumerable<Subscriber>> GetAll();
        Task<Pagination<Subscriber>> GetAllPaging(string? filter, int pageIndex, int pageSize);
        Task<Subscriber> GetById(Guid? id);
        Task<RepositoryResponse> Create(CreateSubscriberViewModel model);
        Task<RepositoryResponse> Update(Guid id, UpdateSubscriberViewModel model);
        Task<int> Delete(Guid id);
    }
}
