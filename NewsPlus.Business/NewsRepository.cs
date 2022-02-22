using Microsoft.EntityFrameworkCore;
using NewsPlus.Common;
using NewsPlus.Data.EF;
using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;
using NewsPlus.Infrastructure;

namespace NewsPlus.Business
{
    public class NewsRepository : INewsRepository
    {
        private readonly DataDbContext _context;

        public NewsRepository(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<News>> GetAll()
        {
            return await _context.News
                            .OrderByDescending(p => p.CreatedDate)
                            .ToListAsync();
        }

        public async Task<Pagination<News>> GetAllPaging(string? filter, int pageIndex, int pageSize)
        {
            var query = _context.News.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Title.Contains(filter)
                || x.Title.Contains(filter));
            }
            var totalRecords = await query.CountAsync();

            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var pagination = new Pagination<News>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            return pagination;
        }

        public async Task<News?> GetById(Guid? id)
        {
            var item = await _context.News
                            .OrderByDescending(p => p.CreatedDate)
                            .DefaultIfEmpty()
                            .FirstOrDefaultAsync(p => p.Id == id);

            return item;

        }

        public async Task<News?> GetBySlug(string? slug)
        {
            var item = await _context.News
                            .OrderByDescending(p => p.CreatedDate)
                            .DefaultIfEmpty()
                            .FirstOrDefaultAsync(p => p.Slug == slug);

            return item;

        }

        public async Task<RepositoryResponse> Create(CreateNewsViewModel model)
        {
            string slug; slug = TextHelper.ToUnsignString(model.Title).ToLower();
            var check = this.CheckUniqueSlug(slug);

            if (!check)
                slug = TextHelper.ToUnsignString(model.Title).ToLower() + TextHelper.GetTimestamp(DateTime.Now);

            int status; status = model.Status;
            if (status == null)
                model.Status = 0;

            News item = new News()
            {
                Title = model.Title,
                Slug = slug,
                Description = model.Description,
                UrlImage = model.UrlImage,
                Content = model.Content,
                ViewCount = 1,
                CategoryId = model.CategoryId,
                CreatedDate = DateTime.Now,
                Status = status,
            };

            _context.News.Add(item);
            var result = await _context.SaveChangesAsync();

            return new RepositoryResponse()
            {
                Result = result,
                Id = item.Id
            };
        }

        public async Task<RepositoryResponse> CreateImage(CreateNewsImageViewModel model, Guid newsId)
        {
            string POST_IMAGE_PATH = "news/images/";

            if (model.UrlImage != null)
            {
                
                var image = UploadImage.UploadImageFile(model.UrlImage, POST_IMAGE_PATH);

                var item = await _context.News.FindAsync(newsId);
                item.UrlImage = image;

                _context.News.Update(item);
                var result = await _context.SaveChangesAsync();

                return new RepositoryResponse()
                {
                    Result = result,
                    Id = newsId
                };
            }

            return new RepositoryResponse()
            {
                Result = 0,
                Id = newsId
            };

        }

        public async Task<RepositoryResponse> Update(Guid id, UpdateNewsViewModel model)
        {
            string slug = String.Empty;

            if (model.Slug == null)
            {
                slug = this.GetSlug(id);

                if (slug == String.Empty)
                {
                    slug = TextHelper.ToUnsignString(model.Title).ToLower();
                    var check = this.CheckUniqueSlug(slug);

                    if (!check)
                        slug = TextHelper.ToUnsignString(model.Title).ToLower() + TextHelper.GetTimestamp(DateTime.Now);
                }
            }
            else
            {
                var check = this.CheckUniqueSlug(model.Slug);

                if (!check)
                    slug = TextHelper.ToUnsignString(model.Title).ToLower() + TextHelper.GetTimestamp(DateTime.Now);
            }

            int status; status = model.Status;
            if (status == null)
                model.Status = 0;


            var item = await _context.News.FindAsync(id);
            item.Title = model.Title;
            item.Slug = slug;
            item.Description = model.Description;
            item.Content = model.Content;
            item.CategoryId = model.CategoryId;
            item.ModifiedDate = DateTime.Now;
            item.Status = status;

            _context.News.Update(item);
            var result = await _context.SaveChangesAsync();

            return new RepositoryResponse()
            {
                Result = result,
                Id = id
            };

        }

        public async Task<int> Delete(Guid id)
        {
            var item = await _context.News.FindAsync(id);

            _context.News.Remove(item);
            var result = await _context.SaveChangesAsync();

            return result;
        }

        public bool CheckUniqueSlug(string slug)
        {
            var item = _context.News.FirstOrDefault(sl => sl.Slug == slug);
            if (item == null)
                return true;
            else
                return false;
        }

        public string GetSlug(Guid? id)
        {
            var item = _context.News.FirstOrDefault(sl => sl.Id == id);
            if (item != null)
                return item.Slug;
            else
                return String.Empty;
        }
    }
}
