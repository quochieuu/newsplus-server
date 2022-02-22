using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPlus.Infrastructure
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Pagination<Category>> GetAllPaging(string? filter, int pageIndex, int pageSize);
        Task<Category> GetById(Guid? id);
        Task<RepositoryResponse> Create(CreateCategoryViewModel model);
        Task<RepositoryResponse> Update(Guid id, UpdateCategoryViewModel model);
        Task<int> Delete(Guid id);
    }
}
