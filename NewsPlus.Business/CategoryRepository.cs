using Microsoft.EntityFrameworkCore;
using NewsPlus.Common;
using NewsPlus.Data.EF;
using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;
using NewsPlus.Infrastructure;

namespace NewsPlus.Business
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataDbContext _context;

        public CategoryRepository(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories
                            .OrderByDescending(p => p.CreatedDate)
                            .ToListAsync();
        }

        public async Task<Pagination<Category>> GetAllPaging(string? filter, int pageIndex, int pageSize)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter)
                || x.Name.Contains(filter));
            }
            var totalRecords = await query.CountAsync();

            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var pagination = new Pagination<Category>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            return pagination;
        }

        public async Task<Category?> GetById(Guid? id)
        {
            var item = await _context.Categories
                            .OrderByDescending(p => p.CreatedDate)
                            .DefaultIfEmpty()
                            .FirstOrDefaultAsync(p => p.Id == id);

            return item;

        }


        public async Task<RepositoryResponse> Create(CreateCategoryViewModel model)
        {
            string slug; slug = TextHelper.ToUnsignString(model.Name).ToLower();
            var check = this.CheckUniqueSlug(slug);

            if (!check)
                slug = TextHelper.ToUnsignString(model.Name).ToLower() + TextHelper.GetTimestamp(DateTime.Now);

            int status; status = model.Status;
            if (status == null)
                model.Status = 0;

            Category item = new Category()
            {
                Name = model.Name,
                Slug = slug,
                ParentId = model.ParentId,
                CreatedDate = DateTime.Now,
                Status = status,
            };

            _context.Categories.Add(item);
            var result = await _context.SaveChangesAsync();

            return new RepositoryResponse()
            {
                Result = result,
                Id = item.Id
            };
        }

        public async Task<RepositoryResponse> Update(Guid id, UpdateCategoryViewModel model)
        {
            string slug = String.Empty;

            if (model.Slug == null)
            {
                slug = this.GetSlug(id);

                if(slug == String.Empty)
                {
                    slug = TextHelper.ToUnsignString(model.Name).ToLower();
                    var check = this.CheckUniqueSlug(slug);

                    if (!check)
                        slug = TextHelper.ToUnsignString(model.Name).ToLower() + TextHelper.GetTimestamp(DateTime.Now);
                }
            } else
            {
                var check = this.CheckUniqueSlug(model.Slug);

                if (!check)
                    slug = TextHelper.ToUnsignString(model.Name).ToLower() + TextHelper.GetTimestamp(DateTime.Now);
            }

            int status; status = model.Status;
            if (status == null)
                model.Status = 0;


            var item = await _context.Categories.FindAsync(id);
            item.Name = model.Name;
            item.Slug = slug;
            item.ParentId = model.ParentId;
            item.ModifiedDate = DateTime.Now;
            item.Status = status;

            _context.Categories.Update(item);
            var result = await _context.SaveChangesAsync();

            return new RepositoryResponse()
            {
                Result = result,
                Id = id
            };

        }

        public async Task<int> Delete(Guid id)
        {
            var item = await _context.Categories.FirstOrDefaultAsync(sl => sl.Id == id);

            _context.Categories.Remove(item);
            var result = await _context.SaveChangesAsync();

            return result;
        }

        public bool CheckUniqueSlug(string slug)
        {
            var item = _context.Categories.FirstOrDefault(sl => sl.Slug == slug);
            if (item == null)
                return true;
            else
                return false;
        }

        public string GetSlug(Guid? id)
        {
            var item = _context.Categories.FirstOrDefault(sl => sl.Id == id);
            if (item != null)
                return item.Slug;
            else
                return String.Empty;
        }


    }
}
