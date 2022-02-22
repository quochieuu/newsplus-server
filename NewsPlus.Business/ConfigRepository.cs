using Microsoft.EntityFrameworkCore;
using NewsPlus.Data.EF;
using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;
using NewsPlus.Infrastructure;

namespace NewsPlus.Business
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly DataDbContext _context;

        public ConfigRepository(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Config>> GetAll()
        {
            return await _context.Configs
                            .OrderByDescending(p => p.CreatedDate)
                            .ToListAsync();
        }

        public async Task<Pagination<Config>> GetAllPaging(string? filter, int pageIndex, int pageSize)
        {
            var query = _context.Configs.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter)
                || x.Name.Contains(filter));
            }
            var totalRecords = await query.CountAsync();

            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var pagination = new Pagination<Config>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            return pagination;
        }

        public async Task<Config?> GetById(Guid? id)
        {
            var item = await _context.Configs
                            .OrderByDescending(p => p.CreatedDate)
                            .DefaultIfEmpty()
                            .FirstOrDefaultAsync(p => p.Id == id);

            return item;

        }


        public async Task<RepositoryResponse> Create(CreateConfigViewModel model)
        {
            Config item = new Config()
            {
                Name = model.Name,
                Value = model.Value,
                CreatedDate = DateTime.Now,
            };

            _context.Configs.Add(item);
            var result = await _context.SaveChangesAsync();

            return new RepositoryResponse()
            {
                Result = result,
                Id = item.Id
            };
        }

        public async Task<RepositoryResponse> Update(Guid id, UpdateConfigViewModel model)
        {
            var item = await _context.Configs.FindAsync(id);
            item.Name = model.Name;
            item.Value = model.Value;
            item.CreatedDate = DateTime.Now;

            _context.Configs.Update(item);
            var result = await _context.SaveChangesAsync();

            return new RepositoryResponse()
            {
                Result = result,
                Id = id
            };

        }

        public async Task<int> Delete(Guid id)
        {
            var item = await _context.Configs.FindAsync(id);

            _context.Configs.Remove(item);
            var result = await _context.SaveChangesAsync();

            return result;
        }
    }
}
