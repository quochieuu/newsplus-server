using Microsoft.EntityFrameworkCore;
using NewsPlus.Data.EF;
using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;
using NewsPlus.Infrastructure;

namespace NewsPlus.Business
{
    public class RatingRepository : IRatingRepository
    {
        private readonly DataDbContext _context;

        public RatingRepository(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rating>> GetAll()
        {
            return await _context.Rating
                            .OrderByDescending(p => p.CreatedDate)
                            .ToListAsync();
        }


        public async Task<Rating?> GetById(Guid? id)
        {
            var item = await _context.Rating
                            .OrderByDescending(p => p.CreatedDate)
                            .DefaultIfEmpty()
                            .FirstOrDefaultAsync(p => p.Id == id);

            return item;

        }


        public async Task<RepositoryResponse> Create(CreateRatingViewModel model)
        {
            Rating item = new Rating()
            {
                NewsId = model.NewsId,
                CreatedDate = DateTime.Now,
                Status = 1,
            };

            _context.Rating.Add(item);
            var result = await _context.SaveChangesAsync();

            return new RepositoryResponse()
            {
                Result = result,
                Id = item.Id
            };
        }

        public async Task<RepositoryResponse> Update(Guid id, UpdateRatingViewModel model)
        {
            var item = await _context.Rating.FindAsync(id);
            item.NewsId = model.NewsId;
            item.CreatedDate = DateTime.Now;

            _context.Rating.Update(item);
            var result = await _context.SaveChangesAsync();

            return new RepositoryResponse()
            {
                Result = result,
                Id = id
            };

        }

        public async Task<int> Delete(Guid id)
        {
            var item = await _context.Rating.FindAsync(id);

            _context.Rating.Remove(item);
            var result = await _context.SaveChangesAsync();

            return result;
        }
    }
}
