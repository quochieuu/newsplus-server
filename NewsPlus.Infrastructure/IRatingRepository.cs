using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;

namespace NewsPlus.Infrastructure
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetAll();
        Task<Rating> GetById(Guid? id);
        Task<RepositoryResponse> Create(CreateRatingViewModel model);
        Task<RepositoryResponse> Update(Guid id, UpdateRatingViewModel model);
        Task<int> Delete(Guid id);
    }
}
