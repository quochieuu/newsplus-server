using Microsoft.EntityFrameworkCore;
using NewsPlus.Data.EF;
using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;
using NewsPlus.Infrastructure;

namespace NewsPlus.Business
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly DataDbContext _context;

        public SubscriberRepository(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subscriber>> GetAll()
        {
            return await _context.Subscribers
                            .OrderByDescending(p => p.CreatedDate)
                            .ToListAsync();
        }

        public async Task<Pagination<Subscriber>> GetAllPaging(string? filter, int pageIndex, int pageSize)
        {
            var query = _context.Subscribers.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Email.Contains(filter)
                || x.Email.Contains(filter));
            }
            var totalRecords = await query.CountAsync();

            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var pagination = new Pagination<Subscriber>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            return pagination;
        }

        public async Task<Subscriber?> GetById(Guid? id)
        {
            var item = await _context.Subscribers
                            .OrderByDescending(p => p.CreatedDate)
                            .DefaultIfEmpty()
                            .FirstOrDefaultAsync(p => p.Id == id);

            return item;

        }


        public async Task<RepositoryResponse> Create(CreateSubscriberViewModel model)
        {

            Subscriber item = new Subscriber()
            {
                Email = model.Email,
                CreatedDate = DateTime.Now,
            };

            _context.Subscribers.Add(item);
            var result = await _context.SaveChangesAsync();

            return new RepositoryResponse()
            {
                Result = result,
                Id = item.Id
            };
        }

        public async Task<RepositoryResponse> Update(Guid id, UpdateSubscriberViewModel model)
        {
            var item = await _context.Subscribers.FindAsync(id);
            item.Email = model.Email;
            item.CreatedDate = DateTime.Now;

            _context.Subscribers.Update(item);
            var result = await _context.SaveChangesAsync();

            return new RepositoryResponse()
            {
                Result = result,
                Id = id
            };

        }

        public async Task<int> Delete(Guid id)
        {
            var item = await _context.Subscribers.FindAsync(id);

            _context.Subscribers.Remove(item);
            var result = await _context.SaveChangesAsync();

            return result;
        }
    }
}
