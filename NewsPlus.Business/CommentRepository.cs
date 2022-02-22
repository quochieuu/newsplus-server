using Microsoft.EntityFrameworkCore;
using NewsPlus.Data.EF;
using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;
using NewsPlus.Infrastructure;

namespace NewsPlus.Business
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataDbContext _context;

        public CommentRepository(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            return await _context.Comments
                            .OrderByDescending(p => p.CreatedDate)
                            .ToListAsync();
        }

        public async Task<Pagination<Comment>> GetAllPaging(string? filter, int pageIndex, int pageSize)
        {
            var query = _context.Comments.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Content.Contains(filter)
                || x.Content.Contains(filter));
            }
            var totalRecords = await query.CountAsync();

            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var pagination = new Pagination<Comment>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            return pagination;
        }

        public async Task<Comment?> GetById(Guid? id)
        {
            var item = await _context.Comments
                            .OrderByDescending(p => p.CreatedDate)
                            .DefaultIfEmpty()
                            .FirstOrDefaultAsync(p => p.Id == id);

            return item;

        }


        public async Task<RepositoryResponse> Create(CreateCommentViewModel model)
        {

            Comment item = new Comment()
            {
                Content = model.Content,
                NewsId = model.NewsId,
                CreatedDate = DateTime.Now,
                Status = 1,
            };

            _context.Comments.Add(item);
            var result = await _context.SaveChangesAsync();

            return new RepositoryResponse()
            {
                Result = result,
                Id = item.Id
            };
        }

        public async Task<RepositoryResponse> Update(Guid id, UpdateCommentViewModel model)
        {
            var item = await _context.Comments.FindAsync(id);
            item.Content = model.Content;
            item.NewsId = model.NewsId;
            item.CreatedDate = DateTime.Now;

            _context.Comments.Update(item);
            var result = await _context.SaveChangesAsync();

            return new RepositoryResponse()
            {
                Result = result,
                Id = id
            };

        }

        public async Task<int> Delete(Guid id)
        {
            var item = await _context.Comments.FindAsync(id);

            _context.Comments.Remove(item);
            var result = await _context.SaveChangesAsync();

            return result;
        }
    }
}
