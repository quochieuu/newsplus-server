using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;

namespace NewsPlus.Infrastructure
{
    public interface IRoleRepository
    {
        Task<IEnumerable<SysAppRole>> GetAll();
        Task<Pagination<SysAppRole>> GetAllPaging(string? filter, int pageIndex, int pageSize);
        Task<SysAppRole> GetById(Guid? id);
        //Task<RepositoryResponse> Create(CreateRoleViewModel model, string currentUsername);
        //Task<RepositoryResponse> Update(Guid id, UpdateRoleViewModel model, string currentUsername);
        Task<RepositoryResponse> ChangeStatus(Guid id, string currentUsername);
        Task<RepositoryResponse> MoveToTrash(Guid id, string currentUsername);
        Task<int> Delete(Guid id);

        //Task<IEnumerable<PermissionViewModel>> GetPermissionByRoleId(Guid roleId);
        //Task<RepositoryResponse> PutPermissionByRoleId(Guid roleId, UpdatePermissionRequest request);
    }
}
