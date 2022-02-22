using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;

namespace NewsPlus.Infrastructure
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAll();
        Task<Pagination<Comment>> GetAllPaging(string? filter, int pageIndex, int pageSize);
        Task<Comment> GetById(Guid? id);
        Task<RepositoryResponse> Create(CreateCommentViewModel model);
        Task<RepositoryResponse> Update(Guid id, UpdateCommentViewModel model);
        Task<int> Delete(Guid id);
    }
}
