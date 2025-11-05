using CodePulse.API.Data;
using CodePulse.API.Models;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepositry : IBlogPostRepository<BlogPost>
    {
        private readonly ApplicationDbContext _dbContext;
        public BlogPostRepositry(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task <BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _dbContext.BlogPosts.AddAsync(blogPost);
            await _dbContext.SaveChangesAsync();
            return blogPost;
        }
        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
        }
        public async Task<BlogPost> GetAsyncById(Guid id)
        {
           return await _dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<BlogPost> UpdateAsync(BlogPost entity)
        {    
            var existingBlogpost = await _dbContext.BlogPosts.Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (existingBlogpost == null)
            {
                return null;
            }
            _dbContext.Entry(existingBlogpost).CurrentValues.SetValues(entity);
            existingBlogpost.Categories = entity.Categories;
            await _dbContext.SaveChangesAsync();
            return existingBlogpost;
        }
        public async Task<BlogPost> DeleteAsync(Guid id)
        {
             var blogPost = await _dbContext.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return null!;
            }
            _dbContext.BlogPosts.Remove(blogPost);

            await _dbContext.SaveChangesAsync();

            return blogPost;
        }
        public async Task<BlogPost> GetByUrlHandleAsync(string urlHandle)
        {
            return await _dbContext.BlogPosts.Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }
    }
}
